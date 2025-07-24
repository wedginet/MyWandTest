using MyWandTest.Models;
using System.Collections.Generic;

namespace MyWandTest.Services
{
    public class SessionService
    {
    public Guid CurrentActivityId { get; private set; } = Guid.NewGuid();

        public List<ScannedTag> ScannedTagsLog { get; set; } = new();

        public void AddScannedTag(string parsedTag, bool isDuplicate, bool isInAnimalTable)
        {
            var scannedTag = new ScannedTag
            {
                ActivityId = CurrentActivityId,
                TagNumber = parsedTag,
                IsDuplicate = isDuplicate,
                IsInAnimalTable = isInAnimalTable
            };

            ScannedTagsLog.Add(scannedTag);
        }

        // Optionally a method to mark a tag as saved:
        public void MarkTagAsSaved(Guid tagId)
        {
            var tag = ScannedTagsLog.FirstOrDefault(t => t.Id == tagId);
            if (tag != null)
            {
                tag.IsSaved = true;
            }
        }
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