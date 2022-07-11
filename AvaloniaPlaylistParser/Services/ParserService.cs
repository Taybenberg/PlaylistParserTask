using Avalonia.Media.Imaging;
using AvaloniaPlaylistParser.Models;
using PlaylistParserLibrary;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace AvaloniaPlaylistParser.Services
{
    public class ParserService
    {
        private readonly IPlaylistParser _parser;

        public ParserService(IPlaylistParser parser) => _parser = parser;

        public async Task<PlaylistModel> GetPlaylistAsync(string url)
        {
            var playlistModel = new PlaylistModel();

            var playlist = await _parser.GetPlaylistAsync(url);

            playlistModel.Name = playlist.Name;
            playlistModel.Description = playlist.Description;

            using (var client = new HttpClient())
            {
                var buffer = await client.GetByteArrayAsync(playlist.Thumbnail);

                using var ms = new MemoryStream(buffer);

                playlistModel.Thumbnail = new Bitmap(ms);
            }

            var songs = new ObservableCollection<SongModel>();
            foreach (var song in playlist.Songs)
                songs.Add(new SongModel
                {
                    SongName = song.SongName,
                    AlbumName = song.AlbumName,
                    ArtistName = song.ArtistName,
                    Duration = song.Duration
                });
            playlistModel.Songs = songs;

            return playlistModel;
        }
    }
}
