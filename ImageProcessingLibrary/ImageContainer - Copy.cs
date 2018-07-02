using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace ImageProcessingLibrary {
    struct ImageContainer {
        public Color[] Pixels; //it has to be one dimensional to allow for ArraySegment
        public int Height { get; set; }
        public int Width { get; set; }

        public ImageContainer(Bitmap input) {
            Console.WriteLine("Loading into container");
            Pixels = new Color[input.Height * input.Width];
            Width = input.Width;
            Height = input.Height;
            for (int x = 0; x < input.Width; x++) {
                Console.WriteLine(x);
                for (int y = 0; y < input.Height; y++) {
                    int index = y * input.Width + x;
                    Pixels[index] = input.GetPixel(x, y);
                }
            }
        }
    }
}
