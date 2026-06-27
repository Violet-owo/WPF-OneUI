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
        private bool _isDialogModalOpen;
        private string _dialogTitle;
        private string _dialogMessage;
        private bool _isConfirmDialog;
        private Action _onConfirmAction;

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

        public bool IsDialogModalOpen
        {
            get => _isDialogModalOpen;
            set { _isDialogModalOpen = value; OnPropertyChanged(nameof(IsDialogModalOpen)); }
        }

        public string DialogTitle
        {
            get => _dialogTitle;
            set { _dialogTitle = value; OnPropertyChanged(nameof(DialogTitle)); }
        }

        public string DialogMessage
        {
            get => _dialogMessage;
            set { _dialogMessage = value; OnPropertyChanged(nameof(DialogMessage)); }
        }

        public bool IsConfirmDialog
        {
            get => _isConfirmDialog;
            set { _isConfirmDialog = value; OnPropertyChanged(nameof(IsConfirmDialog)); }
        }

        public ICommand OpenImageCommand { get; }
        public ICommand CloseImageCommand { get; }
        public ICommand NextImageCommand { get; }
        public ICommand PrevImageCommand { get; }
        public ICommand FavoriteCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand ShareCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand MenuCommand { get; }
        public ICommand DialogOkCommand { get; }
        public ICommand DialogCancelCommand { get; }

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
            FavoriteCommand = new RelayCommand<object>(_ => ToggleFavorite());
            EditCommand = new RelayCommand<object>(_ => ShowDialog("Edit", "Edit mode is not implemented in this demo."));
            ShareCommand = new RelayCommand<object>(_ => ShowDialog("Share", "Sharing options are not implemented in this demo."));
            DeleteCommand = new RelayCommand<object>(_ => DeleteSelectedImage());
            MenuCommand = new RelayCommand<object>(_ => ShowDialog("Menu", "Menu options are not implemented in this demo."));
            
            DialogOkCommand = new RelayCommand<object>(_ => 
            {
                IsDialogModalOpen = false;
                _onConfirmAction?.Invoke();
            });
            DialogCancelCommand = new RelayCommand<object>(_ => IsDialogModalOpen = false);

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

        private void ToggleFavorite()
        {
            if (SelectedImage != null)
            {
                SelectedImage.IsFavorite = !SelectedImage.IsFavorite;
            }
        }

        private void DeleteSelectedImage()
        {
            if (SelectedImage != null)
            {
                ShowDialog("Delete", $"Are you sure you want to delete '{SelectedImage.Title}'?", true, () =>
                {
                    Images.Remove(SelectedImage);
                    IsImageModalOpen = false;
                });
            }
        }

        private void ShowDialog(string title, string message, bool isConfirm = false, Action onConfirm = null)
        {
            DialogTitle = title;
            DialogMessage = message;
            IsConfirmDialog = isConfirm;
            _onConfirmAction = onConfirm;
            IsDialogModalOpen = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public class GalleryItem : INotifyPropertyChanged
    {
        private bool _isFavorite;

        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string Date { get; set; }

        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                _isFavorite = value;
                OnPropertyChanged(nameof(IsFavorite));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
