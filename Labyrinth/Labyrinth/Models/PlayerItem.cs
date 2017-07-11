using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Labyrinth.Models
{
    public class PlayerItem
    {
        public int Id { get; set; }
        public Guid PlayerId { get; set; }
        
        public int ItemId { get; set; }
        public int Quantity { get; set; }

        public virtual Player Player { get; set; }
        public virtual Item Item { get; set; }

    }
}