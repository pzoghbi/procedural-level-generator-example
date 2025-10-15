using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public static class TileHelper
    {
        private static Dictionary<Vector2Int, TileType> map = new()
        {
            { new Vector2Int {x = 1, y = 0}, TileType.Right },
            { new Vector2Int {x = -1, y = 0}, TileType.Left},
            { new Vector2Int {x = 0, y = 1}, TileType.Up},
            { new Vector2Int {x = 0, y = -1}, TileType.Down},
        };
        
        public static List<TileType> startTiles = new()
        {
            TileType.Down | TileType.Left,
            TileType.Down | TileType.Right,
            TileType.Down | TileType.Up,
            TileType.Down | TileType.Left | TileType.Right,
            TileType.Down | TileType.Left | TileType.Up,
            TileType.Left | TileType.Right,
            TileType.Left | TileType.Up,
            TileType.Left | TileType.Right | TileType.Up,
            TileType.Right | TileType.Left | TileType.Up
        };
        
        public static TileType Get(Vector2Int position) => map[position];
    }
}