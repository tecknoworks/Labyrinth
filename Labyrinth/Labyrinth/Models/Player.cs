using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Labyrinth.Models
{
    public class Player
    {
        public Guid Id { get; set; }
        public string Nickname { get; set; }
        public int Gold { get; set; }
        public int Points { get; set; }
    }
}