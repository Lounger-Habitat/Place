using UnityEngine;
using DG.Tweening;

public class EffectAutoDelete : MonoBehaviour
{
    [SerializeField] 

    public float scale = 1f;

    void Start()
    {
        transform.localScale = new Vector3(0, 0, 0);
        transform.DOScale(new Vector3(scale,scale,scale), 1f);
    }

    public void DoDestroy(float time) {
        Destroy(gameObject,time);
    }

    public void ReStart()
    {
        transform.localScale = new Vector3(0, 0, 0);
        transform.DOScale(new Vector3(scale,scale,scale), 1f);
    }

    public void ReBlessing()
    {
        transform.localPosition = new Vector3(0, 2, 0);
        transform.localScale = new Vector3(0, 0, 0);
        transform.DOBlendableLocalMoveBy(Vector3.zero, 1f);
        transform.DOBlendableScaleBy(new Vector3(scale,scale,scale), 1f);
    }
}
