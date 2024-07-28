using CustomInspector;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    public class Shadow : MonoBehaviour
    {
        [Button(nameof(SetupShadow))]
        [SerializeField] float halfHeight = 0.5f;
        [SerializeField] bool updateSprite = true;
        [SerializeField, ReadOnly] float angle = 225;

        [SerializeField] SpriteRenderer targetSR;
        [SerializeField] TilemapRenderer tileRenderer;
        SpriteRenderer childSR;

        private void Awake()
        {
            SetupShadow();
        }

        void SetupShadow()
        {
            if (targetSR == null)
                targetSR = GetComponentInParent<SpriteRenderer>();

            if (targetSR == null)
                Debug.LogError("Shadow needs a sprite renderer");

            childSR = transform.GetChild(0).GetComponent<SpriteRenderer>();
            childSR.sprite = targetSR.sprite;

            transform.localPosition = Vector3.up * -halfHeight;
            transform.GetChild(0).localPosition = Vector3.up * halfHeight;

            transform.eulerAngles = Vector3.forward * angle;
        }

        private void Update()
        {
            if (!updateSprite)
                return;

            childSR.sprite = targetSR.sprite;
            childSR.color = targetSR.color;
        }
    }
}
