using Avalonia;
using Avalonia.Media.Imaging;
using AvaloniaPlaylistParser.Models;
using AvaloniaPlaylistParser.Services;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;

namespace AvaloniaPlaylistParser.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            ParsePlaylistCommand = ReactiveCommand.CreateFromTask(ParsePlaylist);
        }

        private bool _isBusy = false;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                if (value == _isBusy)
                    return;
                _isBusy = value;

                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(IsNotBusy));
            }
        }
        public bool IsNotBusy => !_isBusy;

        private string _url;
        public string URL
        {
            get
            {
                return _url;
            }
            set
            {
                if (value == _url)
                    return;
                _url = value;
            }
        }

        private Bitmap _thumbnail;
        public Bitmap Thumbnail
        {
            get
            {
                return _thumbnail;
            }
            set
            {
                if (value == _thumbnail)
                    return;
                _thumbnail = value;

                this.RaisePropertyChanged();
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (value == _name)
                    return;
                _name = value;

                this.RaisePropertyChanged();
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (value == _description)
                    return;
                _description = value;

                this.RaisePropertyChanged();
            }
        }

        private ObservableCollection<SongModel> _songs;
        public ObservableCollection<SongModel> Songs
        {
            get => _songs;
            set
            {
                if (value != null)
                {
                    if (value == _songs)
                        return;
                    _songs = value;

                    this.RaisePropertyChanged();
                }
            }
        }

        public ReactiveCommand<Unit, Unit> ParsePlaylistCommand { get; }

        public async Task ParsePlaylist()
        {
            IsBusy = true;

            var parserService = AvaloniaLocator.CurrentMutable.GetService<ParserService>();
            var playlist = await parserService.GetPlaylistAsync(URL);

            Thumbnail = playlist.Thumbnail;
            Name = playlist.Name;
            Description = playlist.Description;
            Songs = playlist.Songs;

            IsBusy = false;
        }
    }
}
