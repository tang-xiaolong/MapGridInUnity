using System.Collections.Generic;
using UnityEngine;

namespace LMap
{
#if UNITY_EDITOR
    using UnityEditor;
    [CustomEditor(typeof(TestMapGenerate)), CanEditMultipleObjects]
    public class TestMapGenerateEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("创建地图"))
            {
                (target as TestMapGenerate)?.CreateMap();
            }
            if (GUILayout.Button("销毁地图"))
            {
                (target as TestMapGenerate)?.DestroyMap();
            }
        }
    }
#endif
    
    public class TestMapGenerate : MonoBehaviour
    {
        [SerializeField]
        public MapGenerateConfig MapGenerateConfig;
        private NormalMapShow _mapShow;
        private TestNodeFactory _testNodeFactory = new TestNodeFactory();


        private IMapGrid _grid;
        public IMapGrid MapGrid => _grid;

        //创建地图
        [ContextMenu("创建地图")]
        public void CreateMap()
        {
            if (_grid != null)
            {
                _grid.Dispose();
            }

            if (MapGenerateConfig.MapType == MapType.NormalMap)
            {
                _grid = new NormalMapGrid(_testNodeFactory.CreateNode<MapNode>, MapGenerateConfig.Width, MapGenerateConfig.Height, MapGenerateConfig.CeilSize, 
                    MapGenerateConfig.NormalGridDir == NormalGridDir.EightDir ? (IDir)new EightDir() : new FourDir(), MapGenerateConfig.MapCoordinate, MapGenerateConfig.NeedOffset);
                _mapShow = GetComponent<NormalMapShow>();
            }
            else
            {
                if (MapGenerateConfig.HexMapType == HexMapType.SharpY)
                    _grid = new HexGridSharpY(_testNodeFactory.CreateNode<MapNode>, MapGenerateConfig.Width, MapGenerateConfig.Height, MapGenerateConfig.CeilSize * 0.5f, MapGenerateConfig.MapCoordinate,
                        MapGenerateConfig.NeedOffset);
                else
                    _grid = new HexGridSharpX(_testNodeFactory.CreateNode<MapNode>, MapGenerateConfig.Width, MapGenerateConfig.Height, MapGenerateConfig.CeilSize * 0.5f, MapGenerateConfig.MapCoordinate, MapGenerateConfig.NeedOffset);
                _mapShow = GetComponent<HexMapShow>();
            }

            if (_mapShow)
            {
                _mapShow.SetNode(MapGenerateConfig.node);
            }
            

            _grid.BindMapGridShow(_mapShow);
        }

        public List<Vector2Int> IndexTestQueue;
        [SerializeField] private int _indexTestCount = 4;

        [ContextMenu("测试Index计算")]
        public void TestIndex()
        {
            if (_grid != null)
            {
                if (IndexTestQueue != null && IndexTestQueue.Count > 0)
                {
                    foreach (Vector2Int vector2Int in IndexTestQueue)
                    {
                        var index1 = vector2Int.x;
                        var index2 = vector2Int.y;
                        TestIndexValue(index1, index2);
                    }
                }
                else
                {
                    for (int i = 0; i < _indexTestCount; i++)
                    {
                        var index1 = UnityEngine.Random.Range(0, _grid.GetWidth());
                        var index2 = UnityEngine.Random.Range(0, _grid.GetHeight());
                        TestIndexValue(index1, index2);
                    }
                }
            }
        }

        void TestIndexValue(int index1, int index2)
        {
            var showIndex = _grid.GetShowIndexByNormalIndex(index1, index2);
            var resIndex = _grid.GetNormalIndexByShowIndex(showIndex.x, showIndex.y);
            var worldPosition = _grid.GetNodeWorldPosition(showIndex.x, showIndex.y);
            if (resIndex.x == index1 && resIndex.y == index2)
                Debug.Log(
                    $"TrueIndex: {index1} {index2}  CalIndex: {resIndex.x} {resIndex.y} WorldPosition: {worldPosition}");
            else
                Debug.LogError(
                    $"TrueIndex: {index1} {index2}  CalIndex: {resIndex.x} {resIndex.y} WorldPosition: {worldPosition}");
        }

        private void Awake()
        {
            CreateMap();
        }

        [ContextMenu("销毁地图")]
        public void DestroyMap()
        {
            if (_grid != null)
            {
                _grid.Dispose();
                _grid = null;
            }
        }
    }
}