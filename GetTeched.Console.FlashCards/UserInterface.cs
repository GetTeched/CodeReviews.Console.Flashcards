using Bogus;
using GetTeched.Flash_Cards.Models;
using Spectre.Console;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTeched.Flash_Cards;

internal class UserInterface
{
    DatabaseManager DatabaseManager { get; set; }

    public UserInterface(DatabaseManager databaseManager)
    {
        DatabaseManager = databaseManager;
        databaseManager.UserInterface = this;
    }

    TableVisualEngine tablevisualEngine = new();

    internal void MainMenu()
    {
        bool endApplication = false;
        do
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
            new FigletText("Main Menu")
            .Centered()
            .Color(Color.Teal));
            var userInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select an option with the arrow keys.")
            .PageSize(10)
            .AddChoices(new[]
            {
                "Show Items", "Add Items", "Update Items", "Remove Items", "Seed Database", "Exit"
            }));

            switch (userInput)
            {
                case "Show Items":
                    ShowItems();
                    break;
                case "Add Items":
                    AddItems();
                    break;
                case "Update Items":
                    UpdateItems();
                    break;
                case "Remove Items":
                    RemoveItems();
                    break;
                case "Seed Database":
                    SeedDatabase();
                    break;
                case "Exit":
                    endApplication = true;
                    Environment.Exit(0);    
                    break;
            } 
        } while (!endApplication);
    }

    internal void ShowItems()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Show Items")
            .Centered()
            .Color(Color.Teal));

        var userInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select an option with the arrow keys.")
            .PageSize(10)
            .AddChoices(new[]
            {
                "Show Stacks", "Show Flashcards", "Return to Main Menu"
            }));

        switch (userInput)
        {
            case "Show Stacks":
                tablevisualEngine.DisplayStacks(DatabaseManager.GetStacks());
                break;
            case "Show Flashcards":
                tablevisualEngine.DisplayFlashCards(DatabaseManager.GetFlashCards());
                break;
            case "Return to Main Menu":
                break;
        }
    }

    internal void AddItems()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Add Items")
            .Centered()
            .Color(Color.Teal));

        var userInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select an option with the arrow keys.")
            .PageSize(10)
            .AddChoices(new[]
            {
                "Add Stacks", "Add Flashcards", "Return to Main Menu"
            }));

        switch (userInput)
        {
            case "Add Stacks":
                AddStack();
                break;
            case "Add Flashcards":
                AddFlashCards();
                break;
            case "Return to Main Menu":
                break;
        }
    }

    internal void UpdateItems()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Update Items")
            .Centered()
            .Color(Color.Teal));

        var userInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select an option with the arrow keys.")
            .PageSize(10)
            .AddChoices(new[]
            {
                "Update Stacks", "Update Flashcards", "Return to Main Menu"
            }));

        switch (userInput)
        {
            case "Update Stacks":
                UpdateStack();
                break;
            case "Update Flashcards":
                UpdateFlashCards();
                break;
            case "Return to Main Menu":
                break;
        }
    }

    internal void RemoveItems()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Remove Items")
            .Centered()
            .Color(Color.Teal));

        var userInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select an option with the arrow keys.")
            .PageSize(10)
            .AddChoices(new[]
            {
            "Remove Stacks", "Remove Flashcards", "Return to Main Menu"
            }));

        switch (userInput)
        {
            case "Remove Stacks":
                RemoveStack();
                break;
            case "Remove Flashcards":
                RemoveFlashCards();
                break;
            case "Return to Main Menu":
                break;
        }
    }

    internal void AddStack()
    {
        CardStacks stack = new();
        stack.Name = AnsiConsole.Ask<string>("[blue]Please enter a name for a stack[/]");

        while(string.IsNullOrEmpty(stack.Name))
        {
            stack.Name = AnsiConsole.Ask<string>("[red]Name can not be empty please try again[/]");
        }

        DatabaseManager.SqlAddStack(stack);
    }

    internal void AddFlashCards()
    {
        FlashCards flashCards = new();
        var stacks = DatabaseManager.GetStacks();

        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Add Items TEST")
            .Centered()
            .Color(Color.Teal));

        var userInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select an option with the arrow keys.")
            .PageSize(10)
            .AddChoices(DatabaseManager.StackName())
            .AddChoices("Return To Main Menu"));
        if (userInput == "Return To Main Menu") MainMenu();

        flashCards.StackId = stacks.Single(s => s.Name == userInput).Id;
        flashCards.Front = AnsiConsole.Ask<string>("[blue]Please enter a question[/]");
        while (string.IsNullOrEmpty(flashCards.Front))
        {
            flashCards.Front = AnsiConsole.Ask<string>("[red]Name can not be empty please try again[/]");
        }
        flashCards.Back = AnsiConsole.Ask<string>("[blue]Please enter an answer[/]");
        while (string.IsNullOrEmpty(flashCards.Back))
        {
            flashCards.Back = AnsiConsole.Ask<string>("[red]Name can not be empty please try again[/]");
        }
        DatabaseManager.SqlAddFlashCard(flashCards);

  
    }

    internal void UpdateStack()
    {
        CardStacks stack = new();
        var stacks = DatabaseManager.GetStacks();
        
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Update Items TEST")
            .Centered()
            .Color(Color.Teal));

        var userInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select an option with the arrow keys.")
            .PageSize(10)
            .AddChoices(DatabaseManager.StackName())
            .AddChoices("Return To Main Menu"));
        if(userInput == "Return To Main Menu") MainMenu();
        stack.Id = stacks.Single(s => s.Name == userInput).Id;
        stack.Name = AnsiConsole.Ask<string>("[blue]Please enter a name for a stack[/]");
        while (string.IsNullOrEmpty(stack.Name))
        {
            stack.Name = AnsiConsole.Ask<string>("[red]Name can not be empty please try again[/]");
        }
        DatabaseManager.SqlUpdateStack(stack);
    }

    internal void UpdateFlashCards()
    {
        FlashCards flashCards = new();
        var cards = DatabaseManager.GetFlashCards();

        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Update Items TEST")
            .Centered()
            .Color(Color.Teal));

        var userInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select an option with the arrow keys.")
            .PageSize(10)
            .AddChoices(DatabaseManager.FlashCardName())
            .AddChoices("Return To Main Menu"));
        if (userInput == "Return To Main Menu") MainMenu();

        flashCards.Id = cards.Single(s => s.Front == userInput).Id;
        flashCards.Front = AnsiConsole.Ask<string>("[blue]Please enter a question[/]");
        while (string.IsNullOrEmpty(flashCards.Front))
        {
            flashCards.Front = AnsiConsole.Ask<string>("[red]Name can not be empty please try again[/]");
        }
        flashCards.Back = AnsiConsole.Ask<string>("[blue]Please enter an answer[/]");
        while (string.IsNullOrEmpty(flashCards.Back))
        {
            flashCards.Back = AnsiConsole.Ask<string>("[red]Name can not be empty please try again[/]");
        }
        DatabaseManager.SqlUpdateFlashCards(flashCards);
    }

    internal void RemoveStack()
    {
        CardStacks stack = new();
        var stacks = DatabaseManager.GetStacks();

        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Remove Item Test")
            .Centered()
            .Color(Color.Teal));

        var userInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select an option with the arrow keys.")
            .PageSize(10)
            .AddChoices(DatabaseManager.StackName())
            .AddChoices("Return to Main Menu"));
        if (userInput == "Return to Main Menu") MainMenu();
        stack.Id = stacks.Single(s => s.Name == userInput).Id;
        if(!AnsiConsole.Confirm($"Are you sure you want to delete stack {userInput}?"))
        {
            RemoveStack();
        }
        DatabaseManager.SqlRemoveStack(stack);
    }
    internal void RemoveFlashCards()
    {
        FlashCards flashCards = new();
        var cards = DatabaseManager.GetFlashCards();

        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Remove Item Test")
            .Centered()
            .Color(Color.Teal));

        var userInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select an option with the arrow keys.")
            .PageSize(10)
            .AddChoices(DatabaseManager.FlashCardName())
            .AddChoices("Return to Main Menu"));
        if (userInput == "Return to Main Menu") MainMenu();
        flashCards.Id = cards.Single(s => s.Front == userInput).Id;
        if(!AnsiConsole.Confirm($"Are your sure you want to delete flash card {userInput}"))
        {
            RemoveFlashCards();
        }        
        DatabaseManager.SqlRemoveFlashCards(flashCards);
    }

    internal bool IdInRange(int[] idRange, int selectedId)
    {
        if (idRange.Contains(selectedId))
        {
            return true;
        }
        else AnsiConsole.Write(new Markup($"[red]ID:{selectedId} was not found. Press any key to try again.[/]"));
        Console.ReadLine();
        return false;
    }

    internal void SeedDatabase()
    {
        DatabaseManager.SqlSeedData();
    }
}
