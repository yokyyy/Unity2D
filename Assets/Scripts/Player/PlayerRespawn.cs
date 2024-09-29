using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpoint;
    private Transform currentCheckpoint;
    private Health playerHealth;
    private UIManager uiManager;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        uiManager = FindObjectOfType<UIManager>();
        if (playerHealth == null)
        {
            Debug.LogError("Health ��������� �� ������ �� ������.");
        }
    }

    public void CheckRespawn()
    {
        if (currentCheckpoint == null)
        {
            uiManager.GameOver();
            return;
        }

        // ����������� ������ � ������� ��������� ����� ������� Respawn()
        transform.position = currentCheckpoint.position;
        Debug.Log($"Player moved to checkpoint position: {currentCheckpoint.position}");

        playerHealth.Respawn(); // ������������ �������� � �������� ��������
        Debug.Log("Player health restored.");

        // ����������� ������ � ������� ���������
        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.MoveToNewRoom(currentCheckpoint.parent);
            Debug.Log($"Camera moved to room: {currentCheckpoint.parent.name}");
        }
        else
        {
            Debug.LogError("CameraFollow ��������� �� ������ �� �������� ������.");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Checkpoint"))
        {
            currentCheckpoint = collision.transform;
            collision.GetComponent<Collider2D>().enabled = false;
            Animator checkpointAnimator = collision.GetComponent<Animator>();
            if (checkpointAnimator != null)
            {
                checkpointAnimator.SetTrigger("appear");
            }
            else
            {
                Debug.LogWarning("Animator �� ������ �� ���������.");
            }

            // ��������������� ����� ���������, ���� ����������
            if (checkpoint != null)
            {
                AudioSource.PlayClipAtPoint(checkpoint, transform.position);
            }

            Debug.Log($"Checkpoint activated: {currentCheckpoint.name}");
        }
    }
}
