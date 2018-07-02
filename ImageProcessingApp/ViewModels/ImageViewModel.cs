using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    public class ImageViewModel : ObservableObject {

        private ImageModel _imageModel;

        BitmapImage _imageToDisplay;
        public BitmapImage ImageToDisplay {
            get {
                return _imageToDisplay;
            }
            set {
                _imageToDisplay = value;
                RaisePropertyChangedEvent("ImageToDisplay");
            }
        }

        string _imagePath;
        public string ImagePath {
            get {
                return _imagePath;
            }
            set {
                _imagePath = value;
                RaisePropertyChangedEvent("ImagePath");
            }
        }

        string _timeTaken;
        public string TimeTaken {
            get {
                return _timeTaken;
            }
            set {
                _timeTaken = value;
                RaisePropertyChangedEvent("TimeTaken");
            }
        }

        string _imageWidth;
        public string ImageWidth {
            get {
                return _imageWidth;
            }
            set {
                _imageWidth = value;
                RaisePropertyChangedEvent("ImageWidth");
            }
        }

        string _imageHeight;
        public string ImageHeight {
            get {
                return _imageHeight;
            }
            set {
                _imageHeight = value;
                RaisePropertyChangedEvent("ImageHeight");
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

        public ICommand GrayscaleEncodedCommand {
            get { return new DelegateCommand(GrayscaleEncodedImage); }
        }

        private void LoadImage() {
            if (string.IsNullOrWhiteSpace(ImagePath)) return;
            _imageModel = new ImageModel(ImagePath);
            UpdateDisplayImage();
            UpdateImageInfoLabels();
        }

        private void UpdateImageInfoLabels() {
            ImageWidth = _imageModel.ImageWidth.ToString() + "px";
            ImageHeight = _imageModel.ImageHeight.ToString() + "px";
            TimeTaken = "-";
        }

        private void GrayscaleImage() {
            var sw = StartTimer();
            _imageModel.GrayscaleImage();
            MeasureAndDisplayTime(sw);
            UpdateDisplayImage();
        }

        private async void GrayscaleImageAsync() {
            var sw = StartTimer();
            await _imageModel.GrayscaleImageAsync();
            MeasureAndDisplayTime(sw);
            UpdateDisplayImage();
        }

        private void GrayscaleEncodedImage() {
            _imageModel = new ImageModel(ImagePath);
            var sw = StartTimer();
            _imageModel.GrayscaleEncodedImage();
            MeasureAndDisplayTime(sw);
            UpdateDisplayImage();
        }

        private Stopwatch StartTimer() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            return sw;
        }

        private void MeasureAndDisplayTime(Stopwatch sw) {
            sw.Stop();
            TimeTaken = sw.ElapsedMilliseconds.ToString() + "ms";
        }

        private void UpdateDisplayImage() {
            ImageToDisplay = BitmapToImageSource(_imageModel.ImageBitmap);
        }
    }
}
