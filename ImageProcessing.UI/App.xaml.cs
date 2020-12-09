using System.Reflection;
using System.Windows;
using ReactiveUI;
using Splat;

namespace ImageProcessing.UI
{
    public partial class App : Application
    {
	    public App()
	    {
		    Locator.CurrentMutable.RegisterViewsForViewModels(Assembly.GetCallingAssembly());
        }
    }
}
