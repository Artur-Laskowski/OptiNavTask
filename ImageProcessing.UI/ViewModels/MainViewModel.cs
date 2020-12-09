using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Media.Imaging;
using ImageProcessing.UI.FileLoading;
using ReactiveUI;
using Splat;

namespace ImageProcessing.UI.ViewModels
{
	public class MainViewModel
	{
		private readonly IImageSelection _ImageSelection;

		public MainViewModel(IImageSelection? imageSelection = null)
		{
			imageSelection ??= Locator.Current.GetService<IImageSelection>();

			_ImageSelection = imageSelection;
			Load = ReactiveCommand.CreateFromObservable(
				() => Observable.Create<Unit>(LoadImage));
			Cancel = ReactiveCommand.Create(
				() => { },
				Load.IsExecuting);

		}

		public ReactiveCommand<Unit, Unit> Load { get; }
		public ReactiveCommand<Unit, Unit> Cancel { get; }
		public BitmapImage? LoadedImage { get; set; }


		private IDisposable LoadImage(IObserver<Unit> observer)
		{
			var result = _ImageSelection.SelectImage();

			if (result != null)
			{
				LoadedImage = result;
			}
			observer.OnNext(Unit.Default);
			return Disposable.Empty;
		}
	}
}