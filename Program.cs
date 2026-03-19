///
/// ETML
/// Author: Thibault Gugler FID1
/// Date: 19.03.26
/// 
/// Minesweeper
/// 

// Table ASCII : https://sebastienguillon.com/test/jeux-de-caracteres/windows-ascii-fr.html

// ╣ ║ ╗ ╝ ╚ ╔ ╩ ╦ ╠ ═ ╬

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minesweeper.Board;

namespace Minesweeper
{
    internal class Program
    {
        static void Main()
        {
            Game game = new Game();
            game.Start();
        }
    }
}
