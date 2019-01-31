# A tour through the CWishlist_win repo
## CWishlist_win
This is the main project here, it contains the whole frontend, a
non-standard version of libcwlcs, the thread manager and the browser
backends.
## LegacyFormats
This C# DLL contains all the really old CWL formats, because currently
only CWLU, CWLD and CWLL are supported, it has CWL and CWLB, CWLS 1-3
is also in there.
## file_replace
The file executed by the plugin updater to replace the plugin DLL.
## test_plugin
A plugin to test the beta plugin functionalities of CWL.
## cwla_benchmarks
This is a little tool to compress wishlists into many different PAQ
versions to test which one is the best.
## binutils
This is a collection of all utils we ever need, they are supposed to
be `using static`ed and their naming scheme is also similar to the
C/C++ std library.
## 7ZipLib
A customized version of the SevenZip "API" by Igor Pavlov for LZMA.
## BetterWindows
The new window system for the frontend, it's not done nor working...
## BetterWindows.Test
A few tests for BetterWindows that show that it's broken.