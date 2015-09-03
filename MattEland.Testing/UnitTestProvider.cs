// ---------------------------------------------------------
// UnitTestProvider.cs
// 
// Created on:      09/01/2015 at 1:07 PM
// Last Modified:   09/01/2015 at 1:07 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;

using NUnit.Framework;

namespace MattEland.Testing
{
    /// <summary>
    ///     An <see cref="Attribute"/> used to decorate unit test collections
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UnitTestProvider : TestFixtureAttribute
    {


    }
}