using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloAStar
{
    internal class Space
    {
        public Piece Piece { get; set; }
        //public bool Empty { get; set; }
        public int Row {  get; set; }
        public int Column { get; set; }

        public Space()
        {
            Piece = new Piece(Piece.Type.None);
            //Empty = true;
        }

        internal bool IsEmpty()
        {
            return Piece.Owner == Piece.Type.None;
        }
    }
}
