namespace Embyte.Modules.Product;
using HtmlAgilityPack;
using Embyte.Data.Product;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http;
using MudBlazor;
using static System.Net.WebRequestMethods;
using System.Net;
using System;
using Embyte.Modules.Logging;
using Azure.Core;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using static MudBlazor.CategoryTypes;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Numerics;
using System.Net.Http;

public class WebsiteInfoExtractor
{
    /// <summary>
    /// Uses HtmlAgilityPack to get the meta information from a url
    /// </summary>
    /// <param name="url"></param>
    /// <returns>A tuple consisting of the websites info and a status object</returns>
    public static Tuple<WebsiteInfo, WebsiteInfoStatus> GetMetaDataFromUrl(string url)
    {
        // Get the URL specified
        var webGet = new HtmlWeb();
        WebsiteInfo metaInfo = new(url);
        WebsiteInfoStatus status = new();

        try
        {
            DateTime startRequestTime = DateTime.Now;

            webGet.Timeout = 5000;
            var document = webGet.Load(url);
            status.responseStatusCode = webGet.StatusCode;

            TimeSpan requestDuration = DateTime.Now - startRequestTime;
            status.requestDurationMS = (int)Math.Round(requestDuration.TotalMilliseconds, MidpointRounding.AwayFromZero);

            if (status.responseStatusCode == HttpStatusCode.OK)
            {
                DateTime startParsingTime = DateTime.Now;

                (metaInfo, status) = PareMetaData(metaInfo, status, document);

                TimeSpan parsingDuration = DateTime.Now - startParsingTime;
                status.parsingDurationMS = (int)Math.Round(parsingDuration.TotalMilliseconds, MidpointRounding.AwayFromZero);
            }
            else
            {
                Log.Debug("Request to {url} failed with status code {statuscode}", url, webGet.StatusCode);
                status.statusType = WebsiteInfoStatusType.requestFailed;
                status.message = "Request failed with status code: " + webGet.StatusCode;
            }
        }
        catch (WebException ex)
        {
            Log.Error("WebException occured when requesting url {url}, status : {errmsg} \n Traceback: \n {traceback}", url, ex.Status, ex.Message, ex.ToString());
            status.webStatus = ex.Status;
            status.statusType = WebsiteInfoStatusType.requestFailed; 
            status.message = ex.Message;
            status.exception = ex;
        }
        catch (Exception ex)
        {
            Log.Error("Exception occured when requesting url {url}: {errmsg} \n Traceback: \n {traceback}", url, ex.Message, ex.ToString());
            status.statusType = WebsiteInfoStatusType.internalError;
            status.message = ex.Message;
            status.exception = ex;
        }
        return Tuple.Create(metaInfo, status);
    }

    public static Tuple<WebsiteInfo, WebsiteInfoStatus> PareMetaData(WebsiteInfo metaInfo, WebsiteInfoStatus status, HtmlDocument document)
    {
        var metaTags = document.DocumentNode.SelectNodes("//meta");
        if (metaTags != null)
        {
            int count = 0;
            metaInfo.Title = findMetatagContent("title", null, metaTags, ref count) ?? findMetatagContent(null, "og:title", metaTags, ref count) ?? findMetatagContent("twitter:title", null, metaTags, ref count) ?? string.Empty;
            metaInfo.SiteName = findMetatagContent("pagename", null, metaTags, ref count) ?? findMetatagContent(null, "og:site_name", metaTags, ref count) ?? string.Empty;
            metaInfo.SiteType = findMetatagContent("type", null, metaTags, ref count) ?? findMetatagContent(null, "og:type", metaTags, ref count) ?? string.Empty;
            metaInfo.Locale = findMetatagContent("locale", null, metaTags, ref count) ?? findMetatagContent(null, "og:locale", metaTags, ref count) ?? string.Empty;
            metaInfo.PersonName = findMetatagContent("author", null, metaTags, ref count) ?? findMetatagContent(null, "og:article:author", metaTags, ref count) ?? findMetatagContent(null, "profile:username", metaTags, ref count) ?? string.Empty;
            metaInfo.Description = findMetatagContent("description", null, metaTags, ref count) ?? findMetatagContent(null, "og:description", metaTags, ref count) ?? findMetatagContent("twitter:description", null, metaTags, ref count) ?? string.Empty;
            metaInfo.Keywords = (findMetatagContent("keywords", null, metaTags, ref count) ?? findMetatagContent("news_keywords", null, metaTags, ref count) ?? string.Empty).Split(",").Select(item => item.Trim()).Where(item => !string.IsNullOrEmpty(item)).ToArray();
            metaInfo.FavIconUrl = getFavicon(document) ?? string.Empty;
            metaInfo.ImageUrl = findMetatagContent("image", null, metaTags, ref count) ?? findMetatagContent(null, "og:image", metaTags, ref count) ?? findMetatagContent("twitter:image", null, metaTags, ref count) ?? string.Empty;
            metaInfo.ThemeColor = findMetatagContent("theme-color", null, metaTags, ref count) ?? string.Empty;

            metaInfo.HasData = count > 0;

            if (!metaInfo.HasData)
            {
                status.statusType = WebsiteInfoStatusType.noTagsFound;
                status.message = "No tags were found in the website's response.";
            }
        }
        return Tuple.Create(metaInfo, status);
    }

    private static string? findMetatagContent(string? wantedElementName, string? wantedElementProperty, HtmlNodeCollection metaTags, ref int count, bool contentCanBeNull = false)
    {
        foreach (var element in metaTags)
        {
            if (wantedElementName != null && NullableValueIsNull(element.Attributes["name"]) ||
                wantedElementProperty != null && NullableValueIsNull(element.Attributes["property"]) ||
                !contentCanBeNull && NullableValueIsNull(element.Attributes["content"])
                )
            {
                continue;
            }

            if ((wantedElementName == null || element.Attributes["name"].Value.ToLower() == wantedElementName) &&
                (wantedElementProperty == null || element.Attributes["property"].Value.ToLower() == wantedElementProperty))
            {
                count++;
                return WebUtility.HtmlDecode(element.Attributes["content"].Value);
            }
        }
        return null;
    }

    private static bool NullableValueIsNull(HtmlAttribute nullableVar)
    {
        if (nullableVar == null)
            return true;
        return nullableVar.Value.IsNullOrEmpty();
    }

    private static string? getFavicon(HtmlDocument document)
    {
        var faviconNode = document.DocumentNode.SelectSingleNode("//link[@rel='icon' or @rel='shortcut icon']/@href");
        if (faviconNode != null)
        {
            return WebUtility.HtmlDecode(faviconNode.GetAttributeValue("href", null));
        }
        return null;
    }

    public static bool IsImage(string imageUrl)
    {
        try
        {
            using (HttpClient httpClient = new HttpClient())
            {
                // Send a GET request to the image URL
                var task = Task.Run(() => httpClient.GetAsync(imageUrl));
                task.Wait();
                HttpResponseMessage response = task.Result;

                // Check if the response status code indicates success
                return response.StatusCode == HttpStatusCode.OK;
            }
        }
        catch (Exception)
        {
            // An exception occurred, so it's not an image
            return false;
        }
    }

}