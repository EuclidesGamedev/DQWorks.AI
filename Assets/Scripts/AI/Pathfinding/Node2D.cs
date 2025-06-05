using UnityEngine;

namespace Assets.Scripts.AI.Pathfinding
{
    public struct Node2D
    {
        public Vector2Int GridPosition { get; private set; }
        public bool Walkable { get; private set; }

        public Node2D(int x, int y) { GridPosition = new Vector2Int(x, y); Walkable = true; }
    }
}