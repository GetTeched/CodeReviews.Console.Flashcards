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
                "Select Stack", "View Items", "Seed Database", "Exit"
            }));

            switch (userInput)
            {
                case "Select Stack":
                    SelectStack();
                    break;
                case "View Items":
                    ViewItems();
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

    internal void SelectStack()
    {
        CardStacks stack = new();
        var stacks = DatabaseManager.GetAllStacks();

        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Select Stack")
            .Centered()
            .Color(Color.Teal));

        var userInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select an option with the arrow keys.")
            .PageSize(10)
            .AddChoices(DatabaseManager.StackName())
            .AddChoices("[green]Add Stack[/]", "[red]Return To Main Menu[/]"));
        switch (userInput)
        {
            case "[green]Add Stack[/]":
                AddStack();
                break;
            case "[red]Return To Main Menu[/]":
                MainMenu();
                break;
            default:
                stack.Id = stacks.Single(s => s.Name == userInput).Id;
                stack.Name = userInput;
                StackManagement(stack);
                break;
        }
        
    }

    internal void StackManagement(CardStacks stack)
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText($"{stack.Name} Stack Management")
            .Centered()
            .Color(Color.Teal));

        var userInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select an option with the arrow keys.")
            .PageSize(12)
            .AddChoices(new[]
            {
                "View Flashcards","Study Flashcards"
            })
            .AddChoiceGroup("Manage Cards", new[]
            {
                "Add Flahshcards", "Edit Flashcards","Remove Flashcards"
            })
            .AddChoiceGroup("Manage Stack", new[]
            {
                "Edit Stack Name","Choose Another Stack", "Delete This Stack"
            })
            .AddChoices("Return To Main Menu"));

        switch (userInput)
        {
            case "View Flashcards":
                tablevisualEngine.DisplayFlashCards(DatabaseManager.GetFlashCards(stack));
                break;
            case "Add Flahshcards":
                AddFlashCards(stack);
                break;
            case "Edit Flashcards":
                UpdateFlashCards(stack);
                break;
            case "Remove Flashcards":
                RemoveFlashCards(stack);
                break;
            case "Edit Stack Name":
                UpdateStack(stack);
                break;
            case "Choose Another Stack":
                SelectStack();
                break;
            case "Delete This Stack":
                RemoveStack(stack);
                break;
            case "Return To Main Menu":
                break;
        }
    }

    internal void ViewItems()
    {
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("View Items")
            .Centered()
            .Color(Color.Teal));

        var userInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select an option with the arrow keys.")
            .PageSize(10)
            .AddChoices(new[]
            {
                "View Stacks", "View Flashcards", "Return to Main Menu"
            }));

        switch (userInput)
        {
            case "View Stacks":
                tablevisualEngine.DisplayStacks(DatabaseManager.GetAllStacks());
                break;
            case "View Flashcards":
                tablevisualEngine.DisplayFlashCards(DatabaseManager.GetAllFlashCards());
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

    internal void AddFlashCards(CardStacks stack)
    {
        FlashCards flashCards = new();
        var stacks = DatabaseManager.GetAllStacks();

        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText($"Add Flashcard To {stack.Name}")
            .Centered()
            .Color(Color.Teal));

        var userInput = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("Please select an option with the arrow keys.")
                .PageSize(10)
                .AddChoices("Add Flashcard")
                .AddChoices("Return To Stack Management"));

        if (userInput == "Return To Stack Management")
        {
            StackManagement(stack);
        }
        else
        {
            flashCards.StackId = stack.Id;
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
    }

    internal void UpdateStack(CardStacks stack)
    {
        
        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Update Items TEST")
            .Centered()
            .Color(Color.Teal));

        stack.Name = AnsiConsole.Ask<string>("[blue]Please enter a name for a stack[/]");
        while (string.IsNullOrEmpty(stack.Name))
        {
            stack.Name = AnsiConsole.Ask<string>("[red]Name can not be empty please try again[/]");
        }
        DatabaseManager.SqlUpdateStack(stack);
    }

    internal void UpdateFlashCards(CardStacks stack)
    {
        FlashCards flashCards = new();
        var cards = DatabaseManager.GetAllFlashCards();

        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Update Items TEST")
            .Centered()
            .Color(Color.Teal));

        var userInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select an option with the arrow keys.")
            .PageSize(10)
            .AddChoices(DatabaseManager.FlashCardName(stack))
            .AddChoices("Return To Stack Management"));
        if (userInput == "Return To Stack Management")
        {
            StackManagement(stack);
        } 
        else
        {
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
    }

    internal void RemoveStack(CardStacks stack)
    {

        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Remove Item Test")
            .Centered()
            .Color(Color.Teal));

        if(!AnsiConsole.Confirm($"Are you sure you want to delete stack {stack.Name}?"))
        {
            StackManagement(stack);
        }
        DatabaseManager.SqlRemoveStack(stack);
    }

    internal void RemoveFlashCards(CardStacks stack)
    {
        FlashCards flashCards = new();
        var cards = DatabaseManager.GetAllFlashCards();

        AnsiConsole.Clear();
        AnsiConsole.Write(
            new FigletText("Remove Item Test")
            .Centered()
            .Color(Color.Teal));

        var userInput = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("Please select an option with the arrow keys.")
            .PageSize(10)
            .AddChoices(DatabaseManager.FlashCardName(stack))
            .AddChoices("Return to Main Menu"));
        if (userInput == "Return to Main Menu") MainMenu();
        flashCards.Id = cards.Single(s => s.Front == userInput).Id;
        if(!AnsiConsole.Confirm($"Are your sure you want to delete flash card {userInput}"))
        {
            RemoveFlashCards(stack);
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
