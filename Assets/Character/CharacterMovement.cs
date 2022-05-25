using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{

    private Animator animator;
    private int isWalkingHash;
    private int isRunningHash;
    private int isSneakHash;
    private int isJumpHash;
    private PlayerInput input;
    private Vector2 currentMovement;
    private bool movementPressed;
    private bool sneakPressed;
    private bool runPressed;
    private bool jumpPressed;

    [SerializeField]
    public CharacterController controller;

    [SerializeField]
    public float _speed = 12f;


    void Awake()
    {
        input = new PlayerInput();

        // input.CharacterControls.Movement.started += ctx => currentMovement = ctx.ReadValue<Vector2>();
        // input.CharacterControls.Movement.performed += ctx => currentMovement = ctx.ReadValue<Vector2>();
        // input.CharacterControls.Movement.canceled += ctx => currentMovement = ctx.ReadValue<Vector2>();
        // input.CharacterControls.Run.started += ctx => runPressed = ctx.ReadValueAsButton();
        // input.CharacterControls.Run.performed += ctx => runPressed = ctx.ReadValueAsButton();
        // input.CharacterControls.Run.canceled += ctx => runPressed = ctx.ReadValueAsButton();
        // input.CharacterControls.Jump.started += ctx => jumpPressed = ctx.ReadValueAsButton();
        // input.CharacterControls.Jump.performed += ctx => jumpPressed = ctx.ReadValueAsButton();
        // input.CharacterControls.Jump.canceled += ctx => jumpPressed = ctx.ReadValueAsButton();
        // input.CharacterControls.Sneak.started += ctx => sneakPressed = ctx.ReadValueAsButton();
        // input.CharacterControls.Sneak.performed += ctx => sneakPressed = ctx.ReadValueAsButton();
        // input.CharacterControls.Sneak.canceled += ctx => sneakPressed = ctx.ReadValueAsButton();
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isSneakHash = Animator.StringToHash("isSneaking");
        isJumpHash = Animator.StringToHash("isJumping");
    }

    // Update is called once per frame
    void Update()
    {
        handleMovement();
    }


    void handleMovement()
    {

        float x = currentMovement.x;
        float z = currentMovement.y;
        Vector3 move = transform.forward * x;
        controller.Move(move * _speed * Time.deltaTime);

        movementPressed = x != 0 || z != 0;

        var isRunning = animator.GetBool(isRunningHash);
        var isWalking = animator.GetBool(isWalkingHash);
        var isJumping = animator.GetBool(isJumpHash);
        var isSneaking = animator.GetBool(isSneakHash);


        if (sneakPressed && !isSneaking)
        {
            animator.SetBool(isSneakHash, true);
        }

        if (!sneakPressed && isSneaking)
        {
            animator.SetBool(isSneakHash, false);
        }

        // Jumping only if not jumping and not sneaking
        if (jumpPressed && !isJumping && !isSneaking)
        {
            animator.SetBool(isJumpHash, true);
        }



        if (movementPressed && !isWalking)
        {
            animator.SetBool(isWalkingHash, true);
        }

        if (!movementPressed && isWalking)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if ((movementPressed && runPressed) && !isRunning)
        {
            animator.SetBool(isRunningHash, true);
        }

        if ((!movementPressed || !runPressed) && isRunning)
        {
            animator.SetBool(isRunningHash, false);
        }
    }


    void OnEnable()
    {
        input.CharacterControls.Enable();
    }

    void OnDisable()
    {
        input.CharacterControls.Disable();
    }

}
