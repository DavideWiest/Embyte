using System.Net;

namespace Embyte.Data.Product;

public class WebsiteInfo
{
    public bool HasData { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string SiteName { get; set; } = string.Empty;
    public string SiteType { get; set; } = string.Empty;
    public string Locale { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Keywords { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string FavIconUrl { get; set; } = string.Empty;


    public WebsiteInfo(string url)
    {
        Url = url;
        HasData = false;
    }

    public WebsiteInfo(string url, string title, string siteType, string locale, string description, string keywords, string imageUrl, string favIconUrl, string siteName)
    {
        Url = url;
        Title = title;
        SiteType = siteType;
        Locale = locale;
        Description = description;
        Keywords = keywords;
        ImageUrl = imageUrl;
        FavIconUrl = favIconUrl;
        SiteName = siteName;
    }
}

public class WebsiteInfoStatus
{
    public WebsiteInfoError errorType { get; set; } = WebsiteInfoError.success;
    public HttpStatusCode responseStatusCode { get; set; } = HttpStatusCode.OK;
    public WebExceptionStatus webStatus { get; set; } = WebExceptionStatus.Success;
    public string message { get; set; } = string.Empty;
    public Exception? exception { get; set; } = null;

    public int requestDurationMS;
    public int parsingDurationMS;

    public WebsiteInfoStatus()
    {

    }

    public WebsiteInfoStatus(WebsiteInfoError errorType, HttpStatusCode websiteResponseCode, string message, Exception exception)
    {
        this.errorType = errorType;
        this.responseStatusCode = websiteResponseCode;
        this.message = message;
        this.exception = exception;
    }
}

public enum WebsiteInfoError
{
    success,
    internalError,
    noResponse,
    requestFailed,
    noTagsFound
}