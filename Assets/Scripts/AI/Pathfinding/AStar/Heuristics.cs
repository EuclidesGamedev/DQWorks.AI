using UnityEngine;

namespace DQWorks.AI.Pathfinding.AStar
{
    public static class Heuristics
    {
        public static int CalculateG(PathNode node1, PathNode node2) => CalculateG(node1, node2.GridNode);
        public static int CalculateG(PathNode node1, GridNode node2) {
            return node1.CostG + CalculateH(node1.GridNode, node2);
        }
        public static int CalculateH(GridNode node1, GridNode node2)
        {
            Vector2Int diffVector = new Vector2Int(
                Mathf.Abs(node1.GridPosition.x - node2.GridPosition.x),
                Mathf.Abs(node1.GridPosition.y - node2.GridPosition.y)
            );

            if (diffVector.x > diffVector.y)
                return 14 * diffVector.y + 10 * (diffVector.x - diffVector.y);
            return 14 * diffVector.x + 10 * (diffVector.y - diffVector.x);
        }
    }
}