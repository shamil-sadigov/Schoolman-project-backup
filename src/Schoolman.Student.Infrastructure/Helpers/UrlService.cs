using Microsoft.AspNetCore.Http;
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

        public UrlService(IOptionsMonitor<UrlOptions> urlSettings)
        {
            this.urlSettings = urlSettings;
        }

        public UrlService UseSpaHost()
        {
            SpaHostUrl = true;
            return this;
        }

        public UrlService UseWebapiHost()
        {
            SpaHostUrl = false;
            return this;
        }



        /// <summary>
        /// By default, build url for Aspnet hosting url
        /// </summary>
        /// <param name="webApiPath"></param>
        /// <returns></returns>
        public string BuildConfirmationUrlWithQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentNullException("Query must not be empty or null");

            var builder = new UriBuilder();

            if (SpaHostUrl)
            {
                UrlOptions spaUrlOptions = urlSettings.Get("Angular-AccountConfirmationUrl");
                Build(builder, spaUrlOptions);
            }
            else
            {
                UrlOptions spaUrlOptions = urlSettings.Get("Aspnet-AccountConfirmationUrl");
                Build(builder, spaUrlOptions);
            }

            builder.Query = query;

            return builder.ToString();
        }


        private void Build(UriBuilder uriBuilder, UrlOptions urlOptions)
        {
            if (urlOptions.IsNull())
                throw new ArgumentNullException("Url options are not valid 😡");

            uriBuilder.Scheme = urlOptions.Scheme;
            uriBuilder.Host = urlOptions.Host;
            uriBuilder.Path = urlOptions.Path;

            if (urlOptions.Port.HasValue)
                uriBuilder.Port = urlOptions.Port.Value;

        }

    }
}
