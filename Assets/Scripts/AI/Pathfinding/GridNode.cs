using UnityEngine;

namespace DQWorks.AI.Pathfinding
{
    public struct GridNode
    {
        public Vector2Int GridPosition { get; private set; }
        public bool Walkable { get; set; }

        public GridNode(int x, int y) { GridPosition = new Vector2Int(x, y); Walkable = true; }
    }
}