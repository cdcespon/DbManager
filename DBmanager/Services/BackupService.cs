using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DBmanager.Models;

namespace DBmanager.Services
{
    public class BackupService
    {
        public async Task<List<BackupInfo>> GetBackupsAsync(string dbPath)
        {
            return await Task.Run(() =>
            {
                var dbName = Path.GetFileNameWithoutExtension(dbPath);
                var dbFolder = Path.GetDirectoryName(dbPath);
                // Backups are stored in a "Backups" subfolder relative to the DB location
                // Or simply in Data/Backups if we want a central location. 
                // Let's use Data/Backups for simplicity as per plan.
                
                // Assuming we want backups in specific folder, let's find the project root data folder.
                // But passing just dbPath makes it relative to DB. Let's make it relative to app Data folder generally.
                // For now, let's assume a "Backups" folder next to the DB file or a central one.
                // The plan said "Backups/" folder. Let's assume it's a sibling of the DB file for now if it's in Data.
                
                var backupFolder = Path.Combine(Path.GetDirectoryName(dbPath)!, "Backups");
                if (!Directory.Exists(backupFolder))
                {
                    return new List<BackupInfo>();
                }

                var files = Directory.GetFiles(backupFolder, "*.db"); // Assuming backups are .db files
                var backups = new List<BackupInfo>();

                foreach (var file in files)
                {
                    var fileInfo = new FileInfo(file);
                    // Filter backups relevant to this DB? Or just show all?
                    // Typically backups include the original DB name.
                    if (fileInfo.Name.StartsWith(dbName))
                    {
                        backups.Add(new BackupInfo
                        {
                            FileName = fileInfo.Name,
                            FullPath = fileInfo.FullName,
                            CreatedAt = fileInfo.CreationTime,
                            SizeBytes = fileInfo.Length
                        });
                    }
                }

                return backups.OrderByDescending(b => b.CreatedAt).ToList();
            });
        }

        public async Task CreateBackupAsync(string dbPath)
        {
            await Task.Run(() =>
            {
                var dbName = Path.GetFileNameWithoutExtension(dbPath);
                var extension = Path.GetExtension(dbPath);
                var backupFolder = Path.Combine(Path.GetDirectoryName(dbPath)!, "Backups");
                
                if (!Directory.Exists(backupFolder))
                {
                    Directory.CreateDirectory(backupFolder);
                }

                var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
                var backupFileName = $"{dbName}_{timestamp}{extension}";
                var backupPath = Path.Combine(backupFolder, backupFileName);

                File.Copy(dbPath, backupPath);
            });
        }

        public async Task RestoreBackupAsync(string backupPath, string targetPath)
        {
            await Task.Run(() =>
            {
                // Ensure connections are closed? In SQLite, just copying over should work if no active write transaction.
                // ideally close connection in AppState before this.
                File.Copy(backupPath, targetPath, overwrite: true);
            });
        }

        public async Task DeleteBackupAsync(string backupPath)
        {
            await Task.Run(() =>
            {
                if (File.Exists(backupPath))
                {
                    File.Delete(backupPath);
                }
            });
        }

        public async Task RenameBackupAsync(string currentPath, string newName)
        {
            await Task.Run(() =>
            {
                if (File.Exists(currentPath))
                {
                    var directory = Path.GetDirectoryName(currentPath);
                    // Ensure new name has extension if missing?
                    if (!newName.EndsWith(".db"))
                    {
                        newName += ".db"; // Or keep original extension
                    }
                    var newPath = Path.Combine(directory!, newName);
                    File.Move(currentPath, newPath);
                }
            });
        }
    }
}
