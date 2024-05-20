// Copyright (c) Laserfiche.
// Licensed under the MIT License. See LICENSE in the project root for license information.
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Laserfiche.Api.Client.Utils;

namespace Laserfiche.Api.Client.HttpHandlers
{
    /// <summary>
    /// HttpClient Message Handler to interact with Laserfiche APIs. It provides the following features:
    /// - Uses the IHttpRequestHandler to obtain the value of the Authorization header and refresh it as needed.
    /// - Sets the Api Base Address based on the Laserfiche cloud region Access Token.
    /// - Built-in retries on 401 and transient errors.
    /// </summary>
    public class ApiHttpMessageHandler : DelegatingHandler
    {
        public delegate string GetApiBaseUri(string laserficheCloudRegionalDomain);
        private readonly IHttpRequestHandler _httpRequestHandler;
        private readonly GetApiBaseUri _getApiBaseUri;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="httpRequestHandler"></param>
        /// <param name="getApiBaseUri">Typical argument values: <see cref="DomainUtils.GetRepositoryApiBaseUri"/>, <see cref="DomainUtils.GetODataApiBaseUri"/>, or a function returning a custom API base address.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ApiHttpMessageHandler(IHttpRequestHandler httpRequestHandler, GetApiBaseUri getApiBaseUri) :
          base(new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.GZip })
        {
            _httpRequestHandler = httpRequestHandler ?? throw new ArgumentNullException(nameof(httpRequestHandler));
            _getApiBaseUri = getApiBaseUri ?? throw new ArgumentNullException(nameof(getApiBaseUri));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return await SendExAsync(request, cancellationToken, true).ConfigureAwait(false)
                ?? await SendExAsync(request, cancellationToken, false).ConfigureAwait(false);
        }

        private async Task<HttpResponseMessage> SendExAsync(HttpRequestMessage request, CancellationToken cancellationToken, bool returnNullIfRetriable)
        {
            HttpResponseMessage response;

            // Sets the authorization header
            var beforeSendResult = await _httpRequestHandler.BeforeSendAsync(request, cancellationToken).ConfigureAwait(false);
            Uri apiBaseUri = new Uri(_getApiBaseUri(beforeSendResult.RegionalDomain));
            request.RequestUri = new Uri(apiBaseUri, request.RequestUri.PathAndQuery);

            try
            {
                response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                if (returnNullIfRetriable && IsTransientHttpStatusCode(response.StatusCode) && IsIdempotentHttpMethod(request.Method))
                {
                    return null;
                }
            }
            catch
            {
                if (returnNullIfRetriable)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            bool shouldRetry = await _httpRequestHandler.AfterSendAsync(response, cancellationToken).ConfigureAwait(false);
            if (returnNullIfRetriable && shouldRetry)
            {
                return null;
            }
            return response;
        }
        private static bool IsTransientHttpStatusCode(HttpStatusCode statusCode)
        {
            return (int)statusCode >= 500 || statusCode == HttpStatusCode.RequestTimeout;
        }

        private static bool IsIdempotentHttpMethod(HttpMethod method)
        {
            return method == HttpMethod.Get || method == HttpMethod.Put || method == HttpMethod.Options;
        }
    }
}
