using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("MoveSpeed variables")]
    [SerializeField]
    private float cameraSpeed;
    [SerializeField]
    private float sprintSpeed;
    private float currentSpeed;

    [Header("Zoom variables")]
    [SerializeField] private float minZoom;
    [SerializeField] private float maxZoom;
    [SerializeField] private float zoomSpeed;
    private float currentZoom;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        //camera movement
        if(Input.GetKey(KeyCode.LeftShift))
        { currentSpeed = sprintSpeed; }
        else { currentSpeed = cameraSpeed; }
        if(Input.GetAxisRaw("Horizontal")>0.5f ||Input.GetAxisRaw("Horizontal")<-0.5f)
        {
            transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * currentSpeed * Time.deltaTime,0f,0f));
        }
        if (Input.GetAxisRaw("Vertical") > 0.5f || Input.GetAxisRaw("Vertical") < -0.5f)
        {
            transform.Translate(new Vector3( 0f, Input.GetAxisRaw("Vertical") * currentSpeed * Time.deltaTime, 0f));
        }

        //zoom
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        

        if(scrollInput!=0)
        {
            //Debug.Log(scrollInput);

            float newsize = cam.orthographicSize - scrollInput * zoomSpeed;

            cam.orthographicSize = Mathf.Clamp(newsize, minZoom, maxZoom);
        }
    }
}
