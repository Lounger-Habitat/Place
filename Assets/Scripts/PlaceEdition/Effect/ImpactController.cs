using System.Collections;
using UnityEngine;

public class ImpactController : MonoBehaviour
{
    [SerializeField] public bool usePool = true;
    [SerializeField] private float destroyTime = 1f;


    void OnEnable()
    {
        if (usePool)
        {
            StartCoroutine(WaitForDisableCoroutine(destroyTime));
            Reset();
        }

    }

    private void Reset()
    {
        
    }

    private void Start()
    {
        if (!usePool)
        {
            StartCoroutine(WaitForDestroyCoroutine(destroyTime));
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    // 销毁
    IEnumerator WaitForDestroyCoroutine(float destroyTime = 1)
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }

    // 禁用
    IEnumerator WaitForDisableCoroutine(float disableTime = 1)
    {
        yield return new WaitForSeconds(disableTime);
        gameObject.SetActive(false);
    }

}
