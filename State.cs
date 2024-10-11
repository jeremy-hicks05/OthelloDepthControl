using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OthelloAStar
{
    internal class State
    {
        public Board CurrentBoard {  get; set; }
        public Player Turn {  get; set; }
        public State() {
            CurrentBoard = new Board();
            Turn = new Player();
        }

        public State(Board board)
        {
            CurrentBoard = board;
            Turn = new Player();
        }
    }
}
