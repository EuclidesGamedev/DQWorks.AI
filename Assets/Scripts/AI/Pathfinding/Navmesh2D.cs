using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.AI.Pathfinding
{
    public class Navmesh2D : MonoBehaviour
    {
        #region Serialized fields
        [Header("Navmesh settings")]
        [SerializeField] private Vector2Int _gridSize = new Vector2Int(2, 2);
        [SerializeField] private Vector2 _nodeSize = Vector2.one;
        #endregion

        #region Getters and setters
        public Node2D[,] Grid { get; private set; } = { };
        public Vector2Int GridSize => _gridSize;
        public Vector2 NodeSize => _nodeSize;
        public Vector2 Position => transform.position;
        public Vector2 WorldSize => GridSize * NodeSize;
        #endregion

        #region MonoBehaviour
        private void Start()
        {
            GenerateGrid();
        }

        private void Update()
        {
            if (InputSystem.actions.FindAction("UI/Click").WasPressedThisFrame())
            {
                Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);
                Node2D? node = GetNodeByWorldPosition(ray.origin);

                Debug.Log("GridPosition is: " + (node.HasValue ? node.Value.GridPosition : "null"));
            }
        }

        private void OnDrawGizmos()
        {
            RenderNavmesh();
        }
        #endregion

        #region Navmesh2D private methods
        private void GenerateGrid()
        {
            Grid = new Node2D[GridSize.x, GridSize.y];

            for (int x = 0; x < GridSize.x; x++)
                for (int y = 0; y < GridSize.y; y++)
                    Grid[x, y] = new Node2D(x, y);
        }

        private void RenderNavmesh()
        {
            Gizmos.color = Color.green;
            foreach (Node2D node in Grid)
                Gizmos.DrawWireCube(GridToWorldPosition(node.GridPosition), NodeSize);
        }
        #endregion

        #region Navmesh2D public methods
        public Node2D[] GetNeighbors(Node2D node) { throw new System.NotImplementedException(); }
        public Node2D? GetNodeByGridPosition(Vector2Int gridPosition)
        {
            if (gridPosition.x < 0 || gridPosition.x >= GridSize.x) return null;
            if (gridPosition.y < 0 || gridPosition.y >= GridSize.y) return null;
            return Grid[gridPosition.x, gridPosition.y];
        }
        public Node2D? GetNodeByWorldPosition(Vector2 worldPosition) => GetNodeByGridPosition(WorldToGridPosition(worldPosition));

        public Vector2 GridToWorldPosition(Vector2Int gridPosition) => Position + NodeSize * gridPosition - WorldSize / 2 + NodeSize / 2;
        public Vector2Int WorldToGridPosition(Vector2 worldPosition) => Vector2Int.CeilToInt((worldPosition + WorldSize / 2 - Position - NodeSize) / NodeSize);
        #endregion
    }
}