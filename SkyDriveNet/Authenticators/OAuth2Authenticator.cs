using System.Collections.Generic;
using RestSharp;

namespace SkyDriveNet.Authenticators
{
    /// <summary>
    /// OAuth 2.0 authenticator
    /// </summary>
    public class OAuth2Authenticator : RestSharp.OAuth2Authenticator
    {
        public OAuth2Authenticator(string accessToken)
            : base(accessToken)
        {
        }

        public override void Authenticate(IRestClient client, IRestRequest request)
        {
            if (request.Method == Method.PUT)
            {
                //Do the parameters as URL segments for PUT
                request.AddParameter("access_token", AccessToken, ParameterType.UrlSegment);
                request.Parameters.Sort(new QueryParameterComparer());
            }
            else
            {
                request.AddParameter("access_token", AccessToken);
                request.Parameters.Sort(new QueryParameterComparer());
            }
        }

        // Nested Types
        private class QueryParameterComparer : IComparer<Parameter>
        {
            // Methods
            public int Compare(Parameter x, Parameter y)
            {
                return ((x.Name == y.Name) ? string.Compare(x.Value.ToString(), y.Value.ToString()) : string.Compare(x.Name, y.Name));
            }
        }
    }

}
