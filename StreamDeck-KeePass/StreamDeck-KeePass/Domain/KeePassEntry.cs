using KeePassLib;

namespace streamdeck_keepass.Domain
{
    internal class KeePassEntry
    {
        public string Group { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string URL { get; set; }
        public string Notes { get; set; }

        public KeePassEntry(PwEntry entry)
        {
            Group = entry.ParentGroup.Name;
            Title = entry.Strings.ReadSafe("Title");
            Username = entry.Strings.ReadSafe("UserName");
            Password = entry.Strings.ReadSafe("Password");
            URL = entry.Strings.ReadSafe("URL");
            Notes = entry.Strings.ReadSafe("Notes");
        }
    }
}
