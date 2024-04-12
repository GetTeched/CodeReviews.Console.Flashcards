using Spectre.Console;

namespace GetTeched.Flash_Cards;

internal class Program
{
    static void Main(string[] args)
    {
        DatabaseManager databaseManager = new();
        databaseManager.SqlInitialize();
        AnsiConsole.Write(
            new FigletText("Flash Cards Project")
            .Centered()
            .Color(Color.Teal));

        AnsiConsole.Status()
    .Start("Validating Flash Cards", ctx =>
    {
        // Simulate some work
        AnsiConsole.MarkupLine("Initializing Database...");
        Thread.Sleep(2000);

        // Update the status and spinner
        ctx.Status("Building Menu structure");
        ctx.Spinner(Spinner.Known.Star);
        ctx.SpinnerStyle(Style.Parse("green"));

        // Simulate some work
        AnsiConsole.MarkupLine("Initializing Main Menu...");
        Thread.Sleep(2000);
    });

        //AnsiConsole.Progress()
        //    .Start(ctx =>
        //    {
        //        var task1 = ctx.AddTask("[green]Initializing Flash Cards Database[/]");
        //        var task2 = ctx.AddTask("[green]Validating Falsh Cards[/]");
        //        var task3 = ctx.AddTask("[green]Generating Main Menu[/]");

        //        while (!ctx.IsFinished)
        //        {
        //            task1.Increment(1.5);
        //            task2.Increment(0.5);
        //            task3.Increment(1);
        //        }
        //    });
        UserInterface userInterface = new(databaseManager);
        databaseManager.UserInterface = userInterface;
        userInterface.MainMenu();
    }
}




