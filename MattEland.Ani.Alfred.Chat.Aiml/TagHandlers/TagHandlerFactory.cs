// ---------------------------------------------------------
// TagHandlerFactory.cs
// 
// Created on:      08/14/2015 at 12:14 AM
// Last Modified:   08/14/2015 at 5:26 PM
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
    ///     TagHandlerFactory is responsible for taking a set of input parameters and instantiating the
    ///     appropriate class and providing that class the necessary parameters
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
        /// <exception cref="ArgumentNullException"><paramref name="engine" /> is <see langword="null" />.</exception>
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
        ///     Loads and registers tag handlers in the given assembly. Valid tag handlers are non-abstract
        ///     classes derived from
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
                var attribute =
                    type.GetCustomAttribute(typeof(HandlesAimlTagAttribute)) as
                    HandlesAimlTagAttribute;
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

        /// <summary>
        ///     Builds a tag handler for the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <param name="query">The query.</param>
        /// <param name="request">The request.</param>
        /// <param name="result">The result.</param>
        /// <param name="user">The user.</param>
        /// <param name="tagName">Name of the tag.</param>
        /// <exception cref="ArgumentNullException">node</exception>
        /// <returns>The tag handler.</returns>
        [CanBeNull]
        public AimlTagHandler Build([NotNull] XmlNode node,
                                    SubQuery query,
                                    Request request,
                                    Result result,
                                    User user,
                                    [CanBeNull] string tagName)
        {
            //- Validate
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            if (tagName == null)
            {
                return null;
            }

            //- All construction will require these parameters so build them now
            // TODO: Would thee be better suited for outside of this method and passed in as parameters?
            var parameters = new TagHandlerParameters(_engine, user, query, request, result, node);

            // Check our mapping for an instance of that type and use it
            var handler = BuildTagHandlerDynamic(tagName, parameters);
            if (handler != null)
            {
                return handler;
            }

            return BuildTagHandlerDeprecated(tagName, parameters);
        }

        /// <summary>
        ///     Builds a tag handler using deprecated hard-coded links between tag names and classes. Each one
        ///     of these items is getting revised and converted over to the dynamic invoke model. As that
        ///     happens their corresponding line in this method should be removed and this method can be
        ///     retired eventually.
        /// </summary>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="parameters">The parameters.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns>A tag handler</returns>
        private static AimlTagHandler BuildTagHandlerDeprecated(string tagName,
                                                                [NotNull] TagHandlerParameters
                                                                    parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            //! Construction for tags that have not yet been reviewed and cut over to dynamic invocation 
            switch (tagName.NonNull().ToLowerInvariant())
            {
                case "gender":
                    return new gender(parameters);
                case "get":
                    return new get(parameters);
                case "gossip":
                    return new gossip(parameters);
                case "id":
                    return new id(parameters);
                case "input":
                    return new input(parameters);
                case "javascript":
                    return new javascript(parameters);
                case "learn":
                    return new learn(parameters);
                case "lowercase":
                    return new lowercase(parameters);
                case "person":
                    return new person(parameters);
                case "person2":
                    return new person2(parameters);
                case "sentence":
                    return new sentence(parameters);
                case "set":
                    return new set(parameters);
                case "size":
                    return new size(parameters);
                case "sr":
                    return new sr(parameters);
                case "srai":
                    return new srai(parameters);
                case "star":
                    return new star(parameters);
                case "system":
                    return new system(parameters);
                case "that":
                    return new that(parameters);
                case "thatstar":
                    return new thatstar(parameters);
                case "think":
                    return new think(parameters);
                case "topicstar":
                    return new topicstar(parameters);
                case "uppercase":
                    return new uppercase(parameters);
                case "version":
                    return new version(parameters);
            }

            return null;
        }

        /// <summary>
        ///     Builds a tag handler using dynamic invocation and reflection relying on the HandlesAimlTag
        ///     attribute.
        /// </summary>
        /// <param name="tagName">Name of the tag.</param>
        /// <param name="parameters">The construction parameters.</param>
        /// <returns>The tag handler.</returns>
        [CanBeNull]
        private AimlTagHandler BuildTagHandlerDynamic([NotNull] string tagName,
                                                      [NotNull] TagHandlerParameters parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

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

                var instance = Activator.CreateInstance(type, parameters);

                // The result doesn't have to be an AimlTagHandler. Do a safe cast and check.
                var handler = instance as AimlTagHandler;
                if (handler != null)
                {
                    return handler;
                }

                //- The item is not of the right type. Prepare the log
                logMessage =
                    string.Format(_engine.Locale,
                                  "Dynamic tag handler for {0} instantiated an instance of type {1} that was not an AimlTagHandler. Disabling this handler.",
                                  tagName,
                                  type.FullName);

            }
            catch (TargetInvocationException ex)
            {
                //- Error handling for errors encountered during invocation
                logMessage = string.Format(_engine.Locale,
                                           "Dynamic tag handler for {0} encountered an invocation error instantiating type {1}: {2}\n Disabling this handler.",
                                           tagName,
                                           type.FullName,
                                           ex.Message);

            }
            catch (MissingMethodException ex)
            {
                //- This happens if the constructor wasn't found with the right parameters
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