using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MattEland.Ani.Alfred.MFDMockUp.Models;
using Assisticant;

namespace MattEland.Ani.Alfred.MFDMockUp.ViewModels
{
    public class MainViewModel
    {

        private readonly MFDSelection _selection;
        private readonly Workspace _workspace;

        public MainViewModel(Workspace workspace)
        {
            _workspace = workspace;
            _selection = workspace.MFDSelection;
        }

        public IEnumerable<MFDViewModel> MultifunctionDisplays
        {
            get
            {
                return
                    from item in _workspace.MFDs
                    select new MFDViewModel(item);
            }
        }

        public MFDViewModel SelectedItem
        {
            get
            {
                return _selection.SelectedMFD == null
                    ? null
                    : new MFDViewModel(_selection.SelectedMFD);
            }
            set
            {
                if (value != null)
                    _selection.SelectedMFD = value.Model;
            }
        }

        public MFDViewModel ItemDetail
        {
            get
            {
                return _selection.SelectedMFD == null
                    ? null
                    : new MFDViewModel(_selection.SelectedMFD);
            }
        }

        public ICommand AddItem
        {
            get
            {
                return MakeCommand
                    .Do(delegate
                    {
                        _selection.SelectedMFD = _workspace.NewMFD();
                    });
            }
        }

        public ICommand DeleteItem
        {
            get
            {
                return MakeCommand
                    .When(() => _selection.SelectedMFD != null)
                    .Do(delegate
                    {
                        _workspace.DeleteMFD(_selection.SelectedMFD);
                        _selection.SelectedMFD = null;
                    });
            }
        }

        public ICommand MoveItemDown
        {
            get
            {
                return MakeCommand
                    .When(() =>
                        _selection.SelectedMFD != null &&
                        _workspace.CanMoveDown(_selection.SelectedMFD))
                    .Do(delegate
                    {
                        _workspace.MoveDown(_selection.SelectedMFD);
                    });
            }
        }

        public ICommand MoveItemUp
        {
            get
            {
                return MakeCommand
                    .When(() =>
                        _selection.SelectedMFD != null &&
                        _workspace.CanMoveUp(_selection.SelectedMFD))
                    .Do(delegate
                    {
                        _workspace.MoveUp(_selection.SelectedMFD);
                    });
            }
        }
    }
}
