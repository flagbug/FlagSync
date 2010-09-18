using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FlagSync.Core.PlaylistReader
{
    public class Song
    {
        private string title;
        public string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.title = value;
            }
        }

        private string artist;
        public string Artist
        {
            get
            {
                return this.artist;
            }

            set
            {
                this.artist = value;
            }
        }
        private string album;
        public string Album
        {
            get
            {
                return this.album;
            }

            set
            {
                this.album = value;
            }
        }

        private FileInfo file;
        public FileInfo File
        {
            get
            {
                return this.file;
            }

            set
            {
                this.file = value;
            }
        }

        public Song(string title, string artist, string album, FileInfo file)
        {
            this.title = title;
            this.artist = artist;
            this.album = album;
            this.file = file;
        }
    }
}
