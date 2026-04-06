using Avalonia;
using NOTATerminal;
using NOTATerminal.ViewModels;
using ReactiveUI;
using System.Reactive;

namespace NOTATerminal.ViewModels
{
    public class SettingsWindowViewModel : ViewModelBase
    {
        public string NewTabFontSize
        {
            get => (Application.Current as App).Settings.NewTabFontSize;
            set { (Application.Current as App).Settings.NewTabFontSize = value; }
        }
        public string MainWindowColor
        {
            get => (Application.Current as App).Settings.MainWindowColor;
            set { (Application.Current as App).Settings.MainWindowColor = value; }
        }
        public string TabWindowColor
        {
            get => (Application.Current as App).Settings.TabWindowColor;
            set { (Application.Current as App).Settings.TabWindowColor = value; }
        }
        public string TabWindowTextColor
        {
            get => (Application.Current as App).Settings.TabWindowTextColor;
            set { (Application.Current as App).Settings.TabWindowTextColor = value; }
        }
        private string _SettingsWindowColor = "";
        public string SettingsWindowColor
        {
            get { return _SettingsWindowColor; }
            set => this.RaiseAndSetIfChanged(ref _SettingsWindowColor, value);
        }
        public void SaveSettings()
        {
            (Application.Current as App).Settings.MainWindowColor = MainWindowColor;
            (Application.Current as App).Settings.NewTabFontSize = NewTabFontSize;
            (Application.Current as App).Settings.TabWindowColor = TabWindowColor;
            (Application.Current as App).Settings.TabWindowTextColor = TabWindowTextColor;
            (Application.Current as App).Settings.SettingsWindowColor = SettingsWindowColor;
            (Application.Current as App).SettingsService.Save((Application.Current as App).Settings);
        }
        public ReactiveCommand<Unit, Unit> SaveCommand { get; }
        public SettingsWindowViewModel()
        {
            SaveCommand = ReactiveCommand.Create(SaveSettings);
            SettingsWindowColor = (Application.Current as App).Settings.SettingsWindowColor;
        }
    }
}