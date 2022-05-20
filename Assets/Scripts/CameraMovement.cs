using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    float panSpeed = 10;

    float verticalMovement, horizontalMovement;
    Vector3 movement;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        verticalMovement = Input.GetAxisRaw("Vertical");
        horizontalMovement = Input.GetAxisRaw("Horizontal");

        movement = new Vector2(horizontalMovement, verticalMovement).normalized * panSpeed;

        transform.Translate(movement * Time.deltaTime);
    }
}
