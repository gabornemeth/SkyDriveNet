using System;
using RestSharp;
using System.Net;

namespace SkyDriveNet.Exceptions
{
    public class SkyDriveException : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        /// <summary>
        /// The response of the error call (for Debugging use)
        /// </summary>
        public IRestResponse Response { get; private set; }

        public SkyDriveException()
        {
        }

        public SkyDriveException(string message)
            : base(message)
        {

        }

        public SkyDriveException(IRestResponse r)
        {
            Response = r;
            StatusCode = r.StatusCode;
        }
    }
}
