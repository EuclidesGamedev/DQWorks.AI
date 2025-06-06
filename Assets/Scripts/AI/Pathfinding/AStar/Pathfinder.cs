using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DQWorks.AI.Pathfinding.AStar
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

        #region Private properties
        [Header("Pathfinder settings")]
        [field: SerializeField] private Navmesh2D _navmesh;
        [field: SerializeField] private int _ticksPerSecond = 50;
        private GridNode? _startNode;
        private GridNode? _targetNode;
        #endregion


        #region Getters and setters
        public Stack<GridNode> PathStack => _path;
        public PathfinderStatus Status { get; set; }
        #endregion

        #region MonoBehaviour
        private void Update()
        {
            for (int i = 0; i < 50; i++)
                if (Status == PathfinderStatus.SearchingForAPath)
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

            Gizmos.color = Color.black;
            foreach (PathNode node in _closedList)
                Gizmos.DrawCube(_navmesh.GridToWorldPosition(node.GridNode.GridPosition), _navmesh.NodeSize);

            Gizmos.color = Color.grey;
            foreach (PathNode node in _openList)
                Gizmos.DrawCube(_navmesh.GridToWorldPosition(node.GridNode.GridPosition), _navmesh.NodeSize);
            

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
                    CostH = Heuristics.CalculateH(currentNode.GridNode, _targetNode.Value),
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
                Status = PathfinderStatus.WasNotAbleToFindAPath;
        }

        private void PostProcessFoundPath(PathNode lastNode)
        {
            Status = PathfinderStatus.FoundAValidPath;

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

        public void SearchForThePathInOneFrame(GridNode from, GridNode to, Navmesh2D on)
        {
            StartSearchingPath(from, to, on);
            while (Status == PathfinderStatus.SearchingForAPath)
                PathfinderTick();
        }

        public void StartSearchingPath(GridNode from, GridNode to, Navmesh2D on)
        {
            _navmesh = on;
            StartSearchingPath(from, to);
        }

        public void StartSearchingPath(GridNode from, GridNode to)
        {
            // Initial setup
            _startNode = from; _targetNode = to;
            Status = PathfinderStatus.SearchingForAPath;
            _closedList.Clear();
            _openList.Clear();
            _openList.Add(
                new PathNode(_startNode.Value)
            );
        }
        #endregion

    }
}