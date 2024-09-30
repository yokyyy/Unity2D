using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpPower = 16f;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private LayerMask groundLayer;

    private float horizontal;
    private bool isFacingRight = true;
    private bool doubleJump;
    private bool canDash = true;
    private bool isDashing;

    private Rigidbody2D rb;
    private Animator anim;
    private BoxCollider2D boxCollider;

    private void Awake()
{
    rb = GetComponent<Rigidbody2D>();
    anim = GetComponent<Animator>();
    boxCollider = GetComponent<BoxCollider2D>();
    rb.gravityScale = 7; 
}

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        // ����� �������� ������, ���� �������� �� ����� � �� ������ ������ ������
        if (isGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        // ��������� ������
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded() || doubleJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                doubleJump = !doubleJump;
                anim.SetTrigger("Jump");
            }
        }

        // ����������� ������ ������ ��� ���������� ������
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        // ��������� ����
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        // ��������� ����������� ���������
        Flip();

        // ��������� ���������� ��������
        anim.SetBool("Run", horizontal != 0);
        anim.SetBool("grounded", isGrounded());
    }

  private void FixedUpdate()
{
    if (isDashing)
    {
        return;
    }

    rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

    // Удаляем установку gravityScale из FixedUpdate
}

    private void Flip()
    {
        if ((isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f))
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size,
            0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    // ���������� ����� canAttack ����� �������� �������� �� �����
    public bool canAttack()
    {
        return horizontal == 0 && isGrounded();
    }
    public void ResetJumpState()
{
    doubleJump = false;
    isDashing = false;
    canDash = true;
}

}