using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Dapper;
using GetTeched.Flash_Cards.Models;
using Spectre.Console;

namespace GetTeched.Flash_Cards;

internal class DatabaseManager
{
    IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

    private string ConnectionString;

    public UserInterface UserInterface { get; set; }

    public DatabaseManager()
    {
        ConnectionString = configuration.GetSection("ConnectionStrings")["DefaultConnection"];
    }

    internal void SqlInitialize()
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

    internal List<CardStacks> SqlShowStacks()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            string sqlQuery = @"SELECT * FROM Stacks";
            var tableData = connection.Query<CardStacks>(sqlQuery).ToList();

            if(tableData.Any())
            {
                AnsiConsole.Clear();
                TableVisualEngine.ShowTable(tableData);
            }
            else
            {
                AnsiConsole.Write(new Markup("[red]\n No Data Found, Press any key to return to the Main Menu[/]"));
                Console.ReadLine();
                UserInterface.MainMenu();
            }
        }
        return new List<CardStacks>();
    }

    internal List<FlashCards> SqlShowFlashCards()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            string sqlQuery = @"SELECT * FROM FlashCards";
            var tableData = connection.Query<FlashCards>(sqlQuery).ToList();

            if (tableData.Any())
            {
                AnsiConsole.Clear();
                TableVisualEngine.ShowTable(tableData);
            }
            else
            {
                AnsiConsole.Write(new Markup("[red]\n No Data Found, Press any key to return to the Main Menu[/]"));
                Console.ReadLine();
                UserInterface.MainMenu();
            }
        }
        return new List<FlashCards>();
    }

    internal void SqlAddStack(CardStacks stacks)
    {
        

        using(var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            string sqlQuery = 
                @"INSERT INTO Stacks (Name) VALUES (@Name)";
            connection.Execute(sqlQuery, new { stacks.Name });
        }
    }

    internal int[] GetIds(string tableName)
    {
        List<int> ids = new();
        using(var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            string sqlQuery = 
                @$"SELECT * FROM {tableName}";
            var properties = connection.Query<CardStacks>(sqlQuery);

            foreach (var property in properties)
            {
                ids.Add(property.Id);
            }
            return ids.ToArray();
        }
    }

    internal void SqlAddFlashCard(FlashCards flashCards)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            string sqlQuery =
                @"INSERT INTO FlashCards (Front,Back,StackId) VALUES (@Front,@Back,@StackID)";
            connection.Execute(sqlQuery, new { flashCards.Front, flashCards.Back, flashCards.StackId });
        }
    }

    internal void SqlUpdateStack(CardStacks stacks)
    {
        using(var connection =new SqlConnection(ConnectionString))
        {
            connection.Open();
            string sqlQuery =
                @"UPDATE Stacks SET Name = @Name";
            connection.Execute(sqlQuery, new {stacks.Name});
        }
    }

    internal void SqlUpdateFlashCards(FlashCards flashCards)
    {
        using(var connection =new SqlConnection(ConnectionString))
        {
            connection.Open();
            string sqlQuery =
                @"UPDATE FlashCards SET
                Front = @Front,
                Back = @Back";
            connection.Execute(sqlQuery, new {flashCards.Front, flashCards.Back});
        }
    }

    internal void SqlRemoveStack(CardStacks stacks)
    {
        using(var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            string sqlForeignKey =
                @"DELETE FROM FlashCards WHERE StackId = @Id";
            string sqlQuery =
                @"DELETE FROM Stacks WHERE Id = @Id";
            connection.Execute(sqlForeignKey, new {stacks.Id});
            connection.Execute(sqlQuery, new {stacks.Id});
        }
    }

    internal void SqlRemoveFlashCards(FlashCards flashCards)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            string sqlQuery =
                @"DELETE FROM FlashCards WHERE Id = @Id";
            connection.Execute(sqlQuery, new { flashCards.Id });
        }
    }
}
