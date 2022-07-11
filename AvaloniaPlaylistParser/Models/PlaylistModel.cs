using Avalonia.Media.Imaging;
using System.Collections.ObjectModel;

namespace AvaloniaPlaylistParser.Models
{
    public class PlaylistModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Bitmap Thumbnail { get; set; }
        public ObservableCollection<SongModel> Songs { get; set; }
    }
}
