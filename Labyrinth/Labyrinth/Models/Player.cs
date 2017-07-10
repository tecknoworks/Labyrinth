using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Labyrinth.Models
{
    public class Player
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public int Gold { get; set; }
        public int Points { get; set; }
    }
}