using Assisticant.Collections;
using Assisticant.Fields;
using MattEland.Common.Annotations;
using System.Collections.Generic;

namespace MattEland.Ani.Alfred.MFDMockUp.Models
{
    [PublicAPI]
    public sealed class Workspace
    {
        /// <summary>
        /// The default workspace name. 
        /// </summary>
        private const string DefaultWorkspaceName = "Workspace";

        private readonly ObservableList<MultifunctionDisplay> _mfds = new ObservableList<MultifunctionDisplay>();

        private readonly Observable<MFDSelection> _mfdSelection = new Observable<MFDSelection>();

        /// <summary>
        /// The name's observable backing store. 
        /// </summary>
        private readonly Observable<string> _name = new Observable<string>(DefaultWorkspaceName);

        /// <summary>
        /// Gets the multifunction displays (MFDs). 
        /// </summary>
        /// <value> The displays. </value>
        [NotNull, ItemNotNull]
        public IEnumerable<MultifunctionDisplay> MFDs
        {
            get { return _mfds; }
        }

        /// <summary>
        /// Gets or sets the MFD selection governing which display is selected. 
        /// </summary>
        /// <value> The mfd selection. </value>
        [NotNull]
        public MFDSelection MFDSelection
        {
            get
            {
                return _mfdSelection;
            }
            set
            {
                _mfdSelection.Value = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the <see cref="Workspace"/>. 
        /// </summary>
        /// <value> The name of the workspace. </value>
        [NotNull]
        public string Name
        {
            get { return _name; }
            set { _name.Value = value; }
        }

        /// <summary>
        /// Determine if the <paramref name="mfd"/> can move down. 
        /// </summary>
        /// <param name="mfd"> The mfd. </param>
        /// <returns> <c> True </c> if the mfd can move down, <c> False </c> if not. </returns>
        public bool CanMoveDown([NotNull] MultifunctionDisplay mfd)
        {
            return _mfds.IndexOf(mfd) < _mfds.Count - 1;
        }

        /// <summary>
        /// Determine if the <paramref name="mfd"/> can move up. 
        /// </summary>
        /// <param name="mfd"> The mfd. </param>
        /// <returns> <c> True </c> if the mfd can move up, <c> False </c> if not. </returns>
        public bool CanMoveUp([NotNull] MultifunctionDisplay mfd)
        {
            return _mfds.IndexOf(mfd) > 0;
        }

        /// <summary>
        /// Deletes the specified <paramref name="mfd"/>. 
        /// </summary>
        /// <param name="mfd"> The mfd. </param>
        public void DeleteMFD([NotNull] MultifunctionDisplay mfd)
        {
            _mfds.Remove(mfd);
        }

        public void MoveDown([NotNull] MultifunctionDisplay mfd)
        {
            var index = _mfds.IndexOf(mfd);

            _mfds.RemoveAt(index);
            _mfds.Insert(index + 1, mfd);
        }

        public void MoveUp([NotNull] MultifunctionDisplay mfd)
        {
            var index = _mfds.IndexOf(mfd);

            _mfds.RemoveAt(index);
            _mfds.Insert(index - 1, mfd);
        }

        /// <summary>
        /// Creates a new multifunction display (MFD). 
        /// </summary>
        /// <returns> A new display. </returns>
        [NotNull]
        public MultifunctionDisplay NewMFD()
        {
            var item = new MultifunctionDisplay();
            _mfds.Add(item);
            return item;
        }
    }
}