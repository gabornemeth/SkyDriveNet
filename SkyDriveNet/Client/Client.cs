using System;
using System.Collections.Generic;
using System.Net;
using RestSharp;
using RestSharp.Deserializers;
using System.Threading.Tasks;
using SkyDriveNet.Exceptions;
using SkyDriveNet.Extensions;
using SkyDriveNet.Helpers;
using SkyDriveNet.Models;

#if !WINDOWS_PHONE
using RestSharp.Contrib;
#endif

namespace SkyDriveNet
{
    public enum OAuth2GrantFlow
    {
        Implicit,
        AuthorizationCode
    }

    public partial class SkyDriveClient
    {
        /// <summary>
        /// The URI for the OAuth service's Authorize endpoint.
        /// </summary>
        private const string OAuthAuthorizeUrl = "https://login.live.com/oauth20_authorize.srf";

        /// <summary>
        /// The URI for the API service endpoint.
        /// </summary>
        private const string ApiServiceUrl = "https://apis.live.net/v5.0/";

        /// <summary>
        /// The applications redirect URI (does not need to exist).
        /// </summary>
        public const string RedirectUrl = " https://login.live.com/oauth20_desktop.srf";

        /// <summary>
        /// The applications redirect URI (does not need to exist).
        /// </summary>
        public const string OAuthTokenUrl = " https://login.live.com/oauth20_token.srf";

        private UserLogin _userLogin;

        /// <summary>
        /// Contains the Users Token and Secret
        /// </summary>
        public UserLogin UserLogin
        {
            get { return _userLogin; }
            set
            {
                _userLogin = value;
                SetAuthProviders();
            }
        }

        public OAuth2GrantFlow GrantFlow { get; set; }

        /// <summary>
        /// The applications client ID.
        /// </summary>
        private readonly string _clientId;
        private readonly string _clientSecret;
        private RestClient _restClient;
        private RestClient _restClientToken;
        private RestClient _restClientContent;
        private RequestHelper _requestHelper;

        /// <summary>
        /// Default Constructor for the SkyDriveClient
        /// </summary>
        /// <param name="clientId">The Api Key to use for the Dropbox Requests</param>
        public SkyDriveClient(string clientId)
            : this(clientId, null)
        {
        }

        /// <summary>
        /// Creates an instance of the DropNetClient given an API Key/Secret and a User Token/Secret
        /// </summary>
        /// <param name="apiKey">The Api Key to use for the Dropbox Requests</param>
        /// <param name="appSecret">The Api Secret to use for the Dropbox Requests</param>
        /// <param name="userToken">The User authentication token</param>
        /// <param name="userSecret">The Users matching secret</param>
        public SkyDriveClient(string clientId, string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;

            LoadClient();
        }

        private void LoadClient()
        {
            _restClient = new RestClient(ApiServiceUrl);
            _restClient.ClearHandlers();
            _restClient.AddHandler("*", new JsonDeserializer());

            _restClientToken = new RestClient(OAuthTokenUrl);
            _restClient.ClearHandlers();
            _restClient.AddHandler("*", new JsonDeserializer());

            _requestHelper = new RequestHelper();
        }

        /// <summary>
        /// Helper Method to Build up the Url to authorize
        /// </summary>
        /// <param name="scopes">scopes to be accessed</param>
        /// <returns></returns>
        public string BuildAuthorizeUrl(string redirectUrl, params string[] scopes)
        {
            List<string> paramList;
            if (GrantFlow == OAuth2GrantFlow.Implicit)
                paramList = new List<string>
                                         {
                                             "client_id=" + HttpUtility.UrlEncode(_clientId),
                                             "scope=" + HttpUtility.UrlEncode(String.Join(" ", scopes)),
                                             "response_type=" + HttpUtility.UrlEncode("token"),
#if WINDOWS_PHONE
                                             "display=" + HttpUtility.UrlEncode("touch"),
#endif
                                             "redirect_uri=" + HttpUtility.UrlEncode(string.IsNullOrEmpty(redirectUrl) ? RedirectUrl : redirectUrl)
                                         };
            else
            {
                paramList = new List<string>
                                         {
                                             "client_id=" + HttpUtility.UrlEncode(_clientId),
                                             //"client_secret=" + HttpUtility.UrlEncode(_clientSecret),
                                             "scope=" + HttpUtility.UrlEncode(String.Join(" ", scopes)),
                                             "response_type=" + HttpUtility.UrlEncode("code"),
                                             "display="+HttpUtility.UrlEncode("touch"),
                                             "redirect_uri=" + HttpUtility.UrlEncode(redirectUrl)
                                         };
            }

            var authorizeUri = new UriBuilder(OAuthAuthorizeUrl);
            authorizeUri.Query = String.Join("&", paramList.ToArray());
            return authorizeUri.Uri.AbsoluteUri;
        }

#if !WINDOWS_PHONE && !WINRT
        private T Execute<T>(ApiType apiType, IRestRequest request) where T : new()
        {
            IRestResponse<T> response;
            if (apiType == ApiType.Base)
            {
                response = _restClient.Execute<T>(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new SkyDriveException(response);
                }
            }
            else
            {
                response = _restClientContent.Execute<T>(request);

                if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.PartialContent)
                {
                    throw new SkyDriveException(response);
                }
            }

            return response.Data;
        }

        private IRestResponse Execute(ApiType apiType, IRestRequest request)
        {
            IRestResponse response;
            if (apiType == ApiType.Base)
            {
                response = _restClient.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new SkyDriveException(response);
                }
            }
            else
            {
                response = _restClientContent.Execute(request);

                if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.PartialContent)
                {
                    throw new SkyDriveException(response);
                }
            }

            return response;
        }
#endif

        private void ExecuteAsync(ApiType apiType, IRestRequest request, Action<IRestResponse> success, Action<SkyDriveException> failure)
        {
#if WINDOWS_PHONE
            //check for network connection
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                //do nothing
                failure(new SkyDriveException
                {
                    StatusCode = System.Net.HttpStatusCode.BadGateway
                });
                return;
            }
#endif
            if (apiType == ApiType.Base)
            {
                _restClient.ExecuteAsync(request, (response, asynchandle) =>
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        failure(new SkyDriveException(response));
                    }
                    else
                    {
                        success(response);
                    }
                });
            }
            else
            {
                _restClientContent.ExecuteAsync(request, (response, asynchandle) =>
                {
                    if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.PartialContent)
                    {
                        failure(new SkyDriveException(response));
                    }
                    else
                    {
                        success(response);
                    }
                });
            }
        }

        private void ExecuteAsync<T>(ApiType apiType, IRestRequest request, Action<T> success, Action<SkyDriveException> failure) where T : new()
        {
#if WINDOWS_PHONE
            //check for network connection
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                //do nothing
                failure(new SkyDriveException
                {
                    StatusCode = System.Net.HttpStatusCode.BadGateway
                });
                return;
            }
#endif
            if (apiType == ApiType.Base)
            {
                _restClient.ExecuteAsync<T>(request, (response, asynchandle) =>
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        failure(new SkyDriveException(response));
                    }
                    else
                    {
                        success(response.Data);
                    }
                });
            }
            else if (apiType == ApiType.OAuthToken)
            {
                _restClientToken.ExecuteAsync<T>(request, (response, asynchandle) =>
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        failure(new SkyDriveException(response));
                    }
                    else
                    {
                        success(response.Data);
                    }
                });
            }
            else
            {
                _restClientContent.ExecuteAsync<T>(request, (response, asynchandle) =>
                {
                    if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.PartialContent)
                    {
                        failure(new SkyDriveException(response));
                    }
                    else
                    {
                        success(response.Data);
                    }
                });
            }
        }

#if !WINRT

        private Task<T> ExecuteTask<T>(ApiType apiType, IRestRequest request) where T : new()
        {
            if (apiType == ApiType.Base)
            {
                return _restClient.ExecuteTask<T>(request);
            }
            else
            {
                return _restClientContent.ExecuteTask<T>(request);
            }
        }

        private Task<IRestResponse> ExecuteTask(ApiType apiType, IRestRequest request)
        {
            if (apiType == ApiType.Base)
            {
                return _restClient.ExecuteTask(request);
            }
            else
            {
                return _restClientContent.ExecuteTask(request);
            }
        }

#endif
        /// <summary>
        /// Gets if the client is authorized
        /// </summary>
        public bool IsAuthorized
        {
            get { return _userLogin != null && !string.IsNullOrEmpty(_userLogin.AccessToken); }
        }

        private void SetAuthProviders()
        {
            if (IsAuthorized)
            {
                //Set the OauthAuthenticator only when the UserLogin property changes
                _restClient.Authenticator = new Authenticators.OAuth2Authenticator(_userLogin.AccessToken);
            }
        }

        /// <summary>
        /// Set access token from Url fragment
        /// </summary>
        /// <param name="urlFragment">URL fragment</param>
        /// <returns>true, if access token can be determined from fragment. false otherwise</returns>
        public bool SetAccessTokenFromUrl(string urlFragment)
        {
            var fragments = ProcessFragments(urlFragment);
            string accessToken;
            var ret = fragments.TryGetValue("access_token", out accessToken);
            if (ret)
                UserLogin = new UserLogin { AccessToken = accessToken };
            return ret;
        }

        /// <summary>
        /// Process the URI fragment string.
        /// </summary>
        /// <param name="fragment">The URI fragment.</param>
        /// <returns>The key-value pairs.</returns>
        private Dictionary<string, string> ProcessFragments(string fragment)
        {
            var processedFragments = new Dictionary<string, string>();
            if (processedFragments.Count == 0)
                return processedFragments;

            if (fragment[0] == '#')
            {
                fragment = fragment.Substring(1);
            }

            string[] fragmentParams = fragment.Split('&');

            foreach (string fragmentParam in fragmentParams)
            {
                string[] keyValue = fragmentParam.Split('=');

                if (keyValue.Length == 2)
                {
                    processedFragments.Add(keyValue[0], HttpUtility.UrlDecode(keyValue[1]));
                }
            }

            return processedFragments;
        }

        enum ApiType
        {
            Base,
            OAuthToken,
            Content
        }
    }
}
