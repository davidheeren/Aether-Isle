using UnityEngine;
using UnityEngine.InputSystem;
using Game;

public class TreeChopping2D : MonoBehaviour
{
    [SerializeField] private GameObject woodPrefab;    
    [SerializeField] private int woodDropCount = 3;    
    [SerializeField] private float rayLength = 5f;    
    [SerializeField] private LayerMask treeLayerMask;   

    private void Awake()
    {
       
        InputManager.Instance.input.Game.Attack.performed += OnAttack;
    }

    private void OnDestroy()
    {
        InputManager.Instance.input.Game.Attack.performed -= OnAttack;
    }


    private void OnAttack(InputAction.CallbackContext context)
    {
        
        Vector2 mousePosition = InputManager.Instance.input.Game.MousePosition.ReadValue<Vector2>();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 direction = (worldPosition - (Vector2)transform.position).normalized;

        Debug.DrawRay(transform.position, direction * rayLength, Color.yellow, 1f); 

       
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, treeLayerMask);

        if (hit.collider != null)
        {
            Debug.Log("Raycast hit object: " + hit.collider.gameObject.name);

            
            if (hit.collider.CompareTag("tree"))
            {
               
                Vector2 treePosition = hit.collider.gameObject.transform.position;

                

                Debug.Log("Tree hit: Disabling and Destroying Tree");

                
                for (int i = 0; i < woodDropCount; i++)
                {
                    Vector2 dropPosition = treePosition + (Vector2)Random.insideUnitCircle * 0.5f;
                    Instantiate(woodPrefab, dropPosition, Quaternion.identity);
                }

                Destroy(hit.collider.gameObject);
            }
        }
        else
        {
            Debug.Log("No objects hit by raycast");
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3.right * rayLength));
    }
}
