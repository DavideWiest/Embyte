namespace Embyte.Data.Product;

public class WebsiteInfo
{
    public bool HasData { get; set; }
    public string Url { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Keywords { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string SiteName { get; set; } = string.Empty;

    public WebsiteInfo(string url, bool hasData)
    {
        Url = url;
        HasData = hasData;
    }

    public WebsiteInfo(string url, string title, string description, string keywords, string imageUrl, string siteName)
    {
        Url = url;
        Title = title;
        Description = description;
        Keywords = keywords;
        ImageUrl = imageUrl;
        SiteName = siteName;
    }
}

public class WebsiteInfoStatus
{
    public WebsiteInfoError errorType { get; set; }
    public int websiteResponseCode { get; set; }
    public string message { get; set; } = string.Empty;

    public WebsiteInfoStatus(WebsiteInfoError errorType, int websiteResponseCode, string message)
    {
        this.errorType = errorType;
        this.websiteResponseCode = websiteResponseCode;
        this.message = message;
    }
}

public enum WebsiteInfoError
{
    noResponse,
    couldNotLoad
}