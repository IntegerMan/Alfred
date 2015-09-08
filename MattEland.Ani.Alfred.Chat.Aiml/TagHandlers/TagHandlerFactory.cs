// ---------------------------------------------------------
// TagHandlerFactory.cs
// 
// Created on:      08/19/2015 at 9:31 PM
// Last Modified:   08/24/2015 at 12:49 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     TagHandlerFactory is responsible for taking a set of input parameters and instantiating the
    ///     appropriate class and providing that class the necessary parameters
    /// </summary>
    public sealed class TagHandlerFactory
    {
        /// <summary>
        ///     The chat engine.
        /// </summary>
        [NotNull]
        private readonly ChatEngine _engine;

        [NotNull]
        [ItemNotNull]
        private readonly IDictionary<string, Type> _handlerMapping = new Dictionary<string, Type>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException"><paramref name="engine" /> is <see langword="null" />.</exception>
        public TagHandlerFactory([NotNull] ChatEngine engine)
        {
            //- Validate
            if (engine == null) { throw new ArgumentNullException(nameof(engine)); }

            //- Set fields
            _engine = engine;

            //+ Load types in this assembly that have the HandlesAimlTag attribute
            RegisterTagHandlersInAppDomain();
        }

        /// <summary>
        ///     Registers all AimlTagHandler derived types decorated with the HandlesAimlTag attribute in any
        ///     assembly that is currently loaded.
        /// </summary>
        private void RegisterTagHandlersInAppDomain()
        {
            // Grab all assemblies currently loaded
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            // Loop through and register each tag handler found. This will make registration automatic
            foreach (var assembly in assemblies.Where(assembly => assembly != null))
            {
                RegisterTagHandlersInAssembly(assembly);
            }
        }

        /// <summary>
        ///     Loads and registers tag handlers in the given <paramref name="assembly" /> . Valid tag
        ///     handlers are non-abstract classes derived from <see cref="AimlTagHandler" /> and
        ///     decorated with the HandlesAimlTag attribute.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="assembly" /> is <see langword="null" /> .
        /// </exception>
        /// <param name="assembly"> The assembly. </param>
        private void RegisterTagHandlersInAssembly([NotNull] Assembly assembly)
        {
            // TODO: This method has a high cyclomatic complexity

            //- Validation
            if (assembly == null) { throw new ArgumentNullException(nameof(assembly)); }

            // Read all types in the provided assembly that have this attribute.
            try
            {
                var types =
                    assembly.GetTypesInAssemblyWithAttribute(typeof(HandlesAimlTagAttribute), false);

                //- Loop through each type and register it
                foreach (var type in types)
                {
                    // We can't instantiate an abstract type and we need AimlTagHandlers for this
                    if (type.IsAbstract || !type.IsSubclassOf(typeof(AimlTagHandler)))
                    {
                        continue;
                    }

                    // Grab the attribute and exit early if it's not there
                    var attribute =
                        type.GetCustomAttribute(typeof(HandlesAimlTagAttribute)) as
                        HandlesAimlTagAttribute;
                    if (attribute == null) { continue; }

                    // The attribute is present. Add it to the dictionary as a type handler
                    var name = attribute.Name.ToUpperInvariant();
                    if (!name.IsNullOrWhitespace()) { _handlerMapping.Add(name, type); }
                }
            }
            catch (ReflectionTypeLoadException rex)
            {
                var message = string.Format(CultureInfo.CurrentCulture,
                                            "Encountered reflection type load exception '{0}' loading assembly '{1}'.",
                                            rex.Message,
                                            assembly.FullName);
                _engine.Log(message, LogLevel.Verbose);
            }
            catch (AmbiguousMatchException amex)
            {
                var message = string.Format(CultureInfo.CurrentCulture,
                                            "Encountered ambiguous match exception '{0}' loading assembly '{1}'.",
                                            amex.Message,
                                            assembly.FullName);
                _engine.Log(message, LogLevel.Verbose);
            }
            catch (TypeLoadException lex)
            {
                var message = string.Format(CultureInfo.CurrentCulture,
                                            "Encountered type load exception '{0}' on '{2}' while loading assembly '{1}'.",
                                            lex.Message,
                                            assembly.FullName,
                                            lex.TypeName);
                _engine.Log(message, LogLevel.Verbose);
            }
        }

        /// <summary>
        ///     Builds a tag handler for the specified node.
        /// </summary>
        /// <param name="element">The node.</param>
        /// <param name="query">The query.</param>
        /// <param name="request">The request.</param>
        /// <param name="chatResult">The result.</param>
        /// <param name="user">The user.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <exception cref="ArgumentNullException">element, query, request, user</exception>
        /// <returns>The tag handler.</returns>
        [CanBeNull]
        public AimlTagHandler Build([NotNull] XmlElement element,
                                    [NotNull] SubQuery query,
                                    [NotNull] Request request,
                                    [NotNull] ChatResult chatResult,
                                    [NotNull] User user,
                                    [CanBeNull] string tagName)
        {
            //- Validate
            if (element == null) { throw new ArgumentNullException(nameof(element)); }
            if (query == null) { throw new ArgumentNullException(nameof(query)); }
            if (request == null) { throw new ArgumentNullException(nameof(request)); }
            if (user == null) { throw new ArgumentNullException(nameof(user)); }

            // Early exit
            if (tagName.IsEmpty()) { return null; }

            //- All construction will require these parameters so build them now
            // TODO: Would this be better suited for outside of this method and passed in as parameters?
            var parameters = new TagHandlerParameters(_engine, user, query, request, chatResult, element);

            // Use dynamic invocation to create a TagHandler based on usage of the HandlesAimlTag attribute.
            return BuildTagHandlerDynamic(tagName.NonNull(), parameters);
        }

        /// <summary>
        ///     Builds a tag handler using dynamic invocation and reflection relying on the HandlesAimlTag
        ///     attribute.
        /// </summary>
        /// <remarks>
        ///     This method is public for testability
        /// </remarks>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="parameters">The construction parameters.</param>
        /// <returns>The tag handler.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="tagName" /> is <see langword="null" />.</exception>
        /// <exception cref="ArgumentNullException"><paramref name="parameters" /> is <see langword="null" />.</exception>
        [CanBeNull]
        public AimlTagHandler BuildTagHandlerDynamic([NotNull] string tagName,
                                                     [NotNull] TagHandlerParameters parameters)
        {
            if (parameters == null) { throw new ArgumentNullException(nameof(parameters)); }
            if (tagName.IsEmpty()) { throw new ArgumentNullException(nameof(tagName)); }

            //- Ensure we have the requested tag. If we don't exit now.
            var tagKey = tagName.ToUpperInvariant();
            if (!_handlerMapping.ContainsKey(tagKey)) { return null; }

            //- Grab the type definition
            var type = _handlerMapping[tagKey];
            if (type == null) { return null; }

            string logMessage;

            try
            {
                /* Execute dynamic activation using reflection. This assumes the needed constructor
                   will be present on the type and, if not, a TargetInvocationException will be 
                   thrown. */

                var instance = Activator.CreateInstance(type, parameters);

                // The result doesn't have to be an AimlTagHandler. Do a safe cast and check.
                var handler = instance as AimlTagHandler;
                if (handler != null) { return handler; }

                //- The item is not of the right type. Prepare the log
                logMessage = string.Format(_engine.Locale,
                                           Resources.TagHandlerFactoryBuildTagHandlerDynamicWrongType.NonNull(),
                                           tagName,
                                           type.FullName);
            }
            catch (TargetInvocationException ex)
            {
                //- Error handling for errors encountered during invocation
                logMessage = string.Format(_engine.Locale,
                                           Resources.TagHandlerFactoryBuildTagHandlerDynamicTargetInvocation.NonNull(),
                                           tagName,
                                           type.FullName,
                                           ex.Message);
            }
            catch (MissingMethodException ex)
            {
                //- This happens if the constructor wasn't found with the right parameters
                logMessage = string.Format(_engine.Locale,
                                           Resources.TagHandlerFactoryBuildTagHandlerDynamicMissingMethod.NonNull(),
                                           tagName,
                                           type.FullName,
                                           ex.Message);
            }

            // If we got here, it didn't work; unregister it.
            _engine.Error(logMessage);
            _handlerMapping.Remove(tagKey);

            return null;
        }
    }
}