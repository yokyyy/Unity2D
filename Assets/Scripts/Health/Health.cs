using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float startingHealth = 100f; // ���������, ��� ����������� �������� �� ���������
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration = 2f;
    [SerializeField] private int numberOfFlashes = 2;
    private SpriteRenderer spriteRend;

    [Header("Components")]
    [SerializeField] private Behaviour[] components;
    private bool invulnerable;

    [Header("Death Sound")]
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip hurtSound;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator �� ������ �� �������.");
        }

        spriteRend = GetComponent<SpriteRenderer>();
        if (spriteRend == null)
        {
            Debug.LogError("SpriteRenderer �� ������ �� �������.");
        }

        if (components == null || components.Length == 0)
        {
            Debug.LogWarning("�� ��������� ���������� ��� ���������� ��� ������.");
        }
    }

    public void TakeDamage(float _damage)
    {
        if (invulnerable) return;

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        Debug.Log($"Player took {_damage} damage. Current Health: {currentHealth}");

        if (currentHealth > 0)
        {
            anim.SetTrigger("hurt");
            StartCoroutine(Invunerability());
        }
        else
        {
            if (!dead)
            {
                Die();
            }
        }
    }

    public void AddHealth(float _value)
    {
        currentHealth = Mathf.Clamp(currentHealth + _value, 0, startingHealth);
        Debug.Log($"Player healed by {_value}. Current Health: {currentHealth}");
    }

    private IEnumerator Invunerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true); // ���������, ��� ���� 10 � 11 ������������� ������ � ������
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }

    private void Die()
    {
        dead = true;
        anim.SetBool("grounded", true);
        anim.SetTrigger("die");

        // ���������� ���� ��������� �����������
        foreach (Behaviour component in components)
        {
            component.enabled = false;
        }

        // ��������������� ����� ������, ���� ��������
        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }

        Debug.Log("Player died.");
    }

    //Respawn
    public void Respawn()
    {
        AddHealth(startingHealth);
        anim.ResetTrigger("die");
        anim.Play("idle");
        StartCoroutine(Invunerability());

        // ������������ ��� ��������� ����������
        foreach (Behaviour component in components)
        {
            component.enabled = true;
        }

        dead = false; // ����� ����� ������
        Debug.Log("Player respawned with full health.");
    }
}
