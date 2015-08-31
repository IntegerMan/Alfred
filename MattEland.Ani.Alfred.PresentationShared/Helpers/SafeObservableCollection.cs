// ---------------------------------------------------------
// SafeObservableCollection.cs
// 
// Created on:      08/31/2015 at 12:07 PM
// Last Modified:   08/31/2015 at 12:07 PM
// 
// Last Modified by: Matt Eland
// ---------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows.Threading;

using JetBrains.Annotations;

namespace MattEland.Ani.Alfred.PresentationShared.Helpers
{
    /// <summary>
    ///     A thread-safe observable collection.
    /// </summary>
    /// <typeparam name="T"> The type of item the collection contains. </typeparam>
    public class SafeObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        ///     The threading <see langword="lock"/> <see langword="object"/>.
        /// </summary>
        [NotNull]
        private readonly object _lock = new object();

        /// <summary>
        ///     This <see langword="private"/> variable holds the flag to turn on and off the collection
        ///     changed notification.
        /// </summary>
        private bool _suspendCollectionChangeNotification;

        /// <summary>
        /// Initializes a new instance of the <see cref="SafeObservableCollection{T}"/> class.
        /// </summary>
        public SafeObservableCollection() : base()
        {
            _suspendCollectionChangeNotification = false;
        }

        /// <summary>
        /// This event overrides CollectionChanged event of the observable collection.
        /// </summary>
        public override event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        ///     This method adds the given generic list of <paramref name="items"/> as a range into
        ///     current collection by casting them as type T. It then notifies once after all
        ///     <paramref name="items"/> are added.
        /// </summary>
        /// <param name="items">The source collection.</param>
        public void AddItems(IEnumerable<T> items)
        {
            lock (_lock)
            {
                SuspendCollectionChangeNotification();

                foreach (var i in items)
                {
                    InsertItem(Count, i);
                }

                NotifyChanges();
            }
        }

        /// <summary>
        /// Raises collection change event.
        /// </summary>
        public void NotifyChanges()
        {
            ResumeCollectionChangeNotification();
            var arg = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            OnCollectionChanged(arg);
        }

        /// <summary>
        /// This method removes the given generic list of items as a range
        /// into current collection by casting them as type T.
        /// It then notifies once after all items are removed.
        /// </summary>
        /// <param name="items">The source collection.</param>
        public void RemoveItems(IList<T> items)
        {
            lock (_lock)
            {
                SuspendCollectionChangeNotification();
                foreach (var i in items) { Remove(i); }
                NotifyChanges();
            }
        }

        /// <summary>
        /// Resumes collection changed notification.
        /// </summary>
        public void ResumeCollectionChangeNotification()
        {
            _suspendCollectionChangeNotification = false;
        }

        /// <summary>
        /// Suspends collection changed notification.
        /// </summary>
        public void SuspendCollectionChangeNotification()
        {
            _suspendCollectionChangeNotification = true;
        }

        /// <summary>
        ///     This collection changed event performs thread safe event raising.
        /// </summary>
        /// <param name="e"> The event argument. </param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            /* Recommended approach is to avoid reentry in collection changed event while 
            collection is getting changed on other thread. */
            using (BlockReentrancy())
            {
                if (!_suspendCollectionChangeNotification)
                {
                    NotifyCollectionChangedEventHandler eventHandler = CollectionChanged;
                    if (eventHandler == null) { return; }

                    // Walk through the invocation list.
                    Delegate[] delegates = eventHandler.GetInvocationList();

                    foreach (var @delegate in delegates)
                    {
                        var handler = @delegate as NotifyCollectionChangedEventHandler;
                        if (handler == null) { continue; }

                        // If the subscriber is a DispatcherObject and different thread.
                        var dispatcherObject = handler.Target as DispatcherObject;

                        if (dispatcherObject != null && !dispatcherObject.CheckAccess())
                        {
                            // Invoke handler in the target dispatcher's thread... 
                            // asynchronously for better responsiveness.
                            var dispatcher = dispatcherObject.Dispatcher;
                            dispatcher?.BeginInvoke(DispatcherPriority.DataBind, handler, this, e);
                        }
                        else
                        {
                            // Execute handler as is.
                            handler(this, e);
                        }
                    }
                }
            }
        }

    }
}