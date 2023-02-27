using UnityEngine;

namespace LMap
{
    public class HexPosition : MonoBehaviour
    {
        private TextMesh _textMesh;

        public void ShowPos(Vector2Int pos)
        {
            _textMesh = GetComponent<TextMesh>();
            _textMesh.text = pos.x + "\n" + pos.y;
        }

        public void ShowPos(string posInfo)
        {
            _textMesh = GetComponent<TextMesh>();
            _textMesh.text = posInfo;
        }

        public void ShowPos(Vector3Int pos)
        {
            _textMesh = GetComponent<TextMesh>();
            _textMesh.text = pos.x + "\n" + pos.y + "\n" + pos.z;
        }

        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}