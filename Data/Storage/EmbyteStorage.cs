namespace Embyte.Data.Storage;

public static class EmbyteStorage
{
    public static string domainName = "https://embyte.davidewiest.com";
    public static string absoluteEmbedEndpoint = domainName + "/embed/";
    public static string absoluteEmbedViewEndpoint = domainName + "/embed-view/";
    public static string relativeEmbedEndpoint = "/embed/";
    public static string documentationUrl = "https://github.com/DavideWiest/embyte/wiki";
    public static string DefaultUrl { get; } = "https://www.astronomy.com/picture-of-the-day/";
    public static List<string> DefaultUrlList { get; } = new List<string> { "https://www.bbc.com/news" };
    public static string DefaultUrl2 { get; } = "https://github.com/DavideWiest";
}
