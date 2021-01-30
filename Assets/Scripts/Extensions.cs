using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Extensions
{
    public static class ExtensionMethods
    {
        public static bool In2DArrayBounds(this object[,] array, Vector2Int cell)
        {
            if (cell.x < array.GetLowerBound(0) ||
                cell.x > array.GetUpperBound(0) ||
                cell.y < array.GetLowerBound(1) ||
                cell.y > array.GetUpperBound(1)) return false;
            return true;
        }
    }
}
