// ---------------------------------------------------------
// Node.cs
// 
// Created on:      08/12/2015 at 10:27 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

using MattEland.Ani.Alfred.Chat.Aiml.Normalize;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    [Serializable]
    public class Node
    {
        private readonly Dictionary<string, Node> children = new Dictionary<string, Node>();
        public string filename = string.Empty;
        public string template = string.Empty;
        public string word = string.Empty;

        public int NumberOfChildNodes
        {
            get { return children.Count; }
        }

        public void AddCategory(string path, string template, string filename)
        {
            if (template.Length == 0)
            {
                throw new XmlException("The category with a pattern: " + path + " found in file: " + filename +
                                       " has an empty template tag. ABORTING");
            }
            if (path.Trim().Length == 0)
            {
                this.template = template;
                this.filename = filename;
            }
            else
            {
                var key = UppercaseTextTransformer.TransformInput(path.Trim().Split(" ".ToCharArray())[0]);
                var path1 = path.Substring(key.Length, path.Length - key.Length).Trim();
                if (children.ContainsKey(key))
                {
                    children[key].AddCategory(path1, template, filename);
                }
                else
                {
                    var node = new Node();
                    node.word = key;
                    node.AddCategory(path1, template, filename);
                    children.Add(node.word, node);
                }
            }
        }

        public string evaluate(string path,
                               SubQuery query,
                               Request request,
                               MatchState matchstate,
                               StringBuilder wildcard)
        {
            if (request.StartedOn.AddMilliseconds(request.chatEngine.TimeOut) < DateTime.Now)
            {
                request.chatEngine.writeToLog("WARNING! Request timeout. User: " + request.user.UserID + " raw input: \"" +
                                       request.rawInput + "\"");
                request.hasTimedOut = true;
                return string.Empty;
            }
            path = path.Trim();
            if (children.Count == 0)
            {
                if (path.Length > 0)
                {
                    storeWildCard(path, wildcard);
                }
                return template;
            }
            if (path.Length == 0)
            {
                return template;
            }
            var strArray = path.Split(" \r\n\t".ToCharArray());
            var key = UppercaseTextTransformer.TransformInput(strArray[0]);
            var path1 = path.Substring(key.Length, path.Length - key.Length);
            if (children.ContainsKey("_"))
            {
                var node = children["_"];
                var wildcard1 = new StringBuilder();
                storeWildCard(strArray[0], wildcard1);
                var str = node.evaluate(path1, query, request, matchstate, wildcard1);
                if (str.Length > 0)
                {
                    if (wildcard1.Length > 0)
                    {
                        switch (matchstate)
                        {
                            case MatchState.UserInput:
                                query.InputStar.Add(wildcard1.ToString());
                                wildcard1.Remove(0, wildcard1.Length);
                                break;
                            case MatchState.That:
                                query.ThatStar.Add(wildcard1.ToString());
                                break;
                            case MatchState.Topic:
                                query.TopicStar.Add(wildcard1.ToString());
                                break;
                        }
                    }
                    return str;
                }
            }
            if (children.ContainsKey(key))
            {
                var matchstate1 = matchstate;
                if (key == "<THAT>")
                {
                    matchstate1 = MatchState.That;
                }
                else if (key == "<TOPIC>")
                {
                    matchstate1 = MatchState.Topic;
                }
                var node = children[key];
                var wildcard1 = new StringBuilder();
                var str = node.evaluate(path1, query, request, matchstate1, wildcard1);
                if (str.Length > 0)
                {
                    if (wildcard1.Length > 0)
                    {
                        switch (matchstate)
                        {
                            case MatchState.UserInput:
                                query.InputStar.Add(wildcard1.ToString());
                                wildcard1.Remove(0, wildcard1.Length);
                                break;
                            case MatchState.That:
                                query.ThatStar.Add(wildcard1.ToString());
                                wildcard1.Remove(0, wildcard1.Length);
                                break;
                            case MatchState.Topic:
                                query.TopicStar.Add(wildcard1.ToString());
                                wildcard1.Remove(0, wildcard1.Length);
                                break;
                        }
                    }
                    return str;
                }
            }
            if (children.ContainsKey("*"))
            {
                var node = children["*"];
                var wildcard1 = new StringBuilder();
                storeWildCard(strArray[0], wildcard1);
                var str = node.evaluate(path1, query, request, matchstate, wildcard1);
                if (str.Length > 0)
                {
                    if (wildcard1.Length > 0)
                    {
                        switch (matchstate)
                        {
                            case MatchState.UserInput:
                                query.InputStar.Add(wildcard1.ToString());
                                wildcard1.Remove(0, wildcard1.Length);
                                break;
                            case MatchState.That:
                                query.ThatStar.Add(wildcard1.ToString());
                                break;
                            case MatchState.Topic:
                                query.TopicStar.Add(wildcard1.ToString());
                                break;
                        }
                    }
                    return str;
                }
            }
            if (word == "_" || word == "*")
            {
                storeWildCard(strArray[0], wildcard);
                return evaluate(path1, query, request, matchstate, wildcard);
            }
            wildcard = new StringBuilder();
            return string.Empty;
        }

        private void storeWildCard(string word, StringBuilder wildcard)
        {
            if (wildcard.Length > 0)
            {
                wildcard.Append(" ");
            }
            wildcard.Append(word);
        }
    }
}