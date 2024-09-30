using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Chest : MonoBehaviour, IInteractable
    {
        [SerializeField] Sprite openChestSprite;
        [SerializeField] AudioClip openChestSFX;
        Sprite closeChestSprite;
        SpriteRenderer sr;

        Material _material;
        public Material Material => _material;
        public Vector2 Position => transform.position;

        bool isOpened = false;

        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            _material = sr.material;
            closeChestSprite = sr.sprite;
        }

        public bool CanInteract()
        {
            return true;
        }

        public void Interact(ActorComponents playerComponents)
        {
            if (!isOpened)
                sr.sprite = openChestSprite;
            else
                sr.sprite = closeChestSprite;

            isOpened = !isOpened;

            SFXManager.Instance.PlaySFXClip(openChestSFX, transform.position);
        }

        public bool CanContinue()
        {
            return false;
        }

        public void UpdateInteract()
        {
            
        }
    }
}
