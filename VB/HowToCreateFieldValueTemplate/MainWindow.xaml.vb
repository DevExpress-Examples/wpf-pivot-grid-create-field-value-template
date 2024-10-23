Imports System.Windows

Namespace HowToCreateFieldValueTemplate

    ''' <summary>
    ''' Interaction logic for MainWindow.xaml
    ''' </summary>
    Public Partial Class MainWindow
        Inherits Window

        Private salesPersonDataTable As SalesPerson.SalesPersonDataTable = New SalesPerson.SalesPersonDataTable()

        Private salesPersonDataAdapter As SalesPersonTableAdapters.SalesPersonTableAdapter = New SalesPersonTableAdapters.SalesPersonTableAdapter()

        Public Sub New()
            Me.InitializeComponent()
            Me.pivotGridControl1.DataSource = salesPersonDataTable
        End Sub

        Private Sub Window_Loaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            salesPersonDataAdapter.Fill(salesPersonDataTable)
        End Sub
    End Class
End Namespace
