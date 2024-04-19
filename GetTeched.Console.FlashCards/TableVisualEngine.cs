using GetTeched.Flash_Cards.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTeched.Flash_Cards;

internal class TableVisualEngine
{
    DatabaseManager databaseManager = new();
    internal static void ShowTable<T>(List<T> tableData, bool rawData = false) where T : class
    {
        if (rawData) DisplayRawData(tableData);
        else DisplayData(tableData);
    }

    private static void DisplayData<T>(List<T> data) where T : class
    {
        var table = new Table()
            .Border(TableBorder.Double)
            .Title("[teal]Showing Current Table[/]")
            .Caption("[red]Press any key to return to the Main Menu[/]");

        var properties = typeof(T).GetProperties();

        table.AddColumn(new TableColumn($"[yellow]Id[/]"));

        foreach (var property in properties)
        {
            if(property.Name != "Id")
            table.AddColumn(new TableColumn($"[yellow]{property.Name}[/]"));
        }
        int index = 1;
        foreach(var item in data)
        {
            var row = new List<string>();
            foreach(var property in properties)
            {
                if (property.Name == "Id")
                {
                    property.SetValue(item, index);
                    index++;
                }
                var value = property.GetValue(item);
                row.Add(value?.ToString() ?? string.Empty);
                
            }
           table.AddRow(row.ToArray());
        }
        AnsiConsole.Write(table);
        Console.ReadLine();
    }

    private static void DisplayRawData<T>(List<T> data) where T : class
    {
        var table = new Table()
            .Border(TableBorder.Double)
            .Title("[teal]Showing Current Table[/]")
            .Caption("[red]Press any key to return to the Main Menu[/]");

        var properties = typeof(T).GetProperties();

        foreach (var property in properties)
        {
            table.AddColumn(new TableColumn($"[yellow]{property.Name}[/]"));
        }
        foreach (var item in data)
        {
            var row = new List<string>();
            foreach (var property in properties)
            {
                var value = property.GetValue(item);
                row.Add(value?.ToString() ?? string.Empty);
            }
            table.AddRow(row.ToArray());
        }
        AnsiConsole.Write(table);
        Console.ReadLine();
    }

    internal void DisplayFlashCards(IEnumerable<FlashCards> flashCards)
    {
        var stack = databaseManager.GetStacks();
        var table = new Table()
            .Border(TableBorder.Double)
            .Title("[teal]Showing Table[/]")
            .Caption("[red]Press any key to return to the Main Menu[/]");

        table.AddColumn(new TableColumn("[yellow]Id[/]"));
        table.AddColumn(new TableColumn("[yellow]Question[/]"));
        table.AddColumn(new TableColumn("[yellow]Answer[/]"));
        table.AddColumn(new TableColumn("[yellow]Stack[/]"));

        int index = 1;

        foreach (var card in flashCards)
        {
            var row = new List<string>();
            var stackName = stack.Single(s => s.Id == card.StackId).Name;
            row.Add(index.ToString());
            row.Add(card.Front);
            row.Add(card.Back);
            row.Add(stackName.ToString());
            table.AddRow(row.ToArray());
            index++;
        }
        AnsiConsole.Write(table);
        Console.ReadLine();
    }
    internal void DisplayStacks(IEnumerable<CardStacks> cardStacks)
    {
        var table = new Table()
            .Border(TableBorder.Double)
            .Title("[teal]Showing Table[/]")
            .Caption("[red]Press any key to return to the Main Menu[/]");

        table.AddColumn(new TableColumn("[yellow]Id[/]"));
        table.AddColumn(new TableColumn("[yellow]Name[/]"));

        int index = 1;

        foreach (var card in cardStacks)
        {
            var row = new List<string>();
            row.Add(index.ToString());
            row.Add(card.Name);
            table.AddRow(row.ToArray());
            index++;
        }
        AnsiConsole.Write(table);
        Console.ReadLine();
    }
}
