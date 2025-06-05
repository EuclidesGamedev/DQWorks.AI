using Assets.Scripts.AI.Pathfinding;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.AI.Pathfinding.AStar
{
    public class Pathfinder : MonoBehaviour
    {
        #region Read-only properties
        private readonly List<Node2D> _closedList = new List<Node2D>();
        private readonly List<Node2D> _openList = new List<Node2D>();
        private readonly Stack<Node2D> _path = new Stack<Node2D>();
        #endregion

        [field: SerializeField]
        private Navmesh2D _navmesh;
        private Node2D? _startNode;
        private Node2D? _targetNode;

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
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            if (_startNode.HasValue)
                Gizmos.DrawCube(_navmesh.GridToWorldPosition(_startNode.Value.GridPosition), _navmesh.NodeSize);

            Gizmos.color = Color.green;
            if (_targetNode.HasValue)
                Gizmos.DrawCube(_navmesh.GridToWorldPosition(_targetNode.Value.GridPosition), _navmesh.NodeSize);
        }
        #endregion
    }
}