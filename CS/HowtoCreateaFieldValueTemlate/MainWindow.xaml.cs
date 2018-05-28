using System.Windows;

namespace HowToCreateFieldValueTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        SalesPerson.SalesPersonDataTable salesPersonDataTable = new SalesPerson.SalesPersonDataTable();
        SalesPersonTableAdapters.SalesPersonTableAdapter salesPersonDataAdapter = 
            new SalesPersonTableAdapters.SalesPersonTableAdapter();

        public MainWindow() {
            InitializeComponent();
            pivotGridControl1.DataSource = salesPersonDataTable;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            salesPersonDataAdapter.Fill(salesPersonDataTable);
        }
    }
}
