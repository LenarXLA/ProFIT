using System.Windows;

namespace ProFIT
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new ApplicationViewModel();
        }
    }
}
