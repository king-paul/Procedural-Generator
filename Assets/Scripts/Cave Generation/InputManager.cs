using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Camera camera;
    public GameObject uIPanel;

    [SerializeField]
    float cameraMoveSpeed = 3;

    private Vector3 horizontalMovement;
    private Vector3 verticalMovement;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel") || Input.GetMouseButtonDown(1))
        {
            uIPanel.SetActive(!uIPanel.activeInHierarchy);
        }

        if (Input.mouseScrollDelta.y != 0)
            camera.fieldOfView -= Input.mouseScrollDelta.y;

        horizontalMovement = Vector3.right * Input.GetAxisRaw("Horizontal");
        verticalMovement = Vector3.up * Input.GetAxisRaw("Vertical");
        velocity = (horizontalMovement + verticalMovement) * cameraMoveSpeed;

        camera.transform.Translate(velocity * Time.deltaTime);
    }
}
