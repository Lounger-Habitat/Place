using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsGroupManager : MonoBehaviour
{
    List<Transform> tips_transform = new List<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        // 获取 自身 下得 image 
        foreach (Transform t in this.transform)
        {
            if (t.name.StartsWith("tip"))
            {
                tips_transform.Add(t);
                t.gameObject.SetActive(false);
            }
        }

        StartCoroutine(StartLoop());
    }

    IEnumerator StartLoop()
    {
        while (true)
        {
            foreach (var t in tips_transform)
            {
                t.gameObject.SetActive(true);
                yield return new WaitForSeconds(5f);
                t.gameObject.SetActive(false);
            }
        }
    }
}
