﻿namespace CWishlist_win
{
    public static class Consts
    {
        public static byte[] cwld_header { get; } = new byte[8] { 67, 87, 76, 68, 13, 10, 26, 10 }; //C W L D CR LF EOF LF
        public static byte[] cwls_header { get; } = new byte[8] { 67, 87, 76, 83, 13, 10, 26, 10 }; //C W L S CR LF EOF LF
        public static byte[] cwll_header { get; } = new byte[4] { 67, 87, 76, 76 }; //CWLL

        public static string tinyurl_api { get; } = "http://tinyurl.com/api-create.php?url=";
        
        //all these values are used after a unicode string (name), so we need to use the private use zone (e000-f8ff)
        public static byte cwll_is_tinyurl { get; } = 0xe0;
        public static byte cwll_is_https_www_utf8 { get; } = 0xe1;
        public static byte cwll_is_http_www_utf8 { get; } = 0xe2;
        public static byte cwll_is_https_utf8 { get; } = 0xe3;
        public static byte cwll_is_http_utf8 { get; } = 0xe4;
        public static byte cwll_no_protocol_utf8 { get; } = 0xe5;
        public static byte cwll_is_https_www_utf16 { get; } = 0xe6;
        public static byte cwll_is_http_www_utf16 { get; } = 0xe7;
        public static byte cwll_is_https_utf16 { get; } = 0xe8;
        public static byte cwll_is_http_utf16 { get; } = 0xe9;
        public static byte cwll_no_protocol_utf16 { get; } = 0xea;

        //this is also used after a unicode string (the url, maybe without the protocol), so we need to use the private use zone once more
        public static byte cwll_item_end_utf16 { get; } = 0xe6;
        public static byte cwll_item_end_utf8 { get; } = 23;

        public static string http { get; } = "http://";
        public static string https { get; } = "https://";
        public static string http_www { get; } = "http://www.";
        public static string https_www { get; } = "https://www.";
    }
}
