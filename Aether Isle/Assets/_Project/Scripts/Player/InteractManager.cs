using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class InteractManager : MonoBehaviour
    {
        List<IInteractable> interactables = new List<IInteractable>();

        public IInteractable currentInteractable; // Test

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactables.Add(interactable);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                if (interactables.Contains(interactable))
                    interactables.Remove(interactable);
            }
        }

        // Test
        private void Update()
        {
            IInteractable newInteractable = GetClosestInteractable();
            if (newInteractable != currentInteractable)
            {
                currentInteractable?.Material.SetFloat("_Outline", 0);
                currentInteractable = newInteractable;
                currentInteractable?.Material.SetFloat("_Outline", 1);
            }

            if (InputManager.Instance.input.Game.Interact.WasPressedThisFrame())
                currentInteractable?.Interact(null);
        }

        public IInteractable GetClosestInteractable()
        {
            IEnumerable<IInteractable> possibleInteractables = interactables
                .Where(i => i.CanInteract())
                .OrderBy(i => ((Vector2)transform.position - i.Position).sqrMagnitude);

            return possibleInteractables.FirstOrDefault();
        }
    }
}
