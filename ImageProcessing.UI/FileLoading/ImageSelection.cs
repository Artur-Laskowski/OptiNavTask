using System;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace ImageProcessing.UI.FileLoading
{
	public class ImageSelection : IImageSelection
	{
		public BitmapImage? SelectImage()
		{
			OpenFileDialog openFileDialog = new OpenFileDialog
			{
				Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"
			};

			if (openFileDialog.ShowDialog() == true)
			{
				Uri fileUri = new Uri(openFileDialog.FileName);
				return new BitmapImage(fileUri);
			}

			return null;
		}
	}
}