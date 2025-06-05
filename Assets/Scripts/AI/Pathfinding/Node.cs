using UnityEngine;

namespace Assets.Scripts.AI.Pathfinding
{
    public struct Node
    {
        public Vector2Int GridPosition { get; private set; }
        public bool Walkable { get; set; }

        public Node(int x, int y) { GridPosition = new Vector2Int(x, y); Walkable = true; }
    }

    public struct PathNode
    {
        public int CostF => CostG + CostH;
        public int CostG { get; set; }
        public int CostH { get; set; }
        public Node CurrentNode { get; private set; }
        public Node? ParentNode { get; private set; }

        public PathNode(Node node)
        {
            CurrentNode = node;
            CostG = 0;
            CostH = 0;
            ParentNode = null;
        }

        public void SetParent(Node node)
        {
            ParentNode = node;
        }
    }
}