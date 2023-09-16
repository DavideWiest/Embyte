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
            new Link("navbar link 1", "/navbar1"),
            new Link("navbar link 2", "/navbar2"),
            new Link("navbar link 3", "#navbar3")
        };

        public string SpecialAnnouncement { get; } = "A special announcement";
        public bool SpecialAnnouncementBypass { get; } = false;
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
            ["Category One"] = new List<Link>
            {
                new Link("footer link 1", "/footer1"),
                new Link("footer link 2", "/footer2"),
                new Link("footer link 3", "#footer3")
            },
            ["Category Two"] = new List<Link>
            {
                new Link("footer link 1", "/footer1"),
                new Link("footer link 2", "/footer2"),
                new Link("footer link 3", "#footer3")
            },
            ["Category three"] = new List<Link>
            {
                new Link("footer link 1", "/footer1"),
                new Link("footer link 2", "/footer2"),
                new Link("footer link 3", "#footer3")
            },
            ["Category four"] = new List<Link>
            {
                new Link("footer link 1", "/footer1"),
                new Link("footer link 2", "/footer2"),
                new Link("footer link 3", "#footer3")
            },
            ["Category five"] = new List<Link>
            {
                new Link("footer link 1", "/footer1"),
                new Link("footer link 2", "/footer2"),
                new Link("footer link 3", "#footer3")
            }
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
