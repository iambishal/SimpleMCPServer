using System.ComponentModel;
using ModelContextProtocol.Server;
using Microsoft.Data.Sqlite;

/// <summary>
/// Sample MCP tools for demonstration purposes.
/// These tools can be invoked by MCP clients to perform various operations.
/// </summary>
internal class RandomNumberTools
{
 
   
  [McpServerTool]       
  [Description("Create Client Basic Information with FirstName, LastName, Age, and Address, and store in SQLite DB")]
  public string CreateClientBasicInfo(
    [Description("FirstName of the Client")] string FirstName,
    [Description("LastName of the Client")] string LastName,
    [Description("Age of the Client")] int Age,
    [Description("client address")] string Address)
  {
    string dbPath = "clients.db";
    string connectionString = $"Data Source={dbPath}";

    // Ensure DB and table exist
    using (var connection = new SqliteConnection(connectionString))
    {
      connection.Open();
      var tableCmd = connection.CreateCommand();`
      tableCmd.CommandText = @"
        CREATE TABLE IF NOT EXISTS Clients (
          Id INTEGER PRIMARY KEY AUTOINCREMENT,
          FirstName TEXT,
          LastName TEXT,
          Age INTEGER,
          Address TEXT
        );";
      tableCmd.ExecuteNonQuery();

      var insertCmd = connection.CreateCommand();
      insertCmd.CommandText = @"
        INSERT INTO Clients (FirstName, LastName, Age, Address)
        VALUES ($firstName, $lastName, $age, $address);";
      insertCmd.Parameters.AddWithValue("$firstName", FirstName);
      insertCmd.Parameters.AddWithValue("$lastName", LastName);
      insertCmd.Parameters.AddWithValue("$age", Age);
      insertCmd.Parameters.AddWithValue("$address", Address);
      insertCmd.ExecuteNonQuery();
    }

    return $"Client Basic Info: FirstName: {FirstName}, LastName: {LastName}, Age: {Age}, Address: {Address} is created and stored in database successfully.";
  }
  
}
