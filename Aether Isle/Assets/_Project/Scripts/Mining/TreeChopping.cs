using UnityEngine;
using UnityEngine.InputSystem;

public class TreeChopping2D : MonoBehaviour
{
    public GameObject woodPrefab; 
    public int woodDropCount = 3; 

    private InputAction attackAction;

    private void Awake()
    {
      
        var playerInput = new PlayerInput();
        attackAction = InputManager.Instance.input.Game.Attack;

        // Subscribe to the performed event
        attackAction.performed += OnAttack;
    }

    private void OnEnable()
    {
        attackAction.Enable();
    }

    private void OnDisable()
    {
        attackAction.Disable();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag("Tree"))
        {
            DestroyTree(hit.collider.gameObject);
        }
    }

    private void DestroyTree(GameObject tree)
    {
        
        for (int i = 0; i < woodDropCount; i++)
        {
            Instantiate(woodPrefab, tree.transform.position + (Vector3)Random.insideUnitCircle * 0.5f, Quaternion.identity);
        }

        // Destroy the tree
        Destroy(tree);
    }
}
