using System;
using Minesweeper.Board;

namespace Minesweeper.UI
{
    internal class Menu
    {
        private const int WIN_WIDTH = 120;
        private const int WIN_HEIGHT = 40;

        public Menu()
        {
            Console.SetWindowSize(WIN_WIDTH, WIN_HEIGHT);
        }

        public void DisplayTitle()
        {
            Console.WriteLine("\n   ╔════════════════════════════════════════╗\n   ║          Welcome to Minesweeper        ║\n   ║        Made by Thibault Gugler         ║\n   ╚════════════════════════════════════════╝\n");
        }

        public int AskInteger(string prompt, int min, int max)
        {
            int value;
            bool isValid;

            Console.WriteLine($"   {prompt}");
            Console.Write($"   Value must be between ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(min);
            Console.ResetColor();
            Console.Write(" and ");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(max);
            Console.ResetColor();
            Console.WriteLine(" included.");

            do
            {
                Console.Write("   Your value: ");
                isValid = int.TryParse(Console.ReadLine(), out value);

                if (!isValid)
                {
                    Console.WriteLine("\n   Your value is not a number! Please try again!\n");
                }
                else if (value < min || value > max)
                {
                    Console.WriteLine($"\n   Your value is not within the limits > {min} and < {max}, Please try again!\n");
                    isValid = false;
                }
            } while (!isValid);

            Console.WriteLine();
            return value;
        }

        public void DisplayCompatibilityWarning()
        {
             // Warning about compatibility for big boards for "Windows Terminal" in Windows 11
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write($"   !! Before playing, if you use Windows 11\n   !! check that you are using Windows Console Host\n   !! or that your console size is at least {(WIN_HEIGHT)} lines\n   !! to avoid display issues with large boards\n\n");
            Console.ResetColor();
        }

        public void DisplayHelper(int columns)
        {
            byte basePosLeft = 14;          // Base position of the cursor
            const byte STEP = 4;            // Indentation corresponding to the length of a case (x axis)
            const byte MIN_HEIGHT = 6;      // Min height position for the help text
            // const byte MAX_HEIGHT = 12;     // Max height position for the help text

            // Place the cursor on coords and start a text on theses coords
            byte i = MIN_HEIGHT;
            int xPos = basePosLeft + (columns) * STEP;

            Console.SetCursorPosition(xPos, i++);
            Console.Write("How to play");
            Console.SetCursorPosition(xPos, i++);
            Console.Write("-----------");
            Console.SetCursorPosition(xPos, i++);
            Console.Write("\tMove ->\tArrow keys");
            Console.SetCursorPosition(xPos, i++);
            Console.Write("\tPlay ->\tSpacebar or Enter");
            Console.SetCursorPosition(xPos, i++);
            Console.Write("\tQuit ->\tEscape");
            Console.SetCursorPosition(xPos, i++);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("\tPlayer 1: █");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("\tPlayer 2: █");
            Console.ResetColor();
        }

        public void DisplayVictory(Player player)
        {
            Console.ForegroundColor = player.Color;
            Console.WriteLine($"\n   Congratulations! {player.Name} won in {player.MovesCount} moves!");
            Console.ResetColor();
        }

        public bool AskReplay()
        {
            Console.WriteLine("\n   Do you want to play again? (Y / N):");
            ConsoleKeyInfo restart;
            do
            {
                restart = Console.ReadKey(true);
                if (restart.Key == ConsoleKey.Y)
                {
                    Console.Clear();
                    return true;
                }
                else if (restart.Key == ConsoleKey.N)
                {
                    Console.WriteLine("\n   Thanks for playing. Press any key to quit.");
                    Console.ReadKey();
                    return false;
                }
            } while (true);
        }

        public void DisplayMineInfo(int mines, string difficulty)
        {
            Console.Write("   It's your turn !! Mode : ");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.Write(difficulty);
            Console.ResetColor();
            Console.WriteLine();
            
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"   {mines}");
            Console.ResetColor();
            Console.WriteLine(" mines are hidden in the game !\n");
        }
    }
}

