﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18213
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GetWelds {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class StringResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal StringResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    var temp = new global::System.Resources.ResourceManager("GetWelds.StringResources", typeof(StringResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
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
        ///   Looks up a localized string similar to EnterZone.
        /// </summary>
        internal static string EnterZone {
            get {
                return ResourceManager.GetString("EnterZone", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        internal static string FanucLinWeld {
            get {
                return ResourceManager.GetString("FanucLinWeld", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to STYLE[0-9]+.LS.
        /// </summary>
        internal static string FANUCMAINPROGRAMREGEX {
            get {
                return ResourceManager.GetString("FANUCMAINPROGRAMREGEX", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [^!]CALL\s([^\s;]+).
        /// </summary>
        internal static string FanucProgramCall {
            get {
                return ResourceManager.GetString("FanucProgramCall", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        internal static string FanucPTPWeld {
            get {
                return ResourceManager.GetString("FanucPTPWeld", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        internal static string FanucSealRegex {
            get {
                return ResourceManager.GetString("FanucSealRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [0-9:]*L P\[[0-9:;A-Z-a-z[;-]+]* [0-9]+mm/sec [^ ]* RTCP\s;.
        /// </summary>
        internal static string FanucStudRegex {
            get {
                return ResourceManager.GetString("FanucStudRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to .
        /// </summary>
        internal static string FanucWeldRegex {
            get {
                return ResourceManager.GetString("FanucWeldRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to am.ini.
        /// </summary>
        internal static string KUKA_am {
            get {
                return ResourceManager.GetString("KUKA_am", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ;ENDFOLD.
        /// </summary>
        internal static string KUKAEndfold {
            get {
                return ResourceManager.GetString("KUKAEndfold", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ;FOLD.
        /// </summary>
        internal static string KUKAFold {
            get {
                return ResourceManager.GetString("KUKAFold", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to RobName=.
        /// </summary>
        internal static string KukaRobname {
            get {
                return ResourceManager.GetString("KukaRobname", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ;FOLD SG?.P_(PTP|LIN) ([^,]+, Vel=[0-9]+)[^,]*, Accl=[0-9]+ %, Gun=([0-9X]*), ContCLS=\[([^\]]*)\], ContOPN=\[([^\]]*)\], WeldSchd=([0-9]+), WeldId=([0-9]+), Part=([0-9.]+) mm, Force=([0-9]+) lbs, Eqlzr=([0-9]+), EqRst=([0-9]+) mm, Tool\[([0-9]+)\], Base\[([0-9]+)\], ExtTCP\[([^\]]+)\].
        /// </summary>
        internal static string KUKAServoWeldLIN {
            get {
                return ResourceManager.GetString("KUKAServoWeldLIN", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ;FOLD SG.P_.
        /// </summary>
        internal static string KUKAServoWeldPTP {
            get {
                return ResourceManager.GetString("KUKAServoWeldPTP", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ;FOLD StdWld.SLIN.
        /// </summary>
        internal static string KUKAStudWeldLIN {
            get {
                return ResourceManager.GetString("KUKAStudWeldLIN", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ;FOLD StdWld.PTP.
        /// </summary>
        internal static string KUKAStudWeldPTP {
            get {
                return ResourceManager.GetString("KUKAStudWeldPTP", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Not Used.
        /// </summary>
        internal static string NotUsed {
            get {
                return ResourceManager.GetString("NotUsed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select Root Directory for all robots.
        /// </summary>
        internal static string SelectRootDirectory {
            get {
                return ResourceManager.GetString("SelectRootDirectory", resourceCulture);
            }
        }
    }
}
