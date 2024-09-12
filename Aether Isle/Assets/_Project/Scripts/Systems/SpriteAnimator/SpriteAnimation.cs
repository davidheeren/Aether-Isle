using UnityEngine;

namespace SpriteAnimator
{
    [CreateAssetMenu(menuName = "SpriteAnimator/Animation")]
    public class SpriteAnimation : ScriptableObject
    {
        [field: SerializeField] public string animationName {  get; private set; }
        [field: SerializeField] public bool loop { get; private set; }
        [field: SerializeField] public AnimationEnd animationEnd { get; private set; } = AnimationEnd.lastSprite;

        [SerializeField] Sprite[] _sprites;
        public Sprite[] Sprites
        {
            get
            {
                if (_sprites == null)
                    Debug.LogWarning("Sprite array is null or length is 0: " + name);
                if (_sprites.Length == 0)
                    Debug.LogWarning("Sprite array is null or length is 0: " + name);
                return _sprites;
            }
        }

        public int Length
        {
            get { return _sprites.Length; }
        }

        public enum AnimationEnd
        {
            initialSprite,
            firstSprite,
            lastSprite,
            empty
        }
    }
}
