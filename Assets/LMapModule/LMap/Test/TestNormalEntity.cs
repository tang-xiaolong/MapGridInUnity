using UnityEngine;

namespace LMap
{
    public class TestNormalEntity : MonoBehaviour, INodeEntity
    {
        public void SetToNormal()
        {
            SetSpriteColor(Color.white);
        }

        void SetSpriteColor(Color color)
        {
            var sprite = gameObject.GetComponentInChildren<SpriteRenderer>();
            if (sprite)
                sprite.color = color;
        }

        public void OnNodeInSelectRange()
        {
            SetSpriteColor(new Color(0.85f, 0.3f, 0.2f));
        }

        public void OnNodeInEffectRange()
        {
            SetSpriteColor(new Color(0.6f, 1f, 0.5f));
        }

        public void OnNodeInEffectCenter()
        {
            SetSpriteColor(new Color(0.8f, 0.8f, 0.2f));
        }

        public void Dispose()
        {
#if UNITY_EDITOR
            GameObject.DestroyImmediate(gameObject);
#else
        GameObject.Destroy(gameObject);
#endif
        }
    }
}