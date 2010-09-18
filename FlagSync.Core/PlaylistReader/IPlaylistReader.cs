using System.Collections.Generic;

namespace FlagSync.Core.PlaylistReader
{
    public interface IPlaylistReader
    {
        IEnumerable<Song> GetSongs(string playlistName);
        IEnumerable<string> GetPlaylistNames();
    }
}
