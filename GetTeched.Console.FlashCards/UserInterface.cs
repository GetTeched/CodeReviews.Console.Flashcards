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
                "Show Items", "Add Items", "Update Items", "Remove Items", "Exit"
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
                DatabaseManager.SqlShowStacks();
                break;
            case "Show Flashcards":
                DatabaseManager.SqlShowFlashCards();
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
        bool entryValid = false;
        FlashCards flashCards = new();
        flashCards.Front = AnsiConsole.Ask<string>("[blue]Please enter a question for this card[/]");
        flashCards.Back = AnsiConsole.Ask<string>("[blue]Please enter a answer for this card.[/]");

        while (!entryValid)
        {
            DatabaseManager.SqlShowStacks();
            flashCards.StackId = AnsiConsole.Ask<int>("[blue]Please enter the Id of the stack this card is linked to.[/]\n [red] Enter 0 to return to the Main Menu[/]");
            if (flashCards.StackId == 0) MainMenu();
            AnsiConsole.Clear();
            entryValid = IdInRange(DatabaseManager.GetIds("FlashCards"), flashCards.StackId);
        }

        DatabaseManager.SqlAddFlashCard(flashCards);
    }

    internal void UpdateStack()
    {
        bool entryValid = false;
        CardStacks stack = new();

        while (!entryValid)
        {
            DatabaseManager.SqlShowStacks();
            stack.Id = AnsiConsole.Ask<int>("[blue]Please enter the Id of the stack you want to edit.[/]\n [red] Enter 0 to return to the Main Menu[/]");
            if (stack.Id == 0) MainMenu();
            AnsiConsole.Clear();
            entryValid = IdInRange(DatabaseManager.GetIds("Stacks"), stack.Id);
        }
        stack.Name = AnsiConsole.Ask<string>("[blue]Please enter a name for a stack[/]");
        while (string.IsNullOrEmpty(stack.Name))
        {
            stack.Name = AnsiConsole.Ask<string>("[red]Name can not be empty please try again[/]");
        }
        DatabaseManager.SqlUpdateStack(stack);
    }

    internal void UpdateFlashCards()
    {
        bool entryValid = false;
        FlashCards flashCards = new();

        while(!entryValid)
        {
            DatabaseManager.SqlShowFlashCards();
            flashCards.Id = AnsiConsole.Ask<int>("[blue]Please enter the Id of the Flash Card you want to edit.[/]\n [red] Enter 0 to return to the Main Menu[/]");
            if (flashCards.Id == 0) MainMenu();
            AnsiConsole.Clear();
            entryValid = IdInRange(DatabaseManager.GetIds("FlashCards"), flashCards.Id);
        }

        flashCards.Front = AnsiConsole.Ask<string>("[blue]Please enter a question for this card[/]");
        flashCards.Back = AnsiConsole.Ask<string>("[blue]Please enter a answer for this card.[/]");
        DatabaseManager.SqlUpdateFlashCards(flashCards);
    }

    internal void RemoveStack()
    {
        bool entryValid = false;
        CardStacks stack = new();

        while (!entryValid)
        {
            DatabaseManager.SqlShowStacks();
            stack.Id = AnsiConsole.Ask<int>("[blue]Please enter the Id of the stack you want to remove.[/]\n [red] Enter 0 to return to the Main Menu[/]");
            if (stack.Id == 0) MainMenu();
            AnsiConsole.Clear();
            entryValid = IdInRange(DatabaseManager.GetIds("Stacks"), stack.Id);
        }
        //TODO: Get stack name for confirmation
        if (!AnsiConsole.Confirm($"Are you sure you want to delete the following Stack: "))
            MainMenu();
        DatabaseManager.SqlRemoveStack(stack);
    }
    internal void RemoveFlashCards()
    {
        bool entryValid = false;
        FlashCards flashCards = new();

        while (!entryValid)
        {
            DatabaseManager.SqlShowFlashCards();
            flashCards.Id = AnsiConsole.Ask<int>("[blue]Please enter the Id of the flash card you want to remove.[/]\n [red] Enter 0 to return to the Main Menu[/]");
            if (flashCards.Id == 0) MainMenu();
            AnsiConsole.Clear();
            entryValid = IdInRange(DatabaseManager.GetIds("FlashCards"), flashCards.Id);
        }
        //TODO: Get stack name for confirmation
        if (!AnsiConsole.Confirm($"Are you sure you want to delete the following Stack: "))
            MainMenu();
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
}
