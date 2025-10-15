using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts
{
    public class LevelGenerator : MonoBehaviour
    {
        public TileDataSO tileData;
        public int maxRooms = 5;
        private Grid<Tile> grid = new (new Vector2Int(10, 10));
        private Tile startingTile;
        
        private void Start()
        {
            grid.CenterCursor();
            InitializeLevel();
            ConnectRooms();
            CreateTiles();
            
            Debug.Log(grid.Visualize(cell => cell.type.ToString()
                .Replace("Down", "D")
                .Replace("Left", "L")
                .Replace("Up", "U")
                .Replace("Right", "R")
            ));
        }

        private void CreateTiles()
        {
            grid.cursorPath.ForEach(pos =>
            {
                var prefab = tileData.GetGameObject(grid[pos].type);
                var position = (new Vector2(pos.x, pos.y) - new Vector2(5, 5)) * tileData.sizeUnits;
                Instantiate(prefab, position, Quaternion.identity);
            });
        }
        
        private void ConnectRooms()
        {
            var tempCursor = startingTile.gridPosition;
            foreach (var pathPoint in grid.cursorRelativePath)
            {
                var adjacentType = TileHelper.Get(pathPoint);
                grid[tempCursor].type |= adjacentType;
                grid[tempCursor + pathPoint].type |= TileHelper.Get(-pathPoint);
                tempCursor += pathPoint;
            }
        }

        private void InitializeLevel()
        {
            startingTile = grid.cursorCell = new Tile(grid.cursor);
            for (int i = 0; i < maxRooms; i++)
            {
                var randomNeighborPosition = grid.GetNullNeighbors(grid.cursor).GetRandom();
                grid[randomNeighborPosition] = new Tile(randomNeighborPosition);
                grid.cursor = randomNeighborPosition;
            }
        }

        private void AppendNeighbor(Vector2Int current)
        {
            var nullNeighbors = grid.GetNullNeighbors(current);
            if (nullNeighbors.Count < 1) return;
            
            var randomNeighborPosition = nullNeighbors.GetRandom();
            grid[randomNeighborPosition] = new Tile(randomNeighborPosition);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }
}
