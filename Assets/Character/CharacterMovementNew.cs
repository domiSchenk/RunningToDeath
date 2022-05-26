using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
// using UnityEngine.InputSystem;

public class CharacterMovementNew : MonoBehaviour
{

    [SerializeField]
    private float gravity = -9.81f;
    [SerializeField]
    private float gravityGrounded = -0.05f;
    [SerializeField]
    private float WalkSpeed = 2.0f;

    [SerializeField]
    private float RunSpeed = 6.0f;

    [SerializeField]
    private float rotationFactorPerFrame = 15.0f;



    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;

    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    [Tooltip("Acceleration and deceleration")]
    public float SpeedChangeRate = 10.0f;

    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;

    // player
    private float _speed;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;


    // timeout deltatime
    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;

    private PlayerInput playerInput;
    private CharacterController characterController;
    private Animator animator;


    private Vector2 currentMovementInput;
    private Vector3 currentMovement;
    private Vector3 currentRunMovement;
    private bool isMovementPressed;
    private bool isRunPressed;
    private bool isJumpPressed;
    public bool canJump;


    private int speedHash;
    private int jumpHash;
    private int groundedHash;
    private int freeFallHash;
    private int motionSpeedHash;

    private Camera mainCamera;

    void Awake()
    {
        playerInput = new PlayerInput();
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        AssignAnimationIDs();

        playerInput.CharacterControls.Move.started += onMovementInput;
        playerInput.CharacterControls.Move.canceled += onMovementInput;
        playerInput.CharacterControls.Move.performed += onMovementInput;
        playerInput.CharacterControls.Run.started += onRun;
        playerInput.CharacterControls.Run.canceled += onRun;
        playerInput.CharacterControls.Jump.started += onJump;
        playerInput.CharacterControls.Jump.canceled += onJump;

        mainCamera = Camera.main;
    }

    void onRun(InputAction.CallbackContext context)
    {
        isRunPressed = context.ReadValueAsButton();
    }
    void onJump(InputAction.CallbackContext context)
    {
        isJumpPressed = context.ReadValueAsButton();
    }

    void onMovementInput(InputAction.CallbackContext context)
    {
        currentMovementInput = context.ReadValue<Vector2>();
        currentMovement.x = currentMovementInput.x * WalkSpeed;
        // currentMovement.z = currentMovementInput.y;
        currentRunMovement.x = currentMovementInput.x * RunSpeed;
        // currentRunMovement.z = currentMovementInput.y * runMultiplier;
        isMovementPressed = currentMovementInput.x != 0 /*|| currentMovementInput.y != 0*/;
    }

    void handelAnimation()
    {
        // var isWalking = animator.GetBool(isWalkingHash);
        // var isRunning = animator.GetBool(isRunningHash);

        // if (isMovementPressed && !isWalking)
        // {
        //     // animator.SetBool(isWalkingHash, true);
        // }
        // else if (!isMovementPressed && isWalking)
        // {
        //     // animator.SetBool(isWalkingHash, false);
        // }

        // if (isMovementPressed && isRunPressed && !isRunning)
        // {
        //     // animator.SetBool(isRunningHash, true);
        // }
        // else if ((!isMovementPressed && !isRunPressed) && isRunning)
        // {
        //     // animator.SetBool(isRunningHash, false);
        // }
        // if ((!isMovementPressed || !isRunPressed) && isRunning)
        // {
        //     //animator.SetBool(isRunningHash, false);
        // }
    }

    void handleJumpAndGravity()
    {
        if (characterController.isGrounded)
        {

            // reset the fall timeout timer
            _fallTimeoutDelta = FallTimeout;

            animator.SetBool(groundedHash, true);
            animator.SetBool(jumpHash, false);
            animator.SetBool(freeFallHash, false);

            // stop our velocity dropping infinitely when grounded
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            if (isJumpPressed && _jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * gravity);
                animator.SetBool(jumpHash, true);
            }
            canJump = true;

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            _jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                animator.SetBool(freeFallHash, true);
            }

            // if we are not grounded, do not jump
            canJump = false;
        }

        // var gravityComputed = characterController.isGrounded ? gravityGrounded : gravity;

        // currentMovement.y = gravityComputed;
        // currentRunMovement.y = gravityComputed;

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += gravity * Time.deltaTime;
        }
    }

    void handleRotation()
    {
        var positionToLookAt = new Vector3(currentMovement.x, 0.0f, currentMovement.z);
        // positionToLookAt.Normalize();
        // positionToLookAt.x = currentMovement.x;
        // positionToLookAt.y = 0.0f;
        // positionToLookAt.z = currentMovement.z;

        Quaternion currentRotation = transform.rotation;

        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
    }

    // Update is called once per frame
    void Update()
    {
        handleJumpAndGravity();
        handleRotation();
        handelAnimation();
        handleMovement();
    }

    private void handleMovement()
    {
        float targetSpeed = isRunPressed ? RunSpeed : WalkSpeed;

        if (currentMovementInput == Vector2.zero) { targetSpeed = 0.0f; }

        Vector3 inputDirection = new Vector3(currentMovementInput.x, 0.0f, currentMovementInput.y).normalized;

        if (currentMovementInput != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;

            // rotate to face input direction relative to camera position
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;


        _animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        // move the player
        // characterController.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
        //                  new Vector3(currentMovementInput.x * targetSpeed, _verticalVelocity, currentMovementInput.y * targetSpeed) * Time.deltaTime);
        var movement = isRunPressed ? currentRunMovement : currentMovement;
        characterController.Move(movement * Time.deltaTime +
                             new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

        animator.SetFloat(speedHash, _animationBlend);
        animator.SetFloat(motionSpeedHash, 1f);
    }

    // GroundCheck

    void OnEnable()
    {
        playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        playerInput.CharacterControls.Disable();
    }

    private void AssignAnimationIDs()
    {
        speedHash = Animator.StringToHash("Speed");
        jumpHash = Animator.StringToHash("Jump");
        groundedHash = Animator.StringToHash("Grounded");
        freeFallHash = Animator.StringToHash("FreeFall");
        motionSpeedHash = Animator.StringToHash("MotionSpeed");
    }

    // private void GroundedCheck()
    // {
    //     // set sphere position, with offset
    //     Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
    //     Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
    //         QueryTriggerInteraction.Ignore);

    //     // update animator if using character
    //     if (_hasAnimator)
    //     {
    //         _animator.SetBool(_animIDGrounded, Grounded);
    //     }
    // }

}
