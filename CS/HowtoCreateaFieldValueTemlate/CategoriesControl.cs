using DevExpress.Xpf.PivotGrid;
using HowToCreateFieldValueTemplate.CategoryPicturesTableAdapters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace HowToCreateFieldValueTemplate
{
    public class CategoriesControl : Control, IWeakEventListener {

        #region static staff
        static CategoriesTableAdapter categoriesTableAdapter;
        static Dictionary<string, ImageSource> categoriesPictures;
        public static readonly DependencyProperty ImageSourceProperty;

        static CategoriesControl() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CategoriesControl), 
				new FrameworkPropertyMetadata(typeof(CategoriesControl)));
            Type ownerType = typeof(CategoriesControl);
            ImageSourceProperty = DependencyProperty.Register("ImageSource", typeof(ImageSource),
                   ownerType, new UIPropertyMetadata());
            categoriesPictures = new Dictionary<string, ImageSource>();
        }

        static CategoriesTableAdapter CategoriesTableAdapter {
            get {
                if(categoriesTableAdapter == null)
                    categoriesTableAdapter = new CategoriesTableAdapter();
                return categoriesTableAdapter;
            }
        }

        static ImageSource GetImage(string categoryName) {
            if(string.IsNullOrEmpty(categoryName)) return null;
            if(categoriesPictures.ContainsKey(categoryName)) {
                return categoriesPictures[categoryName];
            } else {
                byte[] icon = CategoriesTableAdapter.GetIconImageByName(categoryName) as byte[];
                if(icon == null || icon.Length == 0) {
                    return null;
                }
                BitmapDecoder img = new PngBitmapDecoder(new MemoryStream(icon), 
					BitmapCreateOptions.None, 
					BitmapCacheOption.OnLoad);
                ImageSource imageSource = img.Frames[0];
                if(imageSource.Height < 1) return null;
                categoriesPictures.Add(categoryName, imageSource);
                return imageSource;
            }
        }

        #endregion

        public CategoriesControl()
            : base() {
            FrameworkElementDataContextChangedEventManager.AddListener(this, this);
            Unloaded += OnUnloaded;
        }

        public ImageSource ImageSource {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        void OnUnloaded(object sender, RoutedEventArgs e) {
            ImageSource = null;
        }
        void OnDataContextChanged() {
            SetImageSource();
        }
        void SetImageSource() {
            FieldValueElementData item = this.DataContext as FieldValueElementData;
            if(item != null && !item.IsOthersRow && !string.IsNullOrEmpty(item.DisplayText)) {
                ImageSource = CategoriesControl.GetImage(item.Value as string);
            } else {
                ImageSource = null;
            }
        }

        #region IWeakEventListener Members
        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e) {
            return OnReceiveWeakEvent(managerType, e);
        }
        protected virtual bool OnReceiveWeakEvent(Type managerType, EventArgs e) {
            if(managerType == typeof(FrameworkElementDataContextChangedEventManager)) {
                OnDataContextChanged();
                return true;
            }
            return false;
        }
        #endregion

        #region FrameworkElementDataContextChangedEventManager

        public class FrameworkElementDataContextChangedEventManager : WeakEventManager {
            static FrameworkElementDataContextChangedEventManager CurrentManager {
                get {
                    Type managerType = typeof(FrameworkElementDataContextChangedEventManager);
                    FrameworkElementDataContextChangedEventManager currentManager =
                        (FrameworkElementDataContextChangedEventManager)WeakEventManager
						.GetCurrentManager(managerType);
                    if(currentManager == null) {
                        currentManager = new FrameworkElementDataContextChangedEventManager();
                        WeakEventManager.SetCurrentManager(managerType, currentManager);
                    }
                    return currentManager;
                }
            }

            FrameworkElementDataContextChangedEventManager() { }

            public static void AddListener(FrameworkElement source, IWeakEventListener listener) {
                CurrentManager.ProtectedAddListener(source, listener);
            }
            public static void RemoveListener(FrameworkElement source, IWeakEventListener listener) {
                CurrentManager.ProtectedRemoveListener(source, listener);
            }
            protected override void StartListening(object source) {
                FrameworkElement FrameworkElement = (FrameworkElement)source;
                FrameworkElement.DataContextChanged += OnDataContextChanged;
            }
            protected override void StopListening(object source) {
                FrameworkElement FrameworkElement = (FrameworkElement)source;
                FrameworkElement.DataContextChanged -= OnDataContextChanged;
            }
            void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
                base.DeliverEvent(sender, null);
            }
        }
        #endregion
    }
}
