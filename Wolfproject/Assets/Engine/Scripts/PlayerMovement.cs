using UnityEngine;

public class PlayerMovement : Singleton<PlayerMovement>
{
    public bool FacingLeft { get { return facingLeft; } set { facingLeft = value; } }

    public float moveSpeed = 5f;
    public float fastMoveSpeed = 10f;
    public float fastMoveDuration = 1f; 
    public float normalAnimationSpeed = 1f; 
    public float fastAnimationSpeed = 2f; 
    public GameObject Trail;

    private float currentMoveSpeed;
    private Vector3 targetPosition;
    public bool isMoving = false;
    private float lastClickTime = 0f;
    private float catchTime = 0.25f; 

    private Rigidbody2D rb;
    private float fastMoveEndTime = 0f;

    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;

    private bool facingLeft = false;

    protected override void Awake()
    {
        base.Awake();

        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time - lastClickTime < catchTime)
            {
                // 더블 클릭
                MoveToTarget(fastMoveSpeed, fastMoveDuration);
                myAnimator.speed = fastAnimationSpeed;
                Trail.SetActive(true);
            }
            else
            {
                // 싱글 클릭
                MoveToTarget(moveSpeed, 0f);
                myAnimator.speed = normalAnimationSpeed;
            }
            lastClickTime = Time.time;
        }

        // 플레이어 이동
        if (isMoving)
        {
            float step = currentMoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
                myAnimator.SetBool("Walk", false); 
                myAnimator.speed = normalAnimationSpeed; 
            }
        }

        if (Time.time >= fastMoveEndTime && currentMoveSpeed != moveSpeed)
        {
            currentMoveSpeed = moveSpeed;
            myAnimator.speed = normalAnimationSpeed;
            Trail.SetActive(false); 
        }
    }

    void MoveToTarget(float speed, float duration)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f; 
        targetPosition = mousePosition;
        currentMoveSpeed = speed; 
        isMoving = true;
        myAnimator.SetBool("Walk", true); 

        if (duration > 0f)
        {
            fastMoveEndTime = Time.time + duration;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            rb.velocity = Vector2.zero;
            isMoving = false; 
            myAnimator.SetBool("Walk", false);
            myAnimator.speed = normalAnimationSpeed; 
            Trail.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        AdjustPlayerFacingDirection();
        if (isMoving)
        {
            Vector2 direction = (targetPosition - (Vector3)rb.position).normalized;
            rb.velocity = direction * currentMoveSpeed;

            if (Vector2.Distance(rb.position, targetPosition) < 0.1f)
            {
                rb.velocity = Vector2.zero;
                isMoving = false;
                myAnimator.SetBool("Walk", false); 
                myAnimator.speed = normalAnimationSpeed; 
                Trail.SetActive(false); 
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void AdjustPlayerFacingDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x)
        {
            mySpriteRenderer.flipX = true;
            facingLeft = true;
        }
        else
        {
            mySpriteRenderer.flipX = false;
            facingLeft = false;
        }
    }
}
