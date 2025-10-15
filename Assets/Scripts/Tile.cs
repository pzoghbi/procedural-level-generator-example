using UnityEngine;

namespace Scripts
{
    public class Tile
    {
        public TileType type = TileType.None;
        public Vector2Int gridPosition { get; set; } = new Vector2Int();
        public TileType possibleType = TileType.None;

        public Tile(Vector2Int gridPosition)
        {
            this.gridPosition = gridPosition;
        }
    }
}