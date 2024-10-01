using CustomInspector;
using UnityEngine;


namespace Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class BackgroundRepeat : MonoBehaviour
    {
        [Button(nameof(SetSize))]
        [SerializeField, TooltipBox("100 will mean it stays with the camera")] float parallaxPercent = 100;
        [SerializeField] Vector2 velocity;

        SpriteRenderer spriteR;
        Camera cam;

        Vector2 posOffset;
        Vector2 spriteSize;
    
        void Awake()
        {
            Setup();
        }

        private void Reset() => SetSize();

        void SetSize()
        {
            Setup();

            if (spriteR.sprite == null) return;

            spriteR.drawMode = SpriteDrawMode.Tiled;

            Vector2 camSize = WorldSize(cam);
            int multiple = (int)Mathf.Ceil(Mathf.Min(camSize.x / spriteSize.x, camSize.y / spriteSize.y)) + 1;
            spriteR.size = spriteSize * multiple;
        }

        void Setup()
        {
            spriteR = GetComponent<SpriteRenderer>();
            cam = Camera.main;

            if (spriteR.sprite == null) return;

            Vector2 resolution = new Vector2(spriteR.sprite.texture.width, spriteR.sprite.texture.height);
            spriteSize = resolution / spriteR.sprite.pixelsPerUnit;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireCube(transform.position, spriteSize);
        }

        private void Update()
        {
            posOffset += velocity * Time.deltaTime;
        }

        void LateUpdate()
        {
            float parallax = parallaxPercent / 100;
            Vector3 targetPos = cam.transform.position * parallax;
            targetPos.z = transform.position.z;

            Vector2 deltaPos = cam.transform.position - (targetPos + (Vector3)posOffset);
            if (Mathf.Abs(deltaPos.x) >= spriteSize.x / 2)
            {
                posOffset += Vector2.right * Mathf.Sign(deltaPos.x) * spriteSize.x;
            }
            else if (Mathf.Abs(deltaPos.y) >= spriteSize.y / 2)
            {
                posOffset += Vector2.up * Mathf.Sign(deltaPos.y) * spriteSize.y;
            }

            transform.position = targetPos + (Vector3)posOffset;
        }

        Vector2 WorldSize(Camera cam) => new Vector2(cam.orthographicSize * 2 * cam.aspect, cam.orthographicSize * 2);
    }
}
