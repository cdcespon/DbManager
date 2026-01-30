using System.Collections.Generic;

namespace DBmanager.Models
{
    public class DbNode
    {
        public string Name { get; set; } = "";
        public string Icon { get; set; } = "";
        public string Type { get; set; } = ""; // "Database", "Category", "Table", "View"
        public string Payload { get; set; } = ""; // Path for DB, TableName for Table/View
        public List<DbNode> Children { get; set; } = new List<DbNode>();
        public DbNode? Parent { get; set; }
        public bool IsExpanded { get; set; } // Track expansion state
    }
}
