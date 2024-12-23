using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UART_SeaBattle.Structs;

namespace UART_SeaBattle.Service
{
    public class ShipsGenerator
    {
        private const int NUMBER_OF_SHIPS = 10; // Количество кораблей
        private const int MIN_SHIP_LENGTH = 1; // Минимальная длина корабля
        private const int MAX_SHIP_LENGTH = 4; // Максимальная длина корабля

        private readonly int tableColsAmount;
        private readonly int tableRowsAmount;

        public ShipsGenerator(int tableColsAmount, int tableRowsAmount) 
        { 
            this.tableColsAmount = tableColsAmount;
            this.tableRowsAmount = tableRowsAmount;
        }

        public ShipCoordinates[] GenerateShips()
        {
            var random = new Random();
            var ships = new List<ShipCoordinates>();
            var occupiedCells = new bool[tableColsAmount, tableRowsAmount];

            for (int i = 0; i < NUMBER_OF_SHIPS; i++)
            {
                bool shipPlaced = false;

                int maxShipLength = MAX_SHIP_LENGTH + 1;
                while (!shipPlaced)
                {
                    int shipLength = random.Next(MIN_SHIP_LENGTH, maxShipLength);
                    int row = random.Next(0, tableRowsAmount);
                    int col = random.Next(0, tableColsAmount - shipLength);

                    // Проверяем, достаточно ли места для корабля и отступа в одну клетку
                    if (CanPlaceShip(occupiedCells, row, col, shipLength))
                    {
                        // Добавляем корабль
                        ships.Add(new ShipCoordinates
                        {
                            FirstSection = new Point(col, row),
                            LastSection = new Point(col + shipLength - 1, row)
                        });

                        // Помечаем клетки как занятые
                        MarkOccupiedCells(occupiedCells, row, col, shipLength);
                        shipPlaced = true;
                    }
                    else
                    {
                        if (maxShipLength != 2)
                        {
                            maxShipLength--;
                        }                        
                    }
                }
            }

            return ships.ToArray();
        }

        private bool CanPlaceShip(bool[,] occupiedCells, int row, int col, int length)
        {
            for (int r = Math.Max(0, row - 1); r <= Math.Min(tableRowsAmount - 1, row + 1); r++)
            {
                for (int c = Math.Max(0, col - 1); c <= Math.Min(tableColsAmount - 1, col + length); c++)
                {
                    if (occupiedCells[c, r]) return false;
                }
            }
            return true;
        }

        private void MarkOccupiedCells(bool[,] occupiedCells, int row, int col, int length)
        {
            for (int r = Math.Max(0, row - 1); r <= Math.Min(tableRowsAmount - 1, row + 1); r++)
            {
                for (int c = Math.Max(0, col - 1); c <= Math.Min(tableColsAmount - 1, col + length); c++)
                {
                    occupiedCells[c, r] = true;
                }
            }
        }
    }
}
