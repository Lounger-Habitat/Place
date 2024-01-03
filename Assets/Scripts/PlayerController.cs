using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Vector3 targetPosition; // 目标位置
    public GameObject consolePosition;
    public Vector3 homePosition;
    public float moveSpeed = 5f; // 移动速度
    public User user;


    
    public Animator playerAnimator;
    void Start()
    {
        // 在Start中初始化位置
        // homePosition = transform.position;
        targetPosition = homePosition;
        //获取 consoleObject
        consolePosition = GameObject.Find("Console");

        // stateMachine = GetComponent<PlayerFSM>();

    }
    void Update()
    {
        // // 在Update中更新位置
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     targetPosition = consolePosition.transform.position;
        // }else if (Input.GetKeyDown(KeyCode.B))
        // {
        //     // Debug.Log("按下了B键");
        //     targetPosition = homePosition;
        // }
        // MoveToTarget(targetPosition);

        // // 如果
        // user.Update();
    }

    public void MoveToTarget(Vector3 targetPosition)
    {
        // 如果已经到达目标位置，可以在这里执行其他操作，比如销毁对象或者触发事件
        // if (transform.position == targetPosition)
        // {
        //     // Debug.Log("已到达目标位置！");
        //     // 在这里执行其他操作...
        //     playerAnimator.SetBool("isRun",false);
        //     return;
        // }
        // 沿着目标位置移动
        transform.LookAt(targetPosition);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        // playerAnimator.SetBool("isRun",true);
    }
}
