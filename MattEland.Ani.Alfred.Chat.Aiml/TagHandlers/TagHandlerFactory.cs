// ---------------------------------------------------------
// TagHandlerFactory.cs
// 
// Created on:      08/14/2015 at 12:14 AM
// Last Modified:   08/14/2015 at 12:15 AM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Xml;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Chat.Aiml.Utils;

namespace MattEland.Ani.Alfred.Chat.Aiml.TagHandlers
{
    /// <summary>
    ///     TagHandlerFactory is responsible for taking a set of input parameters and instantiating the appropriate class and
    ///     providing that class the necessary parameters
    /// </summary>
    public static class TagHandlerFactory
    {
        [CanBeNull]
        public static AimlTagHandler Build(ChatEngine engine,
                                               XmlNode node,
                                               SubQuery query,
                                               Request request,
                                               Result result,
                                               User user,
                                               [CanBeNull] string tagName)
        {
            switch (tagName?.ToLowerInvariant())
            {
                case "bot":
                    return new bot(engine, user, query, request, result, node);
                case "condition":
                    return new condition(engine, user, query, request, result, node);
                case "date":
                    return new date(engine, user, query, request, result, node);
                case "formal":
                    return new formal(engine, user, query, request, result, node);
                case "gender":
                    return new gender(engine, user, query, request, result, node);
                case "get":
                    return new get(engine, user, query, request, result, node);
                case "gossip":
                    return new gossip(engine, user, query, request, result, node);
                case "id":
                    return new id(engine, user, query, request, result, node);
                case "input":
                    return new input(engine, user, query, request, result, node);
                case "javascript":
                    return new javascript(engine, user, query, request, result, node);
                case "learn":
                    return new learn(engine, user, query, request, result, node);
                case "lowercase":
                    return new lowercase(engine, user, query, request, result, node);
                case "person":
                    return new person(engine, user, query, request, result, node);
                case "person2":
                    return new person2(engine, user, query, request, result, node);
                case "random":
                    return new random(engine, user, query, request, result, node);
                case "sentence":
                    return new sentence(engine, user, query, request, result, node);
                case "set":
                    return new set(engine, user, query, request, result, node);
                case "size":
                    return new size(engine, user, query, request, result, node);
                case "sr":
                    return new sr(engine, user, query, request, result, node);
                case "srai":
                    return new srai(engine, user, query, request, result, node);
                case "star":
                    return new star(engine, user, query, request, result, node);
                case "system":
                    return new system(engine, user, query, request, result, node);
                case "that":
                    return new that(engine, user, query, request, result, node);
                case "thatstar":
                    return new thatstar(engine, user, query, request, result, node);
                case "think":
                    return new think(engine, user, query, request, result, node);
                case "topicstar":
                    return new topicstar(engine, user, query, request, result, node);
                case "uppercase":
                    return new uppercase(engine, user, query, request, result, node);
                case "version":
                    return new version(engine, user, query, request, result, node);
            }

            return null;
        }

    }
}