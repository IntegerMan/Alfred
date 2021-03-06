﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MattEland.Ani.Alfred.Chat.Aiml {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MattEland.Ani.Alfred.Chat.Aiml.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to The category with a pattern: {0} has an empty template tag. Aborting..
        /// </summary>
        internal static string AddCategoryErrorEmptyTemplateTag {
            get {
                return ResourceManager.GetString("AddCategoryErrorEmptyTemplateTag", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Processing AIML file: {0}.
        /// </summary>
        internal static string AimlLoaderProcessingFile {
            get {
                return ResourceManager.GetString("AimlLoaderProcessingFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &apos;{0}&apos; resulted in an invalid XmlElement..
        /// </summary>
        internal static string AimlTagHandlerBuildElementBadXml {
            get {
                return ResourceManager.GetString("AimlTagHandlerBuildElementBadXml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not redirect to &apos;{0}&apos; due to bad XML: {1}.
        /// </summary>
        internal static string AimlTagHandlerDoRedirectBadXml {
            get {
                return ResourceManager.GetString("AimlTagHandlerDoRedirectBadXml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Attempted to load a new category with an empty pattern where the directoryPath = {0} and template = {1} produced by a category..
        /// </summary>
        internal static string ChatEngineAddCategoryErrorNoPath {
            get {
                return ResourceManager.GetString("ChatEngineAddCategoryErrorNoPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to I&apos;m sorry but I don&apos;t understand. Can you try asking differently?.
        /// </summary>
        internal static string ChatEngineDontUnderstandFallback {
            get {
                return ResourceManager.GetString("ChatEngineDontUnderstandFallback", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The ChatEngine could not find any response for the input: {0} with the path(s): {1}{2} from the user with an id: {3}.
        /// </summary>
        internal static string ChatEngineErrorCouldNotFindResponse {
            get {
                return ResourceManager.GetString("ChatEngineErrorCouldNotFindResponse", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {0} is not currently available to talk..
        /// </summary>
        internal static string ChatEngineNotAcceptingUserInputMessage {
            get {
                return ResourceManager.GetString("ChatEngineNotAcceptingUserInputMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Your request has timed out. Please try again or phrase your sentence differently..
        /// </summary>
        internal static string ChatEngineRequestTimedOut {
            get {
                return ResourceManager.GetString("ChatEngineRequestTimedOut", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A chat message is required to interact with the system..
        /// </summary>
        internal static string ChatErrorNoMessage {
            get {
                return ResourceManager.GetString("ChatErrorNoMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A problem was encountered when trying to process the input: {0} with the template: &quot;{1}&quot;: {2}.
        /// </summary>
        internal static string ChatProcessNodeError {
            get {
                return ResourceManager.GetString("ChatProcessNodeError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Gossip from user &apos;{0}&apos;: &apos;{1}&apos;.
        /// </summary>
        internal static string GossipTagHandleLogMessage {
            get {
                return ResourceManager.GetString("GossipTagHandleLogMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An input tag with a badly formed index ({0}) was encountered processing the input: {1}.
        /// </summary>
        internal static string InputErrorBadlyFormedIndex {
            get {
                return ResourceManager.GetString("InputErrorBadlyFormedIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An input tag ({0}) caused an overflow processing the input: {1}.
        /// </summary>
        internal static string InputErrorOverflow {
            get {
                return ResourceManager.GetString("InputErrorOverflow", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The javascript tag is not supported.
        /// </summary>
        internal static string JavaScriptNotSupportedMessage {
            get {
                return ResourceManager.GetString("JavaScriptNotSupportedMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not find learning file at path: {1}.
        /// </summary>
        internal static string LearnErrorFileNotFound {
            get {
                return ResourceManager.GetString("LearnErrorFileNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not load learning file due to a {0} exception: {1} on file: {2}.
        /// </summary>
        internal static string LearnErrorIOException {
            get {
                return ResourceManager.GetString("LearnErrorIOException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not load learning file due to an invalid format on file {0}.
        /// </summary>
        internal static string LearnErrorNotSupportedException {
            get {
                return ResourceManager.GetString("LearnErrorNotSupportedException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not interact with file &apos;{0}&apos; due to a security exception: {1}.
        /// </summary>
        internal static string LearnErrorSecurityException {
            get {
                return ResourceManager.GetString("LearnErrorSecurityException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not interact with file &apos;{0}&apos; due to an access exception: {1}.
        /// </summary>
        internal static string LearnErrorUnauthorizedException {
            get {
                return ResourceManager.GetString("LearnErrorUnauthorizedException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not load learning file due to an XML exception: {0} on file: {1}.
        /// </summary>
        internal static string LearnErrorXmlException {
            get {
                return ResourceManager.GetString("LearnErrorXmlException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The path to the configuration files must be provided..
        /// </summary>
        internal static string LibrarianLoadSettingsErrorNoConfigDirectory {
            get {
                return ResourceManager.GetString("LibrarianLoadSettingsErrorNoConfigDirectory", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Path to settings must be provided..
        /// </summary>
        internal static string LibrarianLoadSettingsErrorNoPath {
            get {
                return ResourceManager.GetString("LibrarianLoadSettingsErrorNoPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ChatEngine.AimlDirectoryPath was not set.
        /// </summary>
        internal static string LoadAimlErrorNoDirectoryPath {
            get {
                return ResourceManager.GetString("LoadAimlErrorNoDirectoryPath", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Xml Error Encountered: {0}.
        /// </summary>
        internal static string LoadSettingsXmlXmlException {
            get {
                return ResourceManager.GetString("LoadSettingsXmlXmlException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An input tag with a badly formed index ({0}) was encountered processing the input: {1}.
        /// </summary>
        internal static string OutputTagHandlerProcessChangeInvalidIndex {
            get {
                return ResourceManager.GetString("OutputTagHandlerProcessChangeInvalidIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Request timeout. User: {0} raw input: &quot;{1}&quot;.
        /// </summary>
        internal static string RequestTimedOut {
            get {
                return ResourceManager.GetString("RequestTimedOut", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not load settings from file &apos;{0}&apos; due to a security concern: {1}.
        /// </summary>
        internal static string SettignsLoadErrorSecurity {
            get {
                return ResourceManager.GetString("SettignsLoadErrorSecurity", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not load settings from non-existent file &apos;{0}&apos;.
        /// </summary>
        internal static string SettingsLoadErrorFileNotFound {
            get {
                return ResourceManager.GetString("SettingsLoadErrorFileNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not find a settings file at the given path.
        /// </summary>
        internal static string SettingsLoadErrorFileNotFoundException {
            get {
                return ResourceManager.GetString("SettingsLoadErrorFileNotFoundException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to IO exception encountered loading settings from file &apos;{0}&apos;: {1}.
        /// </summary>
        internal static string SettingsLoadErrorIOException {
            get {
                return ResourceManager.GetString("SettingsLoadErrorIOException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to pathToSettings did not have a value.
        /// </summary>
        internal static string SettingsLoadErrorNoPathToSettings {
            get {
                return ResourceManager.GetString("SettingsLoadErrorNoPathToSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not load settings from file without authorization from operating system &apos;{0}&apos;: {1}.
        /// </summary>
        internal static string SettingsLoadErrorUnauthorized {
            get {
                return ResourceManager.GetString("SettingsLoadErrorUnauthorized", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to XML error parsing settings file &apos;{0}&apos;:{1}.
        /// </summary>
        internal static string SettingsLoadErrorXml {
            get {
                return ResourceManager.GetString("SettingsLoadErrorXml", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to InputStar out of bounds reference caused by input: {0}.
        /// </summary>
        internal static string StarErrorBadIndex {
            get {
                return ResourceManager.GetString("StarErrorBadIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A star tag tried to reference an empty InputStar collection when processing the input: {0}.
        /// </summary>
        internal static string StarErrorNoInputStarElements {
            get {
                return ResourceManager.GetString("StarErrorNoInputStarElements", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The SystemTagHandler tag is not implemented in this ChatEngine.
        /// </summary>
        internal static string SystemNotImplemented {
            get {
                return ResourceManager.GetString("SystemNotImplemented", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dynamic tag handler for {0} encountered a missing method error instantiating type {1}: {2}
        /// Disabling this handler..
        /// </summary>
        internal static string TagHandlerFactoryBuildTagHandlerDynamicMissingMethod {
            get {
                return ResourceManager.GetString("TagHandlerFactoryBuildTagHandlerDynamicMissingMethod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dynamic tag handler for {0} encountered an invocation error instantiating type {1}: {2}
        /// Disabling this handler..
        /// </summary>
        internal static string TagHandlerFactoryBuildTagHandlerDynamicTargetInvocation {
            get {
                return ResourceManager.GetString("TagHandlerFactoryBuildTagHandlerDynamicTargetInvocation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Dynamic tag handler for {0} instantiated an instance of type {1} that was not an AimlTagHandler. Disabling this handler..
        /// </summary>
        internal static string TagHandlerFactoryBuildTagHandlerDynamicWrongType {
            get {
                return ResourceManager.GetString("TagHandlerFactoryBuildTagHandlerDynamicWrongType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An input tag with a badly formed index ({0}) was encountered processing the input: {1}.
        /// </summary>
        internal static string ThatStarTagHandlerProcessChangeInvalidIndex {
            get {
                return ResourceManager.GetString("ThatStarTagHandlerProcessChangeInvalidIndex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Tried to process a topicstar node when query&apos;s TopicStar collection was empty: {0}.
        /// </summary>
        internal static string TopicStarErrorNoItems {
            get {
                return ResourceManager.GetString("TopicStarErrorNoItems", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An input tag with an out of range index ({0}) was encountered processing the input: {1}.
        /// </summary>
        internal static string TopicStarErrorOutOfRange {
            get {
                return ResourceManager.GetString("TopicStarErrorOutOfRange", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The id cannot be empty.
        /// </summary>
        internal static string UserCtorNullId {
            get {
                return ResourceManager.GetString("UserCtorNullId", resourceCulture);
            }
        }
    }
}
