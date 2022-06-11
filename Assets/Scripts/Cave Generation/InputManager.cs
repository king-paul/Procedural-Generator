using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Camera camera3d;
    public Camera camera2d;
    public GameObject uIPanel;

    [SerializeField]
    float cameraMoveSpeed = 3;

    private Vector3 horizontalMovement;
    private Vector3 verticalMovement;
    private Vector3 velocity;

    public bool Is2D { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        Is2D = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel") || Input.GetMouseButtonDown(1))
        {
            uIPanel.SetActive(!uIPanel.activeInHierarchy);
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            if (Is2D)
                camera2d.orthographicSize -= Input.mouseScrollDelta.y;
            else
                camera3d.fieldOfView -= Input.mouseScrollDelta.y;
        }

        horizontalMovement = Vector3.right * Input.GetAxisRaw("Horizontal");
        verticalMovement = Vector3.up * Input.GetAxisRaw("Vertical");
        velocity = (horizontalMovement + verticalMovement) * cameraMoveSpeed;

        if(Is2D)
            camera2d.transform.Translate(velocity * Time.deltaTime);
        else
            camera3d.transform.Translate(velocity * Time.deltaTime);
    }
}
