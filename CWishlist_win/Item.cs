﻿using System.IO;
using System.Text;
using static CWishlist_win.CLinq;
using static CWishlist_win.Consts;

namespace CWishlist_win
{
    public class Item
    {
        public Item()
        {
            name = url = null;
        }

        public Item(string name, string url)
        {
            this.name = name;
            this.url = url;
        }

        public string name;
        public string url;

        public override string ToString() => name != "" ? name : @"[/unnamed item\]";

        public override bool Equals(object obj) => (obj is Item) ? ((Item)obj).name == name && ((Item)obj).url == url : false;

        public bool Equals(Item i) => i.name == name && i.url == url;

        public override int GetHashCode()
        {
            return unchecked(name.GetHashCode() * url.GetHashCode());
        }

        public static bool operator ==(Item first, Item second) => first.name == second.name && first.url == second.url;

        public static bool operator !=(Item first, Item second) => first.name != second.name || first.url != second.url;

        public static bool operator <(Item first, Item second) => first.name.CompareTo(second.name) == 1;

        public static bool operator >(Item first, Item second) => first.name.CompareTo(second.name) == 0;

        public static bool operator <=(Item first, Item second) => first.name.CompareTo(second.name) == 1 || first.name == second.name;

        public static bool operator >=(Item first, Item second) => first.name.CompareTo(second.name) == 0 || first.name == second.name;

        public static implicit operator string(Item i) => i.ToString();

        public static implicit operator long(Item i) => i.LongLength;

        public static implicit operator int(Item i) => i.Length;

        public int Length
        {
            get => name.Length + url.Length;
        }

        public long LongLength
        {
            get => (long)url.Length + (long)name.Length;
        }

        public long MemoryLength
        {
            get => LongLength * 2;
        }

        public byte[] bytes(string format)
        {
            MemoryStream ms = new MemoryStream();
            write_bytes(ms, format);
            ms.Close();
            return ms.ToArray();
        }

        public void write_bytes(Stream s, string format)
        {
            if (format == "D1")
            {
                s.write(utf16(name));
                s.write(10, 13);
                s.write(utf16(url));
                s.write(10, 13);
            }
            else if (format == "D2")
            {
                s.write(utf16(name));
                s.write(11);
                if (url.StartsWith("http://tinyurl.com/"))
                {
                    s.write(1);
                    s.write(ascii(url.Substring(19)));
                }
                else
                {
                    s.write(0);
                    s.write(utf16(url));
                }
                s.write(11);
            }
            else if (format.StartsWith("L1"))
            {
                s.write(utf8(name));
                if (url.StartsWith("http://tinyurl.com/"))
                {
                    s.WriteByte(11);
                    s.write(b64(url.Substring(19)));
                }
                else
                {
                    s.WriteByte(8);
                    s.write(utf8(url));
                    s.WriteByte(11);
                }
            }
        }
    }
}
