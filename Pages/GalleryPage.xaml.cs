using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using SamsungUi.Controls;

namespace SamsungUi.Demo.Pages
{
    public partial class GalleryPage : SamsungExpandablePage, INotifyPropertyChanged
    {
        private GalleryItem _selectedImage;
        private bool _isImageModalOpen;
        private System.Windows.Point _zoomOrigin = new System.Windows.Point(0.5, 0.5);

        public ObservableCollection<GalleryItem> Images { get; set; }

        public GalleryItem SelectedImage
        {
            get => _selectedImage;
            set
            {
                _selectedImage = value;
                OnPropertyChanged(nameof(SelectedImage));
            }
        }

        public bool IsImageModalOpen
        {
            get => _isImageModalOpen;
            set
            {
                _isImageModalOpen = value;
                OnPropertyChanged(nameof(IsImageModalOpen));
            }
        }

        public System.Windows.Point ZoomOrigin
        {
            get => _zoomOrigin;
            set
            {
                _zoomOrigin = value;
                OnPropertyChanged(nameof(ZoomOrigin));
            }
        }

        public ICommand OpenImageCommand { get; }
        public ICommand CloseImageCommand { get; }
        public ICommand NextImageCommand { get; }
        public ICommand PrevImageCommand { get; }

        public GalleryPage()
        {
            InitializeComponent();

            Images = new ObservableCollection<GalleryItem>();
            for (int i = 0; i < 12; i++)
            {
                Images.Add(new GalleryItem
                {
                    ImagePath = i % 2 == 0 ? "pack://application:,,,/Assets/gallery_nature.png" : "pack://application:,,,/Assets/gallery_abstract.png",
                    Title = $"Image {i + 1}",
                    Date = "21 June 2026"
                });
            }

            OpenImageCommand = new RelayCommand<object>(OpenImage);
            CloseImageCommand = new RelayCommand<object>(_ => IsImageModalOpen = false);
            NextImageCommand = new RelayCommand<object>(_ => NavigateImage(1));
            PrevImageCommand = new RelayCommand<object>(_ => NavigateImage(-1));

            DataContext = this;
        }

        private void OpenImage(object parameter)
        {
            if (parameter is System.Windows.FrameworkElement element && element.DataContext is GalleryItem item)
            {
                var window = System.Windows.Window.GetWindow(element);
                if (window != null && window.ActualWidth > 0 && window.ActualHeight > 0)
                {
                    System.Windows.Point pos = element.TransformToAncestor(window).Transform(new System.Windows.Point(0, 0));
                    double centerX = pos.X + element.ActualWidth / 2;
                    double centerY = pos.Y + element.ActualHeight / 2;
                    
                    double originX = centerX / window.ActualWidth;
                    double originY = centerY / window.ActualHeight;
                    
                    ZoomOrigin = new System.Windows.Point(originX, originY);
                }

                SelectedImage = item;
                IsImageModalOpen = true;
            }
        }

        private void NavigateImage(int step)
        {
            if (SelectedImage == null || Images.Count == 0) return;
            int currentIndex = Images.IndexOf(SelectedImage);
            if (currentIndex == -1) return;

            int nextIndex = (currentIndex + step) % Images.Count;
            if (nextIndex < 0) nextIndex += Images.Count;

            SelectedImage = Images[nextIndex];
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class GalleryItem
    {
        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        public RelayCommand(Action<T> execute) => _execute = execute;
        public event EventHandler CanExecuteChanged { add { } remove { } }
        public bool CanExecute(object parameter) => true;
        public void Execute(object parameter) => _execute((T)parameter);
    }
}
