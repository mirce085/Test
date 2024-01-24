using ConnectionMode.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConnectionMode.Models;

namespace ConnectionMode.Messages;

public class AddItemMessage
{
    public AddItemMessage(Item item)
    {
        Item = item;
    }
    public Item Item { get; set; }
}
