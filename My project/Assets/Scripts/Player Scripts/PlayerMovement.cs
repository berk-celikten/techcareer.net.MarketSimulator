using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private AudioSource footstepAudio; // Ad�m sesi i�in bir AudioSource bile�eni


    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float gravity = -9.81f;
    private Vector3 velocity;
    public bool canMove = true;

    private float verticalLookRotation = 0f;
    private CharacterController controller;
    private Transform cam;

    private void Start()
    {
        // Ad�m sesini almak i�in AudioSource bile�enini ba�la
        footstepAudio = GetComponent<AudioSource>();


        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (controller.isGrounded)
        {
            velocity.y = 0f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }

        if (canMove)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 move = transform.right * moveX + transform.forward * moveZ;
            controller.Move(move * moveSpeed * Time.deltaTime);

            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            transform.Rotate(Vector3.up * mouseX);

            verticalLookRotation -= mouseY;
            verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

            cam.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);


            // Ad�m sesini oynatma/durdurma
            if (move.magnitude > 0 && !footstepAudio.isPlaying)
            {
                footstepAudio.Play(); // Hareket varsa ve ses �alm�yorsa ba�lat
            }
            else if (move.magnitude == 0 && footstepAudio.isPlaying)
            {
                footstepAudio.Pause(); // Hareket yoksa sesi durdur
            }


        }
        controller.Move(velocity * Time.deltaTime);
    }
}