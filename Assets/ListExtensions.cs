using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public static class ListExtensions {
        public static T GetRandom<T>(this List<T> list)
        {
            return (list.Count > 0) ? 
                list[Random.Range(0, list.Count)] 
                : default(T);
        }
    }
}