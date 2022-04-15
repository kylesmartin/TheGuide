using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement instance;

    // public variables
    public float speed = 12f;
    public float gravity = -19.62f;
    public float groundCheckRadius = 0.4f;
    public float jumpHeight = 3f;
    public Transform groundCheck;
    public LayerMask groundLayer;

    // private variables
    private Vector3 velocity;
    private CharacterController controller;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object");
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        velocity = Vector3.zero;
        controller = GetComponent<CharacterController>();    
    }

    // Update is called once per frame
    void Update()
    {
        // reset downward velocity if grounded
        bool _isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);
        if (_isGrounded && velocity.y < 0) velocity.y = -2f;
        // apply movement inputs
        float _x = Input.GetAxis("Horizontal");
        float _z = Input.GetAxis("Vertical");
        Vector3 _move = _x * transform.right + _z * transform.forward;
        controller.Move(_move * speed * Time.deltaTime);
        // apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}