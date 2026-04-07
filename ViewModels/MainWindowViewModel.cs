using NOTATerminal.Models;
using ReactiveUI;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NOTATerminal.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private bool _IsPinnedWindow = false;
        public bool IsPinnedWindow
        {
            get => _IsPinnedWindow;
            set => this.RaiseAndSetIfChanged(ref _IsPinnedWindow, value);
        }
        private ObservableCollection<FavoriteCommandModel> _FavoriteCommandMenu;
        public ObservableCollection<FavoriteCommandModel> FavoriteCommandMenu
        {
            get => _FavoriteCommandMenu;
            set => this.RaiseAndSetIfChanged(ref _FavoriteCommandMenu, value);
        }
        public MainWindowViewModel()
        {
            using (var DataSource = new HelpContext())
            {
                List<FavoriteCommandModel> favMenu = DataSource.FavoriteCommandsTable.ToList();
                FavoriteCommandMenu = new ObservableCollection<FavoriteCommandModel>(favMenu);
            }
        }
    }
}
