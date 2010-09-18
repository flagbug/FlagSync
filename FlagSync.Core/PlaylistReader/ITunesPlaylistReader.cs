using System;
using System.Collections.Generic;
using System.Text;
using iTunesLib;

namespace FlagSync.Core.PlaylistReader
{
    public class ITunesPlaylistReader : IPlaylistReader
    {
        public IEnumerable<Song> GetSongs(string playlistName)
        {
            List<Song> songs = new List<Song>();
            iTunesAppClass app = new iTunesAppClass();
            IITPlaylistCollection playlists = app.LibrarySource.Playlists;

            for(int i = 1; i < playlists.Count + 1; i++)
            {
                if(playlists[i].Name.Equals(playlistName, StringComparison.OrdinalIgnoreCase))
                {
                    IITTrackCollection tracks = playlists[i].Tracks;

                    for(int j = 1; j < tracks.Count + 1; j++)
                    {
                        if(tracks[j].Kind == ITTrackKind.ITTrackKindFile)
                        {
                            IITFileOrCDTrack track = (IITFileOrCDTrack)tracks[i];

                            songs.Add(new Song(track.Name, track.Artist, track.Album, new System.IO.FileInfo(track.Location)));
                        }
                    }

                    break;
                }
            }
            
            return songs;
        }

        public IEnumerable<string> GetPlaylistNames()
        {
            List<string> names = new List<string>();
            iTunesAppClass app = new iTunesAppClass();
            IITPlaylistCollection playlists = app.LibrarySource.Playlists;

            for(int i = 1; i < playlists.Count + 1; i++)
            {
                if(playlists[i].Kind == ITPlaylistKind.ITPlaylistKindUser)
                {
                    names.Add(playlists[i].Name);
                }
            }

            return names;
        }
    }
}
