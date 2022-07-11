using HtmlAgilityPack;
using PuppeteerSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlaylistParserLibrary.Parsers
{
    public class YoutubeParser : IPlaylistParser
    {
        private async Task<string> loadPageAsync(string url)
        {
            using (var browserFetcher = new BrowserFetcher())
                await browserFetcher.DownloadAsync();

            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            using (var page = await browser.NewPageAsync())
            {
                await page.SetUserAgentAsync("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/103.0.5060.114 Safari/537.36 Edg/103.0.1264.49");

                await page.GoToAsync(url);

                //ensure that web page loaded
                await page.WaitForXPathAsync("//div[@id='contents' and (@class='style-scope ytd-playlist-video-list-renderer' or @class='style-scope ytmusic-playlist-shelf-renderer' or @class='style-scope ytmusic-shelf-renderer')]");

                return await page.GetContentAsync();
            }
        }

        public async Task<Playlist> GetPlaylistAsync(string url)
        {
            //HtmlAgilityPack can't parse dynamic web pages like Youtube, so we need to load it by web browser
            var htmlPage = await loadPageAsync(url);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(htmlPage);

            bool isYtMusic = url.Contains("music.youtube.com");

            var playlist = new Playlist();

            parseHeader(htmlDoc, ref playlist, isYtMusic);
            parseSongs(htmlDoc, ref playlist, isYtMusic);

            return playlist;
        }

        private void parseHeader(HtmlDocument htmlDoc, ref Playlist playlist, bool isYtMusic)
        {
            var playlistHeaderNode = isYtMusic ?
                htmlDoc.DocumentNode.SelectSingleNode("//div[@class='content-container style-scope ytmusic-detail-header-renderer']") :
                htmlDoc.DocumentNode.SelectSingleNode("//div[@id='items' and @class='style-scope ytd-playlist-sidebar-renderer']");

            if (playlistHeaderNode != null)
            {
                var thumbnailNode = playlistHeaderNode.SelectSingleNode($".//img[@id='img']");
                if (thumbnailNode != null)
                    playlist.Thumbnail = thumbnailNode.Attributes["src"].Value;

                var nameNode = isYtMusic ?
                    playlistHeaderNode.SelectSingleNode($".//*[@class='title style-scope ytmusic-detail-header-renderer']") :
                    playlistHeaderNode.SelectSingleNode($".//a[@class='yt-simple-endpoint style-scope yt-formatted-string']");
                if (nameNode != null)
                    playlist.Name = nameNode.InnerText;

                var statsNodes = playlistHeaderNode.SelectNodes(".//span[@class='style-scope yt-formatted-string']");
                playlist.Description = string.Join(" ", statsNodes.Select(x => x.InnerHtml));
            }
        }

        private void parseSongs(HtmlDocument htmlDoc, ref Playlist playlist, bool isYtMusic)
        {
            var songs = new List<Song>();

            var songNodes = isYtMusic ?
                htmlDoc.DocumentNode.SelectNodes("//div[@id='contents' and (@class='style-scope ytmusic-playlist-shelf-renderer' or @class='style-scope ytmusic-shelf-renderer')]/*") :
                htmlDoc.DocumentNode.SelectNodes("//div[@id='contents' and @class='style-scope ytd-playlist-video-list-renderer']/*");

            if (songNodes.Any())
            {
                foreach (var node in songNodes)
                {
                    var song = new Song
                    {
                        AlbumName = playlist.Name
                    };

                    var container = isYtMusic ?
                        node : node.SelectSingleNode(".//div[@id='container' and @class='style-scope ytd-playlist-video-renderer']");

                    if (container != null)
                    {
                        var durationNode = isYtMusic ?
                            container.SelectSingleNode(".//div[@class='fixed-columns style-scope ytmusic-responsive-list-item-renderer']/*[@title]") :
                            container.SelectSingleNode(".//span[@id='text' and @class='style-scope ytd-thumbnail-overlay-time-status-renderer']");
                        if (durationNode != null)
                            song.Duration = isYtMusic ?
                                durationNode.Attributes["title"].Value :
                                durationNode.InnerText.Trim();

                        var songNameNode = isYtMusic ?
                            container.SelectSingleNode(".//*[@class='title style-scope ytmusic-responsive-list-item-renderer complex-string' and @title]") :
                            container.SelectSingleNode(".//a[@id='video-title' and @title]");
                        if (songNameNode != null)
                            song.SongName = songNameNode.Attributes["title"].Value;

                        var artistNode = isYtMusic ?
                            container.SelectSingleNode(".//*[@class='flex-column style-scope ytmusic-responsive-list-item-renderer complex-string' and @title]") :
                            container.SelectSingleNode(".//a[@class='yt-simple-endpoint style-scope yt-formatted-string']");
                        if (artistNode != null)
                            song.ArtistName = isYtMusic ?
                                artistNode.Attributes["title"].Value :
                                artistNode.InnerText;

                        songs.Add(song);
                    }
                }
            }

            playlist.Songs = songs;
        }
    }
}
