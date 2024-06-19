using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlaceArtItem : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public ArtInfo artInfo;
    public Image artImage;
    public Sprite artSprite;
    ProGifPlayerImage pgpi;

    public GameObject like;
    public GameObject unLike;
    public GameObject artName;
    // Start is called before the first frame update
    void Start()
    {
        // png to sprite
        // read png
        // Debug.Log("ArtInfo: " + artInfo.artTexturePath);    
        artSprite = LoadPNGAsSprite(artInfo.artTexturePath);
        artName.GetComponent<TMP_Text>().text = artInfo.artName;
    }

    Sprite LoadPNGAsSprite(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError("File not found: " + filePath);
            return null;
        }

        // 读取 PNG 文件的字节数据
        byte[] fileData = File.ReadAllBytes(filePath);

        // 创建一个空的 Texture2D 对象
        Texture2D texture = new Texture2D(2, 2);

        // 加载图片数据到 Texture2D 对象
        if (texture.LoadImage(fileData))
        {
            // 将 Texture2D 转换为 Sprite
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
        else
        {
            Debug.LogError("Failed to load texture from file data.");
            return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisPlayArt()
    {
        StartCoroutine(WaitForDisplay());
    }
    IEnumerator WaitForDisplay()
    {
        while (artSprite == null)
        {
            yield return null;
        }
        artImage.sprite = artSprite;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (pgpi != null)
        {
            pgpi.enabled = true;
        }
        ShowGIF(artInfo.artPath,artImage);
        // artImage.sprite = artSprite;
        Debug.Log("Mouse Enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (pgpi == null)
        {
            pgpi = GetComponentInChildren<ProGifPlayerImage>();
        }
        pgpi.enabled = false;
        artImage.sprite = artSprite;
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

    public void ClickLike()
    {
        unLike.SetActive(false);
        like.SetActive(true);
    }

    public void CancelLike()
    {
        like.SetActive(false);
        unLike.SetActive(true);
    }




}
