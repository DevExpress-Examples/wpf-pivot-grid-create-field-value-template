Imports System.Windows

Namespace HowToCreateFieldValueTemplate
    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Partial Public Class MainWindow
        Inherits Window

        Private salesPersonDataTable As New SalesPerson.SalesPersonDataTable()
        Private salesPersonDataAdapter As New SalesPersonTableAdapters.SalesPersonTableAdapter()

        Public Sub New()
            InitializeComponent()
            pivotGridControl1.DataSource = salesPersonDataTable
        End Sub

        Private Sub Window_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            salesPersonDataAdapter.Fill(salesPersonDataTable)
        End Sub
    End Class
End Namespace
