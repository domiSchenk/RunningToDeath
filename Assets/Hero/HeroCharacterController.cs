using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCharacterController : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float gravity = -50f;
    [SerializeField] private float jumpHeight = 3.5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeed = 13f;
    [SerializeField] private Transform[] groundChecks;
    [SerializeField] private Transform[] wallChecks;


    private Animator animator;
    private CharacterController characterController;
    private Vector3 velocity;


    private bool isGrounded;
    private bool isJumping;


    private float horizontalInput;

    private int speedHash;
    private int verticalSpeedHash;
    private int jumpHash;
    private int groundedHash;

    private bool jumpPressed;
    private float jumpTimer;
    private float jumpGracePeriod = 10.0f;

    // Start is called before the first frame update
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        // get animator from child
        animator = GetComponentInChildren<Animator>();
        AssignAnimationIDs();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        float horizontalMultiplier = Input.GetKey(KeyCode.LeftShift) ? runSpeed : moveSpeed;

        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }



        var blocked = false;
        foreach (var wallCheck in wallChecks)
        {
            if (Physics.CheckSphere(wallCheck.position, 0.1f, groundLayer, QueryTriggerInteraction.Ignore))
            {
                blocked = true;
                break;
            }
        }

        if (!blocked)
        {
            var speed = horizontalInput * horizontalMultiplier;
            velocity.x = speed;
            // characterController.Move();
        }

        jumpPressed = Input.GetButtonDown("Jump");

        if (jumpPressed)
        {
            jumpTimer = Time.time;

        }

        if (isGrounded && (jumpPressed || (jumpTimer > 0 && Time.time < jumpGracePeriod)))
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpTimer = -1;
        }


        characterController.Move(velocity * Time.deltaTime);
        animator.SetFloat(speedHash, velocity.x);
        animator.SetFloat(verticalSpeedHash, velocity.y);
        setGrounded(isGrounded);
    }

    private void AssignAnimationIDs()
    {
        speedHash = Animator.StringToHash("Speed");
        verticalSpeedHash = Animator.StringToHash("VerticalSpeed");
        jumpHash = Animator.StringToHash("IsJumping");
        groundedHash = Animator.StringToHash("IsGrounded");
    }

    private void setGrounded(bool isGrounded)
    {
        animator.SetBool(groundedHash, isGrounded);
    }

}
