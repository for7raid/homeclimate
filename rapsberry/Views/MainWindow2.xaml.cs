using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace homeclimate.Views
{
    public class MainWindow2 : Window
    {
        public MainWindow2()
        {
            InitializeComponent();

#if DEBUG
            this.AttachDevTools(new Avalonia.Input.KeyGesture( Avalonia.Input.Key.F12, Avalonia.Input.KeyModifiers.Control));
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}