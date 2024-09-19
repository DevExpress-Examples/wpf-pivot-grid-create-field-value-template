using System.Data;
using System.Windows;

namespace HowToCreateFieldValueTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {

        DataTable salesPersonDataTable = new DataTable();

        public MainWindow() {
            InitializeComponent();
            pivotGridControl1.DataSource = salesPersonDataTable;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            DataSourceHelper dataSourceHelper = new DataSourceHelper();
            dataSourceHelper.FillSalesPerson(salesPersonDataTable);
        }
    }
}
