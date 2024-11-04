using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Rigidbody2D theRb;

    [SerializeField]
    private Animator anim;

    [Header("Interaction")]
    [SerializeField]
    private InputActionReference moveInput;

    [Header("Player Properties")]
    [SerializeField]
    [Range(1f, 10f)]
    private float moveSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        theRb.linearVelocity = moveInput.action.ReadValue<Vector2>().normalized * moveSpeed;

        anim.SetFloat("Speed", theRb.linearVelocity.magnitude);
    }

}
