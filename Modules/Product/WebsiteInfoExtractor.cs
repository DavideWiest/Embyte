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

            TimeSpan requestDuration = startRequestTime - DateTime.Now;
            status.requestDurationMS = (int)Math.Round(requestDuration.TotalMilliseconds, MidpointRounding.AwayFromZero);

            if (status.responseStatusCode == HttpStatusCode.OK)
            {
                DateTime startParsingTime = DateTime.Now;

                (metaInfo, status) = PareMetaData(metaInfo, status, document);

                TimeSpan parsingDuration = startRequestTime - DateTime.Now;
                status.parsingDurationMS = (int)Math.Round(parsingDuration.TotalMilliseconds, MidpointRounding.AwayFromZero);
            }
            else
            {
                status.errorType = WebsiteInfoError.requestFailed;
                status.message = "Request failed with status code: " + webGet.StatusCode;
            }
        }
        catch (WebException ex)
        {
            status.webStatus = ex.Status;
            status.errorType = WebsiteInfoError.internalError; 
            status.message = ex.Message;
            status.exception = ex;
        }
        catch (Exception ex)
        {
            status.errorType = WebsiteInfoError.internalError;
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
            metaInfo.Description = findMetatagContent("description", null, metaTags, ref count) ?? findMetatagContent(null, "og:description", metaTags, ref count) ?? findMetatagContent("twitter:description", null, metaTags, ref count) ?? string.Empty;
            metaInfo.Keywords = findMetatagContent("keywords", null, metaTags, ref count) ?? findMetatagContent("news_keywords", null, metaTags, ref count) ?? string.Empty;
            metaInfo.FavIconUrl = getFavicon(document) ?? string.Empty;
            metaInfo.ImageUrl = findMetatagContent("image", null, metaTags, ref count) ?? findMetatagContent(null, "og:image", metaTags, ref count) ?? findMetatagContent("twitter:image", null, metaTags, ref count) ?? string.Empty;

            metaInfo.HasData = count > 0;

            if (!metaInfo.HasData)
            {
                status.errorType = WebsiteInfoError.noTagsFound;
                status.message = "No tags were found in the website's response.";
            }
        }
        return Tuple.Create(metaInfo, status);
    }

    private static string? findMetatagContent(string? wantedElementName, string? wantedElementProperty, HtmlNodeCollection metaTags, ref int count, bool contentCanBeNull = false)
    {
        foreach (var element in metaTags)
        {
            if (wantedElementName != null && element.Attributes["name"].Value.IsNullOrEmpty() ||
                wantedElementProperty != null && element.Attributes["property"].Value.IsNullOrEmpty() ||
                !contentCanBeNull && element.Attributes["content"].Value.IsNullOrEmpty()
                )
            {
                continue;
            }

            if ((wantedElementName == null || element.Attributes["name"].Value.ToLower() == wantedElementName) &&
                (wantedElementProperty == null || element.Attributes["property"].Value.ToLower() == wantedElementProperty))
            {
                count++;
                return element.Attributes["content"].Value;
            }
        }
        return null;
    }

    private static string? getFavicon(HtmlDocument document)
    {
        var faviconNode = document.DocumentNode.SelectSingleNode("//link[@rel='icon' or @rel='shortcut icon']/@href");
        if (faviconNode != null)
        {
            return faviconNode.GetAttributeValue("href", null);
        }
        return null;
    }
}