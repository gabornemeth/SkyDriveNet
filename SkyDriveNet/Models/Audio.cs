namespace SkyDriveNet.Models
{
    public class Audio : File
    {
        /// <summary>
        /// The audio's title.
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// The audio's artist name.
        /// </summary>
        public string artist { get; set; }
        /// <summary>
        /// The audio's album name.
        /// </summary>
        public string album { get; set; }
         /// <summary>
         /// The artist name of the audio's album.
         /// </summary>
        public string album_artist { get; set; }
        /// <summary>
        /// The audio's genre.
        /// </summary>
        public string genre { get; set; }
        /// <summary>
        /// The audio's playing time, in milliseconds.
        /// </summary>
        public int duration { get; set; }
        /// <summary>
        /// A URL to view the audio's picture on SkyDrive.
        /// </summary>
        public string picture { get; set; }
    }
}
