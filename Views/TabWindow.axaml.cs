using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using AvaloniaEdit.Folding;
using System;
using NOTATerminal.ViewModels;

namespace NOTATerminal.Views
{
    public partial class TabWindow : UserControl
    {
        public TabWindow()
        {
            InitializeComponent();
            DataContext = new TabWindowViewModel();
            //RowTip.Text = "-";
            //ColumnTip.Text = "-";
            //AddHandler(PointerWheelChangedEvent, MouseWheelFontSizer, RoutingStrategies.Tunnel, true);
            //AddHandler(KeyDownEvent, KeyboardFontSizer, RoutingStrategies.Tunnel, true);
            //var fm = TextEditingControl.TextArea.TextView.GetService(typeof(FoldingManager)) as FoldingManager;
            //if (fm != null) {
            //    FoldingManager.Uninstall(fm);
            //}
        }
        private void MouseWheelFontSizer(object? sender, PointerWheelEventArgs e)
        {
            if (e.KeyModifiers != KeyModifiers.Control) return;
            if (e.Delta.Y > 0) FontSize = FontSize < 74 ? FontSize + 1 : 74;
            else FontSize = FontSize > 9 ? FontSize - 1 : 9;
            e.Handled = true;
        }
        private void KeyboardFontSizer(object sender, KeyEventArgs e)
        {
            if (e.KeyModifiers.HasFlag(KeyModifiers.Control))
            {
                if (e.PhysicalKey == PhysicalKey.Equal || e.PhysicalKey == PhysicalKey.NumPadAdd)
                {
                    FontSize = FontSize < 74 ? FontSize + 1 : 74;
                }
                else if (e.PhysicalKey == PhysicalKey.Minus || e.PhysicalKey == PhysicalKey.NumPadSubtract)
                {
                    FontSize = FontSize > 9 ? FontSize - 1 : 9;
                }
            }
        }
        //private void CaretPositionChanged(object sender, EventArgs e)
        //{
        //    RowTip.Text = TextEditingControl.TextArea.Caret.Line.ToString();
        //    ColumnTip.Text = TextEditingControl.TextArea.Caret.Column.ToString();
        //}
    }
}