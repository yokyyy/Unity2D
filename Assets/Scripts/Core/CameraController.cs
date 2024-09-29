using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 offset = new Vector3(0f, 0f, -10f);
    private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;

    private void LateUpdate()
    {
        if (target == null)
            return;

        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    // Метод для мгновенного перемещения камеры к новой позиции
    public void MoveToNewRoom(Transform roomTransform)
    {
        if (roomTransform == null)
        {
            Debug.LogError("roomTransform is null in MoveToNewRoom.");
            return;
        }

        // Установка позиции камеры к центру комнаты плюс смещение
        transform.position = roomTransform.position + offset;
        // Сбросить скорость для SmoothDamp
        velocity = Vector3.zero;
        Debug.Log("Camera moved to new room: " + roomTransform.name);
    }
}
