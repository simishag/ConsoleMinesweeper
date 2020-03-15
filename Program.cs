using System;
using System.Text.RegularExpressions;

namespace ConsoleMinesweeper
{
    class Program
    {
        static readonly int BOARD_SIZE = 15;
        static readonly int MINE_COUNT = 15;

        static void Main(string[] args)
        {
            // initialize board
            var board = new int[BOARD_SIZE, BOARD_SIZE];
            var showBoard = new string[BOARD_SIZE, BOARD_SIZE];

            for (int i = 0; i < BOARD_SIZE; i++)
            {
                for (int j = 0; j < BOARD_SIZE; j++)
                {
                    board[i, j] = 0;
                    showBoard[i, j] = ".";
                }
            }

            // plant mines
            var rand = new Random();
            for (int i = 0; i < MINE_COUNT; i++)
            {
                var x = rand.Next(BOARD_SIZE);
                var y = rand.Next(BOARD_SIZE);
                board[x, y] = 1;
                showBoard[x, y] = "*";
            }

            string message = "";
            string message2 = "";
            Console.WriteLine("Hello World!");
            while (true)
            {
                Console.Clear();

                // draw top row
                Console.Write("   ");
                for (int i = 0; i < BOARD_SIZE; i++)
                {
                    Console.Write($"{i:D2} ");
                }
                Console.Write($"\n\n");

                // draw current board
                for (int i = 0; i < BOARD_SIZE; i++)
                {
                    Console.Write($"{i:D2}  ");
                    for (int j = 0; j < BOARD_SIZE; j++)
                    {
                        Console.Write($"{showBoard[i, j]}  ");
                    }
                    Console.Write($"\n");
                }

                // write out some messages
                Console.WriteLine();
                Console.WriteLine($"You entered: {message}");
                Console.WriteLine($"message2: {message2}");
                Console.WriteLine();
                message = "";
                message2 = "";

                // get & parse next move
                Console.Write("Enter as row, column: ");
                var input = Console.ReadLine();
                var pattern = @"^\d{2},\d{2}$";
                var mc = Regex.Matches(input, pattern);
                if (mc.Count == 0)
                {
                    message = "Invalid input";
                    continue;
                }

                Match m = mc[0];
                var elements = Regex.Split(m.Value, ",");
                var x = Int32.Parse(elements[0]);
                var y = Int32.Parse(elements[1]);
                message += $"{x},{y}";

                // process move
                if (board[x, y] == 1)
                {
                    message += " KABOOM!";
                    // game over
                    //break;
                }
                else
                {
                    message += " Lucky this time...";
                    var squareCount = DoSquareCount(board, x, y, BOARD_SIZE);
                    var squareOut = " ";
                    if (squareCount > 0)
                    {
                        squareOut = squareCount.ToString();
                    }
                    showBoard[x, y] = $"{squareOut}";

                    // calculate for any 0 squares
                    for (int i = 0; i < BOARD_SIZE; i++)
                    {
                        for (int j = 0; j < BOARD_SIZE; j++)
                        {
                            if (string.IsNullOrWhiteSpace(showBoard[i, j]))
                            {
                                // need to find surrounding squares
                                for (int a = i - 1; a <= i + 1; a++)
                                {
                                    if (a < 0 || a >= BOARD_SIZE)
                                    {
                                        continue;
                                    }
                                    for (int b = j - 1; b <= j + 1; b++)
                                    {
                                        if (b < 0 || b >= BOARD_SIZE)
                                        {
                                            continue;
                                        }
                                        //message2 += $"{a},{b} ";
                                        squareCount = DoSquareCount(board, a, b, BOARD_SIZE);
                                        squareOut = " ";
                                        if (squareCount > 0)
                                        {
                                            squareOut = squareCount.ToString();
                                        }
                                        showBoard[a, b] = $"{squareOut}";
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }

        static int DoSquareCount(int[,] board, int x, int y, int boardSize)
        {
            // compute count for this square
            int squareCount = 0;
            for (int i = x - 1; i <= x + 1; i++)
            {
                if (i < 0 || i >= boardSize)
                {
                    continue;
                }
                for (int j = y - 1; j <= y + 1; j++)
                {
                    if (j < 0 || j >= boardSize)
                    {
                        continue;
                    }
                    //message2 += $"{i},{j} ";
                    if (board[i, j] == 1)
                    {
                        squareCount++;
                    }
                }
            }
            return squareCount;
        }
    }
}
