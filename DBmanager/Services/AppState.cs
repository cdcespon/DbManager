using System;

namespace DBmanager.Services
{
    public enum ViewMode
    {
        None,
        Dashboard,
        TableDetails,
        QueryEditor,
        DiagramDesigner
    }

    public class AppState
    {
        public string CurrentConnectionString { get; private set; }
        public string CurrentDatabaseName { get; private set; }
        public string CurrentDatabasePath { get; private set; }
        public string CurrentTableName { get; private set; }
        public string CurrentObjectType { get; private set; }
        public ViewMode CurrentViewMode { get; private set; } = ViewMode.None;

        public event Action? OnChange;

        public void SetDatabase(string path)
        {
            CurrentConnectionString = $"Data Source={path}";
            CurrentDatabaseName = System.IO.Path.GetFileName(path);
            CurrentDatabasePath = path;
            CurrentTableName = null;
            CurrentObjectType = null;
            CurrentViewMode = ViewMode.Dashboard;
            NotifyStateChanged();
        }

        public void SetTable(string tableName, string type)
        {
            CurrentTableName = tableName;
            CurrentObjectType = type;
            CurrentViewMode = ViewMode.TableDetails;
            NotifyStateChanged();
        }

        public void SetViewMode(ViewMode mode)
        {
            CurrentViewMode = mode;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
