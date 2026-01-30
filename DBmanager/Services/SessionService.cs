using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using DBmanager.Models;
using Microsoft.AspNetCore.Hosting;

namespace DBmanager.Services
{
    public class SessionService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _sessionFilePath;
        public SessionData CurrentSession { get; private set; } = new SessionData();

        public event Action? OnSessionChanged;

        public SessionService(IWebHostEnvironment env)
        {
            _env = env;
            _sessionFilePath = Path.Combine(_env.ContentRootPath, "session.json");
            LoadSession();
        }

        public void LoadSession()
        {
            try
            {
                if (File.Exists(_sessionFilePath))
                {
                    var json = File.ReadAllText(_sessionFilePath);
                    CurrentSession = JsonSerializer.Deserialize<SessionData>(json) ?? new SessionData();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading session: {ex.Message}");
                CurrentSession = new SessionData();
            }
        }

        public void SaveSession()
        {
            try
            {
                var json = JsonSerializer.Serialize(CurrentSession, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(_sessionFilePath, json);
                OnSessionChanged?.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving session: {ex.Message}");
            }
        }

        public void AddDatabase(string path)
        {
            if (!CurrentSession.OpenDatabases.Contains(path))
            {
                CurrentSession.OpenDatabases.Add(path);
                SaveSession();
            }
        }

        public void RemoveDatabase(string path)
        {
            if (CurrentSession.OpenDatabases.Contains(path))
            {
                CurrentSession.OpenDatabases.Remove(path);
                if (CurrentSession.DiagramLayouts.ContainsKey(path))
                {
                    CurrentSession.DiagramLayouts.Remove(path);
                }
                SaveSession();
            }
        }

        public void UpdateDiagramLayout(string dbPath, DiagramLayout layout)
        {
            CurrentSession.DiagramLayouts[dbPath] = layout;
            SaveSession();
        }

        public DiagramLayout? GetDiagramLayout(string dbPath)
        {
            if (CurrentSession.DiagramLayouts.TryGetValue(dbPath, out var layout))
            {
                return layout;
            }
            return null;
        }
    }
}
