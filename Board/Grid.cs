///
/// ETML
/// Author: GT FID1
/// Date: 19.03.26
///
/// Minesweeper
///

// ASCII Table : https://sebastienguillon.com/test/jeux-de-caracteres/windows-ascii-fr.html

// ╣ ║ ╗ ╝ ╚ ╔ ╩ ╦ ╠ ═ ╬

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Minesweeper.Board
{
    /// <summary>
    /// Manages the game grid and its rules
    /// </summary>
    internal class Grid
    {
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        private int[,] _grid;

        public Grid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            _grid = new int[Rows, Columns];
        }

        public int GetLowestAvailableRow(int column)
        {
            for (int row = Rows - 1; row >= 0; row--)
            {
                if (_grid[row, column] == 0)
                {
                    return row;
                }
            }
            return -1; // Column full
        }

        public void PlacePawn(int row, int column, int playerId)
        {
            _grid[row, column] = playerId;
        }

        public void Draw()
        {
            byte i, j;

            // Main grid
            Console.Write("\t╔");
            for (i = 1; i < Columns; i++) Console.Write("═══╦");
            Console.WriteLine("═══╗");

            for (j = 0; j < Rows - 1; j++)
            {
                Console.Write("\t║   ");
                for (i = 0; i < Columns; i++) Console.Write("║   ");

                Console.Write("\n\t╠═══╬");
                for (i = 2; i < Columns; i++) Console.Write("═══╬");
                Console.WriteLine("═══╣");
            }

            Console.Write("\t║");
            for (i = 1; i < Columns; i++) Console.Write("   ║");
            Console.WriteLine("   ║");

            Console.Write("\t╚");
            for (i = 1; i < Columns; i++) Console.Write("═══╩");
            Console.WriteLine("═══╝");
        }

        public bool CheckVictory(int lastRow, int lastCol, int playerId)
        {
            // Horizontal check
            int count = 1;
            for (int i = lastCol - 1; i >= 0 && _grid[lastRow, i] == playerId; i--) count++;
            for (int i = lastCol + 1; i < Columns && _grid[lastRow, i] == playerId; i++) count++;
            if (count >= 4) return true;

            // Vertical check
            count = 1;
            for (int i = lastRow + 1; i < Rows && _grid[i, lastCol] == playerId; i++) count++;
            if (count >= 4) return true;

            // Diagonal check 1 (\)
            count = 1;
            for (int i = 1; lastRow - i >= 0 && lastCol - i >= 0 && _grid[lastRow - i, lastCol - i] == playerId; i++) count++;
            for (int i = 1; lastRow + i < Rows && lastCol + i < Columns && _grid[lastRow + i, lastCol + i] == playerId; i++) count++;
            if (count >= 4) return true;

            // Diagonal check 2 (/)
            count = 1;
            for (int i = 1; lastRow - i >= 0 && lastCol + i < Columns && _grid[lastRow - i, lastCol + i] == playerId; i++) count++;
            for (int i = 1; lastRow + i < Rows && lastCol - i >= 0 && _grid[lastRow + i, lastCol - i] == playerId; i++) count++;
            if (count >= 4) return true;

            return false;
        }
    }
}
