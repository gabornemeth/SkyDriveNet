namespace SkyDriveNet.Models
{
    /// <summary>
    /// The Folder object contains info about a user's folders in SkyDrive.
    /// </summary>
    public class Folder : MetaData
    {
        /// <summary>
        /// Total number of items in the folder.
        /// </summary>
        public int count { get; set; }
    }
}
