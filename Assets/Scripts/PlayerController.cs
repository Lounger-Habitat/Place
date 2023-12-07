using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 targetPosition; // 目标位置
    public GameObject consolePosition;
    private Vector3 homePosition;
    public float moveSpeed = 5f; // 移动速度

    void Start()
    {
        // 在Start中初始化位置
        homePosition = transform.position;
        targetPosition = homePosition;
        Debug.Log("homePosition:" + homePosition);
        Debug.Log("homePosition:" + targetPosition);
    }
    void Update()
    {
        // 在Update中更新位置
        if (Input.GetKeyDown(KeyCode.Space))
        {
            targetPosition = consolePosition.transform.position;
        }else if (Input.GetKeyDown(KeyCode.B))
        {
            Debug.Log("按下了B键");
            targetPosition = homePosition;
        }
        MoveToTarget(targetPosition);
    }

    void MoveToTarget(Vector3 targetPosition)
    {
        // 沿着目标位置移动
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 如果已经到达目标位置，可以在这里执行其他操作，比如销毁对象或者触发事件
        if (transform.position == targetPosition)
        {
            Debug.Log("已到达目标位置！");
            // 在这里执行其他操作...
        }
    }
}
