using System;
using System.Reactive.Linq;
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

		[Fact]
		public void ImageIsReturnedByCommand()
		{
			BitmapImage? receivedImage = null;

			var loadedImage = new BitmapImage();
			_imageSelectionMock
				.Setup(m => m.SelectImage())
				.Returns(loadedImage);

			_sut.Load.Execute().Subscribe(image => receivedImage = image);

			receivedImage.Should().Be(loadedImage);
		}

		[Fact]
		public void ReturnNoResultIfNoImageIsSelected()
		{
			_imageSelectionMock
				.Setup(m => m.SelectImage())
				.Returns((BitmapImage?)null);

			var isEmpty = false;
			_sut.Load.Execute().IsEmpty().Subscribe(e => isEmpty = e);

			isEmpty.Should().BeTrue();
		}
	}
}