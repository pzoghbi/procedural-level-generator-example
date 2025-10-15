using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    [CreateAssetMenu(fileName = "TileData_", menuName = "New Tile Data", order = 0)]
    public class TileDataSO : ScriptableObject
    {
        public float sizeUnits = 10;
        [SerializeField] private List<TileData> tileData;
        private Dictionary<TileType, GameObject> tiles;

        [System.Serializable]
        public class TileData
        {
            public TileType type;
            public GameObject gameObject;
        }
        
        public GameObject GetGameObject(TileType type)
        {
            // Lazy initialization of the dictionary
            if (tiles != null) return tiles.GetValueOrDefault(type);
            tiles = new Dictionary<TileType, GameObject>();
            foreach (var data in tileData)
            {
                if (!tiles.ContainsKey(data.type))
                {
                    tiles[data.type] = data.gameObject;
                }
                else
                {
                    Debug.LogWarning($"Duplicate TileType detected: {data.type}. " +
                        $"Only the first occurrence will be used.");
                }
            }

            // Query the dictionary
            return tiles.GetValueOrDefault(type);
        }
    }
}