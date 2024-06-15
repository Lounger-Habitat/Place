using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceFire : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            GameObject fire = PlacePoolManager.Instance.GetInkInstance();
            fire.transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0);
            fire.SetActive(true);
            fire.GetComponent<EffectAutoDelete>().DoDestroy(2f);
        }
    }
}
