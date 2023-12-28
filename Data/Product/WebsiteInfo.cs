using Humanizer;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Policy;
using System.Text;


namespace Embyte.Data.Product;

public class WebsiteInfo
{
    public bool HasData { get; set; } = false;

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    public DateTime Time;

    public WebsiteInfo()
    {
        HasData = false;
        Time = DateTime.Now;
    }

    public WebsiteInfo(string url)
    {
        Url = url;
        HasData = false;
        Time = DateTime.Now;
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

    public bool DataEqual(WebsiteInfo? other)
    {
        if (other == null)
            return false;

        return Url == other.Url &&
               Title == other.Title &&
               SiteName == other.SiteName &&
               SiteType == other.SiteType &&
               Locale == other.Locale &&
               Description == other.Description &&
               Keywords.SequenceEqual(other.Keywords ?? Array.Empty<string>()) &&
               ImageUrl == other.ImageUrl &&
               FavIconUrl == other.FavIconUrl &&
               ThemeColor == other.ThemeColor &&
               PersonName == other.PersonName;
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