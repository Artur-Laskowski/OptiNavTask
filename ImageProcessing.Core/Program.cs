using System;
using DryIoc;
using ImageProcessing.Lib;
using ImageProcessing.UI;

namespace ImageProcessing.Core
{
	public class Program
	{
		[STAThread]
		public static void Main()
		{
			var container = new Container();

			LibModule.RegisterIn(container);
			UiModule.RegisterIn(container);

			var app = container.Resolve<App>();
			app.Start(container);
		}
	}
}