using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuntimeCameraController : MonoBehaviour
{
    public float rotationSpeed = 2f;
    public float moveSpeed = 5f;
    public float zoomSpeed = 5f;

    private bool isPanning = false;
    private Vector3 panOrigin;

    void Update()
    {
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        // 鼠标右键旋转
        if (Input.GetMouseButton(1))
        {
            float rotationX = Input.GetAxis("Mouse Y") * rotationSpeed;
            float rotationY = -Input.GetAxis("Mouse X") * rotationSpeed;

            transform.Rotate(rotationX, rotationY, 0f);
        }

        // 按住滚轮平移
        if (Input.GetMouseButtonDown(2))
        {
            isPanning = true;
            panOrigin = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(2))
        {
            isPanning = false;
        }

        if (isPanning)
        {
            Vector3 offset = panOrigin - Input.mousePosition;
            offset = new Vector3(offset.x, 0, offset.y);

            transform.Translate(offset * moveSpeed * Time.deltaTime);
            panOrigin = Input.mousePosition;
        }

        // 滚轮滚动缩放
        float zoomInput = Input.GetAxis("Mouse ScrollWheel");
        transform.Translate(Vector3.forward * zoomInput * zoomSpeed * Time.deltaTime, Space.Self);
    }
}
