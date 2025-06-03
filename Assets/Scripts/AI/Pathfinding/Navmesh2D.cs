using UnityEngine;

namespace Assets.Scripts.AI.Pathfinding
{
    public class Navmesh2D : MonoBehaviour
    {
        #region Getters and setters]
        [field: Header("Grid settings")]
        [field: SerializeField] public Vector2Int GridSize { get; private set; }
        [field: SerializeField] public Vector2 NodeSize { get; private set; }
        public Vector2 WorldPosition => transform.position;
        public Vector2 WorldSize => GridSize * NodeSize;
        #endregion

        #region MonoBehaviour
        private void OnDrawGizmos()
        {
            RenderNavmesh();
        }
        #endregion

        #region Navmesh2D private methods
        private void RenderNavmesh()
        { 
            for (int x = 0; x < GridSize.x; x++)
            for (int y = 0; y < GridSize.y; y++)
            {
                    Vector2 position = new Vector2(
                        WorldPosition.x + NodeSize.x * x - WorldSize.x / 2 + NodeSize.x / 2,
                        WorldPosition.y + NodeSize.y * y - WorldSize.y / 2 + NodeSize.y / 2
                    );
                        
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(
                        position, NodeSize
                    );
            }
        }
        #endregion

        #region Navmesh2D public methods
        public Node2D[] GetNeighbors(Node2D node) { throw new System.NotImplementedException(); }
        public Node2D GetNodeByGridPosition(Vector2Int gridPosition) { throw new System.NotImplementedException(); }
        public Node2D GetNodeByWorldPosition(Vector2 worldPosition) { throw new System.NotImplementedException(); }

        public Vector2 GridToWorldPosition(Vector2Int gridPosition) { throw new System.NotImplementedException(); }
        public Vector2Int WorldToGridPosition(Vector2 worldPosition) { throw new System.NotImplementedException(); }
        #endregion
    }
}