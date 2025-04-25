using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static Unity.Collections.Unicode;

public class CharacterScript : MonoBehaviour
{
    private Animator animator;
    private AudioSource stepsSound;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private CharacterController characterController;
    public HealthBarScript healthBar;

    private KeyCode runKey;
    private KeyCode jumpKey;
    private KeyCode attackKey;

    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float playerSpeed = 4.0f;
    private float jumpHeight = 1.5f;
    private float gravityValue = -9.81f;
    private AnimationStates prevMoveState = AnimationStates.Idle;
    private bool isAttacking = false, isGathering = false;

    public float attackRange = 1.5f;

    void Start()
    {
        animator = GetComponent<Animator>();
        stepsSound = GetComponent<AudioSource>();
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        characterController = GetComponent<CharacterController>();

        runKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Run", "LeftShift"));
        jumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Jump", "Space"));
        attackKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Attack", "Mouse0"));
    }

    void Update()
    {
        if (isAttacking || isGathering && !MenuKeybindingsScript.IsMenuOpen) return; 

        AnimationStates animationState = AnimationStates.Idle;

        groundedPlayer = characterController.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        float sprintValue = Input.GetKey(runKey) ? 1.0f : 0.0f;

        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0.0f;
        if (cameraForward != Vector3.zero)
        {
            cameraForward.Normalize();
        }

        Vector3 moveStep = playerSpeed * Time.deltaTime * (1.0f + sprintValue) * (
            moveValue.x * Camera.main.transform.right +
            moveValue.y * cameraForward
        );

        if (moveStep.magnitude > 0)
        {
            this.transform.forward = cameraForward;
            if (Input.GetKey(KeyCode.W))
                animationState = sprintValue > 0 
                    ? AnimationStates.Sprint 
                    : AnimationStates.RunForward;
            if (Input.GetKey(KeyCode.A))
                animationState = AnimationStates.RunLeft;
            if (Input.GetKey(KeyCode.D))
                animationState = AnimationStates.RunRight;
            if (Input.GetKey(KeyCode.S))
                animationState = AnimationStates.RunBackward;
        }
        characterController.Move(moveStep);

        // Jump
        if (Input.GetKeyDown(jumpKey) && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
            animationState = AnimationStates.Jump;
        }


        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

        // Attack input (левая кнопка мыши)
        if (Input.GetKeyDown(attackKey) && !isAttacking && !MenuKeybindingsScript.IsMenuOpen)
        {
            StartCoroutine(PlayAttackAnimationTimed());
            return;
        }

        if (animationState != prevMoveState)
        {
            animator.SetInteger("AnimationState", (int)animationState);
            prevMoveState = animationState;
            if (animationState == AnimationStates.RunForward || animationState == AnimationStates.RunBackward || animationState == AnimationStates.RunRight || animationState == AnimationStates.RunLeft || animationState == AnimationStates.Sprint)
            {
                stepsSound.Play();
            }
            else
            {
                stepsSound.Stop();
            }
        }
    }

    public void SetSpeed(float speed)
    {
        playerSpeed = speed;
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
        isGathering = true;
        SetAnimationState(AnimationStates.Gather);
        yield return new WaitForSeconds(duration);
        SetAnimationState(AnimationStates.Idle);
        isGathering = false;
        healthBar.IncreaseHealth();
    }

    public IEnumerator PlayAttackAnimationTimed(float duration = 0.8f)
    {
        isAttacking = true;
        SetAnimationState(AnimationStates.MeleeAttack);

        yield return new WaitForSeconds(duration - 0.6f);

        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                GoblinScript goblin = hit.GetComponent<GoblinScript>();
                if (goblin != null)
                {
                    goblin.TakeDamage(1);
                }
            }
        }

        //yield return new WaitForSeconds(duration - 0.4f);
        SetAnimationState(AnimationStates.Idle);
        isAttacking = false;
    }
    public void ReloadKeys()
    {
        runKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Run", "LeftShift"));
        jumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Jump", "Space"));
        attackKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Attack", "Mouse0"));
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
    MeleeAttack = 8,
    Sprint = 9
}