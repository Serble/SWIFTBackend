using GeneralPurposeLib;
using MySql.Data.MySqlClient;

namespace SwiftBackend.Storage.MySQL; 

public partial class MySqlStorage : IStorageManager {
    
    private string _connectString = "";
    private string _tablePrefix;

    public void Init() {
        Logger.Info("Initialising MySQL...");
        _connectString = $"server={GlobalConfig.Config["mysql_host"]};" +
                         $"userid={GlobalConfig.Config["mysql_user"]};" +
                         $"password={GlobalConfig.Config["mysql_password"]};" +
                         $"database={GlobalConfig.Config["mysql_database"]}";
        _tablePrefix = GlobalConfig.Config["mysql_table_prefix"];
        Logger.Info("Creating tables...");
        UpdateTables();
        Logger.Info("MySQL initialised.");
    }

    public void Deinit() {
        Logger.Info("MySQL de-initialised");
    }
    
    public void UpdateTables() {
        SendMySqlStatement(@$"CREATE TABLE IF NOT EXISTS {_tablePrefix}users(
                           id VARCHAR(64) primary key,
                           username VARCHAR(255),
                           created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
                           premium BOOLEAN DEFAULT FALSE,
                           admin BOOLEAN DEFAULT FALSE)");
        SendMySqlStatement(@$"CREATE TABLE IF NOT EXISTS {_tablePrefix}votes(
                           id VARCHAR(64) primary key,
                           domain VARCHAR(255),
                           user_id VARCHAR(64),
                           value INT,
                           FOREIGN KEY (user_id) REFERENCES {_tablePrefix}users(id),
                           UNIQUE KEY (domain, user_id))");
        SendMySqlStatement(@$"CREATE TABLE IF NOT EXISTS {_tablePrefix}bot_rating(
                           id VARCHAR(64) primary key,
                           domain VARCHAR(255),
                           rating INT,
                           UNIQUE KEY (domain))");
    }

    private void SendMySqlStatement(string statement) {
        MySqlHelper.ExecuteNonQuery(_connectString, statement);
    }
    
    private async Task SendMySqlStatementAsync(string statement) {
        await MySqlHelper.ExecuteNonQueryAsync(_connectString, statement);
    }
    
    
    
}