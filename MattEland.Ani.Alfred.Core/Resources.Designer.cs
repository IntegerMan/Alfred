﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MattEland.Ani.Alfred.Core {
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
    public class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MattEland.Ani.Alfred.Core.Resources", typeof(Resources).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} was already offline when told to shut down..
        /// </summary>
        public static string AlfredComponent_ShutdownAlreadyOffline {
            get {
                return ResourceManager.GetString("AlfredComponent_ShutdownAlreadyOffline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Alfred Control.
        /// </summary>
        public static string AlfredControlSubSystem_Name {
            get {
                return ResourceManager.GetString("AlfredControlSubSystem_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Alfred Provider has not been set.
        /// </summary>
        public static string AlfredCoreModule_AlfredNotSet {
            get {
                return ResourceManager.GetString("AlfredCoreModule_AlfredNotSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} is currently {1}.
        /// </summary>
        public static string AlfredCoreModule_AlfredStatusText {
            get {
                return ResourceManager.GetString("AlfredCoreModule_AlfredStatusText", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to System Power.
        /// </summary>
        public static string AlfredCoreModule_Name {
            get {
                return ResourceManager.GetString("AlfredCoreModule_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} was offline when told to update..
        /// </summary>
        public static string AlfredItemOfflineButToldToUpdate {
            get {
                return ResourceManager.GetString("AlfredItemOfflineButToldToUpdate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} was already online when told to initialize..
        /// </summary>
        public static string AlfredModule_InitializeAlreadyOnline {
            get {
                return ResourceManager.GetString("AlfredModule_InitializeAlreadyOnline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Modules may not contain null entries..
        /// </summary>
        public static string AlfredProvider_AddModules_ErrorNullModule {
            get {
                return ResourceManager.GetString("AlfredProvider_AddModules_ErrorNullModule", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Alfred must be offline in order to add components..
        /// </summary>
        public static string AlfredProvider_AssertMustBeOffline_ErrorNotOffline {
            get {
                return ResourceManager.GetString("AlfredProvider_AssertMustBeOffline_ErrorNotOffline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Alfred.
        /// </summary>
        public static string AlfredProvider_Name {
            get {
                return ResourceManager.GetString("AlfredProvider_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Alfred must be online in order to update modules..
        /// </summary>
        public static string AlfredProvider_Update_ErrorMustBeOnline {
            get {
                return ResourceManager.GetString("AlfredProvider_Update_ErrorMustBeOnline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} is now offline..
        /// </summary>
        public static string AlfredStatusController_ComponentOffline {
            get {
                return ResourceManager.GetString("AlfredStatusController_ComponentOffline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Alfred is now Online..
        /// </summary>
        public static string AlfredStatusController_Initialize_AlfredOnline {
            get {
                return ResourceManager.GetString("AlfredStatusController_Initialize_AlfredOnline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Instructed to initialize but system is already online.
        /// </summary>
        public static string AlfredStatusController_Initialize_ErrorAlreadyOnline {
            get {
                return ResourceManager.GetString("AlfredStatusController_Initialize_ErrorAlreadyOnline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Initializing subsystems and modules....
        /// </summary>
        public static string AlfredStatusController_Initialize_Initializing {
            get {
                return ResourceManager.GetString("AlfredStatusController_Initialize_Initializing", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Initilization Completed; notifying modules and subsystems..
        /// </summary>
        public static string AlfredStatusController_Initialize_InitilizationCompleted {
            get {
                return ResourceManager.GetString("AlfredStatusController_Initialize_InitilizationCompleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Alfred.Initialize.
        /// </summary>
        public static string AlfredStatusController_Initialize_LogHeader {
            get {
                return ResourceManager.GetString("AlfredStatusController_Initialize_LogHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} is now initialized..
        /// </summary>
        public static string AlfredStatusController_InitializeComponentInitialized {
            get {
                return ResourceManager.GetString("AlfredStatusController_InitializeComponentInitialized", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Initializing {0}.
        /// </summary>
        public static string AlfredStatusController_InitializingComponent {
            get {
                return ResourceManager.GetString("AlfredStatusController_InitializingComponent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Shut down completed..
        /// </summary>
        public static string AlfredStatusController_Shutdown_Completed {
            get {
                return ResourceManager.GetString("AlfredStatusController_Shutdown_Completed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Instructed to shut down but system is already offline.
        /// </summary>
        public static string AlfredStatusController_Shutdown_ErrorAlreadyOffline {
            get {
                return ResourceManager.GetString("AlfredStatusController_Shutdown_ErrorAlreadyOffline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Instructed to shut down but system is already shutting down.
        /// </summary>
        public static string AlfredStatusController_Shutdown_ErrorAlreadyTerminating {
            get {
                return ResourceManager.GetString("AlfredStatusController_Shutdown_ErrorAlreadyTerminating", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Alfred.Shutdown.
        /// </summary>
        public static string AlfredStatusController_Shutdown_LogHeader {
            get {
                return ResourceManager.GetString("AlfredStatusController_Shutdown_LogHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Shutting down....
        /// </summary>
        public static string AlfredStatusController_Shutdown_Shutting_down {
            get {
                return ResourceManager.GetString("AlfredStatusController_Shutdown_Shutting_down", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Shutting down {0}.
        /// </summary>
        public static string AlfredStatusController_ShuttingDownComponent {
            get {
                return ResourceManager.GetString("AlfredStatusController_ShuttingDownComponent", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No Subsystems Detected.
        /// </summary>
        public static string AlfredSubSystemListModule_NoSubsystemsDetected {
            get {
                return ResourceManager.GetString("AlfredSubSystemListModule_NoSubsystemsDetected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Shouldn&apos;t we be heading to bed soon?.
        /// </summary>
        public static string AlfredTimeModule_AlfredTimeModule_BedtimeNagMessage {
            get {
                return ResourceManager.GetString("AlfredTimeModule_AlfredTimeModule_BedtimeNagMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Time and Date.
        /// </summary>
        public static string AlfredTimeModule_Name {
            get {
                return ResourceManager.GetString("AlfredTimeModule_Name", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The time is now {0:t}.
        /// </summary>
        public static string AlfredTimeModule_Update_CurrentTimeDisplayString {
            get {
                return ResourceManager.GetString("AlfredTimeModule_Update_CurrentTimeDisplayString", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No Pages Detected.
        /// </summary>
        public static string NoPagesDetected {
            get {
                return ResourceManager.GetString("NoPagesDetected", resourceCulture);
            }
        }
    }
}
