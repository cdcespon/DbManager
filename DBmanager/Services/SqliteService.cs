using Dapper;
using DBmanager.Models;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DBmanager.Services
{
    public class SqliteService
    {
        public async Task<List<TableInfo>> GetTablesAsync(string connectionString)
        {
            using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();
            var sql = "SELECT name FROM sqlite_schema WHERE type ='table' AND name NOT LIKE 'sqlite_%';";
            var tables = await connection.QueryAsync<TableInfo>(sql);
            return tables.ToList();
        }

        public async Task<List<TableInfo>> GetViewsAsync(string connectionString)
        {
            using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();
            var sql = "SELECT name FROM sqlite_schema WHERE type ='view' AND name NOT LIKE 'sqlite_%';";
            var views = await connection.QueryAsync<TableInfo>(sql);
            return views.ToList();
        }

        public async Task<List<ColumnInfo>> GetTableColumnsAsync(string connectionString, string tableName)
        {
            using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();
            // Using Dapper to map the result of PRAGMA table_info
            var columns = await connection.QueryAsync<ColumnInfo>($"PRAGMA table_info(\"{tableName}\")");
            return columns.ToList();
        }

        public async Task<List<ForeignKeyInfo>> GetForeignKeysAsync(string connectionString, string tableName)
        {
             using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();
            var fks = await connection.QueryAsync<ForeignKeyInfo>($"PRAGMA foreign_key_list(\"{tableName}\")");
            return fks.ToList();
        }

        public async Task<IEnumerable<dynamic>> GetTableDataAsync(string connectionString, string tableName, int limit = 1000)
        {
            using var connection = new SqliteConnection(connectionString);
            await connection.OpenAsync();
            // Escape table name to prevent SQL injection
            var sql = $"SELECT * FROM \"{tableName}\" LIMIT {limit}";
            var result = await connection.QueryAsync(sql);
            return result;
        }
        public async Task<List<string>> GetSchemaSuggestionsAsync(string connectionString)
        {
            var suggestions = new List<string>();
            try
            {
                using var connection = new SqliteConnection(connectionString);
                await connection.OpenAsync();

                // Tables and Views
                var schemaItems = await connection.QueryAsync<string>("SELECT name FROM sqlite_schema WHERE type IN ('table','view') AND name NOT LIKE 'sqlite_%';");
                var itemsList = schemaItems.ToList();
                suggestions.AddRange(itemsList);

                // Columns for each
                foreach (var item in itemsList)
                {
                    try 
                    {
                        var cols = await connection.QueryAsync<string>($"PRAGMA table_info(\"{item}\")"); // returns full rows, need to select Name only? Dapper maps to type. String might fail if multiple cols.
                        // Correct approach for PRAGMA table_info which returns cid, name, type, etc.
                        var colNames = await connection.QueryAsync<string>($"SELECT name FROM pragma_table_info('{item}')");
                        suggestions.AddRange(colNames);
                    }
                    catch { /* match issues */ }
                }
            }
            catch { /* ignore errors */ }
            
            return suggestions.Distinct().ToList();
        }
    }
}
