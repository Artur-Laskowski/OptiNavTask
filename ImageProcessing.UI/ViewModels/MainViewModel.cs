using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Media.Imaging;
using ImageProcessing.UI.FileLoading;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;

namespace ImageProcessing.UI.ViewModels
{
	public class MainViewModel : ReactiveObject
	{
		private readonly IImageSelection _ImageSelection;

		public MainViewModel(IImageSelection? imageSelection = null)
		{
			imageSelection ??= Locator.Current.GetService<IImageSelection>();
			_ImageSelection = imageSelection;

			Load = ReactiveCommand.CreateFromObservable<Unit, BitmapImage>(
				_ => Observable.Create<BitmapImage>(LoadImage));
			Load.Subscribe(image =>
			{
				LoadedImage = image;
			});
		}

		public ReactiveCommand<Unit, BitmapImage> Load { get; }

		[Reactive]
		public BitmapImage? LoadedImage { get; set; }

		private IDisposable LoadImage(IObserver<BitmapImage> observer)
		{
			var result = _ImageSelection.SelectImage();

			if (result != null)
			{
				observer.OnNext(result);
			}
			observer.OnCompleted();

			return Disposable.Empty;
		}
	}
}