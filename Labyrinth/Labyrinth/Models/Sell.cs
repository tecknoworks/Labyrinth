using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Labyrinth.Models
{
    public class Sell
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }

        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public bool IsSold { get; set; }
        public virtual Player Player { get; set; }
        public virtual Item Item { get; set; }

    }
}