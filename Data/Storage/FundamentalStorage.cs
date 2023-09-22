namespace Embyte.Data.Storage;

public class FundamentalStorage
{
    public class Navbar
    {
        public class Link
        {
            public string Name { get; }
            public string Url { get; }

            public Link(string name, string url)
            {
                Name = name;
                Url = url;
            }
        }

        public IReadOnlyList<Link> Links { get; } = new List<Link>
        {
            new Link("Repository", "https://github.com/DavideWiest/Embyte"),
            new Link("Documentation", "https://github.com/DavideWiest/Embyte/wiki"),
            new Link("Creator", "https://github.com/DavideWiest"),
            new Link("Contact", "mailto:davide.wiest2@gmail.com")
        };

        public string SpecialAnnouncement { get; } = "Sepember 2023 - Embyte launched!";
        public bool ShowSpecialAnnouncement { get; } = false;
    }

    public class Footer
    {
        public class Link
        {
            public string Name { get; }
            public string Url { get; }

            public Link(string name, string url)
            {
                Name = name;
                Url = url;
            }
        }

        public IReadOnlyDictionary<string, IReadOnlyList<Link>> Links { get; } = new Dictionary<string, IReadOnlyList<Link>>
        {
            ["Footerlinks"] = new List<Link>
            {
                new Link("Github Repository", "https://github.com/DavideWiest/Embyte"),
                new Link("Creator", "https://github.com/DavideWiest"),
                new Link("Contact", "mailto:davide.wiest2@gmail.com")
            },
        };
    }

    public class Subfooter
    {
        public class Link
        {
            public string Name { get; }
            public string Url { get; }

            public Link(string name, string url)
            {
                Name = name;
                Url = url;
            }
        }

        public IReadOnlyList<Link> Links { get; } = new List<Link>
        {
            new Link("Terms of Service", "/terms-of-service"),
            new Link("Privacy Policy", "/impressum-privacy-policy#privacy-policy"),
            new Link("Impressum", "/impressum-privacy-policy#impressum")
        };
    }

    public class SiteBase
    {
        public string ProjectName { get; } = "Embyte";
    }

    public Navbar NavbarData { get; } = new Navbar();
    public Footer FooterData { get; } = new Footer();
    public Subfooter SubfooterData { get; } = new Subfooter();
    public SiteBase SiteBaseData { get; } = new SiteBase();
}
