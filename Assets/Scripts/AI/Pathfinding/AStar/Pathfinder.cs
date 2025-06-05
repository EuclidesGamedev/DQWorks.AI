using Assets.Scripts.AI.Pathfinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.AI.Pathfinding.AStar
{
    public enum PathfinderStatus
    {
        WasNotAbleToFindAPath,
        SearchingForAPath,
        FoundAValidPath,
    }

    public class Pathfinder : MonoBehaviour
    {
        #region Read-only properties
        private readonly List<PathNode> _closedList = new List<PathNode>();
        private readonly List<PathNode> _openList = new List<PathNode>();
        private readonly Stack<GridNode> _path = new Stack<GridNode>();
        #endregion

        private PathfinderStatus _status;

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

            if (_startNode.HasValue && _targetNode.HasValue && _status != PathfinderStatus.SearchingForAPath)
                StartSearchingPath(_startNode.Value, _targetNode.Value);

            if (_status == PathfinderStatus.SearchingForAPath)
                PathfinderTick();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            if (_startNode.HasValue)
                Gizmos.DrawCube(_navmesh.GridToWorldPosition(_startNode.Value.GridPosition), _navmesh.NodeSize);

            Gizmos.color = Color.green;
            if (_targetNode.HasValue)
                Gizmos.DrawCube(_navmesh.GridToWorldPosition(_targetNode.Value.GridPosition), _navmesh.NodeSize);

            RenderPath();
        }
        #endregion

        #region Pathfinding
        private void PathfinderTick()
        {
            // Sorting and getting the new current node
            _openList.Sort((x, y) => x.CostF.CompareTo(y.CostF));
            PathNode currentNode = _openList[0];

            // Processing the neighbors
            foreach (GridNode neighbor in _navmesh.GetNeighbors(currentNode.GridNode))
            {
                if (_closedList.Any(x => x.Equals(neighbor))) continue;
                if (!neighbor.Walkable) continue;

                if (_openList.Any(x => x.Equals(neighbor)))
                {
                    PathNode _sampleName = _openList.Find(x => x.Equals(neighbor));
                    int newCostG = Heuristics.CalculateG(currentNode, _sampleName);

                    if (newCostG < _sampleName.CostG)
                    {
                        _sampleName.CostG = newCostG;
                        _sampleName.ParentNode = currentNode;
                    }
                }

                else _openList.Add(new PathNode(neighbor)
                {
                    CostG = Heuristics.CalculateG(currentNode, neighbor),
                    CostH = Heuristics.CalculateH(currentNode.GridNode, neighbor),
                    ParentNode = currentNode
                });
            }

            // Post-process node
            _closedList.Add(currentNode);
            _openList.Remove(currentNode);

            // Ending condition
            if (currentNode.Equals(_targetNode.Value))
                PostProcessFoundPath(currentNode);

            else if (_openList.Count == 0)
                _status = PathfinderStatus.WasNotAbleToFindAPath;
        }

        private void PostProcessFoundPath(PathNode lastNode)
        {
            _status = PathfinderStatus.FoundAValidPath;

            _path.Clear();
            while (lastNode != null)
            {
                _path.Push(lastNode.GridNode);
                lastNode = lastNode.ParentNode;
            }
        }

        private void RenderPath()
        {
            Gizmos.color = Color.white;
            foreach (GridNode node in _path)
                Gizmos.DrawCube(_navmesh.GridToWorldPosition(node.GridPosition), _navmesh.NodeSize);
        }

        private void StartSearchingPath(GridNode from, GridNode to)
        {
            // Initial setup
            _status = PathfinderStatus.SearchingForAPath;
            _closedList.Clear();
            _openList.Clear();
            _openList.Add(
                new PathNode(_startNode.Value)
            );
        }
        #endregion
    }
}