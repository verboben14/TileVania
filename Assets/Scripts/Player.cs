using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // String reference constants
    const string RUNNING_ANIMATOR_CONDITION = "IsRunning";
    const string CLIMBING_ANIMATOR_CONDITION = "IsClimbing";
    const string DIE_ANIMATOR_TRIGGER = "Die";
    const string GROUND_LAYER_NAME = "Ground";
    const string LADDER_LAYER_NAME = "Climbing";
    const string ENEMY_LAYER_NAME = "Enemy";
    const string HAZARDS_LAYER_NAME = "Hazards";
    const string MOVINGWATER_LAYER_NAME = "MovingWater";
    const string HORIZONTAL_AXIS_NAME = "Horizontal";
    const string VERTICAL_AXIS_NAME = "Vertical";
    const string JUMP_BUTTON_NAME = "Jump";

    // Config parameters
    [SerializeField] float runSpeed = 5f;
    [SerializeField] float jumpSpeed = 10f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(15f, 15f);

    // Cache references
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;
    BoxCollider2D myFeetCollider;
    float originalGravityScale;

    // State
    bool isAlive = true;

    // Start is called before the first frame update
    void Start()
    {
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        originalGravityScale = myRigidBody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAlive)
        {
            Die();
            Run();
            ClimbLadder();
            Jump();
            FlipSprite();
        }
    }

    private void Run()
    {
        float controlThrow = Input.GetAxis(HORIZONTAL_AXIS_NAME);
        Vector2 playerVelocity = new Vector2(controlThrow * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerVelocity;
        ChangeAnimationState(RUNNING_ANIMATOR_CONDITION, HasHorizontalSpeed());
    }

    private void ClimbLadder()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask(LADDER_LAYER_NAME)))
        {
            float controlThrow = Input.GetAxis(VERTICAL_AXIS_NAME);
            Vector2 climbVelocity = new Vector2(myRigidBody.velocity.x, controlThrow * climbSpeed);
            myRigidBody.velocity = climbVelocity;
            myRigidBody.gravityScale = 0;
            ChangeAnimationState(CLIMBING_ANIMATOR_CONDITION, HasVerticalSpeed());
        }
        else
        {
            myRigidBody.gravityScale = originalGravityScale;
            ChangeAnimationState(CLIMBING_ANIMATOR_CONDITION, false);
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown(JUMP_BUTTON_NAME) && myFeetCollider.IsTouchingLayers(LayerMask.GetMask(GROUND_LAYER_NAME)))
        {
            Vector2 jumpVelocityToAdd = new Vector2(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocityToAdd;
        }
    }

    private bool HasHorizontalSpeed()
    {
        return Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
    }

    private bool HasVerticalSpeed()
    {
        return Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;
    }

    private void FlipSprite()
    {
        if (HasHorizontalSpeed())
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), transform.localScale.y);
        }
    }

    private void ChangeAnimationState(string conditionName, bool value)
    {
        myAnimator.SetBool(conditionName, value);
    }

    private void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask(ENEMY_LAYER_NAME))
            || myBodyCollider.IsTouchingLayers(LayerMask.GetMask(HAZARDS_LAYER_NAME))
            || myBodyCollider.IsTouchingLayers(LayerMask.GetMask(MOVINGWATER_LAYER_NAME)))
        {
            isAlive = false;
            myAnimator.SetTrigger(DIE_ANIMATOR_TRIGGER);
            myRigidBody.velocity = deathKick;
            StartCoroutine(FindObjectOfType<GameSession>().ProcessPlayerDeath());
        }
    }
}
