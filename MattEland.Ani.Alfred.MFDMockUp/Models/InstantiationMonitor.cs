using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Assisticant.Fields;

using MattEland.Ani.Alfred.Core.Definitions;
using MattEland.Ani.Alfred.MFDMockUp.ViewModels;
using MattEland.Ani.Alfred.MFDMockUp.ViewModels.Widgets;
using MattEland.Common.Annotations;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    /// <summary>
    ///     An instantiation monitor that alerts when large amounts of instances have been created.
    /// </summary>
    public sealed class InstantiationMonitor
    {
        /// <summary>
        ///     The new items since the last reset.
        /// </summary>
        private int _newItemsSinceReset = 0;

        [NotNull]
        private readonly Observable<int> _newItemsLastMeasurement;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:InstantiationMonitor"/> class.
        /// </summary>
        public InstantiationMonitor()
        {
            _newItemsLastMeasurement = new Observable<int>(0);

            WidgetViewModelFactory.WidgetViewModelCreated += OnWidgetViewModelCreated;
        }

        /// <summary>
        ///     Responds to the creation of new widget view models.
        /// </summary>
        /// <param name="sender"> Source of the event. </param>
        /// <param name="widgetViewModel"> The widget view model. </param>
        private void OnWidgetViewModelCreated([CanBeNull] object sender,
            [NotNull] WidgetViewModel widgetViewModel)
        {
            NotifyItemCreated(widgetViewModel);
        }

        /// <summary>
        ///     Notifies that a new item was created.
        /// </summary>
        /// <param name="item"> The item. </param>
        public void NotifyItemCreated(object item)
        {
            if (item != null)
            {
                _newItemsSinceReset += 1;
            }
        }

        /// <summary>
        ///     Resets the count of items created and .
        /// </summary>
        public void ResetCount()
        {
            // Store our count of new items since the last measurement
            NewItemsLastMeasurement = _newItemsSinceReset;

            _newItemsSinceReset = 0;
        }

        /// <summary>
        ///     Gets or sets the new items created during the last measurement.
        /// </summary>
        /// <value>
        ///     The new items created during the last measurement.
        /// </value>
        public int NewItemsLastMeasurement
        {
            get { return _newItemsLastMeasurement.Value; }
            set { _newItemsLastMeasurement.Value = value; }
        }

    }
}
