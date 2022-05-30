using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class HeroCharacterController : MonoBehaviour
{
    [SerializeField] private float gravity = -40f;
    [SerializeField] private float jumpHeight = 4.5f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float runSpeed = 13f;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController characterController;

    private Vector3 velocity;


    private bool isGrounded;
    private bool isJumping;


    private int speedHash;
    private int verticalSpeedHash;
    private int jumpHash;
    private int groundedHash;


    private float horizontalInput;
    private bool jumpPressed;
    private float jumpTimer;
    private float jumpGracePeriod = 10.0f;
    private float coyoteTimeCounter;
    bool leftControl = false;

    // Start is called before the first frame update
    void Awake()
    {
        AssignAnimationIDs();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        checkHarakiri();
    }

    private void checkHarakiri()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
            leftControl = true;
        if (Input.GetKeyUp(KeyCode.LeftControl))
            leftControl = false;

        if (leftControl && Input.GetKeyUp(KeyCode.H))
        {
            ArchievementManager.instance.ShowArchivement(ArchievementManager.Archievements.HarakiriGoal);
            Destroy(this.gameObject);
            LevelManager.instance.Respawn();
        }
    }
    void Move()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        float horizontalMultiplier = Input.GetKey(KeyCode.LeftShift) ? moveSpeed : runSpeed;

        isGrounded = characterController.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
            coyoteTimeCounter -= Time.deltaTime;
        }

        var speed = horizontalInput * horizontalMultiplier;
        velocity.x = speed;

        jumpPressed = Input.GetButtonDown("Jump");

        if (Input.GetButtonUp("Jump") && velocity.y > 0f)
        {
            coyoteTimeCounter = 0f;
        }

        if (jumpPressed)
        {
            jumpTimer = Time.time;

        }

        if (coyoteTimeCounter > 0f && (jumpPressed || (jumpTimer > 0f && Time.time < jumpGracePeriod)))
        {
            velocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpTimer = -1;
        }

        float rotation = horizontalInput < 0 ? 270 : 90;
        player.rotation = Quaternion.Euler(0, rotation, 0);
        velocity.z = 0;

        characterController.Move(velocity * Time.deltaTime);
        animator.SetFloat(speedHash, Mathf.Abs(velocity.x));
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
