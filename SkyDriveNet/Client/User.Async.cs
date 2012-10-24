using System.Collections.Generic;
using RestSharp;
using System;
#if WINDOWS_PHONE
using System.Net;
#else
using RestSharp.Contrib;
#endif
using SkyDriveNet.Exceptions;
using SkyDriveNet.Models;

namespace SkyDriveNet
{
    public partial class SkyDriveClient
    {
        public string GetAccessTokenUrl(string redirectUrl, string authorizationCode)
        {
            if (GrantFlow != OAuth2GrantFlow.AuthorizationCode)
                throw new SkyDriveException("Access token can be obtained only when using Authorization code grant flow.");

            var paramList = new List<string>
                                         {
                                             "client_id=" + HttpUtility.UrlEncode(_clientId),
                                             "redirect_uri=" + HttpUtility.UrlEncode(redirectUrl),
                                             "client_secret=" + HttpUtility.UrlEncode(_clientSecret),
                                             "grant_type=" + HttpUtility.UrlEncode("authorization_code"),
                                             "code="+ HttpUtility.UrlEncode(authorizationCode)
                                         };

            var authorizeUri = new UriBuilder(OAuthTokenUrl);
            authorizeUri.Query = String.Join("&", paramList.ToArray());
            return authorizeUri.Uri.AbsoluteUri;
        }

        //private UserLogin GetUserLoginFromParams(string urlParams)
        //{
        //    var userLogin = new UserLogin();

        //    //TODO - Make this not suck
        //    var parameters = urlParams.Split('&');

        //    foreach (var parameter in parameters)
        //    {
        //        if (parameter.Split('=')[0] == "oauth_token_secret")
        //        {
        //            userLogin.Secret = parameter.Split('=')[1];
        //        }
        //        else if (parameter.Split('=')[0] == "oauth_token")
        //        {
        //            userLogin.Token = parameter.Split('=')[1];
        //        }
        //    }

        //    return userLogin;
        //}

        public void GetAccessTokenAsync(string redirectUrl, string authorizationCode, Action<UserLogin> success, Action<SkyDriveException> failure)
        {
            var request = new RestRequest(Method.POST);
            request.Resource = "";
            request.AddParameter("client_id", _clientId);
            request.AddParameter("redirect_uri", redirectUrl);
            request.AddParameter("client_secret", _clientSecret);
            request.AddParameter("code", authorizationCode);
            request.AddParameter("grant_type", "authorization_code");

            ExecuteAsync<UserLogin>(ApiType.OAuthToken, request, userLogin =>
                                                                     {
                                                                         UserLogin = userLogin;
                                                                         success(userLogin);
                                                                     }, failure);

            //ExecuteAsync(ApiType.Base, request, response =>
            //{
            //    var userLogin = GetUserLoginFromParams(response.Content);
            //    success(userLogin);
            //}, failure);
        }

        /*
        /// <summary>
        /// Gets a token from the almightly dropbox.com (Token cant be used until authorized!)
        /// </summary>
        public void GetTokenAsync(Action<UserLogin> success, Action<DropboxException> failure)
        {
            _restClient.BaseUrl = ApiBaseUrl;
            _restClient.Authenticator = new OAuthAuthenticator(_restClient.BaseUrl, _apiKey, _appsecret);

            var request = _requestHelper.CreateTokenRequest();

            ExecuteAsync(ApiType.Base, request, response =>
            {
                var userLogin = GetUserLoginFromParams(response.Content);
                UserLogin = userLogin;
                success(userLogin);
            }, failure);
        }

        /// <summary>
        /// Converts a request token into an Access token after the user has authorized access via dropbox.com
        /// </summary>
        /// <param name="success"></param>
        /// <param name="failure"></param>
        public void GetAccessTokenAsync(Action<UserLogin> success, Action<DropboxException> failure)
        {
            _restClient.BaseUrl = ApiBaseUrl;
            _restClient.Authenticator = new OAuthAuthenticator(_restClient.BaseUrl, _apiKey, _appsecret, UserLogin.Token, UserLogin.Secret);

            var request = _requestHelper.CreateAccessTokenRequest();

            ExecuteAsync(ApiType.Base, request, response =>
            {
                var userLogin = GetUserLoginFromParams(response.Content);
                UserLogin = userLogin;
                success(userLogin);
            }, failure);
        }

        /// <summary>
        /// Gets AccountInfo
        /// </summary>
        /// <param name="success"></param>
        /// <param name="failure"></param>
        public void AccountInfoAsync(Action<AccountInfo> success, Action<DropboxException> failure)
        {
            //This has to be here as Dropbox change their base URL between calls
            _restClient.BaseUrl = ApiBaseUrl;
            _restClient.Authenticator = new OAuthAuthenticator(_restClient.BaseUrl, _apiKey, _appsecret, UserLogin.Token, UserLogin.Secret);

            var request = _requestHelper.CreateAccountInfoRequest();

            ExecuteAsync(ApiType.Base, request, success, failure);
        }
         */
    }
}