using System;
using System.Windows;
using DryIoc;
using ReactiveUI;
using Splat;
using Splat.DryIoc;

namespace ImageProcessing.UI
{
	public class App : Application
	{
		public App()
		{
		}

		public void Start(IContainer container)
		{
			StartupUri = new Uri("pack://application:,,,/ImageProcessing.UI;component/Views/MainWindow.xaml", UriKind.Absolute);

			container.UseDryIocDependencyResolver();

			Locator.CurrentMutable.InitializeSplat();
			Locator.CurrentMutable.InitializeReactiveUI();

			Run();
		}
	}
}