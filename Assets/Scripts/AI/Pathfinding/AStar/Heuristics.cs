using UnityEngine;

namespace Assets.Scripts.AI.Pathfinding.AStar
{
    public static class Heuristics
    {
        public static int CalculateF() { throw new System.NotImplementedException(); }
        public static int CalculateG() { throw new System.NotImplementedException(); }
        public static int CalculateH(Node2D node1, Node2D node2)
        {
            Vector2Int diffVector = new Vector2Int(
                Mathf.Abs(node1.GridPosition.x - node2.GridPosition.x),
                Mathf.Abs(node1.GridPosition.y - node2.GridPosition.y)
            );

            if (diffVector.x > diffVector.y)
                return 14 * (diffVector.y) + 10 * (diffVector.x - diffVector.y);
            return 14 * (diffVector.x) + 10 * (diffVector.y - diffVector.x);
        }
    }
}