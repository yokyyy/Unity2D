using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private AudioClip checkpoint;
    private Transform currentCheckpoint;
    private Health playerHealth;

    private void Awake()
    {
        playerHealth = GetComponent<Health>();
        if (playerHealth == null)
        {
            Debug.LogError("Health компонент не найден на игроке.");
        }
    }

    public void Respawn()
    {
        if (currentCheckpoint == null)
        {
            Debug.LogError("No checkpoint set for respawn.");
            return;
        }

        // Переместить игрока к позиции чекпоинта перед вызовом Respawn()
        transform.position = currentCheckpoint.position;
        Debug.Log($"Player moved to checkpoint position: {currentCheckpoint.position}");

        playerHealth.Respawn(); // Восстановить здоровье и сбросить анимацию
        Debug.Log("Player health restored.");

        // Переместить камеру к комнате чекпоинта
        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollow != null)
        {
            cameraFollow.MoveToNewRoom(currentCheckpoint.parent);
            Debug.Log($"Camera moved to room: {currentCheckpoint.parent.name}");
        }
        else
        {
            Debug.LogError("CameraFollow компонент не найден на основной камере.");
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
                Debug.LogWarning("Animator не найден на чекпоинте.");
            }

            // Воспроизведение звука чекпоинта, если необходимо
            if (checkpoint != null)
            {
                AudioSource.PlayClipAtPoint(checkpoint, transform.position);
            }

            Debug.Log($"Checkpoint activated: {currentCheckpoint.name}");
        }
    }
}
