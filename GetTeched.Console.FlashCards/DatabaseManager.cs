﻿using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using Dapper;
using GetTeched.Flash_Cards.Models;
using Spectre.Console;
using Z.Dapper.Plus;

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
                Front NVARCHAR(80) NOT NULL, Back NVARCHAR(80) NOT NULL,
                StackId int NOT NULL, 
                FOREIGN Key(StackId)
                REFERENCES Stacks(ID))";

            connection.Execute(sqlStackTable);
            connection.Execute(sqlFlashTable);
        }

        
    }

    internal void SqlSeedData()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            var stackList = new List<CardStacks>();
            stackList.Add(new CardStacks() { Name = "Mathematics" });
            stackList.Add(new CardStacks() { Name = "History" });
            stackList.Add(new CardStacks() { Name = "Biology" });
            stackList.Add(new CardStacks() { Name = "Geography" });
            stackList.Add(new CardStacks() { Name = "Literature" });
            connection.BulkInsert(stackList);

            var flashList = new List<FlashCards>();
            flashList.Add(new FlashCards() { Front = "What is the value of π (pi) to two decimal places", Back = "3.14", StackId = 1 });
            flashList.Add(new FlashCards() { Front = "What is the formula to find the area of a rectangle?", Back = "Length × Width", StackId = 1 });
            flashList.Add(new FlashCards() { Front = "What is the result of 5 × 7?", Back = "35", StackId = 1 });
            flashList.Add(new FlashCards() { Front = "What is the square root of 25?", Back = "5", StackId = 1 });
            flashList.Add(new FlashCards() { Front = "Solve for x: 2x + 3 = 11", Back = "x = 4", StackId = 1 });

            flashList.Add(new FlashCards() { Front = "Who was the first President of the United States?", Back = "George Washington", StackId = 2 });
            flashList.Add(new FlashCards() { Front = "In what year did World War II end?", Back = "1945", StackId = 2 });
            flashList.Add(new FlashCards() { Front = "Who wrote \"The Communist Manifesto\"?", Back = "Karl Marx", StackId = 2 });
            flashList.Add(new FlashCards() { Front = "What year did the Titanic sink?", Back = "1912", StackId = 2 });
            flashList.Add(new FlashCards() { Front = "Who was the last monarch of Russia?", Back = "Nicholas II", StackId = 2 });

            flashList.Add(new FlashCards() { Front = "What is the powerhouse of the cell?", Back = "Mitochondria", StackId = 3 });
            flashList.Add(new FlashCards() { Front = "What is the largest organ in the human body?", Back = "Skin", StackId = 3 });
            flashList.Add(new FlashCards() { Front = "What is the process by which plants make their own food?", Back = "Photosynthesis", StackId = 3 });
            flashList.Add(new FlashCards() { Front = "What type of blood cells are responsible for carrying oxygen?", Back = "Red blood cells", StackId = 3 });
            flashList.Add(new FlashCards() { Front = "What is the main function of the kidneys?", Back = "Filter blood and remove waste products", StackId = 3 });

            flashList.Add(new FlashCards() { Front = "What is the capital of France?", Back = "Paris", StackId = 4 });
            flashList.Add(new FlashCards() { Front = "Which ocean is the largest by area?", Back = "Pacific Ocean", StackId = 4 });
            flashList.Add(new FlashCards() { Front = "What is the tallest mountain in the world?", Back = "Mount Everest", StackId = 4 });
            flashList.Add(new FlashCards() { Front = "Which country is known as the Land of the Rising Sun?", Back = "Japan", StackId = 4 });
            flashList.Add(new FlashCards() { Front = "What is the longest river in the world?", Back = "Nile River", StackId = 4 });

            flashList.Add(new FlashCards() { Front = "Who wrote \"Romeo and Juliet\"?", Back = "William Shakespeare", StackId = 5 });
            flashList.Add(new FlashCards() { Front = "What is the opening line of \"Moby Dick\"?", Back = "\"Call me Ishmael.\"", StackId = 5 });
            flashList.Add(new FlashCards() { Front = "What is the title of the first Harry Potter book?", Back = "Harry Potter and the Philosopher's Stone (or Sorcerer's Stone in the US)", StackId = 5 });
            flashList.Add(new FlashCards() { Front = "Who wrote the \"Lord of the Rings\" series?", Back = "J.R.R. Tolkien", StackId = 5 });
            flashList.Add(new FlashCards() { Front = "What is the name of the protagonist in \"The Catcher in the Rye\"?", Back = "Holden Caulfield", StackId = 5 });

            connection.BulkInsert(flashList);
        }
    }

    internal List<CardStacks> SqlShowStacks(bool rawData = false)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            string sqlQuery = @"SELECT * FROM Stacks";
            var tableData = connection.Query<CardStacks>(sqlQuery).ToList();

            if(tableData.Any())
            {
                AnsiConsole.Clear();
                TableVisualEngine.ShowTable(tableData, rawData);
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

    internal List<FlashCards> SqlShowFlashCards(bool rawData = false)
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            string sqlQuery = @"SELECT * FROM FlashCards";
            var tableData = connection.Query<FlashCards>(sqlQuery).ToList();

            if (tableData.Any())
            {
                AnsiConsole.Clear();
                TableVisualEngine.ShowTable(tableData, rawData);
                //TableVisualEngine.Display(tableData);
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

    internal IEnumerable<CardStacks> GetStacks()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            string sqlQuery =
                @$"SELECT * FROM Stacks";
            var properties = connection.Query<CardStacks>(sqlQuery);
            return properties;
        }
    }

    internal IEnumerable<FlashCards> GetFlashCards()
    {
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            string sqlQuery =
                @$"SELECT * FROM FlashCards";
            var properties = connection.Query<FlashCards>(sqlQuery);
            return properties;
        }
    }

    internal string[] StackName()
    {
        List<string> stack = new();
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            string sqlQuery =
                @$"SELECT * FROM Stacks";
            var properties = connection.Query<CardStacks>(sqlQuery);

            foreach (var property in properties)
            {
                stack.Add(property.Name);
            }
            return stack.ToArray();
        }
    }

    internal string[] FlashCardName()
    {
        List<string> flashCard = new();
        using (var connection = new SqlConnection(ConnectionString))
        {
            connection.Open();
            string sqlQuery =
                @$"SELECT * FROM FlashCards";
            var properties = connection.Query<FlashCards>(sqlQuery);

            foreach (var property in properties)
            {
                flashCard.Add(property.Front);
            }
            return flashCard.ToArray();
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
                @"UPDATE Stacks SET Name = @Name WHERE Id = @Id";
            connection.Execute(sqlQuery, new {stacks.Name, stacks.Id });
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
                Back = @Back
                WHERE Id = @Id";
            connection.Execute(sqlQuery, new {flashCards.Front, flashCards.Back, flashCards.Id});
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
