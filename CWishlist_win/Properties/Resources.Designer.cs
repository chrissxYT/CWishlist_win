﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CWishlist_win.Properties {
    using System;
    
    
    /// <summary>
    ///   Eine stark typisierte Ressourcenklasse zum Suchen von lokalisierten Zeichenfolgen usw.
    /// </summary>
    // Diese Klasse wurde von der StronglyTypedResourceBuilder automatisch generiert
    // -Klasse über ein Tool wie ResGen oder Visual Studio automatisch generiert.
    // Um einen Member hinzuzufügen oder zu entfernen, bearbeiten Sie die .ResX-Datei und führen dann ResGen
    // mit der /str-Option erneut aus, oder Sie erstellen Ihr VS-Projekt neu.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Gibt die zwischengespeicherte ResourceManager-Instanz zurück, die von dieser Klasse verwendet wird.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CWishlist_win.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Überschreibt die CurrentUICulture-Eigenschaft des aktuellen Threads für alle
        ///   Ressourcenzuordnungen, die diese stark typisierte Ressourcenklasse verwenden.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die &lt;cwl_lang&gt;
        ///	&lt;lang code=&quot;de&quot; name=&quot;deutsch&quot; /&gt;
        ///	&lt;translation name=&quot;misc.changelog&quot; type=&quot;str_arr&quot; value=&quot; CWishlist von chrissx bei chrissx Media Inc. Changelog:\
        ///\
        ///Version 6.0.0:\
        ///-Der CWishlistDeflate v1 Standart wurde hinzugefügt\
        ///-Das Speichern und Laden der Listen wurde bzgl. Performance und RAM-Verbrauch verbessert\
        ///\
        ///Version 5.0.1:\
        ///-Hotfix für einen Bug, bei dem die recents Datei 1. nicht geladen und 2. kaputt war.\
        ///\
        ///Version 5.0.0:\
        ///-Sprachdateien können jetzt erstellt und hinzugefügt we [Rest der Zeichenfolge wurde abgeschnitten]&quot;; ähnelt.
        /// </summary>
        internal static string de_lang_xml {
            get {
                return ResourceManager.GetString("de_lang_xml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Zeichenfolge, die &lt;cwl_lang&gt;
        ///	&lt;lang code=&quot;en&quot; name=&quot;english&quot; /&gt;
        ///	&lt;translation name=&quot;misc.changelog&quot; type=&quot;str_arr&quot; value=&quot; CWishlist by chrissx @ chrissx Media Inc. Changelog:\
        ///\
        ///Version 6.0.0:\
        ///-Added the new CWishlistDeflate v1 standard which reduces your file size by up to 80%\
        ///-Improved loading and saving of the lists performance- and memorywise\
        ///\
        ///Version 5.0.1:\
        ///-Hotfix for broken and not loaded at all recents-files\
        ///\
        ///Version 5.0.0:\
        ///-Language-files can now be created and put into this program.\
        ///-A little [Rest der Zeichenfolge wurde abgeschnitten]&quot;; ähnelt.
        /// </summary>
        internal static string en_lang_xml {
            get {
                return ResourceManager.GetString("en_lang_xml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Ressource vom Typ System.Byte[].
        /// </summary>
        internal static byte[] file_replace_32 {
            get {
                object obj = ResourceManager.GetObject("file_replace_32", resourceCulture);
                return ((byte[])(obj));
            }
        }
        
        /// <summary>
        ///   Sucht eine lokalisierte Ressource vom Typ System.Byte[].
        /// </summary>
        internal static byte[] file_replace_64 {
            get {
                object obj = ResourceManager.GetObject("file_replace_64", resourceCulture);
                return ((byte[])(obj));
            }
        }
    }
}
