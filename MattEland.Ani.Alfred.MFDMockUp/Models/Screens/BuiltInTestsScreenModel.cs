// ---------------------------------------------------------
// BuiltInTestsScreenModel.cs
// 
// Created on:      12/16/2015 at 10:16 PM
// Last Modified:   12/16/2015 at 10:16 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Diagnostics;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.Screens
{
    /// <summary>
    ///     A data model for the built in tests screen.
    /// </summary>
    public sealed class BuiltInTestsScreenModel : ScreenModel
    {
        [NotNull]
        private readonly FaultManager _faultManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuiltInTestsScreenModel"/> class.
        /// </summary>
        public BuiltInTestsScreenModel([NotNull] FaultManager manager) : base("BIT")
        {
            if (manager == null) throw new ArgumentNullException(nameof(manager));

            _faultManager = manager;
        }

        /// <summary>
        ///     Process the screen state and outputs any resulting information to the processorResult.
        /// </summary>
        /// <param name="processor"> The processor. </param>
        /// <param name="processorResult"> The processor result. </param>
        protected override void ProcessScreenState(MFDProcessor processor, MFDProcessorResult processorResult)
        {
            //- No Action Needed
        }

        /// <summary>
        ///     Gets the fault manager.
        /// </summary>
        /// <value>
        ///     The fault manager.
        /// </value>
        [NotNull]
        public FaultManager FaultManager
        {
            [DebuggerStepThrough]
            get
            { return _faultManager; }
        }

    }
}