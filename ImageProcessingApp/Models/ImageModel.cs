using ImageProcessingLibrary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcessingApp {
    class ImageModel : ObservableObject {

        private ImageProcessing _processing;

        public ImageModel(string path) {
            _processing = new ImageProcessing(path);
        }

        public int ImageWidth {
            get {
                return _processing.ProcessedImage.Width;
            }
        }

        public int ImageHeight {
            get {
                return _processing.ProcessedImage.Height;
            }
        }

        public void GrayscaleImage() {
            _processing.Grayscale();
        }

        public async Task GrayscaleImageAsync() {
            await _processing.GrayscaleAsync();
        }

        public void GrayscaleEncodedImage() {
            _processing.GrayscaleEncodedImage();
        }

        public Bitmap ImageBitmap {
            get {
                return _processing.ProcessedImage;
            }
        }
    }
}
