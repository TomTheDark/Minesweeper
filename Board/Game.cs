///
/// ETML
/// Author: GT FID1
/// Date: 19.03.26
///
/// Minesweeper
///

using System;
using Minesweeper.UI;

namespace Minesweeper.Board
{
    /// <summary>
    /// Manages the global flow and game logic
    /// </summary>
    internal class Game
    {
        private const int MIN_ROWS = 6, MAX_ROWS = 30;
        private const int MIN_COLUMNS = 6, MAX_COLUMNS = 30;

        private Grid board;
        private Player player1;
        private Player player2;
        private Player currentPlayer;
        private Menu menu;

        public Game()
        {
            menu = new Menu();

            player1 = new Player(1, "Player 1", ConsoleColor.Red);
            player2 = new Player(2, "Player 2", ConsoleColor.Yellow);
            currentPlayer = player1;
        }

        public void Start()
        {
            menu.DisplayTitle();
            menu.DisplayCompatibilityWarning();

            int rows = menu.AskInteger("Please enter the number of rows", MIN_ROWS, MAX_ROWS);
            int cols = menu.AskInteger("Please enter the number of columns", MIN_COLUMNS, MAX_COLUMNS);
            
            // Difficulty selection
            Console.WriteLine("   Please enter the difficulty for your game");
            Console.WriteLine("   knowing that :");
            Console.WriteLine("      1 --> Easy");
            Console.WriteLine("      2 --> Medium");
            Console.WriteLine("      3 --> Hard");
            int difficulty = menu.AskInteger("Your difficulty :", 1, 3);

            // Calculate mines
            // Formula : Surface = (rows / 2) + 1 * (cols / 2) + 1
            // This formula from the image seems to mean: ((rows / 2) + 1) * ((cols / 2) + 1)
            int surface = ((rows / 2) + 1) * ((cols / 2) + 1);
            double percentage = 0;
            string difficultyName = "";

            switch (difficulty)
            {
                case 1:
                    percentage = 0.10;
                    difficultyName = "Easy";
                    break;
                case 2:
                    percentage = 0.25;
                    difficultyName = "Medium";
                    break;
                case 3:
                    percentage = 0.40;
                    difficultyName = "Hard";
                    break;
            }

            int mines = (int)Math.Floor(surface * percentage);

            board = new Grid(rows, cols);

            Console.Clear();
            menu.DisplayTitle();
            menu.DisplayMineInfo(mines, difficultyName);
            board.Draw();
            menu.DisplayHelper(cols);

            PlayLoop();
        }

        private void PlayLoop()
        {
            bool gameEnded = false;

            while (!gameEnded)
            {
                gameEnded = HandleTurn();
                if (!gameEnded)
                {
                    SwitchPlayer();
                }
            }
        }

        private bool HandleTurn()
        {
            const byte STEP_LATERAL = 4;
            const byte STEP_VERTICAL = 2;
            int basePosLeft = 10;
            int basePosTop = 7;
            int minLeft = basePosLeft;
            int maxRight = basePosLeft + (board.Columns - 1) * STEP_LATERAL;
            int currentColumn = 0;

            Console.CursorVisible = false;
            Console.SetCursorPosition(basePosLeft, basePosTop);
            currentPlayer.DrawPawn();

            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);
                Console.SetCursorPosition(basePosLeft, basePosTop);
                Console.Write(" "); // Delete old cursor

                switch (keyInfo.Key)
                {
                    case ConsoleKey.RightArrow:
                        if (basePosLeft < maxRight) { basePosLeft += STEP_LATERAL; currentColumn++; }
                        else { basePosLeft = minLeft; currentColumn = 0; }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (basePosLeft > minLeft) { basePosLeft -= STEP_LATERAL; currentColumn--; }
                        else { basePosLeft = maxRight; currentColumn = board.Columns - 1; }
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                }

                Console.SetCursorPosition(basePosLeft, basePosTop);
                currentPlayer.DrawPawn();

            } while (keyInfo.Key != ConsoleKey.Spacebar && keyInfo.Key != ConsoleKey.Enter);

            int targetRow = board.GetLowestAvailableRow(currentColumn);

            if (targetRow == -1) // Column full
            {
                Console.SetCursorPosition(basePosLeft, basePosTop);
                Console.Write(" ");
                return false; // The turn is not consumed
            }

            // Delete top cursor
            Console.SetCursorPosition(basePosLeft, basePosTop);
            Console.Write(" ");

            // Place the pawn graphically and logically
            int dropPosTop = 11 + targetRow * STEP_VERTICAL;
            Console.SetCursorPosition(basePosLeft, dropPosTop);
            currentPlayer.DrawPawn();
            board.PlacePawn(targetRow, currentColumn, currentPlayer.Id);
            currentPlayer.MovesCount++;

            // Check victory
            if (board.CheckVictory(targetRow, currentColumn, currentPlayer.Id))
            {
                HandleVictory();
                return true;
            }

            return false;
        }

        private void SwitchPlayer()
        {
            currentPlayer = (currentPlayer == player1) ? player2 : player1;
        }

        private void HandleVictory()
        {
            Console.SetCursorPosition(0, board.Rows * 2 + 15);
            menu.DisplayVictory(currentPlayer);

            if (menu.AskReplay())
            {
                 // Reset to replay
                 currentPlayer = player1;
                 player1.MovesCount = 0;
                 player2.MovesCount = 0;
                 Start();
            }
            else
            {
                 Environment.Exit(0);
            }
        }
    }
}
