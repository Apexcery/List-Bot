using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace List_Bot.Models
{
    public class ListItem
    {
        public Guid Id { get; set; }
        public string Value { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
