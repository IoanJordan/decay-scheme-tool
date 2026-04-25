using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DecaySchemeTool
{
    public class CascadeGroup
    {
        public int GroupId { get; set; }
        public List<double> Members { get; set; } = new List<double>();
        public int GroupSize => Members.Count;
        public int GroupStrength { get; set; }

        public string MembersText =>
            string.Join(", ", Members
                .OrderBy(e => e)
                .Select(e => e.ToString("F3", CultureInfo.InvariantCulture)));
    }
}