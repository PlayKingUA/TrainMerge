using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    [SerializeField] DynamicJoystick variableJoystick;

    public static float speed;

    Rigidbody rb;
    public UnityEngine.CharacterController characterController;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed = 4f;
    }

    private void FixedUpdate()
    {
        Vector3 direction = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;

        characterController.Move(direction * speed * Time.fixedDeltaTime);

        float angleY = Mathf.Atan2(variableJoystick.Horizontal, variableJoystick.Vertical) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, angleY, 0f);
    }
}