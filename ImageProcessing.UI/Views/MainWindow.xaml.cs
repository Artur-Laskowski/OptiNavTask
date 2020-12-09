using System.Reactive.Disposables;
using ImageProcessing.UI.ViewModels;
using ReactiveUI;

namespace ImageProcessing.UI.Views
{
	public partial class MainWindow : ReactiveWindow<MainViewModel>
	{
		public MainWindow()
		{
			InitializeComponent();
			ViewModel = new MainViewModel();

			this.WhenActivated(disposableRegistration =>
			{
				this.OneWayBind(ViewModel,
					vm => vm.TestText,
					view => view.TestText.Content)
					.DisposeWith(disposableRegistration);
			});
		}
	}
}
