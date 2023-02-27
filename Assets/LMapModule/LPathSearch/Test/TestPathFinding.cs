using System.Collections.Generic;
using System.Diagnostics;
using LMap;
using LPathSearch;
using UnityEngine;
using Debug = UnityEngine.Debug;


namespace LPathSearch
{
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(TestPathFinding)), CanEditMultipleObjects]
    public class TestPathFindingEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("测试寻路"))
            {
                (target as TestPathFinding)?.PathSearch();
            }
        }
    }
#endif

    public class TestPathFinding : MonoBehaviour
    {
        public enum PathFindingType
        {
            AStar,
            AStarMinHeap,
            AStarMinHeapPro,
        }

        public PathFindingType pathFindingType = PathFindingType.AStar;
        private AStarSearch aStarSearch = new AStarSearch();
        private AStarMinHeap aStarMinHeap = new AStarMinHeap();
        private AStarMinHeapPro aStarMinHeapPro = new AStarMinHeapPro();
        private TestMapGenerate _testMapGenerate;

        public Transform StartPoint;
        public Transform EndPoint;

        private void Start()
        {
            _testMapGenerate = GetComponent<TestMapGenerate>();
        }

        List<IPathNode> path = null;

        private void OnDrawGizmos()
        {
            if (path != null && path.Count > 2 && _testMapGenerate.MapGrid != null)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < path.Count - 1; i++)
                {
                    Gizmos.DrawLine(_testMapGenerate.MapGrid.GetNodeWorldPosition(path[i].Index1, path[i].Index2),
                        _testMapGenerate.MapGrid.GetNodeWorldPosition(path[i + 1].Index1, path[i + 1].Index2));
                }
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                PathSearch();
            }
        }


        [ContextMenu("PathSearch")]
        public void PathSearch()
        {
            if (_testMapGenerate.MapGrid == null)
                return;
            var startPoint = _testMapGenerate.MapGrid.GetMapIndex(StartPoint.position);
            var destinationPoint = _testMapGenerate.MapGrid.GetMapIndex(EndPoint.position);
            // startPoint = Vector2Int.zero;
            // destinationPoint = new Vector2Int(3, 4);


            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            IPathSearch pathSearch = null;
            switch (pathFindingType)
            {
                case PathFindingType.AStar:
                    pathSearch = aStarSearch;
                    break;
                case PathFindingType.AStarMinHeap:
                    pathSearch = aStarMinHeap;
                    break;
                case PathFindingType.AStarMinHeapPro:
                    pathSearch = aStarMinHeapPro;
                    break;
            }


            if (pathSearch != null)
            {
                path = pathSearch.SearchPath(_testMapGenerate.MapGrid, startPoint, destinationPoint);
            }

            stopwatch.Stop();
            if (path != null)
            {
                Debug.Log($"本次寻路共耗费了{stopwatch.ElapsedMilliseconds / 1000f}秒.路径点数为{path.Count}");
                foreach (INode node in path)
                {
                    Debug.Log(node);
                }
            }
        }
    }
}