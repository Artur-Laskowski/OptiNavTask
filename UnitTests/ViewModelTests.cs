using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImageProcessingApp;
using ImageProcessingLibrary;
using System.Drawing;
using System.Windows.Media.Imaging;
using Moq;
using System.Windows;

namespace UnitTests {
    [TestClass]
    public class ViewModelTests {

        Func<string, MessageBoxResult> mboxMock = (string message) => { throw new Exception(); };

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void LoadImage_IncorrectPath_ArgumentException() {
            // arrange
            var path = @"incorrectPath";
            var ivm = new ImageViewModel(mboxMock);
            ivm.ImagePath = path;

            // act
            ivm.LoadImageCommand.Execute(null);

            // assert
            //exception
        }

        [TestMethod]
        public void LoadImage_PathIsEmpty_PropertiesNotSet() {
            //arrange
            var ivm = new ImageViewModel(mboxMock);

            //act
            ivm.LoadImageCommand.Execute(null);

            //assert
            Assert.AreEqual(null, ivm.ImageHeight);
            Assert.AreEqual(null, ivm.ImageWidth);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void Grayscale_ImageNotSet_Exception() {
            //arrange
            var ivm = new ImageViewModel(mboxMock);

            //act
            ivm.GrayscaleCommand.Execute(null);

            //assert
            //exception
        }
        //not much else to be tested in viewmodel...
    }
}
