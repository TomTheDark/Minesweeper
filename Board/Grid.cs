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
using System.Xml.Linq;

namespace Minesweeper.Board
{
    /// <summary>
    /// Gère la grille du jeu et ses règles
    /// </summary>
    internal class Grid
    {
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        private int[,] grid;

        public Grid(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            grid = new int[Rows, Columns];
        }

        public int GetLowestAvailableRow(int column)
        {
            for (int row = Rows - 1; row >= 0; row--)
            {
                if (grid[row, column] == 0)
                {
                    return row;
                }
            }
            return -1; // Colonne pleine
        }

        public void PlacePawn(int row, int column, int playerId)
        {
            grid[row, column] = playerId;
        }

        public void Draw()
        {
            byte i, j;

            // Grille principale
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
            // Vérification horizontale
            int count = 1;
            for (int i = lastCol - 1; i >= 0 && grid[lastRow, i] == playerId; i--) count++;
            for (int i = lastCol + 1; i < Columns && grid[lastRow, i] == playerId; i++) count++;
            if (count >= 4) return true;

            // Vérification verticale
            count = 1;
            for (int i = lastRow + 1; i < Rows && grid[i, lastCol] == playerId; i++) count++;
            if (count >= 4) return true;

            // Vérification Diagonale 1 (\)
            count = 1;
            for (int i = 1; lastRow - i >= 0 && lastCol - i >= 0 && grid[lastRow - i, lastCol - i] == playerId; i++) count++;
            for (int i = 1; lastRow + i < Rows && lastCol + i < Columns && grid[lastRow + i, lastCol + i] == playerId; i++) count++;
            if (count >= 4) return true;

            // Vérification Diagonale 2 (/)
            count = 1;
            for (int i = 1; lastRow - i >= 0 && lastCol + i < Columns && grid[lastRow - i, lastCol + i] == playerId; i++) count++;
            for (int i = 1; lastRow + i < Rows && lastCol - i >= 0 && grid[lastRow + i, lastCol - i] == playerId; i++) count++;
            if (count >= 4) return true;

            return false;
        }
    }
}
