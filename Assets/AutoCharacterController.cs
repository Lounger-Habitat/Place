using UnityEngine;
public abstract class AutoCharacterController : MonoBehaviour
{
    // 是否正在攻击
    public bool isAttacking;
    // 死亡
    public abstract void Die();
}