using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloAStar
{
    internal class Piece
    {
        public Type Owner {  get; set; }
        public enum Type {
            Black, 
            White,
            None
        }
        public Piece()
        {
            
        }

        public Piece(Type type)
        {
            this.Owner = type;
        }
    }
}
