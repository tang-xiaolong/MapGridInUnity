using System.Collections.Generic;
using UnityEngine;

namespace LMap
{
#if UNITY_EDITOR
    using UnityEditor;
    [CustomEditor(typeof(TestMapHighLight)), CanEditMultipleObjects]
    public class TestMapHighLightEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("高亮环形内的格子"))
            {
                (target as TestMapHighLight)?.TestHighLightRangeNode();
            }
            if (GUILayout.Button("高亮指定方向上的格子"))
            {
                (target as TestMapHighLight)?.TestHighLightLineNode();
            }
            if (GUILayout.Button("高亮指定扇形区域内的格子"))
            {
                (target as TestMapHighLight)?.TestHighLightSectorNode();
            }
        }
    }
#endif
    
    public class TestMapHighLight : MonoBehaviour
    {
        public Transform target;
        private TestMapGenerate _testMapGenerate;
        private List<INode> _hightLightNodes = new List<INode>();

        private void Start()
        {
            _testMapGenerate = FindObjectOfType<TestMapGenerate>();
        }


        [SerializeField] private int highLightInRange = 3;
        [SerializeField] private int highLightOutRange = 3;

        [ContextMenu("高亮范围内的格子")]
        public void TestHighLightRangeNode()
        {
            if (_testMapGenerate != null)
            {
                SetNormal();
                _testMapGenerate.MapGrid.GetRangeNode(ref _hightLightNodes, transform.position, highLightInRange,
                    highLightOutRange);
                SetToValid();
            }
        }

        void SetToValid()
        {
            var mapShow = _testMapGenerate.MapGrid.GetMapGridShow();
            if (mapShow != null)
            {
                foreach (INode hightLightNode in _hightLightNodes)
                {
                    var nodeEntity = mapShow.GetNodeEntity(hightLightNode);
                    if (nodeEntity != null)
                    {
                        nodeEntity.OnNodeInEffectRange();
                    }
                }
            }
        }

        [ContextMenu("高亮指向方向的格子")]
        public void TestHighLightLineNode()
        {
            if (_testMapGenerate != null)
            {
                SetNormal();
                _testMapGenerate.MapGrid.GetLineNode(ref _hightLightNodes, transform.position, target.position,
                    highLightInRange,
                    highLightOutRange);
                SetToValid();
            }
        }

        public int angleWidth = 1;

        [ContextMenu("高亮扇形范围内格子")]
        public void TestHighLightSectorNode()
        {
            if (_testMapGenerate != null)
            {
                SetNormal();
                _testMapGenerate.MapGrid.GetSectorNode(ref _hightLightNodes, transform.position, target.position,
                    angleWidth, highLightInRange,
                    highLightOutRange);
                SetToValid();
            }
        }

        // [ContextMenu("随机路径并高亮格子")]
        // void SetValid()
        // {
        //     if (Map != null)
        //     {
        //         var pos1 = Map.GetPos(Random.Range(0, Map.GetWidth()), Random.Range(0, Map.GetHeight()));
        //         var pos2 = Map.GetPos(Random.Range(0, Map.GetWidth()), Random.Range(0, Map.GetHeight()));
        //         var paths = Map.FindPath(pos1, pos2);
        //         List<GridObject> nodes = new List<GridObject>();
        //         for (int i = 0; i < paths.Count; i++)
        //         {
        //             var nodePos = Map.GetMapIndex(paths[i]);
        //             nodes.Add(Map.GetNode(nodePos.x, nodePos.y));
        //         }
        //
        //         _mapNodePreview.SetToValid(nodes);
        //     }
        // }

        [ContextMenu("取消高亮")]
        void SetNormal()
        {
            var mapShow = _testMapGenerate.MapGrid.GetMapGridShow();
            if (mapShow != null && _hightLightNodes != null)
            {
                foreach (INode hightLightNode in _hightLightNodes)
                {
                    var nodeEntity = mapShow.GetNodeEntity(hightLightNode);
                    if (nodeEntity != null)
                    {
                        nodeEntity.SetToNormal();
                    }
                }

                _hightLightNodes.Clear();
            }
        }


        public float GetAngle(Vector3 vec1, Vector3 vec2)
        {
            float angle = Vector3.Angle(vec1, vec2); //求出两个角的角度
            Vector3 nor = Vector3.Cross(vec1, vec2); //叉乘求出法线向量
            float dot = Vector3.Dot(nor, Vector3.down); //点乘求出相似度
            if (dot < 0)
            {
                angle *= -1;
                angle += 360;
            }

            return angle;
        }
    }
}