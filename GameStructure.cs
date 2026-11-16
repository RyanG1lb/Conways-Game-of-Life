using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yellow_Professional_Conway_Exercise
{
    public class GameStructure
    {
        public GameStructure()
        {
            Console.WriteLine("Enter the number of rows between 5 and 30"); //---------------------------------
            string r = Console.ReadLine();
            int rows;
            int columns;
            while (int.TryParse(r, out rows) == false || rows < 5 || rows > 30 || r == null)
            {
                Console.WriteLine("Enter the number of rows between 5 and 30");
                r = Console.ReadLine();
            }                                                                               //Logic to get grid size
            Console.WriteLine("Enter the number of columns between 5 and 30");
            string c = Console.ReadLine();
            while (int.TryParse(c, out columns) == false || columns < 5 || columns > 30 || c == null)
            {
                Console.WriteLine("Enter the number of columns between 5 and 30");
                c = Console.ReadLine();
            }                                                               //----------------------------------

            //Console.Clear();
            int[,] grid = new int[rows, columns];

            StartingPattern(grid);  //Forming the initial pattern with user input from 3 choices

            Console.WriteLine("How many generations would you like to go over?");
            string gI = Console.ReadLine();
            int generationInput;
            while (int.TryParse(gI, out generationInput) == false || generationInput < 1 || gI == null)
            {
                Console.WriteLine("How many generations would you like to go over?");
                gI = Console.ReadLine();
            }
            //Console.Clear();

            PrintGrid(NextGeneration(grid, generationInput));           //Iterates through user inputted generations and prints final generation
        }

        private void StartingPattern(int[,] grid)   //Method to create the initial pattern. 2 & 3 are from the wiki patterns
        {
            Console.WriteLine("Starting Pattern?:1 - Random    2 - Blinker    3 - Glider");
            int choice;
            while (int.TryParse(Console.ReadLine(), out choice) == false || choice < 1 || choice > 3)
            {
                int.TryParse(Console.ReadLine(), out choice);
            }

            if (choice == 1)
            {
                for (int r = 0; r < grid.GetLength(0); r++)
                {
                    for (int c = 0; c < grid.GetLength(1); c++)
                    {
                        int randomNumber = Random.Shared.Next(0, 2);
                        //Console.WriteLine("Random Number is:" + randomNumber);
                        grid[r, c] = randomNumber;
                    }
                }
            }

            else if (choice == 2)
            {
                int halfRows = grid.GetLength(0) / 2 - 1;
                int halfCols = grid.GetLength(1) / 2 - 1;
                (int, int)[] positions = { (halfRows, halfCols - 1), (halfRows, halfCols), (halfRows, halfCols + 1) };
                for (int p = 0; p < positions.Length; p++)
                {
                        grid[positions[p].Item1, positions[p].Item2] = 1;
                }
            }

            else if (choice == 3)
            {
                int halfRows = grid.GetLength(0) / 2 - 1;
                int halfCols = grid.GetLength(1) / 2 - 1;
                (int, int)[] positions = { (halfRows, halfCols), (halfRows + 1, halfCols + 1), (halfRows + 2, halfCols - 1), (halfRows + 2, halfCols), (halfRows + 2, halfCols + 1) };

                for (int p = 0; p < positions.Length; p++)
                {
                    grid[positions[p].Item1, positions[p].Item2] = 1;
                }
                        //if(row == grid.GetLength(0) / 2 && col == grid.GetLength(1) / 2)
                        //{
                        //    grid[row, col] = 1;
                        //}
            }
            Console.WriteLine("Starting Pattern is:");
            PrintGrid(grid);                        //Nicer to show pattern before next generation
        }

        private int CheckNeighbours(int[,] grid, int row, int col)  //counts the amounts of neighbours within a 3x3 grid around the current cell
        {
            int neighbourCount = 0;

            for (int r = row - 1; r <= row + 1; r++)
            {
                for (int c = col - 1; c <= col + 1; c++)
                {
                    if (r < 0 || r >= grid.GetLength(0) || c < 0 || c >= grid.GetLength(1) || grid[r, c] == 0 || r == row && c == col)
                    {
                        continue;
                    }
                    else
                    {
                        neighbourCount += 1;
                    }
                }
            }

            return neighbourCount;
        }

        private int[,] NextGeneration(int[,] grid, int GenerationNumber)    //Iterates the generations
        {

            //int[,] nextGenGrid = new int[grid.GetLength(0), grid.GetLength(1)];
            //int[,] nextGenGrid = grid;
            int[,] newestGenerationGrid = grid;

            for (int genNum = 0; genNum < GenerationNumber; genNum++)           //Iterating through number of user selected generations
            {

                int[,] nextGenGrid = new int[grid.GetLength(0), grid.GetLength(1)];//New grid to stop each cell change affecting the next cell change. Freshh grid should stop next generation issues

                for (int r = 0; r < nextGenGrid.GetLength(0); r++)
                {
                    for (int c = 0; c < nextGenGrid.GetLength(1); c++)
                    {
                        int neighbours = CheckNeighbours(newestGenerationGrid, r, c);
                        //Rules
                        //Alive
                        if (newestGenerationGrid[r, c] == 1)
                        {
                            if (neighbours < 2)
                            {
                                nextGenGrid[r, c] = 0;
                            }
                            else if (neighbours == 2 || neighbours == 3)
                            {
                                nextGenGrid[r, c] = 1;
                            }
                            else if (neighbours > 3)
                            {
                                nextGenGrid[r, c] = 0;
                            }
                        }
                        //Dead
                        else
                        {
                            if (neighbours == 3)
                            {
                                nextGenGrid[r, c] = 1;
                            }
                        }
                    }
                }
                newestGenerationGrid = nextGenGrid;         //copying over the newest grid as reference before nextGenGrid is wiped

            }

            return newestGenerationGrid;
        }

        public void PrintGrid(int[,] grid)
        {
            for (int row = 0; row < grid.GetLength(0); row++)
            {
                for (int col = 0; col < grid.GetLength(1); col++)
                {
                    if (grid[row, col] == 1)
                    {
                        Console.Write("*");
                    }
                    else
                    {
                        Console.Write("-");
                    }
                }
                Console.WriteLine();
            }
        }

    }
}
