﻿using System.ComponentModel.DataAnnotations.Schema;

namespace GetTeched.Flash_Cards.Models
{
    [Table("Stacks")]
    internal class CardStacks
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
