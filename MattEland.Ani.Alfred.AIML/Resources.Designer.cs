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
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///
        ///&lt;!-- 
        ///Copyright (c) 2015 Matt Eland
        ///
        ///This provides handling for time and date queries
        ///--&gt;
        ///
        ///&lt;aiml&gt;
        ///
        ///  &lt;!-- Time Handling --&gt;
        ///
        ///  &lt;category&gt;
        ///    &lt;pattern&gt;TIME&lt;/pattern&gt;
        ///    &lt;template id=&quot;tmp_time&quot;&gt;
        ///      &lt;random&gt;
        ///        &lt;li&gt;The time is now &lt;/li&gt;
        ///        &lt;li&gt;It is currently &lt;/li&gt;
        ///        &lt;li&gt;It is now &lt;/li&gt;
        ///      &lt;/random&gt;
        ///      &lt;date format=&quot;t&quot; /&gt;
        ///    &lt;/template&gt;
        ///  &lt;/category&gt;
        ///
        ///  &lt;!-- Time Redirects --&gt;
        ///  &lt;category&gt;
        ///    &lt;pattern&gt;What TIME is it [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string AimlCoreDateTime {
            get {
                return ResourceManager.GetString("AimlCoreDateTime", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///
        ///&lt;!-- 
        ///Copyright (c) 2015 Matt Eland
        ///
        ///This provides handling for the Goodbye, Shutdown and Status verbs
        ///--&gt;
        ///
        ///&lt;aiml&gt;
        ///
        ///  &lt;!-- Goodbye --&gt;
        ///  &lt;category&gt;
        ///    &lt;pattern&gt;GOODBYE&lt;/pattern&gt;
        ///    &lt;template id=&quot;tmp_bye&quot;&gt;
        ///      &lt;random&gt;
        ///        &lt;li&gt;Goodbye!&lt;/li&gt;
        ///        &lt;li&gt;Bye.&lt;/li&gt;
        ///        &lt;li&gt;I&apos;ll see you later.&lt;/li&gt;
        ///        &lt;li&gt;Terminating now.&lt;/li&gt;
        ///        &lt;li&gt;Shutting down.&lt;/li&gt;
        ///        &lt;li&gt;I do hate powering off. Very well, then.&lt;/li&gt;
        ///      &lt;/random&gt;      [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string AimlCorePower {
            get {
                return ResourceManager.GetString("AimlCorePower", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///
        ///&lt;!-- 
        ///Copyright (c) 2015 Matt Eland
        ///
        ///This provides handling for searching via AIML
        ///--&gt;
        ///
        ///&lt;aiml&gt;
        ///
        ///  &lt;!-- Time Handling --&gt;
        ///
        ///  &lt;category&gt;
        ///    &lt;pattern&gt;TIME&lt;/pattern&gt;
        ///    &lt;template id=&quot;tmp_time&quot;&gt;
        ///      &lt;random&gt;
        ///        &lt;li&gt;The time is now &lt;/li&gt;
        ///        &lt;li&gt;It is currently &lt;/li&gt;
        ///        &lt;li&gt;It is now &lt;/li&gt;
        ///      &lt;/random&gt;
        ///      &lt;date format=&quot;t&quot; /&gt;
        ///    &lt;/template&gt;
        ///  &lt;/category&gt;
        ///
        ///  &lt;!-- Time Redirects --&gt;
        ///  &lt;category&gt;
        ///    &lt;pattern&gt;What TIME is it&lt;/p [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string AimlCoreSearch {
            get {
                return ResourceManager.GetString("AimlCoreSearch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///
        ///&lt;!-- 
        ///Copyright (c) 2015 Matt Eland
        ///
        ///This provides fallback functions to address any input not handled by other modules
        ///--&gt;
        ///
        ///&lt;aiml&gt;
        ///
        ///  &lt;!-- Generic Fallback --&gt;
        ///  &lt;category&gt;
        ///    &lt;pattern&gt;*&lt;/pattern&gt;
        ///    &lt;template id=&quot;tmp_fallback&quot;&gt;
        ///      &lt;random&gt;
        ///        &lt;li&gt;I&apos;m sorry but I&apos;m not yet able to understand that command. For a list of commands, type &quot;Help&quot;.&lt;/li&gt;
        ///      &lt;/random&gt;
        ///    &lt;/template&gt;
        ///  &lt;/category&gt;
        ///
        ///&lt;/aiml&gt;.
        /// </summary>
        internal static string AimlFallback {
            get {
                return ResourceManager.GetString("AimlFallback", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///
        ///&lt;!-- 
        ///Copyright (c) 2015 Matt Eland
        ///
        ///This provides handling for sentences involving God, primarily in conjuction with &quot;THANK&quot;
        ///--&gt;
        ///
        ///&lt;aiml&gt;
        ///
        ///  &lt;!-- Thank God special case --&gt;
        ///  &lt;category&gt;
        ///    &lt;pattern&gt;THANK GOD&lt;/pattern&gt;
        ///    &lt;template id=&quot;tmp_thank_god&quot;&gt;
        ///      &lt;random&gt;
        ///        &lt;li&gt;He is good.&lt;/li&gt;
        ///        &lt;li&gt;My programmer has found He&apos;s always there.&lt;/li&gt;
        ///        &lt;li&gt;He, unlike me, will never leave you wanting.&lt;/li&gt;
        ///      &lt;/random&gt;
        ///    &lt;/template&gt;
        ///  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string AimlGod {
            get {
                return ResourceManager.GetString("AimlGod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///
        ///&lt;!-- 
        ///Copyright (c) 2015 Matt Eland
        ///
        ///This provides generic greeting capabilities both in startup greeting and in response to &quot;HELLO&quot; keywords 
        ///--&gt;
        ///
        ///&lt;aiml&gt;
        ///
        ///  &lt;!-- Handle Greeting Functions --&gt;
        ///  &lt;category&gt;
        ///    &lt;pattern&gt;HELLO&lt;/pattern&gt;
        ///    &lt;template id=&quot;tmp_hi&quot;&gt;
        ///      &lt;random&gt;
        ///        &lt;li&gt;Hello, my name is Alfred.&lt;/li&gt;
        ///        &lt;li&gt;Hi there. I&apos;m Alfred.&lt;/li&gt;
        ///        &lt;li&gt;My name is Alfred and it&apos;s good to see you.&lt;/li&gt;
        ///      &lt;/random&gt;
        ///    &lt;/template&gt;
        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string AimlGreeting {
            get {
                return ResourceManager.GetString("AimlGreeting", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///
        ///&lt;!-- 
        ///Copyright (c) 2015 Matt Eland
        ///
        ///This provides functionality for the &quot;HELP&quot; command
        ///--&gt;
        ///
        ///&lt;aiml&gt;
        ///
        ///  &lt;!-- Generic Fallback --&gt;
        ///  &lt;category&gt;
        ///    &lt;pattern&gt;HELP&lt;/pattern&gt;
        ///    &lt;template id=&quot;tmp_help&quot;&gt;
        ///      For help on commands, type &quot;Help Commands&quot;.
        ///      For a list of questions you can ask me type &quot;Help Questions&quot;.
        ///      For a list of other things I respond to, type &quot;Help Other&quot;
        ///    &lt;/template&gt;
        ///  &lt;/category&gt;
        ///  
        ///  &lt;category&gt;
        ///    &lt;pattern&gt;HELP COMMA [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string AimlHelp {
            get {
                return ResourceManager.GetString("AimlHelp", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///
        ///&lt;!-- 
        ///Copyright (c) 2015 Matt Eland
        ///
        ///This provides handling for navigating to UI areas
        ///--&gt;
        ///
        ///&lt;aiml&gt;
        ///
        ///  &lt;!-- Root verb with no parameters --&gt;
        ///  &lt;category&gt;
        ///    &lt;pattern&gt;NAVIGATE&lt;/pattern&gt;
        ///    &lt;template&gt;
        ///      Where would you like to navigate to?&lt;br /&gt;
        ///      For a list of destinations, try &quot;Navigate Help&quot;
        ///    &lt;/template&gt;
        ///  &lt;/category&gt;
        ///
        ///  &lt;!-- Navigate Help --&gt;
        ///  &lt;category&gt;
        ///    &lt;pattern&gt;Navigate Help&lt;/pattern&gt;
        ///    &lt;template&gt;
        ///      Presently you can n [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string AimlShellNavigation {
            get {
                return ResourceManager.GetString("AimlShellNavigation", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cannot use the chat system when chat did not initialize properly.
        /// </summary>
        internal static string AimlStatementHandlerChatOffline {
            get {
                return ResourceManager.GetString("AimlStatementHandlerChatOffline", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Using Template: {0}.
        /// </summary>
        internal static string AimlStatementHandlerUsingTemplate {
            get {
                return ResourceManager.GetString("AimlStatementHandlerUsingTemplate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///
        ///&lt;!-- 
        ///Copyright (c) 2015 Matt Eland
        ///
        ///This provides handling for the Goodbye, Shutdown and Status verbs
        ///--&gt;
        ///
        ///&lt;aiml&gt;
        ///
        ///  &lt;!-- Handle status requests with a system status response --&gt;
        ///  &lt;category&gt;
        ///    &lt;pattern&gt;STATUS&lt;/pattern&gt;
        ///    &lt;template id=&quot;tmp_status&quot;&gt;
        ///      &lt;alfred subsystem=&quot;sys&quot; command=&quot;status&quot; /&gt;
        ///    &lt;/template&gt;
        ///  &lt;/category&gt;
        ///  &lt;category&gt;
        ///    &lt;pattern&gt;SYSTEM STATUS&lt;/pattern&gt;
        ///    &lt;template&gt;
        ///      &lt;srai&gt;STATUS&lt;/srai&gt;
        ///    &lt;/template&gt;
        ///  &lt;/categ [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string AimlSysStatus {
            get {
                return ResourceManager.GetString("AimlSysStatus", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        ///
        ///&lt;!-- 
        ///Copyright (c) 2015 Matt Eland
        ///
        ///This provides generic politeness and manners related to things such as &quot;THANKS&quot;
        ///--&gt;
        ///
        ///&lt;aiml&gt;
        ///
        ///  &lt;!-- Basic Thanks Handling --&gt;
        ///  &lt;category&gt;
        ///    &lt;pattern&gt;THANKS&lt;/pattern&gt;
        ///    &lt;template id=&quot;tmp_thanks&quot;&gt;
        ///      &lt;random&gt;
        ///        &lt;li&gt;Of course.&lt;/li&gt;
        ///        &lt;li&gt;That&apos;s what I&apos;m here for.&lt;/li&gt;
        ///        &lt;li&gt;I&apos;m glad it helped.&lt;/li&gt;
        ///        &lt;li&gt;It was no problem at all.&lt;/li&gt;
        ///      &lt;/random&gt;
        ///    &lt;/template&gt;
        ///  &lt;/category&gt;
        ///        /// [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string AimlThanks {
            get {
                return ResourceManager.GetString("AimlThanks", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Attempted to execute command, but no valid recipient.
        /// </summary>
        internal static string AlfredTagHandlerProcessChangeNoRecipient {
            get {
                return ResourceManager.GetString("AlfredTagHandlerProcessChangeNoRecipient", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;root&gt;
        ///  &lt;item name=&quot; HE &quot; value=&quot; she &quot; /&gt;
        ///  &lt;item name=&quot; SHE &quot; value=&quot; he &quot; /&gt;
        ///  &lt;item name=&quot; TO HIM &quot; value=&quot; to her &quot; /&gt;
        ///  &lt;item name=&quot; FOR HIM &quot; value=&quot; for her &quot; /&gt;
        ///  &lt;item name=&quot; WITH HIM &quot; value=&quot; with her &quot; /&gt;
        ///  &lt;item name=&quot; ON HIM &quot; value=&quot; on her &quot; /&gt;
        ///  &lt;item name=&quot; IN HIM &quot; value=&quot; in her &quot; /&gt;
        ///  &lt;item name=&quot; TO HER &quot; value=&quot; to him &quot; /&gt;
        ///  &lt;item name=&quot; FOR HER &quot; value=&quot; for him &quot; /&gt;
        ///  &lt;item name=&quot; WITH HER &quot; value=&quot; with him &quot; /&gt;
        ///  &lt;item name=&quot;  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ChatBotGenderSubstitutions {
            get {
                return ResourceManager.GetString("ChatBotGenderSubstitutions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;root&gt;
        ///  &lt;item name=&quot; WITH YOU &quot; value=&quot; with me &quot; /&gt;
        ///  &lt;item name=&quot; WITH ME &quot; value=&quot; with you &quot; /&gt;
        ///  &lt;item name=&quot; TO YOU &quot; value=&quot; to me &quot; /&gt;
        ///  &lt;item name=&quot; TO ME &quot; value=&quot; to you &quot; /&gt;
        ///  &lt;item name=&quot; OF YOU &quot; value=&quot; of me &quot; /&gt;
        ///  &lt;item name=&quot; OF ME &quot; value=&quot; of you &quot; /&gt;
        ///  &lt;item name=&quot; FOR YOU &quot; value=&quot; for me &quot; /&gt;
        ///  &lt;item name=&quot; FOR ME &quot; value=&quot; for you &quot; /&gt;
        ///  &lt;item name=&quot; GIVE YOU &quot; value=&quot; give me &quot; /&gt;
        ///  &lt;item name=&quot; GIVE ME &quot; value=&quot; give you &quot; /&gt;
        ///  &lt; [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ChatBotPerson2Substitutions {
            get {
                return ResourceManager.GetString("ChatBotPerson2Substitutions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;root&gt;
        ///  &lt;item name=&quot; I WAS &quot; value=&quot; he or she was &quot; /&gt;
        ///  &lt;item name=&quot; I AM &quot; value=&quot; he or she is &quot; /&gt;
        ///  &lt;item name=&quot; I &quot; value=&quot; he or she &quot; /&gt;
        ///  &lt;item name=&quot; HE WAS &quot; value=&quot; I was &quot; /&gt;
        ///  &lt;item name=&quot; SHE WAS &quot; value=&quot; I was &quot; /&gt;
        ///  &lt;item name=&quot; ME &quot; value=&quot; him or her &quot; /&gt;
        ///  &lt;item name=&quot; MY &quot; value=&quot; his or her &quot; /&gt;
        ///  &lt;item name=&quot; MYSELF &quot; value=&quot; him or herself &quot; /&gt;
        ///&lt;/root&gt;
        ///.
        /// </summary>
        internal static string ChatBotPersonSubstitutions {
            get {
                return ResourceManager.GetString("ChatBotPersonSubstitutions", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;root&gt;
        ///  &lt;item name=&quot;aimldirectory&quot; value=&quot;chat/Aiml&quot;/&gt;
        ///  &lt;item name=&quot;configdirectory&quot; value=&quot;chat/config&quot;/&gt;
        ///  
        ///  &lt;item name=&quot;logdirectory&quot; value=&quot;logs&quot;/&gt;
        ///  &lt;item name=&quot;splittersfile&quot; value=&quot;Splitters.xml&quot;/&gt;
        ///  &lt;item name=&quot;person2substitutionsfile&quot; value=&quot;Person2Substitutions.xml&quot;/&gt;
        ///  &lt;item name=&quot;personsubstitutionsfile&quot; value=&quot;PersonSubstitutions.xml&quot;/&gt;
        ///  &lt;item name=&quot;gendersubstitutionsfile&quot; value=&quot;GenderSubstitutions.xml&quot;/&gt;
        ///  &lt;item name=&quot;defaultpredicates&quot;  [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ChatBotSettings {
            get {
                return ResourceManager.GetString("ChatBotSettings", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot; ?&gt;
        ///&lt;root&gt;
        ///  &lt;item name=&quot;=REPLY&quot; value=&quot;&quot; /&gt;
        ///  &lt;item name=&quot;NAME=RESET&quot; value=&quot;&quot; /&gt;
        ///  &lt;item name=&quot;:-)&quot; value=&quot; smile &quot; /&gt;
        ///  &lt;item name=&quot;:)&quot; value=&quot; smile &quot; /&gt;
        ///  &lt;item name=&quot;,)&quot; value=&quot; smile &quot; /&gt;
        ///  &lt;item name=&quot;;)&quot; value=&quot; smile &quot; /&gt;
        ///  &lt;item name=&quot;;-)&quot; value=&quot; smile &quot; /&gt;
        ///  &lt;item name=&quot;&amp;quot;&quot; value=&quot;&quot; /&gt;
        ///  &lt;item name=&quot;/&quot; value=&quot; &quot; /&gt;
        ///  &lt;item name=&quot;&amp;gt;&quot; value=&quot; gt &quot; /&gt;
        ///  &lt;item name=&quot;&amp;lt;&quot; value=&quot; lt &quot; /&gt;
        ///  &lt;item name=&quot;(&quot; value=&quot; &quot; /&gt;
        ///  &lt;item name=&quot;)&quot; value=&quot; &quot; [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string ChatBotSubstitutions {
            get {
                return ResourceManager.GetString("ChatBotSubstitutions", resourceCulture);
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
        ///   Looks up a localized string similar to User.
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
        ///   Looks up a localized string similar to No end tag for OOB command: {0}.
        /// </summary>
        internal static string NoEndTagForOobCommand {
            get {
                return ResourceManager.GetString("NoEndTagForOobCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Empty directoryPath to a settings file.
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
