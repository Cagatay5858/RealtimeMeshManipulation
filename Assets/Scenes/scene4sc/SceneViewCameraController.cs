using UnityEngine;

public class FreeCameraController : MonoBehaviour
{
    public Transform player; // Oyuncunun referansý
    public float mouseSensivity = 100f;
    float cameraVerticalRotation = 0;
    public float movementSpeed = 10f;
    public float cameraDistance = 2f; // Kameranýn oyuncudan uzaklýðý

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // Mouse inputlarýný al
        float inputX = Input.GetAxis("Mouse X") * mouseSensivity * Time.deltaTime;
        float inputY = Input.GetAxis("Mouse Y") * mouseSensivity * Time.deltaTime;

        // Kameranýn dikey eksende dönmesini kontrol et
        cameraVerticalRotation -= inputY;
        cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);

        // Oyuncunun etrafýnda döndür
        player.Rotate(Vector3.up * inputX);

        // Kamera dikey rotasyonunu güncelle
        transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        // Kamerayý oyuncunun etrafýna yerleþtir
        Vector3 cameraOffset = new Vector3(0, 0, -cameraDistance);
        transform.position = player.position + player.rotation * cameraOffset;

        // Oyuncunun hareketi
        float moveX = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        float moveZ = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;

        Vector3 move = player.right * moveX + player.forward * moveZ;
        player.position += move;
    }
}
