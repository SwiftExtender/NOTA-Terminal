using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using System;
using System.IO;
using NOTATerminal.ViewModels;

namespace NOTATerminal.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Name = "TheHighestWindow";
            InitializeComponent();
            AddTab();
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
        public void AddTabClick(object sender, RoutedEventArgs args)
        {
            AddTab();
        }
        private void AddTab()
        {
            string header = "Terminal " + HighestMultiTab.Items.Count;
            GridTerminal content = new GridTerminal();
            DockPanel panel = new DockPanel();
            panel.Children.Add(new Label() { Content = header });
            Button btn = AddTabDeleteButton();
            panel.Children.Add(btn);
            TabItem newItem = new TabItem()
            {
                Header = panel,
                Content = content,
            };
            HighestMultiTab.Items.Add(newItem);
            newItem.IsSelected = true;
            newItem.Focus();
        }
        private void MacrosOpenWindow_Clicked(object sender, RoutedEventArgs args)
        {
            MacrosCodeWindow w1 = new MacrosCodeWindow() { DataContext = new MacrosWindowViewModel(), WindowState = WindowState.Maximized };
            w1.Show();
        }
        //private async void SaveFile_Clicked(object sender, RoutedEventArgs args)
        //{
        //    TabItem tab = GetActiveTab();
        //    GridTerminal? tabWindow = tab.Content as GridTerminal; //getting child GridTerminal
        //    TabWindowViewModel? tabWindowViewModel = tabWindow.DataContext as TabWindowViewModel;
        //    if (tabWindowViewModel.FileFullPath == "")
        //    {
        //        TopLevel topLevel = GetTopLevel(this);
        //        FilePickerSaveOptions saveOptions = new FilePickerSaveOptions { Title = "Save new file" };
        //        IStorageFile file = await topLevel.StorageProvider.SaveFilePickerAsync(saveOptions);
        //        if (file != null)
        //        {
        //            try
        //            {
        //                await using (var stream = await file.OpenWriteAsync())
        //                {
        //                    using (StreamWriter writer = new StreamWriter(stream))
        //                    {
        //                        await writer.WriteAsync(tabWindowViewModel.RawText.Text);
        //                    }
        //                }

        //                DockPanel panel = new DockPanel();
        //                panel.Children.Add(new Label() { Content = file.Name });
        //                Button btn = AddTabDeleteButton();
        //                panel.Children.Add(btn);
        //                tab.Header = panel;
        //                file.Dispose();
        //                string createdFilePath = file.TryGetLocalPath();
        //                tabWindowViewModel.StatusText = "New file saved " + createdFilePath;
        //                tabWindowViewModel.FileFullPath = createdFilePath;
        //            }
        //            catch (Exception e)
        //            {
        //                tabWindowViewModel.StatusText = "Exception: New file saving error " + e.ToString();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        try
        //        {
        //            File.WriteAllText(tabWindowViewModel.FileFullPath, tabWindowViewModel.RawText.Text);
        //            tabWindowViewModel.StatusText = "File saved " + tabWindowViewModel.FileFullPath;
        //        }
        //        catch (Exception e)
        //        {
        //            tabWindowViewModel.StatusText = "Exception: file saving error " + e.ToString();
        //        }
        //    }
        //}
    }
}