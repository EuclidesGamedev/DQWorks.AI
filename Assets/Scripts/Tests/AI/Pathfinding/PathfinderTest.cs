using DQWorks.AI.Pathfinding;
using DQWorks.AI.Pathfinding.AStar;
using NUnit.Framework;
using UnityEngine;

namespace DQWorks.Tests.AI.Pathfinding
{
    public class PathfinderTests : MonoBehaviour
    {
        #region Setup for tests
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
        #endregion

        #region Test cases
        // Tests related to pathfinding process behaviour
        public void TestCanFindPathInLargeGrids() { throw new System.NotImplementedException(); }
        public void TestCanFindPathInSmallGrids() { throw new System.NotImplementedException(); }
        public void TestCanFindPathToItself() { throw new System.NotImplementedException(); }
        public void TestCanFindPathToNeighbors() { throw new System.NotImplementedException(); }
        public void TestCantFindImpossiblePaths() { throw new System.NotImplementedException(); }
        public void TestCantPassThroughBlockedCorner() { throw new System.NotImplementedException(); }

        // Tests related to Pathfinder class behaviour
        [TestCase] public void TestSetNavmeshStopsPathfindingProcess()
        {
            // Navmesh setup for the test
            _navmesh.GridSize = new Vector2Int(10, 10);

            // Start pathfinding and assert it really started
            _finder.StartPathfinding(
                _navmesh.GetNodeByGridPosition(new Vector2Int(0, 0)).Value,
                _navmesh.GetNodeByGridPosition(new Vector2Int(9, 9)).Value
            );
            Assert.AreEqual(PathfinderStatus.Searching, _finder.Status);

            // Set value _finder.Navmesh and assert the searching process was interrupted
            _finder.Navmesh = _finder.Navmesh;
            Assert.AreEqual(PathfinderStatus.Impossible, _finder.Status);
        }

        [TestCase] public void TestStopPathfindingSetsStatusToImpossible() {
            // Navmesh setup for the test
            _navmesh.GridSize = new Vector2Int(10, 10);

            // Start pathfinding and assert it really started
            _finder.StartPathfinding(
                _navmesh.GetNodeByGridPosition(new Vector2Int(0, 0)).Value,
                _navmesh.GetNodeByGridPosition(new Vector2Int(9, 9)).Value
            );
            Assert.AreEqual(PathfinderStatus.Searching, _finder.Status);

            // Use StopPathfinding method and assert the searching process was interrupted
            _finder.StopPathfinding();
            Assert.AreEqual(PathfinderStatus.Impossible, _finder.Status);
        }
        #endregion
    }
}