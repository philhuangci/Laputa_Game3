using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MainCamera : MonoBehaviour
{
    private Camera mainCamera;
    private Transform cameraTrans;

    private int xAxisCoefficient = 1;
    private int yAxisCoefficient = 1;

    private string _mouseXString = "Mouse X";
    private string _mouseYString = "Mouse Y";
    private string _mouseScrollWheel = "Mouse ScrollWheel";

    private Vector3 _currentMouseRotate = Vector3.zero;

    public float cameraMin = 30.0f;
    public float cameraMax = 300.0f;
    public float moveSpeed = 3;
    public float sensitivityDrag = 2;
    public float sensitivityRotate = 2;
    public float sensitivetyMouseWheel = 100f;

    private bool xAxisinversion = false;
    private bool yAxisinversion = false;


    // Start is called before the first frame update
    void Start()
    {
        mainCamera = gameObject.GetComponent<Camera>();
        if (mainCamera == null)
        {
            Debug.LogErrorFormat("empty camera !");
            return;
        }
        cameraTrans = mainCamera.transform;

        xAxisCoefficient = xAxisinversion ? -1 : 1;
        yAxisCoefficient = yAxisinversion ? -1 : 1;




    }

    // Update is called once per frame
    void Update()
    {
        if (cameraTrans == null)
            return;
        //KeyBoradControl();
        MouseLeftDragControl();
        MouseRightRotateControl();
        MouseScrollWheelScale();

    }

    private void KeyBoradControl()
    {
        if (Input.GetKey(KeyCode.W))
        {
            cameraTrans.Translate(Vector3.forward * Time.deltaTime * moveSpeed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            cameraTrans.Translate(Vector3.back * Time.deltaTime * moveSpeed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            cameraTrans.Translate(Vector3.left * Time.deltaTime * moveSpeed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            cameraTrans.Translate(Vector3.right * Time.deltaTime * moveSpeed);
        }

    }

    private void MouseLeftDragControl()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 p1 = cameraTrans.position - cameraTrans.right *
                Input.GetAxisRaw(_mouseXString) * sensitivityDrag * Time.timeScale;
            Vector3 p2 = p1 - new Vector3(cameraTrans.forward.x,0,cameraTrans.forward.z) * 
                Input.GetAxisRaw(_mouseYString) * sensitivityDrag * Time.timeScale;
            cameraTrans.position = p2;
        }
    }

    private void MouseRightRotateControl()
    {
        if (Input.GetMouseButton(1))
        {
            _currentMouseRotate.x = 0;
            _currentMouseRotate.y = (Input.GetAxis(_mouseXString) * sensitivityRotate) * yAxisCoefficient;
            cameraTrans.rotation = Quaternion.Euler(cameraTrans.eulerAngles + _currentMouseRotate);
        }
    }

    private void MouseScrollWheelScale()
    {
        if (Input.GetAxis(_mouseScrollWheel) == 0)
            return;

        float mouseScroll = this.sensitivetyMouseWheel * Input.GetAxisRaw (_mouseScrollWheel);
		if (mouseScroll != 0f) 
		{
            Vector3 p =  cameraTrans.position + cameraTrans.forward * mouseScroll * Time.timeScale;
            if(p.y < cameraMin ){
                p = cameraTrans.position;
            } else if (p.y > cameraMax ){
                p = cameraTrans.position;
            }
			cameraTrans.position = p;
		}
    }



}
