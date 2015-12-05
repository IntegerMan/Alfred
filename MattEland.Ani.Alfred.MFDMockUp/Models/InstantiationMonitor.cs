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

        private static InstantiationMonitor _instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:InstantiationMonitor"/> class.
        /// </summary>
        private InstantiationMonitor()
        {
            _newItemsLastMeasurement = new Observable<int>(0);
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

        /// <summary>
        ///     Gets the instantiation monitor instance.
        /// </summary>
        /// <value>
        ///     The instantiation monitor instance.
        /// </value>
        [NotNull]
        public static InstantiationMonitor Instance
        {
            get
            {
                Contract.Ensures(Contract.Result<InstantiationMonitor>() != null);
                Contract.Ensures(_instance != null);

                // Lazy load instance as needed
                return _instance ?? (_instance = new InstantiationMonitor());
            }
        }

    }
}
