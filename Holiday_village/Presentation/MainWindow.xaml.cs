using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// This is the main window which on run displays window with options to choose either booking section or customer section.
    /// </summary>
    

    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        

        private void bt_CustomerSection_Click(object sender, RoutedEventArgs e)
        {
            CustomerWindow openCustomer = new CustomerWindow();
            openCustomer.Owner = this;
            openCustomer.ShowDialog();

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void bt_BookingSection_Click(object sender, RoutedEventArgs e)
        {
            BookingWindow openBooking = new BookingWindow();
            openBooking.Owner = this;
            openBooking.ShowDialog();
        }
    }
}
