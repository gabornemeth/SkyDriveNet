namespace SkyDriveNet.Models
{
    /// <summary>
    /// The Album object contains info about a user's albums in Microsoft SkyDrive.
    /// There is virtually no difference between Album and Folder, just the type
    /// </summary>
    public class Album : MetaData
    {
        /// <summary>
        /// Total number of items in the album.
        /// </summary>
        public int count { get; set; }
    }
}
