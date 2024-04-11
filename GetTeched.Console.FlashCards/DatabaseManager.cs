using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Dapper;

namespace GetTeched.FlashCards;

internal class DatabaseManager
{
    IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

    private string ConnectionString;

    public DatabaseManager()
    {
        ConnectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"];
    }

    public void SqlInitialize()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            string sqlStackTable =
                @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name ='Stacks')
                CREATE TABLE Stacks(ID int IDENTITY (1,1) NOT NULL,
                Name NVARCHAR(30) NOT NULL UNIQUE,
                Primary Key(Id))";
            string sqlFlashTable =
                @"IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'FlashCards')
                CREATE TABLE FlashCards(Id int IDENTITY (1,1) NOT NULL,
                Front NVARCHAR(30) NOT NULL, Back NVARCHAR(30) NOT NULL,
                StackId int NOT NULL, 
                FOREIGN Key(StackId)
                REFERENCES Stacks(ID))";

            connection.Execute(sqlStackTable);
            connection.Execute(sqlFlashTable);

        }
    }

    public void SqlShowItems()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            string sqlQuery = @"";
        }
    }
}
