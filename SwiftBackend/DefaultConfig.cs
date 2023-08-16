using GeneralPurposeLib;

namespace SwiftBackend; 

public static class DefaultConfig {

    public static Dictionary<string, Property> Config = new() {
        { "logging_level", "Debug" },
        { "mysql_host", "mysql.serble.net" },
        { "mysql_user", "admin" },
        { "mysql_password", "admin" },
        { "mysql_database", "chatserver" },
        { "mysql_table_prefix", "chatserver_" },
        { "serble_app_id", "xxxxxxxxxxxx" },
        { "serble_app_secret", "xxxxxxxxxxxx" },
        { "open_ai_token", "xxxxxxxxxxxx" }
    };

}