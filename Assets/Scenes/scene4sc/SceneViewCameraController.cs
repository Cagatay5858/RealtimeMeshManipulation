using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    public Transform player; // Oyuncunun referans�
    public float mouseSensivity = 100f;
    float cameraVerticalRotation = 0;
    public float movementSpeed = 10f;
    public float cameraDistance = 2f; // Kameran�n oyuncudan uzakl���

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Mouse inputlar�n� al
        float inputX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        float inputY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;

        // Kameran�n dikey eksende d�nmesini kontrol et
        cameraVerticalRotation -= inputY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);

        // Oyuncunun etraf�nda d�nd�r
        player.Rotate(Vector3.up * inputX);

        // Kamera dikey rotasyonunu g�ncelle
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        // Kameray� oyuncunun etraf�na yerle�tir
        Vector3 cameraOffset = new Vector3(0, 0, -cameraDistance);
        transform.position = player.position + player.rotation * cameraOffset;

        // Oyuncunun hareketi
        float moveX = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;

        Vector3 move = player.right * moveX + player.forward * moveZ;
        player.position += move;
    }
}
