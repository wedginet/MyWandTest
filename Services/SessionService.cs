using System.Collections.Generic;

namespace MyWandTest.Services
{
    public class SessionService
    {
        public List<string> RawScans { get; private set; } = new();
        public List<string> ParsedScans { get; private set; } = new();

        public void AddRaw(string value)
        {
            if (!string.IsNullOrWhiteSpace(value))
                RawScans.Add(value);
        }

        public void AddParsed(string tag)
        {
            if (!string.IsNullOrWhiteSpace(tag) && !ParsedScans.Contains(tag))
                ParsedScans.Add(tag);
        }

        public void Clear()
        {
            RawScans.Clear();
            ParsedScans.Clear();
        }
    }
}
