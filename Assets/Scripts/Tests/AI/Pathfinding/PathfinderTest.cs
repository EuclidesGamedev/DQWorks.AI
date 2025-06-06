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
        public void TestSetNavmeshStopsPathfindingProcess() { throw new System.NotImplementedException(); }
        public void TestStopPathfindingSetsStatusToImpossible() { throw new System.NotImplementedException(); }
        #endregion
    }
}