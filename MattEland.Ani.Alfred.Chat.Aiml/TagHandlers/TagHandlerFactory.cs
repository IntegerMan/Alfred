// ---------------------------------------------------------
// TagHandlerFactory.cs
// 
// Created on:      08/14/2015 at 12:14 AM
// Last Modified:   08/14/2015 at 1:58 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;
using MattEland.Ani.Alfred.Core.Console;
using MattEland.Common;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     TagHandlerFactory is responsible for taking a set of input parameters and instantiating the appropriate class and
    ///     providing that class the necessary parameters
    /// </summary>
    public sealed class TagHandlerFactory
    {
        [NotNull]
        private readonly ChatEngine _engine;

        [NotNull]
        [ItemNotNull]
        private readonly IDictionary<string, Type> _handlerMapping = new Dictionary<string, Type>();

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public TagHandlerFactory([NotNull] ChatEngine engine)
        {
            //- Validate
            if (engine == null)
            {
                throw new ArgumentNullException(nameof(engine));
            }

            //- Set fields
            _engine = engine;

            //+ Load types in this assembly that have the HandlesAimlTag attribute
            RegisterTagHandlersInAppDomain();
        }

        /// <summary>
        ///     Registers all AimlTagHandler derived types decorated with the HandlesAimlTag attribute in any assembly that is
        ///     currently loaded.
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
        ///     Loads and registers tag handlers in the given assembly. Valid tag handlers are non-abstract classes derived from
        ///     AimlTagHandler and decorated with the HandlesAimlTag attribute.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        private void RegisterTagHandlersInAssembly([NotNull] Assembly assembly)
        {
            //- Validation
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            // Read all types in the provided assembly that have this attribute.
            var types = assembly.GetTypesInAssemblyWithAttribute<HandlesAimlTagAttribute>(false);

            //- Loop through each type and register it
            foreach (var type in types)
            {
                // We can't instantiate an abstract type and we need AimlTagHandlers for this
                if (type.IsAbstract || !type.IsSubclassOf(typeof(AimlTagHandler)))
                {
                    continue;
                }

                // Grab the attribute and exit early if it's not there
                var attribute = type.GetCustomAttribute(typeof(HandlesAimlTagAttribute)) as HandlesAimlTagAttribute;
                if (attribute == null)
                {
                    continue;
                }

                // The attribute is present. Add it to the dictionary as a type handler
                var name = attribute.Name?.ToUpperInvariant();
                if (!name.IsNullOrWhitespace())
                {
                    _handlerMapping.Add(name, type);
                }

            }
        }

        [CanBeNull]
        public AimlTagHandler Build(
            XmlNode node,
            SubQuery query,
            Request request,
            Result result,
            User user,
            [CanBeNull] string tagName)
        {
            // Ensure not null on the tag
            if (tagName == null)
            {
                return null;
            }

            // Check our mapping for an instance of that type and use it
            var handler = BuildDynamic(node, query, request, result, user, tagName);
            if (handler != null)
            {
                return handler;
            }

            switch (tagName?.ToLowerInvariant())
            {
                case "condition":
                    return new condition(_engine, user, query, request, result, node);
                case "date":
                    return new date(_engine, user, query, request, result, node);
                case "formal":
                    return new formal(_engine, user, query, request, result, node);
                case "gender":
                    return new gender(_engine, user, query, request, result, node);
                case "get":
                    return new get(_engine, user, query, request, result, node);
                case "gossip":
                    return new gossip(_engine, user, query, request, result, node);
                case "id":
                    return new id(_engine, user, query, request, result, node);
                case "input":
                    return new input(_engine, user, query, request, result, node);
                case "javascript":
                    return new javascript(_engine, user, query, request, result, node);
                case "learn":
                    return new learn(_engine, user, query, request, result, node);
                case "lowercase":
                    return new lowercase(_engine, user, query, request, result, node);
                case "person":
                    return new person(_engine, user, query, request, result, node);
                case "person2":
                    return new person2(_engine, user, query, request, result, node);
                case "sentence":
                    return new sentence(_engine, user, query, request, result, node);
                case "set":
                    return new set(_engine, user, query, request, result, node);
                case "size":
                    return new size(_engine, user, query, request, result, node);
                case "sr":
                    return new sr(_engine, user, query, request, result, node);
                case "srai":
                    return new srai(_engine, user, query, request, result, node);
                case "star":
                    return new star(_engine, user, query, request, result, node);
                case "system":
                    return new system(_engine, user, query, request, result, node);
                case "that":
                    return new that(_engine, user, query, request, result, node);
                case "thatstar":
                    return new thatstar(_engine, user, query, request, result, node);
                case "think":
                    return new think(_engine, user, query, request, result, node);
                case "topicstar":
                    return new topicstar(_engine, user, query, request, result, node);
                case "uppercase":
                    return new uppercase(_engine, user, query, request, result, node);
                case "version":
                    return new version(_engine, user, query, request, result, node);
            }

            return null;
        }

        [CanBeNull]
        private AimlTagHandler BuildDynamic(XmlNode node,
                                            SubQuery query,
                                            Request request,
                                            Result result,
                                            User user,
                                            [NotNull] string tagName)
        {

            //- Ensure we have the requested tag. If we don't exit now.
            var tagKey = tagName.ToUpperInvariant();
            if (!_handlerMapping.ContainsKey(tagKey))
            {
                return null;
            }

            //- Grab the type definition
            var type = _handlerMapping[tagKey];
            if (type == null)
            {
                return null;
            }

            string logMessage;

            try
            {
                /* Execute dynamic activation using reflection. This assumes the needed constructor
                   will be present on the type and, if not, a TargetInvocationException will be 
                   thrown. */

                var instance = Activator.CreateInstance(type, _engine, user, query, request, result, node);

                // The result doesn't have to be an AimlTagHandler. Do a safe cast and check.
                var handler = instance as AimlTagHandler;
                if (handler != null)
                {
                    return handler;
                }

                // The item is not of the right type. Prepare the log
                logMessage =
                    string.Format(_engine.Locale,
                                  "Dynamic tag handler for {0} instantiated an instance of type {1} that was not an AimlTagHandler. Disabling this handler.",
                                  tagName,
                                  type.FullName);

            }
            catch (TargetInvocationException ex)
            {
                logMessage = string.Format(_engine.Locale,
                                           "Dynamic tag handler for {0} encountered an invocation error instantiating type {1}: {2}\n Disabling this handler.",
                                           tagName,
                                           type.FullName,
                                           ex.Message);

            }
            catch (MissingMethodException ex)
            {
                logMessage = string.Format(_engine.Locale,
                                           "Dynamic tag handler for {0} encountered a missing method error instantiating type {1}: {2}\n Disabling this handler.",
                                           tagName,
                                           type.FullName,
                                           ex.Message);

            }

            // If we got here, it didn't work; unregister it.
            _engine.Log(logMessage, LogLevel.Error);
            _handlerMapping.Remove(tagKey);
            return null;
        }
    }
}