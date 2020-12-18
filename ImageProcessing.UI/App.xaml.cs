using System.Reflection;
using System.Windows;
using Autofac;
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

			var resolver = container.UseAutofacDependencyResolver();

			resolver.InitializeSplat();
			resolver.InitializeReactiveUI();

			container.RegisterInstance(resolver);

			var scope = container.Build();
			resolver.SetLifetimeScope(scope);
	    }
	}
}
