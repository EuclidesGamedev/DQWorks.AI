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
        //// Tests related to pathfinding process behaviour
        // Positive tests //
        [TestCase] public void TestCanFindPathInLargeGrids()
        {
            // Navmesh setup for the test
            _navmesh.GridSize = new Vector2Int(128, 128);

            // Try to find small path in large grid and assert it was found
            _finder.SearchPathInOneFrame(
                _navmesh.GetNodeByGridPosition(new Vector2Int(0, 0)).Value,
                _navmesh.GetNodeByGridPosition(new Vector2Int(9, 9)).Value
            );
            Assert.AreEqual(PathfinderStatus.Found, _finder.Status);

            // Try to find large path in large grid and assert it was found
            _finder.SearchPathInOneFrame(
                _navmesh.GetNodeByGridPosition(new Vector2Int(0, 0)).Value,
                _navmesh.GetNodeByGridPosition(new Vector2Int(127, 127)).Value
            );
            Assert.AreEqual(PathfinderStatus.Found, _finder.Status);
        }
        [TestCase] public void TestCanFindPathInSmallGrids()
        {
            // Navmesh setup for the test
            _navmesh.GridSize = new Vector2Int(2, 2);

            // Try to find the path in the 2x2 grid
            _finder.SearchPathInOneFrame(
                    _navmesh.GetNodeByGridPosition(new Vector2Int(0, 0)).Value,
                    _navmesh.GetNodeByGridPosition(new Vector2Int(1, 1)).Value
                );
            Assert.AreEqual(PathfinderStatus.Found, _finder.Status);

            // Navmesh setup for multiple tests
            _navmesh.GridSize = new Vector2Int(3, 3);

            // It will test all possible paths in a 3x3 grid, and assert all are valid.
            // There's no blocked node in this grid, it'll be 81 tests.
            foreach (GridNode node1 in _navmesh.Grid)
                foreach (GridNode node2 in _navmesh.Grid)
                {
                    _finder.SearchPathInOneFrame(node1, node2);
                    Assert.AreEqual(PathfinderStatus.Found, _finder.Status);
                }
        }
        [TestCase] public void TestCanFindPathToItself() {
            // Navmesh setup for the test
            _navmesh.GridSize = new Vector2Int(2, 2);

            // Try to find the path to itself
            _finder.SearchPathInOneFrame(
                    _navmesh.GetNodeByGridPosition(new Vector2Int(0, 0)).Value,
                    _navmesh.GetNodeByGridPosition(new Vector2Int(0, 0)).Value
                );
            Assert.AreEqual(PathfinderStatus.Found, _finder.Status);
        }
        [TestCase] public void TestCanFindPathToNeighbors() {
            // Navmesh setup for the test
            _navmesh.GridSize = new Vector2Int(3, 3);
            GridNode? node; // It stores the node used in the tests

            // Test node in corner can find path to its neighbors
            node = _navmesh.GetNodeByGridPosition(new Vector2Int(0, 0)); // Node on grid corner
            foreach (GridNode neighbor in _navmesh.GetNeighbors(node.Value))
            {
                // Try to find the path to neighbord and assert it was found
                _finder.SearchPathInOneFrame(node.Value, neighbor);
                Assert.AreEqual(PathfinderStatus.Found, _finder.Status);
            }
            
            // Test node in sideline can find path to its neighbors
            node = _navmesh.GetNodeByGridPosition(new Vector2Int(0, 1)); // Node on grid sideline
            foreach (GridNode neighbor in _navmesh.GetNeighbors(node.Value))
            {
                // Try to find the path to neighbord and assert it was found
                _finder.SearchPathInOneFrame(node.Value, neighbor);
                Assert.AreEqual(PathfinderStatus.Found, _finder.Status);
            }

            // Test node inside can find path to its neighbors
            node = _navmesh.GetNodeByGridPosition(new Vector2Int(1, 1)); // Node inside grid
            foreach (GridNode neighbor in _navmesh.GetNeighbors(node.Value))
            {
                // Try to find the path to neighbord and assert it was found
                _finder.SearchPathInOneFrame(node.Value, neighbor);
                Assert.AreEqual(PathfinderStatus.Found, _finder.Status);
            }
        }
        [TestCase] public void TestCanFindPathWhenGridHasBlockedNodes()
        {
            // Navmesh setup for the test
            // [ ] [ ] [T]
            // [ ] [X] [ ]
            // [S] [ ] [ ]
            _navmesh.GridSize = new Vector2Int(5, 3);
            _navmesh.Grid[1, 1].Walkable = false;

            // Try to find the path
            _finder.SearchPathInOneFrame(
                _navmesh.GetNodeByGridPosition(new Vector2Int(0, 0)).Value,
                _navmesh.GetNodeByGridPosition(new Vector2Int(2, 2)).Value
            );

            // Assert path was found
            Assert.AreEqual(PathfinderStatus.Found, _finder.Status);
        }
        // Negative tests //
        [TestCase] public void TestCantFindPathFromBlockedNode()
        {
            // Navmesh setup for the test
            _navmesh.GridSize = new Vector2Int(3, 3);
            _navmesh.Grid[0, 0].Walkable = false;

            // Try to find the path from a blocked node
            _finder.SearchPathInOneFrame(
                _navmesh.GetNodeByGridPosition(new Vector2Int(0, 0)).Value,
                _navmesh.GetNodeByGridPosition(new Vector2Int(2, 2)).Value
            );

            // Assert path was not found
            Assert.AreEqual(PathfinderStatus.Impossible, _finder.Status);
        }
        [TestCase] public void TestCantFindImpossiblePaths()
        {
            // Navmesh setup for the test
            // [ ] [X] [T]
            // [ ] [X] [ ]
            // [S] [X] [ ] 
            _navmesh.GridSize = new Vector2Int(3, 3);
            _navmesh.Grid[1, 0].Walkable = false;
            _navmesh.Grid[1, 1].Walkable = false;
            _navmesh.Grid[1, 2].Walkable = false;

            // Try to find the path
            _finder.SearchPathInOneFrame(
                _navmesh.GetNodeByGridPosition(new Vector2Int(0, 0)).Value,
                _navmesh.GetNodeByGridPosition(new Vector2Int(2, 2)).Value
            );

            // Assert path was not found
            Assert.AreEqual(PathfinderStatus.Impossible, _finder.Status);
        }
        [TestCase] public void TestCantFindPathToBlockedNode()
        {
            // Navmesh setup for the test
            _navmesh.GridSize = new Vector2Int(3, 3);
            _navmesh.Grid[2, 2].Walkable = false;

            // Try to find the path to a blocked node
            _finder.SearchPathInOneFrame(
                _navmesh.GetNodeByGridPosition(new Vector2Int(0, 0)).Value,
                _navmesh.GetNodeByGridPosition(new Vector2Int(2, 2)).Value
            );

            // Assert path was not found
            Assert.AreEqual(PathfinderStatus.Impossible, _finder.Status);
        }
        [TestCase] public void TestCantPassThroughBlockedCorner()
        {
            // Navmesh setup for the test
            // [X] [ ] [T]
            // [ ] [X] [ ]
            // [S] [ ] [X] 
            _navmesh.GridSize = new Vector2Int(3, 3);
            _navmesh.Grid[0, 2].Walkable = false;
            _navmesh.Grid[1, 1].Walkable = false;
            _navmesh.Grid[2, 0].Walkable = false;

            // Try to find the path
            _finder.SearchPathInOneFrame(
                _navmesh.GetNodeByGridPosition(new Vector2Int(0, 0)).Value,
                _navmesh.GetNodeByGridPosition(new Vector2Int(2, 2)).Value
            );

            // Assert path was not found
            Assert.AreEqual(PathfinderStatus.Impossible, _finder.Status);
        }
        [TestCase] public void TestPathDontIncludeAnyBlockedNode()
        {
            // Navmesh setup for the test
            // [ ] [ ] [ ] [X] [T]
            // [ ] [X] [ ] [X] [ ]
            // [S] [X] [ ] [ ] [ ]
            _navmesh.GridSize = new Vector2Int(5, 3);
            _navmesh.Grid[1, 0].Walkable = false;
            _navmesh.Grid[1, 1].Walkable = false;
            _navmesh.Grid[3, 1].Walkable = false;
            _navmesh.Grid[3, 2].Walkable = false;

            // Try to find the path
            _finder.SearchPathInOneFrame(
                _navmesh.GetNodeByGridPosition(new Vector2Int(0, 0)).Value,
                _navmesh.GetNodeByGridPosition(new Vector2Int(4, 2)).Value
            );

            // Assert path was found
            Assert.AreEqual(PathfinderStatus.Found, _finder.Status);

            // Assert none node in path is blocked
            foreach (GridNode node in _finder.PathStack)
                Assert.True(node.Walkable);
        }

        //// Tests related to Pathfinder class behaviour
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