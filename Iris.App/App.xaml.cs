using System.Windows;
using Iris.App.ViewModels;
using Iris.Core;

namespace Iris.App;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var root = GalleryStore.DefaultRoot();
        var vm = new MainViewModel(new PollinationsClient(), new GalleryStore(root), new HistoryStore(root));
        var window = new MainWindow { DataContext = vm };
        window.Show();
    }
}
