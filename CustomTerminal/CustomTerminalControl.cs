using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Iciclecreek.Terminal
{
    public class CustomTerminalControl : TemplatedControl
    {
        private CustomTerminalView? _terminalView;
        private ScrollBar? _scrollBar;

        public static readonly StyledProperty<TextDecorationLocation?> TextDecorationsProperty =
            AvaloniaProperty.Register<CustomTerminalControl, TextDecorationLocation?>(
                nameof(TextDecorations),
                defaultValue: null);

        public static readonly StyledProperty<IBrush> SelectionBrushProperty =
            AvaloniaProperty.Register<CustomTerminalControl, IBrush>(
                nameof(SelectionBrush),
                defaultValue: new SolidColorBrush(Color.FromArgb(128, 0, 120, 215)));

        public static readonly StyledProperty<string> ProcessProperty =
            AvaloniaProperty.Register<CustomTerminalControl, string>(
                nameof(Process),
                defaultValue: RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "cmd.exe" : "bash");

        public static readonly StyledProperty<IList<string>> ArgsProperty =
            AvaloniaProperty.Register<CustomTerminalControl, IList<string>>(
                nameof(Args),
                defaultValue: System.Array.Empty<string>());

        public static readonly StyledProperty<int> BufferSizeProperty =
                  AvaloniaProperty.Register<CustomTerminalControl, int>(
                      nameof(BufferSize),
                      defaultValue: 1000);

        public static readonly StyledProperty<XTerm.Options.TerminalOptions?> OptionsProperty =
            AvaloniaProperty.Register<CustomTerminalControl, XTerm.Options.TerminalOptions?>(
                nameof(Options),
                defaultValue: null);

        public IBrush SelectionBrush
        {
            get => GetValue(SelectionBrushProperty);
            set => SetValue(SelectionBrushProperty, value);
        }

        public string Process
        {
            get => GetValue(ProcessProperty);
            set => SetValue(ProcessProperty, value);
        }

        public IList<string> Args
        {
            get => GetValue(ArgsProperty);
            set => SetValue(ArgsProperty, value);
        }


        public int BufferSize
        {
            get => GetValue(BufferSizeProperty);
            set => SetValue(BufferSizeProperty, value);
        }

        public XTerm.Options.TerminalOptions? Options
        {
            get => GetValue(OptionsProperty);
            set => SetValue(OptionsProperty, value);
        }

        private static bool _stylesLoaded = false;

        static CustomTerminalControl()
        {

            // TerminalControl is focusable - it will delegate to inner TerminalView
            FocusableProperty.OverrideDefaultValue<CustomTerminalControl>(true);
        }

        public CustomTerminalControl()
        {
        }

        public XTerm.Terminal Terminal => _terminalView!.Terminal;


        public void WaitForExit(int ms) => _terminalView!.WaitForExit(ms);

        public void Kill() => _terminalView!.Kill();

        public int ExitCode => _terminalView!.ExitCode;

        public int Pid => _terminalView!.Pid;

        protected override void OnGotFocus(GotFocusEventArgs e)
        {
            base.OnGotFocus(e);

            // Only focus the inner TerminalView if it doesn't already have focus
            if (_terminalView != null && !_terminalView.IsFocused)
            {
                // Defer until layout is ready
                Dispatcher.UIThread.Post(() =>
                {
                    if (_terminalView != null && !_terminalView.IsFocused)
                    {
                        _terminalView.Focus();
                    }
                }, DispatcherPriority.Input);
            }
        }

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            // Ensure styles are loaded (handles case where static constructor ran before Application was ready)
            //LoadDefaultStyles();

            base.OnApplyTemplate(e);

            // Unsubscribe from old controls
            if (_scrollBar != null)
            {
                _scrollBar.Scroll -= OnScrollBarScroll;
            }

            if (_terminalView != null)
            {
                _terminalView.PropertyChanged -= OnTerminalViewPropertyChanged;
            }

            // Get template parts
            _terminalView = e.NameScope.Find<CustomTerminalView>("PART_CustomTerminalView");
            _scrollBar = e.NameScope.Find<ScrollBar>("PART_ScrollBar");

            // Wire up scrollbar
            if (_scrollBar != null && _terminalView != null)
            {
                _scrollBar.Scroll += OnScrollBarScroll;
                _terminalView.Options = Options ?? new XTerm.Options.TerminalOptions();
                _terminalView.PropertyChanged += OnTerminalViewPropertyChanged;
                // (no window event hooking needed)
            }
        }

        private void OnScrollBarScroll(object? sender, ScrollEventArgs e)
        {
            if (_terminalView != null)
            {
                _terminalView.ViewportY = (int)e.NewValue;
            }
        }

        private void OnTerminalViewPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.Property == CustomTerminalView.MaxScrollbackProperty ||
                e.Property == CustomTerminalView.ViewportLinesProperty ||
                e.Property == CustomTerminalView.ViewportYProperty ||
                e.Property == CustomTerminalView.IsAlternateBufferProperty)
            {
                UpdateScrollBar();
            }
        }

        private void UpdateScrollBar()
        {
            if (_scrollBar == null || _terminalView == null)
                return;

            if (_terminalView.IsAlternateBuffer)
            {
                _scrollBar.IsVisible = false;
                _scrollBar.Value = 0;
                return;
            }

            var maxScrollback = _terminalView.MaxScrollback;
            var viewportLines = _terminalView.ViewportLines;
            var currentScroll = _terminalView.ViewportY;

            // Scrollbar range: 0 (top of buffer) to maxScrollback (bottom/current output)
            _scrollBar.Minimum = 0;
            _scrollBar.Maximum = maxScrollback;
            _scrollBar.ViewportSize = viewportLines;
            _scrollBar.Value = currentScroll;
            _scrollBar.IsVisible = maxScrollback > 0;
        }
    }
}