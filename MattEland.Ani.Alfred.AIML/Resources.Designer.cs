﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MattEland.Ani.Alfred.Chat {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MattEland.Ani.Alfred.Chat.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Last Log Message:{0}.
        /// </summary>
        internal static string ChatErrorLastLogMessageFormat {
            get {
                return ResourceManager.GetString("ChatErrorLastLogMessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Chat.Input.
        /// </summary>
        internal static string ChatInputHeader {
            get {
                return ResourceManager.GetString("ChatInputHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Chat.
        /// </summary>
        internal static string ChatModuleName {
            get {
                return ResourceManager.GetString("ChatModuleName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Chat.Output.
        /// </summary>
        internal static string ChatOutputHeader {
            get {
                return ResourceManager.GetString("ChatOutputHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Chat.Processing.
        /// </summary>
        internal static string ChatProcessingHeader {
            get {
                return ResourceManager.GetString("ChatProcessingHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Batman.
        /// </summary>
        internal static string ChatUserName {
            get {
                return ResourceManager.GetString("ChatUserName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to I&apos;m afraid I can&apos;t help you with that, sir..
        /// </summary>
        internal static string DefaultFailureResponseText {
            get {
                return ResourceManager.GetString("DefaultFailureResponseText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error initializing chat: {0}.
        /// </summary>
        internal static string ErrorInitializingChat {
            get {
                return ResourceManager.GetString("ErrorInitializingChat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error parsing OOB command: {0} for command: {1}.
        /// </summary>
        internal static string ErrorParsingCommand {
            get {
                return ResourceManager.GetString("ErrorParsingCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Greetings sir. What can I do for you?.
        /// </summary>
        internal static string InitialGreeting {
            get {
                return ResourceManager.GetString("InitialGreeting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Hi.
        /// </summary>
        internal static string InitialGreetingText {
            get {
                return ResourceManager.GetString("InitialGreetingText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No end tag for OOB command: {0}.
        /// </summary>
        internal static string NoEndTagForOobCommand {
            get {
                return ResourceManager.GetString("NoEndTagForOobCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Empty path to a settings file.
        /// </summary>
        internal static string NoSettingsPathError {
            get {
                return ResourceManager.GetString("NoSettingsPathError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to OOB root element was not an alfred XElement.
        /// </summary>
        internal static string OobRootNotAlfredXElement {
            get {
                return ResourceManager.GetString("OobRootNotAlfredXElement", resourceCulture);
            }
        }
    }
}
