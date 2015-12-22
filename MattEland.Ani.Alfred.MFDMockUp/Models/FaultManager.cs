// ---------------------------------------------------------
// FaultManager.cs
// 
// Created on:      10/29/2015 at 12:21 PM
// Last Modified:   10/29/2015 at 12:21 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

using Assisticant.Collections;

using MattEland.Common;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     Manager for faultIndicator indicators. This class cannot be inherited.
    /// </summary>
    public sealed class FaultManager
    {
        /// <summary>
        ///     The faultIndicator indicators collection.
        /// </summary>
        [NotNull, ItemNotNull]
        private readonly ObservableList<FaultIndicatorModel> _faultIndicators;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:FaultManager"/> class.
        /// </summary>
        public FaultManager()
        {
            Contract.Ensures(_faultIndicators != null);

            _faultIndicators = new ObservableList<FaultIndicatorModel>();
        }

        /// <summary>
        ///     Contains code contract invariants that describe facts about this class that will be true
        ///     after any public method in this class is called.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInvariants()
        {
            Contract.Invariant(_faultIndicators != null);
        }


        /// <summary>
        ///     The faultIndicator indicators collection.
        /// </summary>
        [NotNull]
        public IEnumerable<FaultIndicatorModel> FaultIndicators
        {
            [DebuggerStepThrough]
            get
            { return _faultIndicators; }
        }

        /// <summary>
        ///     Registers a new fault indicator.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are <lang keyword="null" />.
        /// </exception>
        /// <param name="provider"> The provider. </param>
        public void Register([NotNull] FaultIndicatorProvider provider)
        {
            Contract.Requires(provider != null);

            if (provider == null) throw new ArgumentNullException(nameof(provider));

            Register(provider.Indicator);

        }

        /// <summary>
        ///     Registers a new fault indicator.
        /// </summary>
        /// <param name="indicator"> The indicator. </param>
        public void Register([NotNull] FaultIndicatorModel indicator)
        {
            Contract.Requires(indicator != null);

            if (_faultIndicators.Contains(indicator))
            {
                throw new InvalidOperationException("indicator is already part of FaultIndicators");
            }
            if (_faultIndicators.Any((f) => f.IndicatorLabel.Matches(indicator.IndicatorLabel)))
            {
                var message = string.Format("FaultIndicators already contains an indicator with a label of {0}", indicator.IndicatorLabel);
                throw new InvalidOperationException(message);
            }

            _faultIndicators.Add(indicator);
        }

        /// <summary>
        ///     Updates the indicators.
        /// </summary>
        public void Update()
        {
            foreach (var indicator in _faultIndicators)
            {
                indicator.Update();
            }
        }
    }
}