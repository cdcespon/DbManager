using System.Collections.Generic;

namespace DBmanager.Models
{
    public class SessionData
    {
        public List<string> OpenDatabases { get; set; } = new List<string>();
        public Dictionary<string, DiagramLayout> DiagramLayouts { get; set; } = new Dictionary<string, DiagramLayout>();
    }

    public class DiagramLayout
    {
        public List<NodePosition> Nodes { get; set; } = new List<NodePosition>();
        // Links are auto-generated based on FKs, so we might not need to save them unless we support custom links.
        // For now, let's just save nodes.
    }

    public class NodePosition
    {
        public string TableName { get; set; } = "";
        public double X { get; set; }
        public double Y { get; set; }
    }
}
