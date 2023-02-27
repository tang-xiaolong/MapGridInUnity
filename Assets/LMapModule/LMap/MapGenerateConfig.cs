using System;
using UnityEngine;

namespace LMap
{
#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(MapGenerateConfig)), CanEditMultipleObjects]
    public class MapGenerateConfigEditor : Editor
    {
        private MapGenerateConfig _mapGenerateConfig;
        private SerializedProperty _hexMapTypeProperty;
        private SerializedProperty _normalGridDirProperty;

        private void OnEnable()
        {
            _mapGenerateConfig = target as MapGenerateConfig;
            _hexMapTypeProperty = serializedObject.FindProperty("HexMapType");
            _normalGridDirProperty = serializedObject.FindProperty("NormalGridDir");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            serializedObject.Update();
            if (_mapGenerateConfig.MapType == MapType.HexMap)
            {
                EditorGUILayout.PropertyField(_hexMapTypeProperty);
            }
            else
            {
                EditorGUILayout.PropertyField(_normalGridDirProperty);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif

    public enum NormalGridDir
    {
        FourDir,
        EightDir
    }

    [CreateAssetMenu(fileName = "MapGenerateConfig", menuName = "地图/地图生成配置", order = 1)]
    [Serializable]
    public class MapGenerateConfig : ScriptableObject
    {
        public MapType MapType = MapType.NormalMap;
        [Header("HexMap")] public MapCoordinate MapCoordinate;
        [HideInInspector] public HexMapType HexMapType = HexMapType.SharpX;
        [HideInInspector] public NormalGridDir NormalGridDir = NormalGridDir.EightDir;
        [Tooltip("地图起点与第一个节点是否有偏移")] public bool NeedOffset = false;
        public int Width = 20;
        public int Height = 20;
        public int CeilSize = 1;
        public GameObject node;
    }
}