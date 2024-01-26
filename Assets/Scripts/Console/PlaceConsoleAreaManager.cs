using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceConsoleAreaManager : MonoBehaviour
{

    public GameObject ConsoleAreaName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 沿sin曲线上下浮动
        transform.position = new Vector3(transform.position.x, transform.position.y + Mathf.Sin(Time.time) * 0.001f, transform.position.z);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 确保触发器有一个特定的标签
        {

            // 角色刚刚进入触发器
            PlayerController pc = other.transform.root.gameObject.GetComponent<PlayerController>();
            string name = pc.user.username;
            Debug.Log(name + " 进入触发器");

            // 检查角色是否在队伍区域内
            
            PlayerFSM stateMachine = other.transform.root.gameObject.GetComponent<PlayerFSM>();
            if (pc.user.currentState == CharacterState.TransportingCommandToConsole)
            {
                stateMachine.ChangeState(CharacterState.WaitingForCommandExecutionAtConsole);
            }
            
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 角色刚刚离开触发器
            Debug.Log("角色离开触发器");
        }
    }
}
