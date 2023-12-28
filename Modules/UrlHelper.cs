using System.Text.RegularExpressions;
namespace Embyte.Modules;

public static class UrlHelper
{
    public static string AddHttpsIfNotExists(string url)
    {
        if (!url.StartsWith("https://") && !url.StartsWith("http://"))
        {
            url = "https://" + url;
        }
        return url;
    }

    public static bool CorrectUrlPattern(string url)
    {
        string pattern = @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)";
        return Regex.IsMatch(url, pattern, RegexOptions.IgnoreCase);
    }

    public static string TrimDoubleHttpses(string url)
    {
        while (url.StartsWith("https://https://"))
        {
            url = url.Substring(8);
        }
        return url;
    }
}
