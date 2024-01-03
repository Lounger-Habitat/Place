using System.Collections.Generic;
using UnityEngine;

public class ConsoleAreaManager : MonoBehaviour
{
    // TeamManager teamManager;
    // PixelsContainer pixelsContainer;


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // 确保触发器有一个特定的标签
        {
            // 角色刚刚进入触发器
            Debug.Log("角色进入触发器");
            // 如果角色在此队伍区域内

            // 检查角色是否在队伍区域内
            
            PlayerFSM stateMachine = other.transform.root.gameObject.GetComponent<PlayerFSM>();
            stateMachine.ChangeState(CharacterState.WaitingForCommandExecutionAtConsole);
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