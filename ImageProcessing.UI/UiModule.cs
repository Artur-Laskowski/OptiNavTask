using ImageProcessing.UI.FileLoading;
using ImageProcessing.UI.ViewModels;
using ImageProcessing.UI.Views;
using ReactiveUI;
using DryIoc;

namespace ImageProcessing.UI
{
	public class UiModule
	{
		public static void RegisterIn(Container container)
		{
			container.Register<App>();

			container.Register<IImageSelection, ImageSelection>();
			container.Register<IViewFor<MainViewModel>, MainWindow>();
		}
	}
}