using System.Windows;
using Autofac;
using Autofac.Core;
using ImageProcessing.UI.FileLoading;
using ImageProcessing.UI.ViewModels;
using ImageProcessing.UI.Views;
using ReactiveUI;
using Splat;
using Splat.Autofac;

namespace ImageProcessing.UI
{
    public partial class App : Application
    {
	    public App()
	    {
		    var container = new ContainerBuilder();
		    container.RegisterType<MainWindow>().As<IViewFor<MainViewModel>>();
		    container.RegisterType<ImageSelection>().As<IImageSelection>();

			container.UseAutofacDependencyResolver();

			Locator.CurrentMutable.InitializeSplat();
		    Locator.CurrentMutable.InitializeReactiveUI();

			//Locator.CurrentMutable.Register(() => new ImageSelection(), typeof(IImageSelection));
	    }
    }
}
