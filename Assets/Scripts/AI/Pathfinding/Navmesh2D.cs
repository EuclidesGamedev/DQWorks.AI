using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace Assets.Scripts.AI.Pathfinding
{
    public class Navmesh2D : MonoBehaviour
    {
        #region Serialized fields
        [Header("Navmesh settings")]
        [SerializeField] private Vector2Int _gridSize = new Vector2Int(2, 2);
        [SerializeField] private Vector2 _nodeSize = Vector2.one;
        [SerializeField] private LayerMask _wallMask = default;
        #endregion

        #region Getters and setters
        public GridNode[,] Grid { get; private set; } = { };
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

        private void OnDrawGizmos()
        {
            RenderNavmesh();
        }
        #endregion

        #region Navmesh2D private methods
        private void BakeCollisions()
        {
            for (int x = 0; x < GridSize.x; x++)
                for (int y = 0; y < GridSize.y; y++)
                    Grid[x, y].Walkable = !Physics2D.OverlapBox(GridToWorldPosition(Grid[x, y].GridPosition), NodeSize, 0f, _wallMask);
        }

        private void GenerateGrid()
        {
            Grid = new GridNode[GridSize.x, GridSize.y];

            for (int x = 0; x < GridSize.x; x++)
                for (int y = 0; y < GridSize.y; y++)
                    Grid[x, y] = new GridNode(x, y);

            BakeCollisions();
        }

        private void RenderNavmesh()
        {
            foreach (GridNode node in Grid)
            {
                if (node.Walkable)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(GridToWorldPosition(node.GridPosition), NodeSize);
                }

                else
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(GridToWorldPosition(node.GridPosition), NodeSize);
                }
            }
        }
        #endregion

        #region Navmesh2D public methods
        public GridNode[] GetNeighbors(GridNode node)
        {
            List<GridNode> neighbors = new List<GridNode>();
            for (int dx = -1; dx <= 1; dx++)
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0) continue;
                    if (node.GridPosition.x + dx < 0 || node.GridPosition.x + dx >= GridSize.x) continue;
                    if (node.GridPosition.y + dy < 0 || node.GridPosition.y + dy >= GridSize.y) continue;
                    neighbors.Add(Grid[node.GridPosition.x + dx, node.GridPosition.y + dy]);
                }
            return neighbors.ToArray();
        }
        public GridNode? GetNodeByGridPosition(Vector2Int gridPosition)
        {
            if (gridPosition.x < 0 || gridPosition.x >= GridSize.x) return null;
            if (gridPosition.y < 0 || gridPosition.y >= GridSize.y) return null;
            return Grid[gridPosition.x, gridPosition.y];
        }
        public GridNode? GetNodeByWorldPosition(Vector2 worldPosition) => GetNodeByGridPosition(WorldToGridPosition(worldPosition));

        public Vector2 GridToWorldPosition(Vector2Int gridPosition) => Position + NodeSize * gridPosition - WorldSize / 2 + NodeSize / 2;
        public Vector2Int WorldToGridPosition(Vector2 worldPosition) => Vector2Int.CeilToInt((worldPosition + WorldSize / 2 - Position - NodeSize) / NodeSize);
        #endregion
    }
}