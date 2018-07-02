using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ImageProcessingLibrary;

namespace ImageProcessingApp {
    class ImageViewModel : INotifyPropertyChanged {

        private ImageProcessing processing;

        BitmapImage _imageToDisplay;
        public BitmapImage ImageToDisplay {
            get {
                return _imageToDisplay;
            }
            set {
                _imageToDisplay = value;
                RaisePropertyChanged("ImageToDisplay");
            }
        }

        string _imagePath;
        public string ImagePath {
            get {
                return _imagePath;
            }
            set {
                _imagePath = value;
                RaisePropertyChanged("ImagePath");
            }
        }

        public ImageViewModel() {
            ImagePath = @"C:\test\image.jpg";
            //ImageToDisplay = new BitmapImage(
            //    new Uri(@"/ImageProcessingApp;component/Resources/image.jpg", UriKind.Relative));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string propertyName) {
            // take a copy to prevent thread issues
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap) {
            using (MemoryStream memory = new MemoryStream()) {
                bitmap.Save(memory, ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        public ICommand LoadImageCommand {
            get { return new DelegateCommand(LoadImage); }
        }

        public ICommand GrayscaleCommand {
            get { return new DelegateCommand(GrayscaleImage); }
        }

        public ICommand GrayscaleAsyncCommand {
            get { return new DelegateCommand(GrayscaleImageAsync); }
        }

        private void LoadImage() {
            if (string.IsNullOrWhiteSpace(ImagePath)) return;
            processing = new ImageProcessing(ImagePath);
            UpdateDisplayImage();
        }

        private void GrayscaleImage() {
            processing.Grayscale();
            UpdateDisplayImage();
        }

        private async void GrayscaleImageAsync() {
            await processing.GrayscaleAsync();
            UpdateDisplayImage();
        }

        private void UpdateDisplayImage() {
            ImageToDisplay = BitmapToImageSource(processing.ProcessedImage);
        }
    }
}
