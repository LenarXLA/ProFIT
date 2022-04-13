using System.Windows;

namespace ProFIT
{
    public partial class AddingDataFitnes : Window
    {
        public FitnesData FitnesData { get; private set; }

        public AddingDataFitnes(FitnesData fitnesData)
        {
            InitializeComponent();

            FitnesData = fitnesData;
            this.DataContext = FitnesData;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
