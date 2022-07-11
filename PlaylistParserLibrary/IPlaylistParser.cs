using System.Threading.Tasks;

namespace PlaylistParserLibrary
{
    public interface IPlaylistParser
    {
        Task<Playlist> GetPlaylistAsync(string url);
    }
}
