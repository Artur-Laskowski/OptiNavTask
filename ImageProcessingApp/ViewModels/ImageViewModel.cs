﻿using System;
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

        string _timeTaken;
        public string TimeTaken {
            get {
                return _timeTaken;
            }
            set {
                _timeTaken = value;
                RaisePropertyChanged("TimeTaken");
            }
        }

        string _imageWidth;
        public string ImageWidth {
            get {
                return _imageWidth;
            }
            set {
                _imageWidth = value;
                RaisePropertyChanged("ImageWidth");
            }
        }

        string _imageHeight;
        public string ImageHeight {
            get {
                return _imageHeight;
            }
            set {
                _imageHeight = value;
                RaisePropertyChanged("ImageHeight");
            }
        }

        public ImageViewModel() {
            ImagePath = @"C:\test\image.jpg";
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

        public ICommand GrayscaleEncodedCommand {
            get { return new DelegateCommand(GrayscaleEncodedImage); }
        }

        private void LoadImage() {
            if (string.IsNullOrWhiteSpace(ImagePath)) return;
            processing = new ImageProcessing(ImagePath);
            UpdateDisplayImage();
            ImageWidth = processing.ProcessedImage.Width.ToString() + "px";
            ImageHeight = processing.ProcessedImage.Height.ToString() + "px";
            TimeTaken = "-";
        }

        private void GrayscaleImage() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            processing.Grayscale();
            sw.Stop();
            TimeTaken = sw.ElapsedMilliseconds.ToString() + "ms";
            UpdateDisplayImage();
        }

        private async void GrayscaleImageAsync() {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            await processing.GrayscaleAsync();
            sw.Stop();
            TimeTaken = sw.ElapsedMilliseconds.ToString() + "ms";
            UpdateDisplayImage();
        }

        private void GrayscaleEncodedImage() {
            processing = new ImageProcessing(ImagePath);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            processing.GrayscaleEncodedImage();
            sw.Stop();
            TimeTaken = sw.ElapsedMilliseconds.ToString() + "ms";
            UpdateDisplayImage();
        }

        private void UpdateDisplayImage() {
            ImageToDisplay = BitmapToImageSource(processing.ProcessedImage);
        }
    }
}