using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private float vertical;
    private float speed = 8f;
    private bool isLadder;
    private bool isClimbing;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator; // Ссылка на Animator

    void Update()
    {
        vertical = Input.GetAxisRaw("Vertical");

        if (isLadder)
        {
            if (Mathf.Abs(vertical) > 0f)
            {
                isClimbing = true;
            }
            else
            {
                isClimbing = false;
            }
        }
        else
        {
            isClimbing = false;
        }

        // Устанавливаем параметр Animator
        animator.SetBool("isClimbing", isClimbing);
    }

    private void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(0, vertical * speed);
        }
        else
        {
            rb.gravityScale = 4f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
            Debug.Log("Вошел на лестницу");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
            Debug.Log("Вышел с лестницы");
        }
    }
}
