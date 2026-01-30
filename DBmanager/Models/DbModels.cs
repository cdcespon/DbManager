namespace DBmanager.Models
{
    public class TableInfo
    {
        public string Name { get; set; }
    }

    public class ColumnInfo
    {
        public int Cid { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int NotNull { get; set; }
        public string Dflt_Value { get; set; }
        public int Pk { get; set; }
    }

    public class ForeignKeyInfo
    {
        public int Id { get; set; }
        public int Seq { get; set; }
        public string Table { get; set; } = "";
        public string From { get; set; } = "";
        public string To { get; set; } = "";
        public string On_Update { get; set; } = "";
        public string On_Delete { get; set; } = "";
        public string Match { get; set; } = "";
    }

    public class BackupInfo
    {
        public string FileName { get; set; } = "";
        public string FullPath { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public long SizeBytes { get; set; }
    }
}
