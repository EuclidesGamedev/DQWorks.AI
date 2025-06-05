using UnityEngine;

namespace Assets.Scripts.AI.Pathfinding
{
    public struct GridNode
    {
        public Vector2Int GridPosition { get; private set; }
        public bool Walkable { get; set; }

        public GridNode(int x, int y) { GridPosition = new Vector2Int(x, y); Walkable = true; }
    }

    public struct PathNode
    {
        public int CostF => CostG + CostH;
        public int CostG { get; set; }
        public int CostH { get; set; }
        public GridNode CurrentNode { get; private set; }
        public GridNode? ParentNode { get; private set; }

        public PathNode(GridNode node)
        {
            CurrentNode = node;
            CostG = 0;
            CostH = 0;
            ParentNode = null;
        }

        public void SetParent(GridNode node)
        {
            ParentNode = node;
        }
    }
}