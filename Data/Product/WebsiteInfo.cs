using Humanizer;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Security.Policy;
using System.Text;


namespace Embyte.Data.Product;

public class WebsiteInfo
{
    public bool HasData { get; set; } = false;
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = "Unknown";
    public string SiteName { get; set; } = string.Empty;
    public string SiteType { get; set; } = string.Empty;
    public string Locale { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string[] Keywords { get; set; } = {};
    public string ImageUrl { get; set; } = string.Empty;
    public string FavIconUrl { get; set; } = string.Empty;
    public string ThemeColor { get; set; } = "#295c8f";
    public string PersonName { get; set; } = string.Empty;


    public WebsiteInfo(string url)
    {
        Url = url;
        HasData = false;
    }

    public void validateData()
    {
        string host = new Uri(Url).Host;
        string httpType = Url.StartsWith("https") ? "https://" : "http://";
        if (Title.IsNullOrEmpty())
            Title = host;
        if (SiteName.IsNullOrEmpty())
            SiteName = host;
        if (ThemeColor.IsNullOrEmpty())
            ThemeColor = "#295c8f";
        if (ImageUrl.StartsWith("/"))
            ImageUrl = httpType + host + ImageUrl;
        if (FavIconUrl.StartsWith("/"))
            FavIconUrl = httpType + host + FavIconUrl;
    }
}

public class WebsiteInfoStatus
{
    public WebsiteInfoStatusType statusType { get; set; } = WebsiteInfoStatusType.success;
    public HttpStatusCode responseStatusCode { get; set; } = HttpStatusCode.OK;
    public WebExceptionStatus webStatus { get; set; } = WebExceptionStatus.Success;
    public string message { get; set; } = string.Empty;
    public Exception? exception { get; set; } = null;

    public int requestDurationMS;
    public int parsingDurationMS;

    public WebsiteInfoStatus()
    {

    }

    public WebsiteInfoStatus(WebsiteInfoStatusType errorType, HttpStatusCode websiteResponseCode, string message, Exception exception)
    {
        this.statusType = errorType;
        this.responseStatusCode = websiteResponseCode;
        this.message = message;
        this.exception = exception;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.AppendLine($"Status Type: {statusType}");
        sb.AppendLine($"Response Status Code: {responseStatusCode}");
        sb.AppendLine($"Web Status: {webStatus}");
        sb.AppendLine($"Message: {message}");

        if (exception != null)
        {
            sb.AppendLine($"Exception: {exception.Message}");
        }

        sb.AppendLine($"Request Duration (ms): {requestDurationMS}");
        sb.AppendLine($"Parsing Duration (ms): {parsingDurationMS}");

        return sb.ToString();
    }
}

public enum WebsiteInfoStatusType
{
    success,
    internalError,
    noResponse,
    requestFailed,
    noTagsFound
}