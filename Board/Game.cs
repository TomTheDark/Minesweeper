using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Minesweeper.Board;

namespace Minesweeper.Board
{
    /// <summary>
    /// Gère le flux global et la logique du jeu
    /// </summary>
    internal class Game
    {
        private const byte MIN_ROWS = 6, MAX_ROWS = 30;
        private const byte MIN_COLUMNS = 6, MAX_COLUMNS = 30;
        private const byte WIN_LENGTH = 200, WIN_HEIGHT = 60;

        private Grid board;
        private Player player1;
        private Player player2;
        private Player currentPlayer;

        public Game()
        {
            {
                Console.SetWindowSize(WIN_LENGTH, WIN_HEIGHT);
            }

            player1 = new Player(1, "Joueur 1", ConsoleColor.Red);
            player2 = new Player(2, "Joueur 2", ConsoleColor.Yellow);
            currentPlayer = player1;
        }

        public void Start()
        {
            DisplayTitle();
            int rows = AskParameter("lignes", MIN_ROWS, MAX_ROWS);
            int cols = AskParameter("colonnes", MIN_COLUMNS, MAX_COLUMNS);

            board = new Grid(rows, cols);

            Console.Clear();
            DisplayTitle();
            board.Draw();
            DisplayHelper();

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
                Console.Write(" "); // Efface l'ancien curseur

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

            if (targetRow == -1) // Colonne pleine
            {
                Console.SetCursorPosition(basePosLeft, basePosTop);
                Console.Write(" ");
                return false; // Le tour n'est pas consommé
            }

            // Efface le curseur du haut
            Console.SetCursorPosition(basePosLeft, basePosTop);
            Console.Write(" ");

            // Place le pion graphiquement et logiquement
            int dropPosTop = 11 + targetRow * STEP_VERTICAL;
            Console.SetCursorPosition(basePosLeft, dropPosTop);
            currentPlayer.DrawPawn();
            board.PlacePawn(targetRow, currentColumn, currentPlayer.Id);
            currentPlayer.MovesCount++;

            // Vérifie la victoire
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
            Console.ForegroundColor = currentPlayer.Color;
            Console.WriteLine($"\n   Bravo! Le {currentPlayer.Name} a gagné en {currentPlayer.MovesCount} coups!");
            Console.ResetColor();

            Console.WriteLine("\n   Voulez-vous recommencer? (O / N):");
            ConsoleKeyInfo restart;
            do
            {
                restart = Console.ReadKey(true);
                if (restart.Key == ConsoleKey.O)
                {
                    Console.Clear();
                    // Réinitialisation pour rejouer
                    currentPlayer = player1;
                    player1.MovesCount = 0;
                    player2.MovesCount = 0;
                    Start();
                    return;
                }
                else if (restart.Key == ConsoleKey.N)
                {
                    Console.WriteLine("\n   Merci d'avoir joué. Appuyez sur une touche pour quitter.");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            } while (true);
        }

        private void DisplayTitle()
        {
            Console.WriteLine("\n   ╔════════════════════════════════════════╗");
            Console.WriteLine("   ║    Bienvenue dans le jeu Puissance 4   ║");
            Console.WriteLine("   ║    Réalisé par Thibault Gugler         ║");
            Console.WriteLine("   ╚════════════════════════════════════════╝\n");
        }

        private int AskParameter(string paramName, int min, int max)
        {
            int value;
            bool isValid;

            if (paramName == "lignes")
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write($"   !! Avant de jouer, si vous utilisez Windows 11\n   !! vérifiez que vous utilisez bien l'Hôte de la console Windows\n   !! ou que la taille de votre console soit suffisante\n\n");
                Console.ResetColor();
            }

            do
            {
                Console.Write($"   Merci d'entrer le nombre de {paramName}\n   La valeur doit être plus grande que ");
                Console.ForegroundColor = ConsoleColor.Red; Console.Write(min); Console.ResetColor();
                Console.Write(" et plus petite que ");
                Console.ForegroundColor = ConsoleColor.Red; Console.Write(max + "\n"); Console.ResetColor();
                Console.Write("   Votre valeur : ");

                isValid = int.TryParse(Console.ReadLine(), out value);

                if (!isValid)
                    Console.WriteLine("\n   Votre valeur n'est pas un nombre ! Merci de réessayer !\n");
                else if (value < min || value > max)
                    Console.WriteLine($"\n   Votre valeur n'est pas dans les limites fixées > {min} et < {max}, Merci de réessayer !\n");

            } while (!isValid || value < min || value > max);

            Console.WriteLine();
            return value;
        }

        private void DisplayHelper()
        {
            byte basePosLeft = 14;
            const byte STEP = 4;
            byte startY = 6;
            int offsetX = basePosLeft + (board.Columns) * STEP;

            Console.SetCursorPosition(offsetX, startY++); Console.Write("Mode d'utilisation");
            Console.SetCursorPosition(offsetX, startY++); Console.Write("------------------");
            Console.SetCursorPosition(offsetX, startY++); Console.Write("\tDéplacement ->\tTouches directionnelles");
            Console.SetCursorPosition(offsetX, startY++); Console.Write("\tJouer ->\tSpacebar ou Enter");
            Console.SetCursorPosition(offsetX, startY++); Console.Write("\tQuitter ->\tEscape");
            Console.SetCursorPosition(offsetX, startY++);
            Console.ForegroundColor = player1.Color; Console.Write($"\t{player1.Name}: █");
            Console.ForegroundColor = player2.Color; Console.Write($"\t{player2.Name}: █");
            Console.ResetColor();
        }
    }
}
