Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Windows
Imports DevExpress.Xpf.PivotGrid
Imports HowtoCreateaFieldValueTemlate.SalesPersonTableAdapters

Namespace HowtoCreateaFieldValueTemlate
    Partial Public Class MainWindow
        Inherits Window
        Private salesPersonDataTable As New SalesPerson.SalesPersonDataTable()
        Private salesPersonDataAdapter As New SalesPersonTableAdapter()

        Public Sub New()
            InitializeComponent()
            pivotGridControl1.DataSource = salesPersonDataTable
        End Sub

        Private Sub Window_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            salesPersonDataAdapter.Fill(salesPersonDataTable)
        End Sub

        Private Sub Window_RequestNavigate(ByVal sender As Object, _
			ByVal e As System.Windows.Navigation.RequestNavigateEventArgs)
        End Sub
    End Class
End Namespace

