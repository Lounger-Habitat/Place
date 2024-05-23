using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceArtItem : MonoBehaviour
{
    public ArtInfo artInfo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisPlayArt()
    {

    }
    
    void ShowGIF(string path,Image image)
    {
        ProGifManager.Instance.m_OptimizeMemoryUsage = true;

        //Open the Pro GIF player to show the converted GIF
        ProGifManager.Instance.PlayGif(path, image, (loadProgress) =>
        {
            //Debug.Log("Load Progress: " + loadProgress);
        });
    }
}
