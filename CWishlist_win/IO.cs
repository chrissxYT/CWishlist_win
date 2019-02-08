﻿using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.IO.Compression;
using System;
using System.Net;
using static CWishlist_win.CLinq;
using static CWishlist_win.Consts;
using static SevenZip.SevenZipHelper;
using static System.IO.FileMode;
using static System.Text.Encoding;
using static binutils.bin;
using static binutils.c;
using static binutils.io;
using static binutils.str;

namespace CWishlist_win
{
    static class IO
    {
        public static string tinyurl_create(string url)
        {
            return new WebClient().DownloadString(tinyurl_api + Uri.EscapeDataString(url));
        }

        public static bool valid_url(string url)
        {
            string s = url.ToLower();
            return s.StartsWith(http) || s.StartsWith(https) ||
                s.StartsWith(ftp) || s.StartsWith(lbry) && fccontains(s, '.');
        }

        /// <summary>
        /// Loads the given file with the format recognized from the extention.
        /// </summary>
        public static WL load(string f)
        {
            char c = f[f.Length - 1];
            return f == "" ? WL.NEW : (c == 'l' && f[f.Length - 2] == 'l') ? cwll_load(f)
                : c == 'd' ? cwld_load(f) : c == 'u' ? cwlu_load(f) : throw new Exception(
                    "Only CWLL, CWLD and CWLU files are supported by this version of CWL.");
        }

        public static WL backup_load(string f)
        {
            return cwld_load(f);
        }

        public static void backup_save(WL wl, string f)
        {
            cwld_save(wl, f);
        }

        /// <summary>
        /// Save func for the CWLL-format<para />
        /// For information on the format check the load/read func
        /// </summary>
        public static void cwll_save(WL wl, string file)
        {
            dbg("[CWLL]Saving file...");
            FileStream fs = File.Open(file, Create, FileAccess.Write);
            fs.write(cwll_header);
            fs.write(1);
            dbg("[CWLL]Wrote header...");
            MemoryStream ms = new MemoryStream();
            foreach (Item i in wl)
            {
                i.write_bytes(ms, L1);
                dbg("[CWLL]Wrote {0}...", i.dbgfmt());
            }
            ms.Seek(0, SeekOrigin.Begin);
            dbg("[CWLL]Compressing {0} bytes: {1}", ms.Length, hex(ms.ToArray()));
            Compress(ms, fs);
            ms.Close();
            fs.Close();
            dbg("[CWLL]Wrote file.");
        }

        /// <summary>
        /// Read func for the CWLL-format<para />
        /// Name: CWishlistLZMA (LZMA compressed binary+UTF8 format)<para />
        /// File version: 4 (not saved)<para />
        /// Format versions: 1(saved, checked)
        /// </summary>
        public static WL cwll_load(string file)
        {
            dbg("[CWLL]Loading file...");
            FileStream fs = File.Open(file, Open, FileAccess.Read);
            byte[] hdr = new byte[4];
            fs.Read(hdr, 0, 4);
            if (!arrequ(hdr, cwll_header))
            {
                fs.Close();
                throw new InvalidHeaderException("CWLL", cwll_header, hdr);
            }
            if (fs.ReadByte() != 1)
            {
                fs.Close();
                throw new Exception("This CWL version only supports v1 of the CWLL standard.");
            }
            dbg("[CWLL]Read header...");
            MemoryStream ms = new MemoryStream();
            Decompress(fs, ms);
            fs.Close();
            List<Item> items = new List<Item>();
            int j;
            List<byte> bfr = new List<byte>();
            ms.Seek(0, SeekOrigin.Begin);
            dbg("[CWLL]Decompressed data is {0} bytes: {1}", ms.Length, hex(ms.ToArray()));
            while ((j = ms.ReadByte()) != -1)
            {
                while (j != 11 && j != 8)
                {
                    bfr.Add((byte)j);
                    j = ms.ReadByte();
                }
                string name = utf8(bfr.ToArray());
                string url;
                if (j == 11)
                {
                    byte[] b = new byte[6];
                    ms.Read(b, 0, 6);
                    url = tinyurl + b64(b);
                }
                else if (j == 8)
                {
                    bfr.Clear();
                    while ((j = ms.ReadByte()) != 11)
                        bfr.Add((byte)j);
                    url = utf8(bfr.ToArray());
                }
                else
                    throw new Exception("CWLL reading seems to be broken.");
                Item itm = new Item(name, url);
                dbg("[CWLL]Read {0}...", itm.dbgfmt());
                items.Add(itm);
            }
            dbg("[CWLL]Read file.");
            return new WL(items);
        }

        /// <summary>
        /// Save func for the CWLD-format<para />
        /// For information on the format check the load/read func
        /// </summary>
        public static void cwld_save(WL wl, string file)
        {
            dbg("[CWLD]Saving file...");
            Stream s = File.Open(file, Create, FileAccess.Write);
            s.write(cwld_header);
            s.write(4, 2);
            dbg("[CWLD]Wrote header...");
            DeflateStream d = new DeflateStream(s, CompressionLevel.Optimal, false);
            foreach (Item i in wl)
                i.write_bytes(d, D2);
            d.Close();
            dbg("[CWLD]Saved file.");
        }

        /// <summary>
        /// Read func for the CWLD-format<para />
        /// Name: CWishlistDeflate (A custom binary format compressed with Deflate)<para />
        /// File version: 4 (saved, checked)<para />
        /// Format versions: 1, 2(saved, checked)
        /// </summary>
        public static WL cwld_load(string file)
        {
            dbg("[CWLD]Reading file...");
            Stream raw = File.Open(file, Open, FileAccess.Read);

            byte[] h = new byte[8]; //header
            raw.Read(h, 0, 8);
            int v = -1;

            if (!arrequ(h, cwld_header))
            {
                raw.Close();
                throw new InvalidHeaderException("CWLD", cwld_header, h);
            }
            if (raw.ReadByte() != 4 || (v = raw.ReadByte()) > 2)
            {
                raw.Close();
                throw new Exception("This CWLD file is invalid.");
            }

            DeflateStream d = new DeflateStream(raw, CompressionMode.Decompress, false);
            List<Item> itms = new List<Item>();
            StringBuilder s = new StringBuilder();

            if (v == 1)
            {
                dbg("[CWLD]Initialized, checked header, continuing with v1.");
                bool nus = false; //Name Url Switch
                Item i = new Item();
                char c;
                int j = -1;

                while ((j = d.ReadByte()) != -1)
                    if ((c = utf16(j, d.ReadByte())) == '\u0d0a')
                    {
                        if (nus)
                        {
                            i.url = s.ToString();
                            itms.Add(i);
                            i = new Item();
                        }
                        else
                            i.name = s.ToString();
                        s.Clear();
                        nus = !nus;
                    }
                    else
                        s.Append(c);

                d.Close();

                return new WL(itms);
            }
            else
            {
                dbg("[CWLD]Initialized, checked header, continuing with v2.");
                bool cs = false; //char switch
                bool nus = false; //name url switch
                bool tu = false; //tinyurl
                Item i = new Item();
                int j = -1;
                byte b = 0;

                while((j = d.ReadByte()) != -1)
                    if (j == 11 && !cs)
                    {
                        tu = false;
                        if (!nus)
                        {
                            i.name = s.ToString();
                            nus = true;
                            tu = d.ReadByte() != 0;
                        }
                        else
                        {
                            i.url = s.ToString();
                            itms.Add(i);
                            i = new Item();
                            nus = false;
                        }
                        s.Clear();
                        if (tu)
                            s.Append("http://tinyurl.com/");
                    }
                    else if (tu)
                        s.Append(ascii(j));
                    else
                    {
                        if (cs)
                            s.Append(utf16(b, j));
                        else
                            b = (byte)j;
                        cs = !cs;
                    }
                return new WL(itms);
            }
        }

        /// <summary>
        /// Read func for the CWLU-format<para />
        /// Name: CWishlistUncde (UTF16/Unicode and no longer useless UTF32 in Base64)<para />
        /// File version: 3 (saved, checked)<para />
        /// Format versions: 1 (saved, checked)
        /// </summary>
        static WL cwlu_load(string file)
        {
            dbg("[CWLU]Reading file...");
            ZipArchive zip = ZipFile.Open(file, ZipArchiveMode.Read, ASCII);
            if (zip.read_entry_byte("F") != 3 || zip.read_entry_byte("V") != 1)
                throw new Exception("Invalid CWLU file.");
            XmlReader xml = XmlReader.Create(new StreamReader(zip.GetEntry("W").Open(), Unicode));
            dbg("[CWLU]Initialized ZIP, XML.");
            List<Item> items = new List<Item>();
            while (xml.Read())
                if (xml.Name == "i")
                {
                    Item i = new Item(xml.GetAttribute("n"), xml.GetAttribute("u"));
                    items.Add(i);
                    dbg($"[CWLU]Read {i.dbgfmt()}");
                }
            xml.Close();
            zip.Dispose();
            WL wl = new WL(items);
            dbg("[CWLU]Finished.");
            return wl;
        }

        /// <summary>
        /// Write func for the CWLS-format<para />
        /// For information on the format check the load/read func
        /// </summary>
        public static void write_recents(string file, IEnumerable<string> recents)
        {
            dbg("[CWLS]Writing file...");
            Stream fs = File.Open(file, Create, FileAccess.Write);
            fs.write(cwls_header);
            fs.WriteByte(6);
            dbg("[CWLS]Wrote header.");
            MemoryStream ms = new MemoryStream();
            foreach (string r in recents)
            {
                ms.write(utf8(r));
                ms.WriteByte(11);
                dbg("[CWLS]Wrote \"" + r + "\".");
            }
            ms.Position = 0;
            dbg("[CWLS]Compressing to file...");
            Compress(ms, fs);
            dbg("[CWLS]Compressed to file.");
            fs.Close();
            dbg("[CWLS]Finished.");
        }

        /// <summary>
        /// Read func for the CWLS-format<para />
        /// Name: CWishlists<para />
        /// File version 1 (since v2 more or less saved (magic string CWLS))<para />
        /// Format versions: 1, 2, 3, 4, 5, 6 (saved, checked)
        /// </summary>
        public static List<string> load_recents(string file)
        {
            dbg("[CWLS]Reading file...");
            int v;
            Stream s = File.Open(file, Open, FileAccess.Read);
            if (s.ReadByte() == 80 && s.ReadByte() == 75)
            {
                s.Close();
                using (ZipArchive z = ZipFile.Open(file, ZipArchiveMode.Read, ASCII))
                    v = z.read_entry_byte("V");
            }
            else if (readequals(s, 8, cwls4_header))
            {
                s.Close();
                v = 4;
            }
            else
            {
                s.Seek(4, SeekOrigin.Begin);
                v = s.ReadByte();
                s.Close();
            }
            dbg($"[CWLS]Got version {v}.");
            if (v > 6)
                throw new TooNewRecentsFileException();
            if (v < 4)
                throw new Exception($"CWLSv{v} is deprecated, it's no longer supported by CWL.");
            else if (v == 4)
            {
                dbg("[CWLS]Starting reading with version 4.");
                List<string> r = new List<string>();
                Stream rawfs = File.Open(file, Open, FileAccess.Read);
                rawfs.Seek(10, SeekOrigin.Begin);
                s = new DeflateStream(rawfs, CompressionMode.Decompress, false);
                int i;
                byte[] bfr = new byte[131070]; //ushort.MaxValue * 2 (128KiB)
                while ((i = s.ReadByte()) != -1)
                {
                    int len = (i << 8) | s.ReadByte();
                    s.Read(bfr, 0, len * 2);
                    r.Add(utf16(bfr, len));
                }
                s.Close();
                return r;
            }
            else if (v == 5)
            {
                dbg("[CWLS]Starting reading with version 5.");
                List<string> r = new List<string>();
                FileStream fs = File.Open(file, Open, FileAccess.Read);
                fs.Seek(5, SeekOrigin.Begin);
                MemoryStream ms = new MemoryStream();
                Decompress(fs, ms);
                int i;
                StringBuilder b = new StringBuilder();
                while ((i = ms.ReadByte()) != -1)
                {
                    b.Clear();
                    bool u8 = i != 0;
                    if (u8)
                        while ((i = ms.ReadByte()) != 8 && i != -1)
                            b.Append(utf8(i));
                    else
                        while ((i = ms.ReadByte()) != 0xe5 && i != -1)
                            b.Append(utf16(i, ms.ReadByte()));
                    r.Add(b.ToString());
                }
                return r;
            }
            else
            {
                dbg("[CWLS]Starting reading with version 6.");
                List<string> r = new List<string>();
                FileStream fs = File.Open(file, Open, FileAccess.Read);
                fs.Seek(5, SeekOrigin.Begin);
                MemoryStream ms = new MemoryStream();
                Decompress(fs, ms);
                fs.Close();
                ms.Position = 0;
                int i;
                List<byte> bfr = new List<byte>();
                while ((i = ms.ReadByte()) != -1)
                {
                    if (i != 11)
                        bfr.Add((byte)i);
                    else
                    {
                        r.Add(utf8(bfr.ToArray()));
                        bfr.Clear();
                    }
                }
                ms.Dispose();
                return r;
            }
        }

        static bool readequals(Stream stream, int len, byte[] arr)
        {
            if (len != arr.Length)
                return false;
            byte[] bfr = new byte[len];
            stream.Read(bfr, 0, len);
            return arrequ(bfr, arr);
        }
    }

    class InvalidHeaderException : Exception
    {
        public InvalidHeaderException(string format, byte[] expected, byte[] invalid) :
            this(format, hex(expected), hex(invalid)) { }

        public InvalidHeaderException(string format, string expected, string invalid) :
            base($"This {format}-File's header is not correct, it's expected to be {expected} by the standard, but it's {invalid}.") { }
    }

    class TooNewRecentsFileException : Exception
    {
        public TooNewRecentsFileException() :
            base("The recents-file saved in the AppData is too new for this version of the program, please update.") { }
    }
}
