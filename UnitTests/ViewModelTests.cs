using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessingApp;
using ImageProcessingLibrary;
using System.Drawing;
using System.Windows.Media.Imaging;
using Moq;

namespace UnitTests {
    [TestClass]
    public class ViewModelTests {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void LoadImage_IncorrectPath_ArgumentException() {
            // arrange
            var path = @"incorrectPath";
            var ivm = new ImageViewModel();
            ivm.ImagePath = path;

            // act
            ivm.LoadImageCommand.Execute(null);

            // assert
            //exception
        }
        
        [TestMethod]
        public void LoadImage_PathIsEmpty_PropertiesNotSet() {
            //arrange
            var ivm = new ImageViewModel();

            //act
            ivm.LoadImageCommand.Execute(null);

            //assert
            Assert.AreEqual(null, ivm.ImageHeight);
            Assert.AreEqual(null, ivm.ImageWidth);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Grayscale_ImageNotSet_Exception() {
            //arrange
            var ivm = new ImageViewModel();

            //act
            ivm.GrayscaleCommand.Execute(null);
            
            //assert
            //exception
        }
    }
}
