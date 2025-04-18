using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterScript : MonoBehaviour
{
    private Animator animator;
    private AudioSource stepsSound;
    private InputAction moveAction;
    private InputAction jumpAction;
    private CharacterController characterController;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 4.0f;
    private float jumpHeight = 1.5f;
    private float gravityValue = -9.81f;
    private AnimationStates prevMoveState = AnimationStates.Idle;
    private bool isAttacking = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        stepsSound = GetComponent<AudioSource>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (isAttacking) return;

        AnimationStates animationState = AnimationStates.Idle;

        groundedPlayer = characterController.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0.0f;
        if (cameraForward != Vector3.zero)
        {
            cameraForward.Normalize();
        }

        Vector3 moveStep = playerSpeed * Time.deltaTime * (
            moveValue.x * Camera.main.transform.right +
            moveValue.y * cameraForward
        );
        if (moveStep.magnitude > 0)
        {
            this.transform.forward = cameraForward;
            if (Input.GetKey(KeyCode.W))
                animationState = AnimationStates.RunForward;
            if (Input.GetKey(KeyCode.A))
                animationState = AnimationStates.RunLeft;
            if (Input.GetKey(KeyCode.D))
                animationState = AnimationStates.RunRight;
            if (Input.GetKey(KeyCode.S))
                animationState = AnimationStates.RunBackward;
        }
        characterController.Move(moveStep);

        // Jump
        if (jumpAction.ReadValue<float>() > 0 && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
            animationState = AnimationStates.Jump;
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

        // Attack input (����� ������ ����)
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            StartCoroutine(PlayAttackAnimationTimed());
            return;
        }

        if (animationState != prevMoveState)
        {
            animator.SetInteger("AnimationState", (int)animationState);
            prevMoveState = animationState;
            if (animationState == AnimationStates.RunForward || animationState == AnimationStates.RunBackward || animationState == AnimationStates.RunRight || animationState == AnimationStates.RunLeft)
            {
                stepsSound.Play();
            }
            else
            {
                stepsSound.Stop();
            }
        }
    }

    public void SetAnimationState(AnimationStates newState)
    {
        animator.SetInteger("AnimationState", (int)newState);
        prevMoveState = newState;
    }

    public void PlayGatheringAnimationTimed(float duration = 1.2f)
    {
        StartCoroutine(GatherAndReturn(duration));
    }

    private IEnumerator GatherAndReturn(float duration)
    {
        SetAnimationState(AnimationStates.Gather);
        yield return new WaitForSeconds(duration);
        SetAnimationState(AnimationStates.Idle);
    }

    public IEnumerator PlayAttackAnimationTimed(float duration = 0.8f)
    {
        isAttacking = true;
        SetAnimationState(AnimationStates.MeleeAttack);
        yield return new WaitForSeconds(duration);
        SetAnimationState(AnimationStates.Idle);
        isAttacking = false;
    }
}

public enum AnimationStates
{
    Idle = 1,
    Jump = 2,
    RunForward = 3,
    RunRight = 4,
    RunLeft = 5,
    RunBackward = 6,
    Gather = 7,
    MeleeAttack = 8
}