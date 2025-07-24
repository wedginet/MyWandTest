using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWandTest.Models
{
    public class ScannedTag
    {
        public Guid Id { get; set; } = Guid.NewGuid(); // Unique tag instance
        public Guid ActivityId { get; set; }           // ID for the session/activity
        public string TagNumber { get; set; } = string.Empty; // 15-digit tag
        public DateTime ScannedAt { get; set; } = DateTime.UtcNow;

        public bool IsDuplicate { get; set; } = false;
        public bool IsSaved { get; set; } = false;
        public bool IsInAnimalTable { get; set; } = false; // true = existing animal, false = new
    }
}
