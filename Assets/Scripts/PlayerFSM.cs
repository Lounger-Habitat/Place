using System.Collections.Generic;
using UnityEngine;
public class PlayerFSM : MonoBehaviour
{
    private IState currentState;
    private Dictionary<CharacterState, IState> states;


    public GameObject player;

    public List<Instruction> li = new List<Instruction>();

    void Awake()
    {
        // DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        player = transform.gameObject;
        states = new Dictionary<CharacterState, IState>
        {
            { CharacterState.WaitingForCommandInTeamArea, new WaitingForCommandInTeamArea(this) },
            { CharacterState.TransportingCommandToConsole, new TransportingCommandToConsole(this) },
            { CharacterState.WaitingForCommandExecutionAtConsole, new WaitingForCommandExecutionAtConsole(this) },
            { CharacterState.ReturningFromConsoleToGetCommand, new ReturningFromConsoleToGetCommand(this) },
            // 添加其他状态s
        };

        ChangeState(CharacterState.WaitingForCommandInTeamArea);
    }

    void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(CharacterState newState)
    {
        currentState?.OnExit();
        currentState = states[newState];
        currentState?.OnEnter();
    }
}


public enum CharacterState
{
    WaitingForCommandInTeamArea,
    TransportingCommandToConsole,
    WaitingForCommandExecutionAtConsole,
    ReturningFromConsoleToGetCommand
}


public interface IState
{
    void OnEnter();
    void OnExit();
    void Update();
}

public class WaitingForCommandInTeamArea : IState
{
    private PlayerFSM manager;

    private PlayerController pc;


    public WaitingForCommandInTeamArea(PlayerFSM playerFSM)
    {
        this.manager = playerFSM;
    }

    public void OnEnter() { 
        // Debug.Log("进入WaitingForCommandInTeamArea状态");
        // manager.player.GetComponent<Animator>().SetBool("isRun",false);
        pc = manager.player.GetComponent<PlayerController>();
        Debug.Log(pc);
        pc.playerAnimator.SetBool("isRun",false);
     }
    public void OnExit() { /* 清理逻辑 */ }
    public void Update() { 
        if (pc.user.instructionQueue.Count > 0)
        {
            // 遍历instructionQueue ，取出全部 Instruction
            // Debug.Log("队列中有命令");
            for (int i = 0; i < pc.user.instructionQueue.Count; i++)
            {
                Instruction instruction = pc.user.instructionQueue.Dequeue();
                // Debug.Log(instruction.mode);
                // 这里调用绘制像素的逻辑
                // PixelsContainer.Instance.DrawPixel(instruction.x, instruction.y, instruction.r, instruction.g, instruction.b);
                manager.li.Add(instruction);
                Debug.Log("x:"+instruction.x + ",y:"+instruction.y+",r:"+instruction.r+",g:"+ instruction.g+",b:"+ instruction.b);
                // Debug.Log(instruction.x + instruction.y+ instruction.r+ instruction.g+ instruction.b);

            }
            manager.ChangeState(CharacterState.TransportingCommandToConsole);
        }
     }
}


public class TransportingCommandToConsole : IState
{
    private PlayerFSM manager;

    private PlayerController pc;

    public TransportingCommandToConsole(PlayerFSM playerFSM)
    {
        this.manager = playerFSM;
    }

    public void OnEnter() { 
        // Debug.Log("进入WaitingForCommandInTeamArea状态");
        // manager.player.GetComponent<Animator>().SetBool("isRun",false);
        pc = manager.player.GetComponent<PlayerController>();
        pc.playerAnimator.SetBool("isRun",true);
        pc.targetPosition = pc.consolePosition.transform.position;

     }
    public void OnExit() { /* 清理逻辑 */ }
    public void Update() { 
        pc.MoveToTarget(pc.targetPosition);
    }
}

public class WaitingForCommandExecutionAtConsole : IState
{
    private PlayerFSM manager;

    private PlayerController pc;

    public WaitingForCommandExecutionAtConsole(PlayerFSM playerFSM)
    {
        this.manager = playerFSM;
    }

    public void OnEnter() { 
        // Debug.Log("进入WaitingForCommandInTeamArea状态");
        // manager.player.GetComponent<Animator>().SetBool("isRun",false);
        pc = manager.player.GetComponent<PlayerController>();
        pc.playerAnimator.SetBool("isRun",false);
        pc.targetPosition = pc.consolePosition.transform.position;

     }
    public void OnExit() { /* 清理逻辑 */ }
    public void Update() { 

        if (manager.li.Count > 0)
        {
            foreach (Instruction instruction in manager.li)
            {
                // Debug.Log(instruction.mode);
                // 这里调用绘制像素的逻辑
                // PixelsContainer.Instance.DrawPixel(instruction.x, instruction.y, instruction.r, instruction.g, instruction.b);
                Debug.Log("x:"+instruction.x + ",y:"+instruction.y+",r:"+instruction.r+",g:"+ instruction.g+",b:"+ instruction.b);
                PixelsCanvasController.Instance.DrawCommand(instruction.mode,instruction.x,instruction.y,instruction.r,instruction.g,instruction.b);

            }
            manager.li.Clear();
        }
        

        if (manager.li.Count == 0)
        {
            manager.ChangeState(CharacterState.ReturningFromConsoleToGetCommand);
        }
    }
}

public class ReturningFromConsoleToGetCommand : IState
{
    private PlayerFSM manager;

    private PlayerController pc;

    public ReturningFromConsoleToGetCommand(PlayerFSM playerFSM)
    {
        this.manager = playerFSM;
    }

    public void OnEnter() { 
        // Debug.Log("进入WaitingForCommandInTeamArea状态");
        // manager.player.GetComponent<Animator>().SetBool("isRun",false);
        pc = manager.player.GetComponent<PlayerController>();
        pc.playerAnimator.SetBool("isRun",true);
        pc.targetPosition = pc.homePosition;

     }
    public void OnExit() { /* 清理逻辑 */ }
    public void Update() { 
        pc.MoveToTarget(pc.targetPosition);
    }
}