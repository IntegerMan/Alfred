// ---------------------------------------------------------
// SelectionHelpers.cs
// 
// Created on:      08/20/2015 at 11:51 PM
// Last Modified:   08/20/2015 at 11:51 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Linq;
using System.Windows.Controls.Primitives;

using JetBrains.Annotations;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Common;

namespace MattEland.Ani.Alfred.PresentationShared.Helpers
{
    /// <summary>
    /// A helper class containing methods for manipulating user interface selections.
    /// </summary>
    public static class SelectionHelper
    {

        /// <summary>
        /// Sets the selected tab.
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <param name="id">The id to match.</param>
        /// <exception cref="ArgumentNullException">selector</exception>
        /// <returns><c>true</c> if a selection was made, <c>false</c> otherwise.</returns>
        public static bool SelectItemById([NotNull] Selector selector, string id)
        {
            //- Validation
            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }

            // Ensure items exist
            var itemCollection = selector.Items;
            if (itemCollection == null)
            {
                return false;
            }

            // Loop through and find the first item that matches
            foreach (var item in itemCollection.Cast<IHasIdentifier>().Where(item => item != null && item.Id.Matches(id)))
            {
                // We have a match. Select it and return that a match was made
                selector.SelectedItem = item;
                return true;
            }

            // No dice. Bail out.
            return false;
        }


    }
}