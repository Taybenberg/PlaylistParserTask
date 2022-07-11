using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlaylistParserLibrary.Parsers
{
    public class StubParser : IPlaylistParser
    {
        public async Task<Playlist> GetPlaylistAsync(string url)
        {
            return new Playlist
            {
                Thumbnail = "https://i.ytimg.com/vi/MRTX1M591lA/hqdefault.jpg?sqp=-oaymwEbCKgBEF5IVfKriqkDDggBFQAAiEIYAXABwAEG&rs=AOn4CLCFgGGtFs54P3fmFzIL7atlFH_ZQQ",
                Name = "Evening 60 Years",
                Description = "2 items",
                Songs = new List<Song>
                {
                    new Song
                    {
                        SongName = "Spring",
                        AlbumName = "The Four Seasons (Vivaldi) Wedding String Quartet",
                        ArtistName = "The String Quartet Channel",
                        Duration = "2:27"
                    },
                    new Song
                    {
                        SongName = "Winter",
                        AlbumName = "The Four Seasons (Vivaldi) Wedding String Quartet",
                        ArtistName = "The String Quartet Channel",
                        Duration = "1:47"
                    }
                }
            };
        }
    }
}
