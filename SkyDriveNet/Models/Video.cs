namespace SkyDriveNet.Models
{
    /// <summary>
    /// The Video object contains info about a user's videos on SkyDrive. 
    /// </summary>
    public class Video : File
    {
        /// <summary>
        /// The number of tags on the video.
        /// </summary>
        public int tags_count { get; set; }
        /// <summary>
        /// A value that indicates whether tags are enabled for the video.
        /// If tags can be set, this value is true; otherwise, it is false.
        /// </summary>
        public int tags_enabled { get; set; }
        /// <summary>
        /// A URL of a picture that represents the video.
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// The height, in pixels, of the video.
        /// </summary>
        public int height { get; set; }
        /// <summary>
        /// The width, in pixels, of the video.
        /// </summary>
        public int width { get; set; }
        /// <summary>
        /// The duration, in milliseconds, of the video run time.
        /// </summary>
        public int duration { get; set; }
        /// <summary>
        /// The bit rate, in bits per second, of the video.
        /// </summary>
        public int bitrate { get; set; }
    }
}
