using UnityEngine;

namespace Game
{
    public class Chest : InteractableMB
    {
        [SerializeField] Sprite openChestSprite;
        [SerializeField] AudioClip openChestSFX;
        Sprite closeChestSprite;

        bool isOpened = false;

        protected override void Awake()
        {
            base.Awake();

            closeChestSprite = spriteRenderer.sprite;
        }

        public override void EnterInteract(ActorComponents playerComponents)
        {
            base.EnterInteract(playerComponents);

            if (!isOpened)
                spriteRenderer.sprite = openChestSprite;
            else
                spriteRenderer.sprite = closeChestSprite;

            isOpened = !isOpened;

            SFXManager.Instance.PlaySFXClip(openChestSFX, transform.position);
        }
    }
}
