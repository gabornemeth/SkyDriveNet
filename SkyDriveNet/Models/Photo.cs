using System.Collections.Generic;

namespace SkyDriveNet.Models
{
    /// <summary>
    /// Geographical location
    /// </summary>
    public class Location
    {
        /// <summary>
        /// The latitude portion of the location where the photo was taken, expressed as positive (north) or negative (south) degrees relative to the equator.
        /// </summary>
        public double latitude { get; set; }
        /// <summary>
        /// The longitude portion of the location where the photo was taken, expressed as positive (east) or negative (west) degrees relative to the Prime Meridian.
        /// </summary>
        public double longitude { get; set; }
        /// <summary>
        /// The altitude portion of the location where the photo was taken, expressed as positive (above) or negative (below) values relative to sea level, in units of measurement as determined by the camera.
        /// </summary>
        public double altitude { get; set; }
    }

    /// <summary>
    /// The Photo object contains info about a user's photos on SkyDrive. 
    /// </summary>
    public class Photo : File
    {
        /// <summary>
        /// The number of tags on the photo.
        /// </summary>
        public int tags_count { get; set; }
        /// <summary>
        /// A value that indicates whether tags are enabled for the photo.
        /// If tags can be set, this value is true; otherwise, it is false.
        /// </summary>
        public bool tags_enabled { get; set; }
        /// <summary>
        /// A URL of the photo's picture.
        /// </summary>
        public string picture { get; set; }
        /// <summary>
        /// Info about various sizes of the photo.
        /// </summary>
        public List<Image> images;
        /// <summary>
        /// The date, in ISO 8601 format, on which the photo was taken, or null if no date is specified.
        /// </summary>
        public string when_taken { get; set; }
        /// <summary>
        /// The height, in pixels, of the photo.
        /// </summary>
        public int height { get; set; }
        /// <summary>
        /// The width, in pixels, of the photo.
        /// </summary>
        public int width { get; set; }
        /// <summary>
        /// The location where the photo was taken.
        /// Note The location object is not available for shared photos.
        /// </summary>
        public Location location { get; set; }
        /// <summary>
        /// The manufacturer of the camera that took the photo.
        /// </summary>
        public string camera_make { get; set; }
        /// <summary>
        /// The brand and model number of the camera that took the photo.
        /// </summary>
        public string camera_model { get; set; }
        /// <summary>
        /// The f-number that the photo was taken at.
        /// </summary>
        public double focal_ratio { get; set; }
        /// <summary>
        /// The focal length that the photo was taken at, typically expressed in millimeters for newer lenses.
        /// </summary>
        public double focal_length { get; set; }
        /// <summary>
        /// The numerator of the shutter speed (for example, the "1" in "1/15 s") that the photo was taken at.
        /// </summary>
        public double exposure_numerator { get; set; }
        /// <summary>
        /// The denominator of the shutter speed (for example, the "15" in "1/15 s") that the photo was taken at.
        /// </summary>
        public double exposure_denominator { get; set; }
    }

    /// <summary>
    /// Info about a particular size of photo
    /// </summary>
    public class Image
    {
        /// <summary>
        /// maximum size: 2048 × 2048 pixels
        /// </summary>
        public const string full = "full";
        /// <summary>
        /// maximum size 800 × 800 pixels
        /// </summary>
        public const string normal = "normal";
        /// <summary>
        /// maximum size 176 × 176 pixels
        /// </summary>
        public const string album = "album";
        /// <summary>
        /// maximum size 96 × 96 pixels
        /// </summary>
        public const string small = "small";

        /// <summary>
        /// The height, in pixels, of this image of this particular size.
        /// </summary>
        public int height { get; set; }
        /// <summary>
        /// The width, in pixels, of this image of this particular size.
        /// </summary>
        public int width { get; set; }
        /// <summary>
        /// A URL of the source file of this image of this particular size.
        /// </summary>
        public string source { get; set; }
        /// <summary>
        /// The type of this image of this particular size. Valid values are:
        /// </summary>
        public string type { get; set; }
    }
}
