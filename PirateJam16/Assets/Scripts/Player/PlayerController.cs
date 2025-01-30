using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rigidBody;

    public SpriteRenderer spriteRenderer;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private LayerMask groundLayer;

    private float horizontal;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float walkingSpeed;

    [SerializeField]
    private float runningSpeed;

    [SerializeField]
    private float jumpingPower;

    private bool isJumping;

    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    [SerializeField]
    private float fallMultiplier;
    
    [SerializeField]
    private float interactRange;

    [SerializeField]
    private LayerMask itemLayerMask;

    [SerializeField]
    private bool isActivePlayer;

    [SerializeField]
    private Transform otherPlayer;

    private PlayerController otherPlayerController;

    [SerializeField]
    private float initialFollowSpeed;
    private float currentFollowSpeed;

    [SerializeField]
    private float followXOffset;

    private PowerUps powerUps;
    private int facingDirection = 1;
    private float holdingShotTime;
    private bool isHoldingShoot;

    [SerializeField]
    private float recoilForce;

    [HideInInspector]
    public bool pushedByRecoil;

    void Start(){
        rigidBody = GetComponent<Rigidbody2D>();
        powerUps = GetComponent<PowerUps>();
        otherPlayerController = otherPlayer.GetComponent<PlayerController>();
        currentFollowSpeed = initialFollowSpeed;
    }

    private void FixedUpdate()
    {
        if(isActivePlayer){
            if(!isHoldingShoot && !pushedByRecoil){
                rigidBody.velocity = new Vector2(horizontal * speed, rigidBody.velocity.y);
            }

            if(rigidBody.velocity.y < 0){
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * fallMultiplier);
            }
        } else{
            MoveWeaponTowardsPlayer();
        }
    }

    private void Update()
    {
        if(!isActivePlayer) return;

        horizontal = Input.GetAxisRaw("Horizontal");

        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (coyoteTimeCounter > 0f && jumpBufferCounter > 0f && !isJumping)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpingPower);

            jumpBufferCounter = 0f;

            StartCoroutine(JumpCooldown());
        }

        if (Input.GetButtonUp("Jump") && rigidBody.velocity.y > 0f)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, rigidBody.velocity.y * 0.5f) * Time.deltaTime;

            coyoteTimeCounter = 0f;
        }

        if(Input.GetKeyDown(KeyCode.E)){
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange, itemLayerMask);
            foreach(Collider collider in colliderArray){
                //Debug.Log(collider); //This shows the object that is colliding to
                // if(collider.TryGetComponent(out NPCInteractable interactableObject)){
                //     interactableObject.Interact(); //Call the method thad controls what happen when E is pressed
                // }
            } 
        }

        if (Input.GetKey(KeyCode.LeftShift)){
            speed = runningSpeed;
        } else{
            speed = walkingSpeed;
        }

        CheckFacingDirection();

        if(Input.GetKey(KeyCode.Mouse0)){
            isHoldingShoot = true;
            rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
            holdingShotTime += Time.deltaTime;
        }

        if(Input.GetKeyUp(KeyCode.Mouse0)){
            isHoldingShoot = false;
            powerUps.ShootBullet(facingDirection, holdingShotTime);
            if(holdingShotTime >= powerUps.chargedShotTime && !IsGrounded()){
                pushedByRecoil = true;
                otherPlayerController.pushedByRecoil = true;
                rigidBody.AddForce(new Vector2(recoilForce * -facingDirection, 0), ForceMode2D.Impulse);
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, 0);
            }
            holdingShotTime = 0;
        }

        if(pushedByRecoil && IsGrounded()){
            otherPlayerController.pushedByRecoil = false;
            pushedByRecoil = false;
        }
    }

    private IEnumerator JumpCooldown()
    {
        isJumping = true;
        yield return new WaitForSeconds(0.4f);
        isJumping = false;
    }

    private void MoveWeaponTowardsPlayer()
	{
        CheckFacingDirection();
        Vector3 followPosition = otherPlayer.position;
        followPosition.x += followXOffset * facingDirection;
        if(pushedByRecoil){
            currentFollowSpeed = initialFollowSpeed * 3;
        } else{
            currentFollowSpeed = initialFollowSpeed;
        }
        var step = currentFollowSpeed * Time.fixedDeltaTime;
        transform.position = Vector3.MoveTowards(transform.position, followPosition, step);
	}

    private void CheckFacingDirection(){
        if(horizontal != 0 && horizontal < 0){
            spriteRenderer.flipX = true;
            facingDirection = -1;
        }else if(horizontal != 0 && horizontal > 0){
            spriteRenderer.flipX = false;
            facingDirection = 1;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}
