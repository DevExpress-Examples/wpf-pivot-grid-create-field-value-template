using System.Data;
using System.Data.OleDb;
using System.Windows;
using DevExpress.Xpf.PivotGrid;
using HowtoCreateaFieldValueTemlate.SalesPersonTableAdapters;

namespace HowtoCreateaFieldValueTemlate {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        SalesPerson.SalesPersonDataTable salesPersonDataTable = new SalesPerson.SalesPersonDataTable();
        SalesPersonTableAdapter salesPersonDataAdapter = new SalesPersonTableAdapter();

        public MainWindow() {
            InitializeComponent();
            pivotGridControl1.DataSource = salesPersonDataTable;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            salesPersonDataAdapter.Fill(salesPersonDataTable);
        }

        private void Window_RequestNavigate(object sender, 
											System.Windows.Navigation.RequestNavigateEventArgs e) {

        }
    }
}
