using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using ReactiveUI;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Avalonia.Platform.Storage;
using NOTATerminal.ViewModels;

namespace NOTATerminal.Views
{
    public partial class MainWindow : Window, IActivatableView
    {
        private SettingsWindow settingsWindow;
        public MainWindow()
        {
            Name = "TheHighestWindow";
            InitializeComponent();
            this.WhenActivated(disposables =>
            {
                (Application.Current as App).Settings.
                WhenAnyValue(x => x.MainWindowColor).
                Subscribe<string>(onNext: s =>
                {
                    this.Background = Brush.Parse(s);
                });
            });
            NewTerminal();
        }
        private TabItem GetActiveTab()
        {
            return (TabItem)HighestMultiTab.SelectedItem;
        }
        private Button AddTabDeleteButton()
        {
            Button btn = new Button() { Content = "X" };
            btn.Click += (sender, e) =>
            {
                if (sender is Button btn && btn.Parent is DockPanel dckPanel && dckPanel.Parent is TabItem titem)
                {

                    HighestMultiTab.Items.Remove(titem);
                }
            };
            return btn;
        }
        public void NewTerminalClick(object sender, RoutedEventArgs args)
        {
            NewTerminal();
        }
        private void NewTerminal(string folder = "")
        {
            string header = "Terminal " + HighestMultiTab.Items.Count;
            TerminalCustomControl content = new TerminalCustomControl(folder);
            DockPanel panel = new DockPanel();
            panel.Children.Add(new Label() { Content = header });
            Button btn = AddTabDeleteButton();
            panel.Children.Add(btn);
            TabItem newItem = new TabItem()
            {
                Header = panel,
                Content = content,
            };
            newItem.DetachedFromLogicalTree += (sender, e) =>
            {
                if (sender is TabItem titem && titem.Content is TerminalCustomControl customTerminal) customTerminal.Closing();
            };
            HighestMultiTab.Items.Add(newItem);
            newItem.IsSelected = true;
            newItem.Focus();
        }
        //public void NewGridTerminalClick(object sender, RoutedEventArgs args)
        //{
        //    NewGridTerminal();
        //}
        //private void NewGridTerminal(string folder = "")
        //{
        //    string header = "Grid Terminal " + HighestMultiTab.Items.Count;
        //    GridTerminal content = new GridTerminal();
        //    DockPanel panel = new DockPanel();
        //    panel.Children.Add(new Label() { Content = header });
        //    Button btn = AddTabDeleteButton();
        //    panel.Children.Add(btn);
        //    TabItem newItem = new TabItem()
        //    {
        //        Header = panel,
        //        Content = content,
        //    };
        //    HighestMultiTab.Items.Add(newItem);
        //    newItem.IsSelected = true;
        //    newItem.Focus();
        //}
        public async void NewTerminalInFolderClick(object sender, RoutedEventArgs args)
        {
            TopLevel topLevel = GetTopLevel(this);
            var folder = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
            {
                Title = "Choose folder",
                AllowMultiple = false
            });
            if (folder.Count == 1)
            {
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    NewTerminal(folder[0].TryGetLocalPath());
                });
            }
        }
        //public async void NewGridTerminalInFolderClick(object sender, RoutedEventArgs args)
        //{
        //    TopLevel topLevel = GetTopLevel(this);
        //    var folder = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        //    {
        //        Title = "Choose folder",
        //        AllowMultiple = false
        //    });
        //    if (folder != null)
        //    {
        //        await Dispatcher.UIThread.InvokeAsync(() =>
        //        {
        //            NewGridTerminal();
        //        });
        //    }
        //}
        private void SettingsOpenWindow_Clicked(object sender, RoutedEventArgs args)
        {
            if (settingsWindow != null)
            { 
                settingsWindow.Activate();
            } else
            {
                settingsWindow = new SettingsWindow() { DataContext = new SettingsWindowViewModel(), WindowState = WindowState.Maximized };
                settingsWindow.Closed += (s, e) => settingsWindow = null;
                settingsWindow.Show();
            }
                
        }
    }
}