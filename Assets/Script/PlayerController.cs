using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirection))]   

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameObject gameOver;
    Vector2 moveInput;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 3f;
    private float jumpImpluse = 8f;
    Rigidbody2D rb;
    [SerializeField]
    private bool _isMoving = false;
    [SerializeField]
    private bool _isRunning = false;
    Animator animator;
    public bool _right = true;
    TouchingDirection touchingDirections;
    Damgeable damgeable;

    public bool IsMoving { 
        get
        {
            return _isMoving;
        }
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationString.isMoving, value);
        }
    }

    public bool IsRunning {
        get 
        { 
            return _isRunning; 
        } 
        set 
        { 
            _isRunning = value;
            animator.SetBool(AnimationString.isRunning, value);
        } 
    }

    public float CurrentMoveSpeed 
    { get
        {
            if (CanMove)
            {
                if (IsMoving && !touchingDirections.IsOnWall)
                {
                    if (touchingDirections.IsGrounded)
                    {
                        if (IsRunning)
                        {
                            return runSpeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else
                    {
                        return airWalkSpeed;
                    }
                }
                else return 0;
            }
            else
            {
                return 0;
            }
        }
      
    }

    public bool right
    { 
        get
        { 
            return _right; 
        } 
        set 
        {  
            if(_right != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _right = value;
        } 
    }

    public bool CanMove { get
        {
            return animator.GetBool(AnimationString.canMove);
        } }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationString.isAlive);
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        touchingDirections = GetComponent<TouchingDirection>();
        damgeable = GetComponent<Damgeable>();
    }

    private void FixedUpdate()
    {
        if(!damgeable.LockVelocity)
            rb.velocity = new Vector2(moveInput.x * CurrentMoveSpeed, rb.velocity.y);
        animator.SetFloat(AnimationString.yVelocity, rb.velocity.y);
        if(!IsAlive)
        {
            gameOver.SetActive(true);
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if(IsAlive && CanMove)
        {
            IsMoving = moveInput != Vector2.zero;

            Rotation(moveInput);
        }
        else
        {
            IsMoving = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.started && CanMove && touchingDirections.IsGrounded)
        {
            IsRunning = true;
        }
        else if(context.canceled)
        {
            IsRunning = false;
        }
    }

    private void Rotation(Vector2 moveinput)
    {
        if(moveInput.x > 0 && !right)
        {
            right= true;
        }
        if(moveInput.x < 0 && right)
        {
            right = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // TODO Check
        if(context.started && touchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationString.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpluse);
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationString.attackTrigger);
        }
    }

    public void OnRangedAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            animator.SetTrigger(AnimationString.rangeAttackTrigger);
        }
    }

    public void OnHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
}
