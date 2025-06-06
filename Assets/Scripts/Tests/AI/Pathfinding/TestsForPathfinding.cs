using DQWorks.AI.Pathfinding;
using DQWorks.AI.Pathfinding.AStar;
using NUnit.Framework;
using UnityEngine;

namespace DQWorks.Tests.AI.Pathfinding
{
    public class TestsForPathfinding : MonoBehaviour
    {
        private Navmesh2D _navmesh;
        private Pathfinder _finder;

        [SetUp]
        public void SetUp()
        {
            var gameObject = new GameObject();

            _navmesh = gameObject.AddComponent<Navmesh2D>();
            _finder = gameObject.AddComponent<Pathfinder>();
            _finder.Navmesh = _navmesh;
        }


        [TestCase]
        public void TestCanFindPathIfTargetIsANeighbor()
        {
            _navmesh.GridSize = new Vector2Int(3, 3);
            _finder.SearchPathInOneFrame(
                _navmesh.GetNodeByGridPosition(new Vector2Int(1, 0)).Value,
                _navmesh.GetNodeByGridPosition(new Vector2Int(0, 1)).Value
            );

            Assert.AreEqual(PathfinderStatus.Found, _finder.Status);
        }

        [TestCase]
        public void TestCanFindPathToItself()
        {
            _navmesh.GridSize = new Vector2Int(3, 3);
            _finder.SearchPathInOneFrame(
                _navmesh.GetNodeByGridPosition(new Vector2Int(0, 0)).Value,
                _navmesh.GetNodeByGridPosition(new Vector2Int(0, 0)).Value
            );

            Assert.AreEqual(PathfinderStatus.Found, _finder.Status);
        }

        [TestCase]
        public void TestCanFindPathOnBigGrids()
        {
            _navmesh.GridSize = new Vector2Int(128, 128);
            _finder.SearchPathInOneFrame(
                _navmesh.GetNodeByGridPosition(new Vector2Int(0, 0)).Value,
                _navmesh.GetNodeByGridPosition(new Vector2Int(127, 127)).Value
            );

            Assert.AreEqual(PathfinderStatus.Found, _finder.Status);
        }

        [TestCase]
        public void CantFindImpossiblePaths()
        {
            // "X" is an unwalkable node,
            // "S" is the starting node,
            // "T" it the target node.
            // That is the grid of the test:
            //  XT
            //  X
            // SX
            _navmesh.GridSize = new Vector2Int(3, 3);
            _navmesh.Grid[1, 2].Walkable = false;
            _navmesh.Grid[1, 1].Walkable = false;
            _navmesh.Grid[1, 0].Walkable = false;
            _finder.SearchPathInOneFrame(
                _navmesh.GetNodeByGridPosition(new Vector2Int(0, 0)).Value,
                _navmesh.GetNodeByGridPosition(new Vector2Int(2, 2)).Value
            );

            Assert.AreEqual(PathfinderStatus.Impossible, _finder.Status);
        }

        [TestCase]
        public void TestCantFindPassingThroughBlockerCorner()
        {
            // "X" is an unwalkable node,
            // "S" is the starting node,
            // "T" it the target node.
            // That is the grid of the test:
            // X T
            //  X
            // S X
            _navmesh.GridSize = new Vector2Int(3, 3);
            _navmesh.Grid[0, 2].Walkable = false;
            _navmesh.Grid[1, 1].Walkable = false;
            _navmesh.Grid[2, 0].Walkable = false;
            _finder.SearchPathInOneFrame(
                _navmesh.GetNodeByGridPosition(new Vector2Int(0, 0)).Value,
                _navmesh.GetNodeByGridPosition(new Vector2Int(2, 2)).Value
            );

            Assert.AreEqual(PathfinderStatus.Impossible, _finder.Status);
        }
    }
}