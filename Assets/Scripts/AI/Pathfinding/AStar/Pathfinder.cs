using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DQWorks.AI.Pathfinding.AStar
{
    public enum PathfinderStatus
    {
        Impossible,
        Searching,
        Found,
    }

    public class Pathfinder : MonoBehaviour
    {
        #region Read-only properties
        private readonly List<PathNode> _closedList = new List<PathNode>();
        private readonly List<PathNode> _openedList = new List<PathNode>();
        private readonly Stack<GridNode> _path = new Stack<GridNode>();
        #endregion

        #region Private properties
        [Header("Pathfinder settings")]
        [field: SerializeField] private Navmesh2D _navmesh;
        [field: SerializeField] private int _updatesPerSec = 50;
        private GridNode? _startNode, _targetNode;
        #endregion

        #region Getters and setters
        public Navmesh2D Navmesh { get => _navmesh; set { _navmesh = value; StopPathfinding(); } }
        public Stack<GridNode> PathStack => _path;
        public PathfinderStatus Status { get; set; }
        #endregion

        #region MonoBehaviour
        private void Update()
        {
            for (int i = 0; i < _updatesPerSec; i++)
                DoPathfinding();
        }

        private void OnDrawGizmos()
        {
            RenderPath();
            RenderPathfinding();
            RenderStartAndTarget();
        }
        #endregion

        #region Pathfinder
        private void DoPathfinding()
        {
            // Inverted if to check if status is "Searching"
            if (Status != PathfinderStatus.Searching)
                return;

            // Sorting and getting the new current node
            _openedList.Sort((x, y) => x.CostF.CompareTo(y.CostF));
            PathNode currentNode = _openedList[0];

            // Processing the neighbors
            foreach (GridNode neighbor in _navmesh.GetNeighbors(currentNode.GridNode))
            {
                if (!neighbor.Walkable) continue;
                if (_closedList.Any(x => x.Equals(neighbor))) continue;

                // If it's already on _openedList, try to optimize the path
                if (_openedList.Any(x => x.Equals(neighbor)))
                {
                    PathNode _sampleName = _openedList.Find(x => x.Equals(neighbor));
                    int newCostG = Heuristics.CalculateG(currentNode, _sampleName);

                    if (newCostG < _sampleName.CostG)
                    {
                        _sampleName.CostG = newCostG;
                        _sampleName.ParentNode = currentNode;
                    }
                }

                // Otherwise, add the neighbor to open list
                else _openedList.Add(new PathNode(neighbor)
                {
                    CostG = Heuristics.CalculateG(currentNode, neighbor),
                    CostH = Heuristics.CalculateH(currentNode.GridNode, _targetNode.Value),
                    ParentNode = currentNode
                });
            }

            // Add node on closed list and remove from opened one
            _closedList.Add(currentNode);
            _openedList.Remove(currentNode);

            // Ending conditions
            if (currentNode.Equals(_targetNode.Value))
            {
                ProcessFoundPath(currentNode);
                Status = PathfinderStatus.Found;
            }

            else if (_openedList.Count == 0)
                Status = PathfinderStatus.Impossible;
        }

        private void ProcessFoundPath(PathNode lastNode)
        {
            _path.Clear();
            while (lastNode != null)
            {
                _path.Push(lastNode.GridNode);
                lastNode = lastNode.ParentNode;
            }
        }

        private void ResetClosedAndOpenList()
        {
            _closedList.Clear();
            _openedList.Clear();
        }

        public void SearchPathInOneFrame(GridNode from, GridNode to)
        {
            StartPathfinding(from, to);
            while (Status == PathfinderStatus.Searching)
                DoPathfinding();
        }

        public void StartPathfinding(GridNode from, GridNode to)
        {
            // Conditions to pathfinding not to start
            if (Status == PathfinderStatus.Searching)
                return;
            if (!from.Walkable || !to.Walkable)
                return;

            // Set start and target node
            _startNode = from;
            _targetNode = to;

            // Initial lists' setup
            ResetClosedAndOpenList();
            _openedList.Add(
                new PathNode(_startNode.Value)
            );

            // Set status to searching
            Status = PathfinderStatus.Searching;
        }

        public void StopPathfinding()
        {
            Status = PathfinderStatus.Impossible;
            ResetClosedAndOpenList();
        }
        #endregion

        #region Path rendering
        private void RenderPath()
        {
            Gizmos.color = Color.white;
            foreach (GridNode node in _path)
                Gizmos.DrawCube(_navmesh.GridToWorldPosition(node.GridPosition), _navmesh.NodeSize);
        }

        private void RenderPathfinding()
        {
            Gizmos.color = Color.black;
            foreach (PathNode node in _closedList)
                Gizmos.DrawCube(_navmesh.GridToWorldPosition(node.GridNode.GridPosition), _navmesh.NodeSize);

            Gizmos.color = Color.grey;
            foreach (PathNode node in _openedList)
                Gizmos.DrawCube(_navmesh.GridToWorldPosition(node.GridNode.GridPosition), _navmesh.NodeSize);
        }

        private void RenderStartAndTarget()
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