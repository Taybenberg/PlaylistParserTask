using System.Collections.Generic;

namespace PlaylistParserLibrary
{
    public class Playlist
    {
        public string Thumbnail { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IEnumerable<Song> Songs { get; set; }
    }
}
