using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingLibrary {
    internal unsafe class RawDataConverter {

        private byte* PixelData;
        private BitmapData RawData;
        private int PixelSize;
        private ColorRatios Ratios;

        private int RowBytesCount;

        public RawDataConverter(Bitmap processedImage, ColorRatios ratios) {
            Rectangle bounds = new Rectangle(Point.Empty, processedImage.Size);
            RawData = processedImage.LockBits(bounds, ImageLockMode.ReadWrite, processedImage.PixelFormat);
            PixelData = (byte*)RawData.Scan0.ToPointer();

            PixelSize = Image.GetPixelFormatSize(RawData.PixelFormat) / 8;
            RowBytesCount = bounds.Width * PixelSize;
            if (RowBytesCount % 4 != 0) {
                RowBytesCount = 4 * ((RowBytesCount / 4) + 1);
            }
            Ratios = ratios;
        }

        public void ConvertColor(int index) {
            byte* address = PixelData + index * PixelSize;
            double b = *address * Ratios.BlueRatio;
            double g = *(address + 1) * Ratios.GreenRatio;
            double r = *(address + 2) * Ratios.RedRatio;
            byte gray = (byte)(r + g + b);
            *address = gray;
            *(address + 1) = gray;
            *(address + 2) = gray;
        }

        public void SaveRawDataToBitmap(Bitmap output) {
            output.UnlockBits(RawData);
        }
    }
}
