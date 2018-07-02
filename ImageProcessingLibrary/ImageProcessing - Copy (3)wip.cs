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
    public class ImageProcessing {
        private Bitmap ProcessedImage { get; set; }
        public int TasksCount { get; set; }

        private byte[] PixelData;
        private BitmapData RawData;
        private int RowBytesCount;
        private int PixelSize;

        public double RedRatio { get; private set; }
        public double GreenRatio { get; private set; }
        public double BlueRatio { get; private set; }

        public ImageProcessing(string filePath, int tasksCount) {
            TasksCount = tasksCount;
            LoadImageAsBitmap(filePath);
            LoadRawData();

            //default ratios
            RedRatio = 0.2126;
            GreenRatio = 0.7152;
            BlueRatio = 0.0722;

            //RedRatio = 0;
            //GreenRatio = 1;
            //BlueRatio = 0;
        }

        public ImageProcessing(string filePath) : this(filePath, Environment.ProcessorCount) {

        }

        public ImageProcessing() {

        }

        private void LoadImageAsBitmap(string filePath) {
            ProcessedImage = new Bitmap(filePath);
        }


        private unsafe void LoadRawData() {
            Rectangle bounds = new Rectangle(Point.Empty, ProcessedImage.Size);
            RawData = ProcessedImage.LockBits(bounds, ImageLockMode.ReadWrite, ProcessedImage.PixelFormat);
            var data = RawData.Scan0;

            PixelSize = Image.GetPixelFormatSize(RawData.PixelFormat) / 8;

            PixelData = new byte[RawData.Height * RawData.Width * PixelSize];
            System.Runtime.InteropServices.Marshal.Copy(data, PixelData, 0, PixelData.Length);

            RowBytesCount = bounds.Width * PixelSize;
            if (RowBytesCount % 4 != 0) {
                RowBytesCount = 4 * ((RowBytesCount / 4) + 1);
            }

        }

        private void SaveRawDataToBitmap() {
            ProcessedImage.UnlockBits(RawData);
        }

        public void SaveImage(string newFilePath) {
            SaveRawDataToBitmap();
            ProcessedImage.Save(newFilePath);
        }

        public void Grayscale() {

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
            int endOfRowOffset = 0;
            for (int i = startIndex; i < startIndex + length; i++) {
                int address =  i * PixelSize + endOfRowOffset;
                ConvertColor(address);
            }
        }

        private void ConvertColor(int address) {
            double b = PixelData[address] * BlueRatio;
            double g = (address + 1) * GreenRatio;
            double r = (address + 2) * RedRatio;
            byte gray = (byte)(r + g + b);
            *address = gray;
            *(address + 1) = gray;
            *(address + 2) = gray;
        }
    }
}
