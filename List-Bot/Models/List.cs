using System;
using System.Collections.Generic;

namespace List_Bot.Models
{
    public class List
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ListItem> Items { get; set; } = new List<ListItem>();
        public DateTime CreationDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
