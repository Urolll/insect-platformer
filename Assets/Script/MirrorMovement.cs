using System.Collections;
using UnityEngine;

public class MirrorMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 3.5f;
    [SerializeField] float jumpSpeed = 8.0f;

    [SerializeField] GameObject bullet;
    [SerializeField] GameObject altBullet;
    GameObject temp;
    [SerializeField] Transform gun;
    [SerializeField] int isMirror = 1;
    Rigidbody2D rgbd2D;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;
    bool isAlive = true;
    [SerializeField] AudioSource deadSoundEffect;

    private Vector2 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        rgbd2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCapsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) { return; }
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");

        //if (isMirror == 1) { moveInput = -1f * moveInput; }

        Run();
        FlipSprite();
        Die();

        if (Input.GetButtonDown("Fire1"))
        {
            Fire();
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Mushroom") {
            temp = altBullet;
            altBullet = bullet;
            bullet = temp;
            if (isMirror == 0) isMirror = 1;
            else isMirror = 0;
        }
    }

    void Fire()
    {
        if (!isAlive) { return; }
        Instantiate(bullet, gun.position, transform.rotation);
        //gun.position.transform.right = transform.right.normalized;
        myAnimator.SetTrigger("Shooting");
    }

    void Jump()
    {
        if (!isAlive) { return; }
        if (!myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return; }
        rgbd2D.velocity += new Vector2(0f, jumpSpeed);
    }

    void Run()
    {
        Vector2 playerVelocity;
        //playerVelocity = new Vector2(moveInput.x * runSpeed, rgbd2D.velocity.y);
        if (isMirror == 1) {
            playerVelocity = new Vector2(moveInput.x * runSpeed * -1f, rgbd2D.velocity.y);
        }
        else {
            playerVelocity = new Vector2(moveInput.x * runSpeed, rgbd2D.velocity.y);
        }
        
        rgbd2D.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(rgbd2D.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            myAnimator.SetBool("isRunning", true);
        }
        else
        {
            myAnimator.SetBool("isRunning", false);
        }
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rgbd2D.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(rgbd2D.velocity.x), 1f);
        }
    }

    void Die()
    {
        if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            deadSoundEffect.Play();

            StartCoroutine(informGameSession());
        }
    }

    IEnumerator informGameSession()
    {
        yield return new WaitForSecondsRealtime(1f);
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }
}
