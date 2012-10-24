using System.Collections.Generic;
using Newtonsoft.Json;

namespace SkyDriveNet.Models
{
    /// <summary>
    /// Helper class for deserializing metadata list
    /// e.g. /me/FOLDER_ID/files
    /// </summary>
    //public class MetaDataCollection
    //{
    //    public List<MetaData> data { get; set; }
    //}

    /// <summary>
    /// Share information
    /// </summary>
    public class ShareInfo
    {
        public const string PeopleISelected = "People I selected";
        public const string JustMe = "Just me";
        public const string Everyone = "Everyone (public)";
        public const string Friends = "Friends";
        public const string MyFriendsAndTheirFrends = "My friends and their friends";
        public const string PeopleWithALink = "People with a link";

        /// <summary>
        /// Access type
        /// </summary>
        public string Access { get; set; }
    }

    /// <summary>
    /// Base class of SkyDrive file like objects
    /// TODO: subtypes and deserializing proper type when query files
    /// </summary>
    public class MetaData
    {
        /// <summary>
        /// The object's ID.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Info about the user who created the object.
        /// </summary>
        public User From { get; set; }
        /// <summary>
        /// The name of the object.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// A description of the folder, or null if no description is specified.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// The resource ID of the parent.
        /// </summary>
        public string Parent_Id { get; set; }
        /// <summary>
        /// The URL to upload content hosted in SkyDrive.
        /// </summary>
        public string Upload_Location { get; set; }
        /// <summary>
        /// The number of comments that are associated with the object.
        /// </summary>
        //public int comments_count { get; set; }
        /// <summary>
        /// A value that indicates whether comments are enabled for the file.
        /// If comments can be made, this value is true; otherwise, it is false.
        /// </summary>
        //public bool comments_enabled { get; set; }
        /// <summary>
        /// A value that indicates whether this object can be embedded.
        ///  If this object can be embedded, this value is true; otherwise, it is false.
        /// </summary>
        [JsonProperty(PropertyName = "is_embeddable")]
        public bool IsEmbeddable { get; set; }
        /// <summary>
        /// The URL of the object, hosted in SkyDrive.
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// The type of object.
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Object that contains permission info.
        /// </summary>
        public ShareInfo Shared_With { get; set; }
        /// <summary>
        /// The time, in ISO 8601 format, at which the object was created.
        /// </summary>
        public string created_time { get; set; }
        /// <summary>
        /// The time, in ISO 8601 format, at which the object was last updated.
        /// </summary>
        public string updated_time { get; set; }

        public IList<MetaData> Files
        {
            get;
            internal set;
        }

        public bool IsDirectory
        {
            get { return Id.StartsWith("folder"); }
        }
    }
}
