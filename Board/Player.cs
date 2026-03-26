///
/// ETML
/// Author: GT FID1
/// Date: 19.03.26
/// 
/// Minesweeper
///

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.Board
{
    /// <summary>
    /// Represents a player
    /// </summary>
    internal class Player
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public ConsoleColor Color { get; private set; }
        public int MovesCount { get; set; }

        public Player(int id, string name, ConsoleColor color)
        {
            Id = id;
            Name = name;
            Color = color;
            MovesCount = 0;
        }

        public void DrawPawn()
        {
            Console.ForegroundColor = Color;
            Console.Write("█");
            Console.ResetColor();
        }
    }
}
