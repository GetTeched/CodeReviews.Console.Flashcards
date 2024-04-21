using System.ComponentModel.DataAnnotations.Schema;

namespace GetTeched.Flash_Cards.Models;

[Table("FlashCards")]
internal class FlashCards
{
    public int Id { get; set; }
    public string Front { get; set; }
    public string Back { get; set; }
    public int StackId { get; set; }
}
