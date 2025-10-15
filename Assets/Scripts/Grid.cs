using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scripts
{
    public class Grid<T>
    {
        /// <summary>
        /// The size of the grid in Vector2Int representation.
        /// </summary>
        public Vector2Int size { get; }

        /// <summary>
        /// Width of the grid
        /// </summary>
        public int w => size.x;
        
        /// <summary>
        /// Height of the grid
        /// </summary>
        public int h => size.y;

        /// <summary>
        /// The data structure containing elements of type T.
        /// </summary>
        public T[,] grid;
        
        /// <summary>
        /// A pointer to a grid cell.
        /// </summary>
        public Vector2Int cursor
        {
            get => m_Cursor;
            set
            {
                if (m_Cursor != value)
                {
                    cursorRelativePath.Add(value - m_Cursor);
                    cursorPath.Add(value);
                    m_Cursor = value;
                }
            }
        }

        /// <summary>
        /// Returns a value at cursor position.
        /// </summary>
        public T cursorCell
        {
            get => grid[cursor.x, cursor.y];
            set => grid[cursor.x, cursor.y] = value;
        }

        /// <summary>
        /// A private cursor member
        /// </summary>
        private Vector2Int m_Cursor = default;
        
        /// <summary>
        /// The cursors relative path. Can be retraced/backtraced in steps.
        /// </summary>
        public List<Vector2Int> cursorRelativePath = new();

        /// <summary>
        /// The cursor's path. Holds a stack of the exact steps.
        /// </summary>
        public List<Vector2Int> cursorPath = new();
        
        public Grid(Vector2Int size)
        {
            this.size = size;
            this.grid = new T[size.x, size.y];
            Initialize();
        }

        /// <summary>
        /// A delegate action intended for a grid cell.
        /// </summary>
        public delegate void WalkAction(int x, int y);

        /// <summary>
        /// A sequential algorithm that executes on each element in a row, and every row, respectively.
        /// </summary>
        /// <param name="walkAction">Lambda function to execute on each element.</param>
        public void Walk(WalkAction walkAction)
        {
            for (int j = 0; j < size.x; j++)
                for (int i = 0; i < size.y; i++)
                    walkAction(i, j);
        }

        /// <summary>
        /// Sets the cursor to the center of the grid.
        /// </summary>
        public void CenterCursor()
        {
            m_Cursor = new Vector2Int(
                Mathf.FloorToInt((float)w / 2), 
                Mathf.FloorToInt((float)h / 2)
            );
            
            cursorPath.Add(m_Cursor);
        }

        /// <summary>
        /// Apply a value at cursor.
        /// </summary>
        /// <param name="value">Value of type T</param>
        public void Reduce(T value)
        {
            grid[cursor.x, cursor.y] = value;
        }
        
        /// <summary>
        /// Alias for the grid data structure access.
        /// </summary>
        /// <param name="x">X position in a grid</param>
        /// <param name="y">Y position in a grid</param>
        public T this[int x, int y]
        {
            get => grid[x, y];
            set => grid[x, y] = value;
        }

        /// <summary>
        /// Alias for grid access
        /// </summary>
        /// <param name="position">Vector2Int position</param>
        public T this[Vector2Int position]
        {
            get => grid[position.x, position.y];
            set => grid[position.x, position.y] = value;
        }
        
        /// <summary>
        /// Returns neighbors of a cell at given (x, y) position.
        /// </summary>
        /// <param name="x">X position on the grid</param>
        /// <param name="y">Y position on the grid</param>
        /// <returns>List of neighbors of type T.</returns>
        public List<T> GetNeighbors(int x, int y)
        {
            List<(int x, int y)> directions = new()
            {
                (1, 0),  // right
                (-1, 0), // left
                (0, 1),  // up
                (0, -1)  // down
            };

            var neighbors = (from direction in directions
                let newX = x + direction.x
                let newY = y + direction.y
                where newX >= 0 && newX < w && newY >= 0 && newY < h
                select grid[newX, newY]).ToList();

            return neighbors;
        }
        
        /// <summary>
        /// Returns a List of neighbors of type T to the given cell position.
        /// </summary>
        /// <param name="position">Position on the grid</param>
        /// <returns>List of neighbors</returns>
        public List<T> GetNeighbors(Vector2Int position) => GetNeighbors(position.x, position.y);

        public List<Vector2Int> GetNeighborCells(int x, int y)
        {
            List<(int x, int y)> directions = new()
            {
                (1, 0),  // right
                (-1, 0), // left
                (0, 1),  // up
                (0, -1)  // down
            };
            
            var neighborCells = from direction in directions
                let newX = x + direction.x
                let newY = y + direction.y
                where newX >= 0 && newX < w && newY >= 0 && newY < h
                select new Vector2Int(newX, newY);
            
            return neighborCells.ToList();
        }
        
        public List<Vector2Int> GetNeighborCells(Vector2Int position) => GetNeighborCells(position.x, position.y);

        /// <summary>
        /// Gets neighbors that are null
        /// </summary>
        /// <param name="x">X position on the grid</param>
        /// <param name="y">Y position on the grid</param>
        /// <returns>List of neighbors that are null of type T</returns>
        public List<Vector2Int> GetNullNeighbors(int x, int y) => 
            GetNeighborCells(x, y).Where(cell => grid[cell.x, cell.y] == null).ToList();
        
        /// <summary>
        /// Override that takes Vector2Int as an input
        /// </summary>
        /// <param name="position">Position on a grid</param>
        /// <returns>List of neighbors that are null of type T</returns>
        public List<Vector2Int> GetNullNeighbors(Vector2Int position) => 
            GetNullNeighbors(position.x, position.y);
        
        /// <summary>
        /// Initializes every cell of the grid data structure with a default value.
        /// </summary>
        private void Initialize()
        {
            Walk((x, y) => grid[x, y] = default);
        }

        /// <summary>
        /// Initializes every cell of the grid data structure with a value.
        /// </summary>
        /// <param name="value">Value of type T</param>
        public void Initialize(T value)
        {
            Walk((x, y) => grid[x, y] = value);
        }

        public string Visualize(Func<T, object> visualizer)
        {
            StringBuilder rows = new StringBuilder("");
            
            for (int j = size.y - 1; j >= 0; j--)
            {
                for (int i = 0; i < size.x; i++)
                {
                    var cellValue = grid[i, j];
                    var cellText = cellValue == null ? "0" : "<color=lime>"+visualizer(cellValue)+"</color>";
                    rows.Append($"{cellText}\t");
                }
                
                rows.Append("\n");
            }
            
            return rows.ToString();
        }
    }
}