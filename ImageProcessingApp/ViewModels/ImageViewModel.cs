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
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using ImageProcessingLibrary;

namespace ImageProcessingApp {
    public class ImageViewModel : ObservableObject {

        public ImageViewModel() {
            _imageLoaded = false;
        }

        public ImageViewModel(Func<string, MessageBoxResult> userNotification) : this() {
            _userNotification = userNotification;
        }

        private Func<string, MessageBoxResult> _userNotification = MessageBox.Show;
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

        string _savePath;
        public string SavePath {
            get {
                return _savePath;
            }
            set {
                _savePath = value;
                RaisePropertyChangedEvent("SavePath");
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

        bool _imageLoaded;
        public bool ImageLoaded {
            get {
                return _imageLoaded;
            }
            set {
                _imageLoaded = value;
                RaisePropertyChangedEvent("ImageLoaded");
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

        public ICommand SaveImageCommand {
            get { return new DelegateCommand(SaveImage); }
        }

        private void LoadImage() {
            if (string.IsNullOrWhiteSpace(ImagePath)) return;
            try {
                _imageModel = new ImageModel(ImagePath);
                UpdateDisplayImage();
                UpdateImageInfoLabels();
                ImageLoaded = true;
            }
            catch (ArgumentException) {
                _userNotification("Incorrect path specified");
            }
            catch (Exception e) {
                _userNotification("Error encountered while loading file");
                _userNotification(e.Message);
            }
        }

        private void SaveImage() {
            if (string.IsNullOrWhiteSpace(SavePath)) return;
            try {
                _imageModel.SaveImage(SavePath);
                _userNotification("Successfuly saved the image");
            } catch (ArgumentException) {
                _userNotification("Incorrect path specified");
            } catch (Exception e) {
                _userNotification("Error encountered while loading file");
                _userNotification(e.Message);
            }
        }

        private void UpdateImageInfoLabels() {
            ImageWidth = _imageModel.ImageWidth.ToString() + "px";
            ImageHeight = _imageModel.ImageHeight.ToString() + "px";
            TimeTaken = "-";
        }

        private void GrayscaleImage() {
            try {
                var sw = StartTimer();
                _imageModel.GrayscaleImage();
                MeasureAndDisplayTime(sw);
                UpdateDisplayImage();
            }
            catch (NullReferenceException e) {
                _userNotification("Image is not loaded");
            }
        }

        private async void GrayscaleImageAsync() {
            try {
                var sw = StartTimer();
                await _imageModel.GrayscaleImageAsync();
                MeasureAndDisplayTime(sw);
                UpdateDisplayImage();
            }
            catch (NullReferenceException e) {
                _userNotification("Image is not loaded");
            }
}

        private void GrayscaleEncodedImage() {
            try {
                _imageModel = new ImageModel(ImagePath);
                var sw = StartTimer();
                _imageModel.GrayscaleEncodedImage();
                MeasureAndDisplayTime(sw);
                UpdateDisplayImage();
            }
            catch (NullReferenceException e) {
                _userNotification("Image is not loaded");
            }
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
