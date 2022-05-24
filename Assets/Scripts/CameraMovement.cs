using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    float minPanSpeed = 10;
    [SerializeField]
    float panSpeed = 20;

    float verticalMovement, horizontalMovement;
    Vector3 movement;

    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        verticalMovement = Input.GetAxisRaw("Vertical");
        horizontalMovement = Input.GetAxisRaw("Horizontal");
        camera.orthographicSize -= Input.mouseScrollDelta.y;
        panSpeed -= Input.mouseScrollDelta.y;

        if (panSpeed < minPanSpeed)
            panSpeed = minPanSpeed;

        movement = new Vector2(horizontalMovement, verticalMovement).normalized * panSpeed;

        transform.Translate(movement * Time.deltaTime);
    }
}
