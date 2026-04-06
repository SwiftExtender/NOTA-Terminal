using Avalonia.Controls;
using Avalonia.Input;
using NOTATerminal.ViewModels;

namespace NOTATerminal.Views
{
    public partial class TerminalCustomControl : UserControl
    {
        public TerminalCustomControl(string cwd = "")
        {
            InitializeComponent();
            DataContext = new TerminalViewModel();
            
            this.Loaded += (s, e) =>
            {
                if (cwd != "")
                {
                    TabTerminal.Terminal.CurrentDirectory = cwd;
                }
                TabTerminal.Focus();
            };
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
        public void Closing()
        {
            TabTerminal.Kill();
        }
    }
}
