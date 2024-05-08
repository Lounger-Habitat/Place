using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerFSM : MonoBehaviour
{
    private IState currentState;
    private Dictionary<CharacterState, IState> states;


    public GameObject player;

    public List<Instruction> insList = new List<Instruction>();

    void Awake()
    {
        // DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        player = transform.gameObject;
        states = new Dictionary<CharacterState, IState>
        {
            { CharacterState.Trance, new Trance(this) },
            { CharacterState.WaitingForCommandInTeamArea, new WaitingForCommandInTeamArea(this) },
            { CharacterState.TransportingCommandToConsole, new TransportingCommandToConsole(this) },
            { CharacterState.WaitingForCommandExecutionAtConsole, new WaitingForCommandExecutionAtConsole(this) },
            { CharacterState.MoveToTeamArea, new MoveToTeamArea(this) },
            // 添加其他状态s
        };

        ChangeState(CharacterState.MoveToTeamArea);
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
    Trance,
    WaitingForCommandInTeamArea,
    TransportingCommandToConsole,
    WaitingForCommandExecutionAtConsole,
    MoveToTeamArea
}


public interface IState
{
    void OnEnter();
    void OnExit();
    void Update();
}

public class Trance : IState
{
    private PlayerFSM manager;

    private PlayerController pc;

    public Trance(PlayerFSM playerFSM)
    {
        this.manager = playerFSM;
    }

    public void OnEnter()
    {
        // Debug.Log("进入WaitingForCommandInTeamArea状态");
        // manager.player.GetComponent<Animator>().SetBool("isRun",false);
        pc = manager.player.GetComponent<PlayerController>();
        Debug.Log(pc);
        pc.playerAnimator.SetBool("isRun", false);
        // pc.user.currentState = CharacterState.Trance;
    }
    public void OnExit() { /* 清理逻辑 */ }
    public void Update()
    {
        // 如果没事就去等颜料
        if (pc.user.currentCarryingInkCount == 0)
        {
            manager.ChangeState(CharacterState.WaitingForCommandInTeamArea);
        }
    }

}

public class WaitingForCommandInTeamArea : IState
{
    private PlayerFSM manager;

    private PlayerController pc;

    public WaitingForCommandInTeamArea(PlayerFSM playerFSM)
    {
        this.manager = playerFSM;
    }

    public void OnEnter()
    {
        // Debug.Log("进入WaitingForCommandInTeamArea状态");
        // manager.player.GetComponent<Animator>().SetBool("isRun",false);
        pc = manager.player.GetComponent<PlayerController>();
        Debug.Log(pc);
        pc.playerAnimator.SetBool("isRun", false);
        // pc.user.currentState = CharacterState.WaitingForCommandInTeamArea;
    }
    public void OnExit() { /* 清理逻辑 */ }
    public void Update()
    {
        int teamInkCount = PlaceCenter.Instance.GetTeamInkCount(pc.user.Camp);
        if (pc.user.instructionQueue.Count > 0 && teamInkCount > 0)
        {
            // 遍历instructionQueue ，取出全部 Instruction
            // Debug.Log("队列中有命令");
            for (int i = 0; i < pc.user.instructionQueue.Count; i++)
            {
                Instruction instruction = pc.user.instructionQueue.Dequeue();

                // 判断指令颜料 和 当前有的数量 是否满足
                int needInkCount = PlaceCenter.Instance.ComputeInstructionColorCount(instruction);
                instruction.needInkCount = needInkCount;

                Debug.Log("needInkCount" + needInkCount);



                // if (needInkCount > teamInkCount)
                // {
                //     // 颜料不足
                //     Debug.Log("颜料不足");
                //     // 持续等待
                //     sleep = true;
                //     manager.StartCoroutine(SleepCoroutine());

                // }

                pc.user.currentCarryingInkCount += needInkCount;

                PlaceCenter.Instance.SetTeamInkCount(pc.user.Camp, -needInkCount);


                manager.insList.Add(instruction);


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

    public void OnEnter()
    {
        // Debug.Log("进入WaitingForCommandInTeamArea状态");
        // manager.player.GetComponent<Animator>().SetBool("isRun",false);
        pc = manager.player.GetComponent<PlayerController>();
        pc.playerAnimator.SetBool("isRun", true);
        pc.targetPosition = pc.consolePosition.transform.position;
        // pc.user.currentState = CharacterState.TransportingCommandToConsole;

    }
    public void OnExit() { /* 清理逻辑 */ }
    public void Update()
    {
        Vector3 v = new Vector3(pc.targetPosition.x, 0.25f, pc.targetPosition.z);
        pc.MoveToTarget(v);
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

    public void OnEnter()
    {
        // Debug.Log("进入WaitingForCommandInTeamArea状态");
        // manager.player.GetComponent<Animator>().SetBool("isRun",false);
        pc = manager.player.GetComponent<PlayerController>();
        pc.playerAnimator.SetBool("isRun", false);
        pc.targetPosition = pc.consolePosition.transform.position;
        // pc.user.currentState = CharacterState.WaitingForCommandExecutionAtConsole;

    }
    public void OnExit() { /* 清理逻辑 */ }
    public void Update()
    {

        if (manager.insList.Count > 0)
        {
            foreach (Instruction ins in manager.insList)
            {
                // Debug.Log(instruction.mode);
                // 这里调用绘制像素的逻辑
                // PixelsContainer.Instance.DrawPixel(instruction.x, instruction.y, instruction.r, instruction.g, instruction.b);
                switch (ins.mode)
                {
                    case "/draw":
                    case "/d":
                        PlaceBoardManager.Instance.DrawCommand(ins.x, ins.y, ins.r, ins.g, ins.b, pc.user.Camp);
                        pc.user.currentCarryingInkCount -= ins.needInkCount;
                        pc.user.score += ins.needInkCount;
                        break;
                    case "/line":
                    case "/l":
                        PlaceBoardManager.Instance.LineCommand(ins.x, ins.y, ins.ex, ins.ey, ins.r, ins.g, ins.b, pc.user.Camp);
                        pc.user.currentCarryingInkCount -= ins.needInkCount;
                        pc.user.score += ins.needInkCount;
                        break;
                    case "/paint":
                    case "/p":
                        PlaceBoardManager.Instance.PaintCommand(ins.mode, ins.x, ins.y, ins.dx, ins.dy, ins.r, ins.g, ins.b, pc.user.Camp);
                        pc.user.currentCarryingInkCount -= ins.needInkCount;
                        pc.user.score += ins.needInkCount;
                        break;
                    default:
                        break;
                }
            }
            if (pc.user.currentCarryingInkCount != 0)
            {
                Debug.Log("账不对啊");
                pc.user.currentCarryingInkCount = 0;
            }
            manager.insList.Clear();
        }


        if (manager.insList.Count == 0)
        {
            manager.ChangeState(CharacterState.MoveToTeamArea);
        }
    }
}

public class MoveToTeamArea : IState
{
    private PlayerFSM manager;

    private PlayerController pc;

    public MoveToTeamArea(PlayerFSM playerFSM)
    {
        this.manager = playerFSM;
    }

    public void OnEnter()
    {
        // Debug.Log("进入WaitingForCommandInTeamArea状态");
        // manager.player.GetComponent<Animator>().SetBool("isRun",false);
        pc = manager.player.GetComponent<PlayerController>();
        pc.playerAnimator.SetBool("isRun", true);
        pc.targetPosition = pc.homePosition;
        // pc.user.currentState = CharacterState.MoveToTeamArea;

    }
    public void OnExit() { /* 清理逻辑 */ }
    public void Update()
    {
        Vector3 v = new Vector3(pc.targetPosition.x, 0.25f, pc.targetPosition.z);
        pc.MoveToTarget(v);
    }
}