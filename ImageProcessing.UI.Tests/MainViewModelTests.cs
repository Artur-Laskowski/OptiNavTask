using System;
using System.Windows.Media.Imaging;
using FluentAssertions;
using ImageProcessing.UI.FileLoading;
using ImageProcessing.UI.ViewModels;
using Moq;
using Xunit;

namespace ImageProcessing.UI.Tests
{
	public class MainViewModelTests
	{
		private readonly MainViewModel _sut;
		private readonly Mock<IImageSelection> _imageSelectionMock;

		public MainViewModelTests()
		{
			_imageSelectionMock = new Mock<IImageSelection>();
			_sut = new MainViewModel(_imageSelectionMock.Object);
		}

		[Fact]
		public void LoadingInvokesImageSelection()
		{
			_sut.Load.Execute().Subscribe();

			_imageSelectionMock.Verify(m => m.SelectImage(), Times.Once);
		}

		[Fact]
		public void ImageFromLoaderIsAssigned()
		{
			var loadedImage = new BitmapImage();
			_imageSelectionMock
				.Setup(m => m.SelectImage())
				.Returns(loadedImage);

			_sut.Load.Execute().Subscribe();

			_sut.LoadedImage.Should().Be(loadedImage);
		}
	}
}