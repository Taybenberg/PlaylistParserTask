using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaPlaylistParser.Services;
using AvaloniaPlaylistParser.ViewModels;
using AvaloniaPlaylistParser.Views;
using PlaylistParserLibrary.Parsers;

namespace AvaloniaPlaylistParser
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaLocator.CurrentMutable.BindToSelf(new ParserService(new YoutubeParser()));

            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
