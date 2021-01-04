using System.Reactive.Disposables;
using ImageProcessing.UI.ViewModels;
using ReactiveUI;

namespace ImageProcessing.UI.Views
{
#nullable disable
	public partial class MainWindow : ReactiveWindow<MainViewModel>
#nullable enable
	{
		public MainWindow()
		{
			InitializeComponent();
			ViewModel = new MainViewModel();

			this.WhenActivated(disposableRegistration =>
			{
				this.BindCommand(ViewModel,
						vm => vm.Load,
						view => view.Load)
					.DisposeWith(disposableRegistration);
				this.OneWayBind(ViewModel,
						vm => vm.LoadedImage,
						view => view.LoadedImage.Source)
					.DisposeWith(disposableRegistration);
			});
		}
	}
}
