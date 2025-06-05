using Assets.Scripts.AI.Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.AI.Pathfinding.AStar
{
    public class Pathfinder : MonoBehaviour
    {
        #region Read-only properties
        private readonly List<GridNode> _closedList = new List<GridNode>();
        private readonly List<GridNode> _openList = new List<GridNode>();
        private readonly Stack<GridNode> _path = new Stack<GridNode>();
        #endregion

        [field: SerializeField]
        private Navmesh2D _navmesh;
        private GridNode? _startNode;
        private GridNode? _targetNode;

        #region MonoBehaviour
        private void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.value);

            if (InputSystem.actions.FindAction("UI/Click").WasPressedThisFrame())
                _startNode = _navmesh.GetNodeByGridPosition(_navmesh.WorldToGridPosition(ray.origin));

            if (InputSystem.actions.FindAction("UI/RightClick").WasPressedThisFrame())
                _targetNode = _navmesh.GetNodeByGridPosition(_navmesh.WorldToGridPosition(ray.origin));

            if (InputSystem.actions.FindAction("UI/MiddleClick").WasPressedThisFrame())
                Debug.Log(_navmesh.WorldToGridPosition(ray.origin));

            if (_startNode.HasValue && _targetNode.HasValue)
                FindPathInOneFrame();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            if (_startNode.HasValue)
                Gizmos.DrawCube(_navmesh.GridToWorldPosition(_startNode.Value.GridPosition), _navmesh.NodeSize);

            Gizmos.color = Color.green;
            if (_targetNode.HasValue)
                Gizmos.DrawCube(_navmesh.GridToWorldPosition(_targetNode.Value.GridPosition), _navmesh.NodeSize);

            if (_path.Count > 0)
                RenderPath();
        }
        #endregion

        #region Pathfinding
        private void FindPathInOneFrame() { throw new System.NotImplementedException(); }
        private void RenderPath()
        {
            Gizmos.color = Color.white;
            foreach (GridNode node in _path)
                Gizmos.DrawCube(_navmesh.GridToWorldPosition(node.GridPosition), _navmesh.NodeSize);
        }
        #endregion
    }
}