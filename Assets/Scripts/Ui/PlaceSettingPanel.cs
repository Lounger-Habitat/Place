using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceSettingPanel : MonoBehaviour
{
    public GameObject placeSettingUI;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCloseSetting()
    {
        placeSettingUI.SetActive(false);
    }
}
