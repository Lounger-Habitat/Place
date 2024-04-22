using UnityEngine;
using DG.Tweening;

public class EffectAutoDelete : MonoBehaviour
{
    [SerializeField] 
    public float destroyTime = 1f;

    void Start()
    {
        transform.localScale = new Vector3(0, 0, 0);
        transform.DOScale(new Vector3(0.7f,0.7f,0.7f), 1f);
        destroyTime = Mathf.Clamp(destroyTime, 1f, 10f);
        Destroy(gameObject, destroyTime);
    }
}
