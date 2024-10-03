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
using System.Windows.Shapes;

namespace InterportCargoWPF
{
    /// <summary>
    /// Interaction logic for QuotationWindow.xaml
    /// </summary>
    public partial class QuotationWindow : Window
    {
        public QuotationWindow()
        {
            InitializeComponent();
        }
        private void SubmitQuotation_Click(object sender, RoutedEventArgs e)
        {
            string source = SourceBox.Text;
            string destination = DestinationBox.Text;
            int containers;
            if (!int.TryParse(ContainersBox.Text, out containers))
            {
                MessageBox.Show("Please enter a valid number of containers.");
                return;
            }
            string job = JobBox.Text;

            // Simulate submitting the quotation
            MessageBox.Show($"Quotation submitted:\nSource: {source}\nDestination: {destination}\nContainers: {containers}\nJob: {job}");
        }


    }
}
