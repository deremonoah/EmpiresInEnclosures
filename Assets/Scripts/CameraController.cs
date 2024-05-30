using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraSpeed;
    public float sprintSpeed;
    private float currentSpeed;
    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        { currentSpeed = sprintSpeed; }
        else { currentSpeed = cameraSpeed; }
        if(Input.GetAxisRaw("Horizontal")>0.5f ||Input.GetAxisRaw("Horizontal")<-0.5f)
        {
            transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * currentSpeed * Time.deltaTime,0f,0f));
        }
    }
}
