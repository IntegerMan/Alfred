// ---------------------------------------------------------
// MasterModeCycler.cs
// 
// Created on:      11/20/2015 at 10:08 PM
// Last Modified:   11/20/2015 at 10:08 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

using Assisticant.Fields;

using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models.MasterModes
{
    /// <summary>
    ///     A utility class that cycles through master modes
    /// </summary>
    public sealed class MasterModeCycler
    {
        [NotNull]
        private readonly MultifunctionDisplay _display;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public MasterModeCycler([NotNull] MultifunctionDisplay display, [NotNull, ItemNotNull] IEnumerable<MasterModeBase> availableModes)
        {
            Contract.Requires(display != null);
            Contract.Requires(availableModes != null);
            Contract.Requires(availableModes.Any());
            Contract.Requires(availableModes.All(m => m != null));
            Contract.Ensures(_modes != null);
            Contract.Ensures(_modes.Count == availableModes.Count());
            Contract.Ensures(_modes.All(n => n != null));

            _display = display;

            _modes = new LinkedList<MasterModeBase>();

            // Add all items to the end of the linked list
            foreach (var mode in availableModes)
            {
                _modes.AddLast(mode);
            }

            _currentListNode = _modes.First;

            _currentMode = new Observable<MasterModeBase>(_currentListNode.Value);

            var nextNode = GetNextNode();
            _nextMode = new Observable<MasterModeBase>(nextNode.Value);

        }

        /// <summary>
        ///     Contains code contract invariants that describe facts about this class that will be true
        ///     after any public method in this class is called.
        /// </summary>
        [ContractInvariantMethod]
        private void ClassInvariants()
        {
            Contract.Invariant(_modes != null);
            Contract.Invariant(_modes.All(n => n != null));
            Contract.Invariant(CurrentMode != null);
            Contract.Invariant(CurrentMode == _currentListNode.Value);
            Contract.Invariant(GetNextNode() != null);
            Contract.Invariant(NextMode == GetNextNode().Value);
        }


        [NotNull]
        private LinkedListNode<MasterModeBase> GetNextNode()
        {
            Contract.Ensures(Contract.Result<LinkedListNode<MasterModeBase>>() != null);

            return _currentListNode.Next ?? _modes.First;
        }

        [NotNull]
        private readonly Observable<MasterModeBase> _currentMode;

        [NotNull]
        private readonly Observable<MasterModeBase> _nextMode;

        [NotNull, ItemNotNull]
        private readonly LinkedList<MasterModeBase> _modes;

        [NotNull, ItemNotNull]
        private LinkedListNode<MasterModeBase> _currentListNode;

        /// <summary>
        ///     Gets the current mode.
        /// </summary>
        /// <value>
        ///     The current mode.
        /// </value>
        public MasterModeBase CurrentMode
        {
            get { return _currentMode.Value; }
        }

        /// <summary>
        ///     Gets the next mode. This mode will become the <see cref="CurrentMode"/> when
        ///     <see cref="MoveToNextMode"/> is invoked.
        /// </summary>
        /// <value>
        ///     The next mode.
        /// </value>
        public MasterModeBase NextMode
        {
            get { return _nextMode.Value; }
        }

        /// <summary>
        ///     Moves to the next master mode and returns the new master mode.
        /// </summary>
        /// <returns>
        ///     The newly selected mode.
        /// </returns>
        public MasterModeBase MoveToNextMode()
        {
            var nextNode = GetNextNode();
            _currentListNode = nextNode;

            _currentMode.Value = nextNode.Value;

            _currentListNode = nextNode;

            _nextMode.Value = GetNextNode().Value;

            return nextNode.Value;
        }

    }
}