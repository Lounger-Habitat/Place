using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class ProjectileController : MonoBehaviour
{
    public int camp = 0;
    void OnEnable()
    {
    }

    private void Reset()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
    }

    private void Start()
    {
        // SphereCollider sc = transform.AddComponent<SphereCollider>();
        // sc.isTrigger = true;
        // sc.radius = 0.1f;
        // StartCoroutine(DisableAfterStart());
    }



    // Update is called once per frame
    void Update()
    {

    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    public void Launch(Vector3 end, float time)
    {

        GameObject flashGo;
        if (camp == 1)
        {
            flashGo = PlaceInkPoolManager.Instance.GetBlueInkFlash();
        }
        else
        {
            flashGo = PlaceInkPoolManager.Instance.GetGreenInkFlash();
        }

        flashGo.transform.position = transform.position;
        transform.DOMove(end, time).SetEase(Ease.OutQuint).OnComplete(() =>
         {
             gameObject.SetActive(false);
         });
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

    IEnumerator DisableAfterStart()
    {
        // 执行 Start 方法的内容
        Debug.Log("Start method is executing.");

        // 等待一帧，确保 Start 方法已经执行完
        yield return null;

        // 禁用 GameObject
        gameObject.SetActive(false);

        Debug.Log("GameObject is now disabled.");
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
        if (other.CompareTag("Board"))
        {
            GameObject go;
            if (camp == 1)
            {
                go = PlaceInkPoolManager.Instance.GetBlueInkImpact();
            }
            else
            {
                go = PlaceInkPoolManager.Instance.GetGreenInkImpact();
            }
            
            go.transform.position = transform.position;
            go.transform.rotation = Quaternion.identity;
            go.SetActive(true);

            DOTween.Kill(transform);
            gameObject.SetActive(false);
        }
    }

}
