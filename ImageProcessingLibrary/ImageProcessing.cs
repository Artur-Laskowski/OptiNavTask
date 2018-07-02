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
        private Bitmap ProcessedImage { get; set; }
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
        }
        
        public void SaveImage(string newFilePath) {
            RawDataManager.SaveRawDataToBitmap(ProcessedImage);
            ProcessedImage.Save(newFilePath);
        }

        public void Grayscale() {
            int totalLength = ProcessedImage.Height * ProcessedImage.Width;
            ProcessSegment(0, totalLength);
        }

        public async Task GrayscaleAsync() {
            int totalLength = ProcessedImage.Height * ProcessedImage.Width;
            int chunkLength = totalLength / TasksCount;
            //TODO: handle image height < TasksCount

            Task[] tasks = new Task[TasksCount];

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
        
        public void ProcessSegment(int startIndex, int length) {
            for (int i = startIndex; i < startIndex + length; i++) {
                RawDataManager.ConvertColor(i);
            }
        }
    }
}
