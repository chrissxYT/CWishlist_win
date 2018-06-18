﻿using System.IO;
using System.IO.Compression;

namespace CWishlist_win
{
    static class Deflate
    {
        public static byte[] compress(byte[] b)
        {
            MemoryStream s = new MemoryStream(b, false);
            MemoryStream d = new MemoryStream();
            DeflateStream ds = new DeflateStream(d, CompressionMode.Compress);
            s.CopyTo(ds, short.MaxValue);
            ds.Close();
            s.Close();
            ds.Dispose();
            s.Dispose();
            d.Close();
            return d.ToArray();
        }

        public static byte[] decompress(byte[] b)
        {
            MemoryStream s = new MemoryStream(b, false);
            MemoryStream d = new MemoryStream();
            DeflateStream ds = new DeflateStream(d, CompressionMode.Decompress);
            s.CopyTo(ds, short.MaxValue);
            ds.Close();
            s.Close();
            ds.Dispose();
            s.Dispose();
            d.Close();
            return d.ToArray();
        }
    }
}
