using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageProcessingLibrary;
using System.Diagnostics;
using System.Threading;

namespace ConsoleApp1 {
    class Program {
        static void Main(string[] args) {
            var h = new Helper();
            var t = h.Work();
            t.Wait();
        }
    }
    class Helper {
        public async Task Work() {
            Stopwatch sw = new Stopwatch();
            ImageProcessing ip = new ImageProcessing(@"C:\test\image2.jpg");

            Console.WriteLine("starting async");
            sw.Start();
            await ip.GrayscaleAsync();
            sw.Stop();
            Console.WriteLine($"ended async: {sw.ElapsedMilliseconds}ms");
            /*
            ip = new ImageProcessing(@"C:\test\image2.jpg");
            Console.WriteLine("starting sync");
            sw.Start();
            ip.Grayscale();
            sw.Stop();
            Console.WriteLine($"ended sync: {sw.ElapsedMilliseconds}ms");
            
            ip = new ImageProcessing(@"C:\test\image2.jpg");
            Console.WriteLine("starting encoded");
            sw.Start();
            ip.GrayscaleEncodedImage();
            sw.Stop();
            Console.WriteLine($"ended encoded: {sw.ElapsedMilliseconds}ms");*/

            ip.SaveImage(@"C:\test\newImage2.jpg");
        }
    }
}
