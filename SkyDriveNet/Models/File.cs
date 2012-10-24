namespace SkyDriveNet.Models
{
    /// <summary>
    /// The File object contains info about a user's files in SkyDrive. 
    /// </summary>
    public class File : MetaData
    {
        // The following attibutes are not common!
        /// <summary>
        /// The size, in bytes, of the file.
        /// </summary>
        public int size { get; set; }
        /// <summary>
        /// A value that indicates whether comments are enabled for the file.
        /// If comments can be made, this value is true; otherwise, it is false.
        /// </summary>
        public bool comments_enabled { get; set; }
        /// <summary>
        /// The number of comments that are associated with the file.
        /// </summary>
        public int comments_count { get; set; }
        /// <summary>
        /// The download URL for the file.
        /// Warning  
        /// This value is not persistent. Use it immediately after making the request, and avoid caching.
        /// </summary>
        public string source { get; set; }
    }
}
