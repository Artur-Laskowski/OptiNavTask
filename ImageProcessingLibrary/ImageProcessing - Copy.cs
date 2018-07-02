using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImageProcessingLibrary
{
    public class ImageProcessing
    {
        //my new container for my stuff
        //Task and await, async
        private Bitmap ProcessedImage { get; set; }
        public int TasksCount { get; set; }
        private ImageContainer LoadedImage { get; set; }

        private object LockObject = new object();

        public ImageProcessing(string filePath, int tasksCount = 8) {
            TasksCount = tasksCount;
            LoadImage(filePath);
        }

        public ImageProcessing() {

        }

        private void LoadImage(string filePath) {
            ProcessedImage = new Bitmap(filePath);
            LoadImageIntoContainer();
        }

        private void LoadImageIntoContainer() {
            LoadedImage = new ImageContainer(ProcessedImage);
        }

        private void SaveImageFromContainer() {
            for (int x = 0; x < ProcessedImage.Width; x++) {
                for (int y = 0; y < ProcessedImage.Height; y++) {
                    int index = y * ProcessedImage.Width + x;
                    ProcessedImage.SetPixel(x, y, LoadedImage.Pixels[index]);
                }
            }
        }

        public void SaveImage(string newFilePath) {
            SaveImageFromContainer();
            ProcessedImage.Save(newFilePath);
        }

        public void Grayscale() {
            int totalLength = LoadedImage.Height * LoadedImage.Width;
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
                ArraySegment<Color> segment = new ArraySegment<Color>(LoadedImage.Pixels, startIndex, length);
                tasks[i] = Task.Run(() => ProcessChunk(segment));
                //ProcessChunk(yStart, yEnd);
            }
            Task.WaitAll(tasks);
        }

        //public async Task Grayscale() {
        //
        //}
        
        private void ProcessChunk(IList<Color> segment) {
            Color newColor = new Color();
            int width = ProcessedImage.Width;
            for (int i = 0; i < segment.Count; i++) {
                newColor = ConvertColor(segment[i]);
                segment[i] = newColor;
            }
        }

        private Color ConvertColor(Color color) {
            double r = color.R * 0.2126;
            double g = color.G * 0.7152;
            double b = color.B * 0.0722;
            byte gray = (byte)(r + g + b);
            return Color.FromArgb(gray, gray, gray);
        }
    }
}
