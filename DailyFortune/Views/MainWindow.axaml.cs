using Avalonia.Controls;

namespace DailyFortune.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Closing += (_, e) =>
            {
                e.Cancel = true;
                Hide();
            };
        }
    }
}