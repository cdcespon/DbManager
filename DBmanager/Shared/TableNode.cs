using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;
using DBmanager.Models;
using System.Collections.Generic;

namespace DBmanager.Shared
{
    public class TableNode : NodeModel
    {
        public TableNode(Point position) : base(position) 
        {
        }

        public List<ColumnInfo> Columns { get; set; } = new List<ColumnInfo>();
    }
}
