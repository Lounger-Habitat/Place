using UnityEngine;
public class AutoHand : MonoBehaviour
{

    private AutoContainer container;
    private AutoHitController hitController;

    public Vector3 Velocity
    {
        set
        {
            for (int i = 0; i < velocities.Length - 1; i++)
            {
                velocities[i + 1] = velocities[i];
            }
            velocities[0] = value;
        }
        get
        {
            Vector3 avg = Vector3.zero;
            foreach (Vector3 vel in velocities)
            {
                avg += vel;
            }
            avg /= velocities.Length;
            return avg;
        }
    }
    public Vector3[] velocities = new Vector3[2];
    private Vector3 oldPos;

    void OnCollisionEnter(Collision col)
    {
        //if (col.impulse == Vector3.zero) return;
        // 获取碰撞体的父级
        Transform targetParent = col.transform.parent;
        // 如果父级为空 或者 父级名字不是 Ragdoll 或者 父级等于自己的父级
        if (targetParent == null || targetParent.name != "Ragdoll" || targetParent == transform.parent) return;
        Debug.Log($"2");
        // 获取碰撞体的父级的AutoContainer
        AutoContainer targetContainer = col.transform.root.GetComponent<AutoContainer>();
        // 如果目标重定向器为空
        AutoHitController targetHitController = targetContainer.hitController;
        Debug.Log($"3");
        // 如果不能打击为空
        if (!hitController.CanHitThisTarget(targetHitController)) return;
        // 如果目标重定向器为空
        Debug.Log($"4");
        if (targetHitController.TakeHit(col.gameObject, col.contacts[0].point, Velocity))
        {
            Debug.Log($"Hit game object : {col.gameObject}");
            hitController.GiveHit(targetHitController);
        }
    }

    // Use this for initialization
    void Start()
    {
        container = transform.root.GetComponent<AutoContainer>();
        hitController = container.hitController;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Velocity = (transform.position - oldPos) / Time.fixedDeltaTime;
        oldPos = transform.position;
    }
}