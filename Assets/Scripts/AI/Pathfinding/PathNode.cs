
namespace DQWorks.AI.Pathfinding
{
    public class PathNode
    {
        #region Getters and setters
        public int CostF => CostG + CostH;
        public int CostG { get; set; } = 0;
        public int CostH { get; set; } = 0;
        public GridNode GridNode { get; private set; }
        public PathNode ParentNode { get; set; } = null;
        #endregion

        #region Operators
        public bool Equals(PathNode pathNode) => GridNode.Equals(pathNode.GridNode);
        public bool Equals(GridNode gridNode) => GridNode.Equals(gridNode);
        #endregion

        public PathNode(GridNode node)
        {
            CostG = 0; CostH = 0;
            GridNode = node;
        }
    }
}
