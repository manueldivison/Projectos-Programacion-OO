using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinesTest
{
   public class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public bool IsFlagged { get; set; }
        public bool IsBomb { get; set; }
        public int BombsAround { get; set; }
        public bool IsEnabled { get; set; }

        public MineButton Boton { get; set; }
    }
}
