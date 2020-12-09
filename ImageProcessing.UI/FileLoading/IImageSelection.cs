using System.Windows.Media.Imaging;

namespace ImageProcessing.UI.FileLoading
{
	public interface IImageSelection
	{
		BitmapImage? SelectImage();
	}
}