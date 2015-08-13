// ---------------------------------------------------------
// TagHandler.cs
// 
// Created on:      08/12/2015 at 10:36 PM
// Last Modified:   08/12/2015 at 11:59 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Reflection;

namespace MattEland.Ani.Alfred.Chat.Aiml.Utils
{
    public class TagHandler
    {
        public string AssemblyName;
        public string ClassName;
        public string TagName;

        public AimlTagHandler Instantiate(Dictionary<string, Assembly> Assemblies)
        {
            if (!Assemblies.ContainsKey(AssemblyName))
            {
                return null;
            }
            var assembly = Assemblies[AssemblyName];
            assembly.GetTypes();
            return (AimlTagHandler) assembly.CreateInstance(ClassName);
        }
    }
}