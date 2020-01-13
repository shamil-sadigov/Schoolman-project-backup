using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Schoolman.Student.Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Schoolman.Student.Infrastructure.Helpers
{
    public class UrlService
    {
        private readonly IOptionsMonitor<UrlOptions> urlSettings;
        private bool SpaHostUrl;
        private Uri uri;

        public UrlService(IOptionsMonitor<UrlOptions> urlSettings)
        {
            this.urlSettings = urlSettings;
        }

        /// <summary>
        /// Designed for developing mode to user mainly angular localhost:4200 as based address to build verification email url
        /// </summary>
        /// <returns></returns>
        public UrlService UseSpaUrlAddress()
        {
            SpaHostUrl = true;
            return this;
        }

        public UrlService UseWebapiUrlAddress()
        {
            SpaHostUrl = false;
            return this;
        }


        /// <summary>
        /// By default, build url for Aspnet hosting url. Url building based on UrlOptions configure in appsettings
        /// </summary>
        /// <param name="webApiPath"></param>
        /// <returns></returns>
        public Uri BuildConfirmationUrl(string userId, string token)
        {
           
            var builder = new UriBuilder();
            UrlOptions urlOptions;

            if (SpaHostUrl)
                urlOptions = urlSettings.Get("Angular-AccountConfirmationUrl");
            else
                urlOptions = urlSettings.Get("Aspnet-AccountConfirmationUrl");
            

            BuildBaseAddress(builder, urlOptions);
            builder.Query = $"userID={userId}&token={token}";
            return uri = builder.Uri;
        }

        #region Local methods

        private void BuildBaseAddress(UriBuilder uriBuilder, UrlOptions urlOptions)
        {
            if (urlOptions.IsNull())
                throw new ArgumentNullException("Url options are not valid 😡");

            uriBuilder.Scheme = urlOptions.Scheme;
            uriBuilder.Host = urlOptions.Host;
            uriBuilder.Path = urlOptions.Path;

            if (urlOptions.Port.HasValue)
                uriBuilder.Port = urlOptions.Port.Value;
        }

       

        #endregion
    }
}
