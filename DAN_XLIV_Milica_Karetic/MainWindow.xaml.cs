using DAN_XLIV_Milica_Karetic.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace DAN_XLIV_Milica_Karetic
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string quantity = " ";
        public static int totalPrice = 0;


        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainWindowViewModel(this);
        }

        private void TxtQuantity_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextBox objTextBox = (TextBox)sender;
            quantity = objTextBox.Text;
        }

        private void TxtTotalPrice_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox objTextBox = (TextBox)sender;
            totalPrice = int.Parse(objTextBox.Text);
            
            
        }

    }
}
