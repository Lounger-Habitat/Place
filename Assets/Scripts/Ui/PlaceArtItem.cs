using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlaceArtItem : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
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
        artImage.sprite = artInfo.artSprite;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowGIF(artInfo.artPath,artImage);
        Debug.Log("Mouse Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        artImage.sprite = artInfo.artSprite;
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
