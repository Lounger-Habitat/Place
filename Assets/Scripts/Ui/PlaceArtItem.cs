using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlaceArtItem : MonoBehaviour
{
    public ArtInfo artInfo;
    public Image artImage;
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
        ShowGIF(artInfo.artPath,artImage);
    }

    void OnMouseEnter()
    {
        Debug.Log("Mouse Enter");
    }

    void OnMouseExit()
    {
        Debug.Log("Mouse Exit");
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
