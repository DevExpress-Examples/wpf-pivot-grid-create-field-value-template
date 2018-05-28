Imports DevExpress.Xpf.PivotGrid
Imports HowToCreateFieldValueTemplate.CategoryPicturesTableAdapters
Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media
Imports System.Windows.Media.Imaging

Namespace HowToCreateFieldValueTemplate
    Public Class CategoriesControl
        Inherits Control
        Implements IWeakEventListener

        #Region "static staff"

        Private Shared categoriesTableAdapter_Renamed As CategoriesTableAdapter
        Private Shared categoriesPictures As Dictionary(Of String, ImageSource)
        Public Shared ReadOnly ImageSourceProperty As DependencyProperty

        Shared Sub New()
            DefaultStyleKeyProperty.OverrideMetadata(GetType(CategoriesControl), New FrameworkPropertyMetadata(GetType(CategoriesControl)))
            Dim ownerType As Type = GetType(CategoriesControl)
            ImageSourceProperty = DependencyProperty.Register("ImageSource", GetType(ImageSource), ownerType, New UIPropertyMetadata())
            categoriesPictures = New Dictionary(Of String, ImageSource)()
        End Sub

        Private Shared ReadOnly Property CategoriesTableAdapter() As CategoriesTableAdapter
            Get
                If categoriesTableAdapter_Renamed Is Nothing Then
                    categoriesTableAdapter_Renamed = New CategoriesTableAdapter()
                End If
                Return categoriesTableAdapter_Renamed
            End Get
        End Property

        Private Shared Function GetImage(ByVal categoryName As String) As ImageSource
            If String.IsNullOrEmpty(categoryName) Then
                Return Nothing
            End If
            If categoriesPictures.ContainsKey(categoryName) Then
                Return categoriesPictures(categoryName)
            Else
                Dim icon() As Byte = TryCast(HowToCreateFieldValueTemplate.CategoryPicturesTableAdapters.CategoriesTableAdapter.GetIconImageByName(categoryName), Byte())
                If icon Is Nothing OrElse icon.Length = 0 Then
                    Return Nothing
                End If
                Dim img As BitmapDecoder = New PngBitmapDecoder(New MemoryStream(icon), BitmapCreateOptions.None, BitmapCacheOption.OnLoad)

                Dim imageSource_Renamed As ImageSource = img.Frames(0)
                If imageSource_Renamed.Height < 1 Then
                    Return Nothing
                End If
                categoriesPictures.Add(categoryName, imageSource_Renamed)
                Return imageSource_Renamed
            End If
        End Function

        #End Region

        Public Sub New()
            MyBase.New()
            FrameworkElementDataContextChangedEventManager.AddListener(Me, Me)
            AddHandler Unloaded, AddressOf OnUnloaded
        End Sub

        Public Property ImageSource() As ImageSource
            Get
                Return DirectCast(GetValue(ImageSourceProperty), ImageSource)
            End Get
            Set(ByVal value As ImageSource)
                SetValue(ImageSourceProperty, value)
            End Set
        End Property

        Private Sub OnUnloaded(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ImageSource = Nothing
        End Sub
        Private Sub OnDataContextChanged()
            SetImageSource()
        End Sub
        Private Sub SetImageSource()
            Dim item As FieldValueElementData = TryCast(Me.DataContext, FieldValueElementData)
            If item IsNot Nothing AndAlso Not item.IsOthersRow AndAlso Not String.IsNullOrEmpty(item.DisplayText) Then
                ImageSource = CategoriesControl.GetImage(TryCast(item.Value, String))
            Else
                ImageSource = Nothing
            End If
        End Sub

        #Region "IWeakEventListener Members"
        Private Function IWeakEventListener_ReceiveWeakEvent(ByVal managerType As Type, ByVal sender As Object, ByVal e As EventArgs) As Boolean Implements IWeakEventListener.ReceiveWeakEvent
            Return OnReceiveWeakEvent(managerType, e)
        End Function
        Protected Overridable Function OnReceiveWeakEvent(ByVal managerType As Type, ByVal e As EventArgs) As Boolean
            If managerType Is GetType(FrameworkElementDataContextChangedEventManager) Then
                OnDataContextChanged()
                Return True
            End If
            Return False
        End Function
        #End Region

        #Region "FrameworkElementDataContextChangedEventManager"

        Public Class FrameworkElementDataContextChangedEventManager
            Inherits WeakEventManager

            Private Shared ReadOnly Property CurrentManager() As FrameworkElementDataContextChangedEventManager
                Get
                    Dim managerType As Type = GetType(FrameworkElementDataContextChangedEventManager)

                    Dim currentManager_Renamed As FrameworkElementDataContextChangedEventManager = CType(WeakEventManager.GetCurrentManager(managerType), FrameworkElementDataContextChangedEventManager)
                    If currentManager_Renamed Is Nothing Then
                        currentManager_Renamed = New FrameworkElementDataContextChangedEventManager()
                        WeakEventManager.SetCurrentManager(managerType, currentManager_Renamed)
                    End If
                    Return currentManager_Renamed
                End Get
            End Property

            Private Sub New()
            End Sub

            Public Shared Sub AddListener(ByVal source As FrameworkElement, ByVal listener As IWeakEventListener)
                CurrentManager.ProtectedAddListener(source, listener)
            End Sub
            Public Shared Sub RemoveListener(ByVal source As FrameworkElement, ByVal listener As IWeakEventListener)
                CurrentManager.ProtectedRemoveListener(source, listener)
            End Sub
            Protected Overrides Sub StartListening(ByVal source As Object)
                Dim FrameworkElement As FrameworkElement = DirectCast(source, FrameworkElement)
                AddHandler FrameworkElement.DataContextChanged, AddressOf OnDataContextChanged
            End Sub
            Protected Overrides Sub StopListening(ByVal source As Object)
                Dim FrameworkElement As FrameworkElement = DirectCast(source, FrameworkElement)
                RemoveHandler FrameworkElement.DataContextChanged, AddressOf OnDataContextChanged
            End Sub
            Private Sub OnDataContextChanged(ByVal sender As Object, ByVal e As DependencyPropertyChangedEventArgs)
                MyBase.DeliverEvent(sender, Nothing)
            End Sub
        End Class
        #End Region
    End Class
End Namespace
