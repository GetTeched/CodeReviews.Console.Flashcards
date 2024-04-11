using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTeched.FlashCards;

internal class UserInterface
{
    DatabaseManager DatabaseManager { get; set; }

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
                AnsiConsole.Write(new Markup("[green]Not yet implemented. Press any key to return to menu[/]"));
                Console.ReadLine();
                break;
            case "Show Flashcards":
                AnsiConsole.Write(new Markup("[green]Not yet implemented. Press any key to return to menu[/]"));
                Console.ReadLine();
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
                AnsiConsole.Write(new Markup("[green]Not yet implemented. Press any key to return to menu[/]"));
                Console.ReadLine();
                break;
            case "Add Flashcards":
                AnsiConsole.Write(new Markup("[green]Not yet implemented. Press any key to return to menu[/]"));
                Console.ReadLine();
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
                AnsiConsole.Write(new Markup("[green]Not yet implemented. Press any key to return to menu[/]"));
                Console.ReadLine();
                break;
            case "Update Flashcards":
                AnsiConsole.Write(new Markup("[green]Not yet implemented. Press any key to return to menu[/]"));
                Console.ReadLine();
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
                AnsiConsole.Write(new Markup("[green]Not yet implemented. Press any key to return to menu[/]"));
                Console.ReadLine();
                break;
            case "Remove Flashcards":
                AnsiConsole.Write(new Markup("[green]Not yet implemented. Press any key to return to menu[/]"));
                Console.ReadLine();
                break;
            case "Return to Main Menu":
                break;
        }
    }
}
