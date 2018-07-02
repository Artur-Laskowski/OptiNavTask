using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace ImageProcessingLibrary
{
    internal struct ColorRatios {
        public double RedRatio;
        public double GreenRatio;
        public double BlueRatio;
    }

    public class ImageProcessing {
        public Bitmap ProcessedImage { get; private set; }
        public int TasksCount { get; set; }

        private RawDataConverter RawDataManager;

        public double RedRatio { get; private set; }
        public double GreenRatio { get; private set; }
        public double BlueRatio { get; private set; }

        public ImageProcessing(string filePath, int tasksCount) {
            TasksCount = tasksCount;
            LoadImageAsBitmap(filePath);

            //default ratios
            RedRatio = 0.2126;
            GreenRatio = 0.7152;
            BlueRatio = 0.0722;

            RawDataManager = new RawDataConverter(ProcessedImage, new ColorRatios {
                BlueRatio = BlueRatio,
                RedRatio = RedRatio,
                GreenRatio = GreenRatio,
            });
        }

        public ImageProcessing(string filePath) : this(filePath, Environment.ProcessorCount) {

        }

        public ImageProcessing() {

        }

        private void LoadImageAsBitmap(string filePath) {
            ProcessedImage = new Bitmap(filePath);

            int depth = Image.GetPixelFormatSize(ProcessedImage.PixelFormat);
            if (depth != 24 && depth != 32) {
                throw new ArgumentException("Only 24 and 32 bpp images are supported.");
            }
        }
        
        public void SaveImage(string newFilePath) {
            RawDataManager.SaveRawDataToBitmap(ProcessedImage);
            ProcessedImage.Save(newFilePath);
        }

        public void SaveEncodedImage(string newFilePath) {
            ProcessedImage.Save(newFilePath);
        }

        public void Grayscale() {
            int totalLength = ProcessedImage.Height * ProcessedImage.Width;
            ProcessSegment(0, totalLength);
        }

        public async Task GrayscaleAsync() {
            int totalLength = ProcessedImage.Height * ProcessedImage.Width;
            int chunkLength = totalLength / TasksCount;

            int numberOfTasks = TasksCount;
            if (ProcessedImage.Height < TasksCount)
                numberOfTasks = 1;

            Task[] tasks = new Task[numberOfTasks];

            for (int i = 0; i < TasksCount; i++) {
                int startIndex = i * chunkLength;
                int length;
                if (i == TasksCount - 1) {
                    length = totalLength - startIndex;
                } else {
                    length = chunkLength;
                }
                tasks[i] = Task.Run(() => ProcessSegment(startIndex, length));
            }

            await Task.WhenAll(tasks);
        }

        public void GrayscaleEncodedImage() {
            //RawDataManager.SaveRawDataToBitmap(ProcessedImage);
            for (int x = 0; x < ProcessedImage.Width; x++) {
                for (int y = 0; y < ProcessedImage.Height; y++) {
                    Color pixel = ProcessedImage.GetPixel(x, y);
                    double r = pixel.R * RedRatio;
                    double g = pixel.G * GreenRatio;
                    double b = pixel.B * BlueRatio;
                    byte gray = (byte)(r + g + b);
                    Color newColor = Color.FromArgb(gray, gray, gray);
                    ProcessedImage.SetPixel(x, y, newColor);
                }
            }
        }
        
        public void ProcessSegment(int startIndex, int length) {
            for (int i = startIndex; i < startIndex + length; i++) {
                RawDataManager.ConvertColor(i);
            }
        }
    }
}
