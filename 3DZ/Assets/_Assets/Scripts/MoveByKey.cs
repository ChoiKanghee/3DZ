using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveByKey : MonoBehaviour
{
    public CharacterController characterController;
    public float movingSpeed = 5f;

    [Header("Jump Settings")]
    public float jumpHeight = 2f;   // độ cao nhảy
    public float gravity = -9.81f;  // trọng lực

    private Vector3 velocity;       // vector vận tốc (dùng cho trục Y)
    private bool isGrounded;

    private void OnValidate() => characterController = GetComponent<CharacterController>();

    private void Update()
    {
        // --- Kiểm tra nhân vật có đứng trên mặt đất không ---
        isGrounded = characterController.isGrounded;
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // giữ nhân vật dính mặt đất
        }

        // --- Di chuyển ngang ---
        float hInput = Input.GetAxis("Horizontal");
        float vInput = Input.GetAxis("Vertical");
        Vector3 direction = transform.right * hInput + transform.forward * vInput;
        characterController.Move(direction * movingSpeed * Time.deltaTime);

        // --- Nhảy ---
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // --- Gravity ---
        velocity.y += gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
