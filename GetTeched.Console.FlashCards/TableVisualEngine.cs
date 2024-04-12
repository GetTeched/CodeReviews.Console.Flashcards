using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetTeched.Flash_Cards;

internal class TableVisualEngine
{
    internal static void ShowTable<T>(List<T> tableData) where T : class
    {
        DisplayData(tableData);
    }

    private static void DisplayData<T>(List<T> data) where T : class
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

        foreach(var item in data)
        {
            var row = new List<string>();
            foreach(var property in properties)
            {
                var value = property.GetValue(item);
                row.Add(value?.ToString() ?? string.Empty);
            }
           table.AddRow(row.ToArray());
        }
        AnsiConsole.Write(table);
        Console.ReadLine();
    }
}
