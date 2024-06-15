using UnityEngine;
using DG.Tweening;
using System.Collections;
using Unity.VisualScripting;

public class EffectAutoDelete : MonoBehaviour
{
    [SerializeField] 

    public float scale = 1f;

    void OnEnable()
    {
        transform.localScale = new Vector3(0, 0, 0);
        transform.DOScale(new Vector3(scale,scale,scale), 1f);
    }

    // void Start()
    // {
    //     transform.localScale = new Vector3(0, 0, 0);
    //     transform.DOScale(new Vector3(scale,scale,scale), 1f);
    // }

    public void DoDestroy(float time) {
        StartCoroutine(Destroy(time));
        
    }
    IEnumerator Destroy(float time) {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    public void ReStart()
    {
        transform.localScale = new Vector3(0, 0, 0);
        transform.DOScale(new Vector3(scale,scale,scale), 1f);
    }

    public void ReBlessing()
    {
        // transform.position = new Vector3(0, 2, 0);
        transform.localScale = new Vector3(0, 0, 0);
        // transform.DOBlendableLocalMoveBy(Vector3.zero, 1f);
        transform.DOScale(new Vector3(scale,scale,scale), 1f);
    }

    void OnDisable()
    {
        // 取消此物体上的所有协程
        StopAllCoroutines();
    }
}
