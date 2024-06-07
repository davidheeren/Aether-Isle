using UnityEngine;
using Utilities;

namespace Game
{
    public class AttackTest : MonoBehaviour
    {
        [SerializeField] LayerMask layerMask;
        [SerializeField] float hitSpeed;

        Material lastMat;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (layerMask.Compare(collision.gameObject.layer) && collision.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                rb.velocity = (Vector2)transform.right * hitSpeed;

                lastMat = collision.gameObject.GetComponentInChildren<SpriteRenderer>().material;
                lastMat.SetFloat("_Contrast", 1);
                Invoke(nameof(SetContrastOff), 0.25f);
            }
        }

        void SetContrastOff()
        {
            lastMat.SetFloat("_Contrast", 0);
        }
    }
}
