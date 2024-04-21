using System.ComponentModel.DataAnnotations.Schema;

namespace GetTeched.Flash_Cards.Models;

[Table("Study")]
internal class Study
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public int Quantity { get; set; }
    public float Percentage { get; set; }
    public int StackID { get; set; }
}
