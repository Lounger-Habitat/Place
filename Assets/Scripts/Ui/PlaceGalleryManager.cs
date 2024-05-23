
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlaceGalleryManager : MonoBehaviour
{
    public List<string> artsPath = new List<string>();

    public List<ArtInfo> artsInfo = new List<ArtInfo>(); 


    Dictionary<string,List<Texture2D>> arts = new Dictionary<string, List<Texture2D>>();

    public GameObject artPrefab;

    public string disby = "time";

    public GameObject fof;


/*
    /// 图片保存在 datapath + "/Images/TUID" 文件夹下
    /// TUID 为图片所在的文件夹名
*/
    public void Start() {
        // 位置
        // 读取 json 文件
        // dir : 画廊文件夹
        fof.SetActive(false);
        #if UNITY_EDITOR
        string dir = Application.dataPath + "/Images/Log";
        #else
        string dir = Application.streamingAssetsPath + "/Images/Log";
        #endif

        if (!Directory.Exists(dir))
        {
            Debug.Log("No such directory found.");
            // 创建文件夹
            Directory.CreateDirectory(dir);
            return;
        }


        LoadArts(dir);


        SetLayout();
        // 读取图片 地址
        // 创建 图片 List

        // 显示图片
    }

    public void OpenGallery()
    {
        gameObject.SetActive(true);
        (transform as RectTransform).anchoredPosition = new Vector2(0, 0);
    }

    public void CloseGallery()
    {
        (transform as RectTransform).anchoredPosition = new Vector2(7000, 0);
        gameObject.SetActive(false);
        
    }

    void LoadArts(string rootPath)
    {
        string[] subDirectories = Directory.GetDirectories(rootPath, "*", SearchOption.AllDirectories);
        if (subDirectories.Length == 0)
        {
            Debug.Log("No subdirectories found in the directory.");
            fof.SetActive(true);
            return;
        }
        foreach (string subDir in subDirectories)
        {
            artsPath.Add(subDir);
            arts.Add(subDir,PlaceCenter.Instance.LoadResources(subDir));
            List<string> jsons = LoadJson(subDir);
            if (jsons.Count != 1 )
            {
                Debug.Log("没有或不止一个json,不正确");
            }

            foreach (string json in jsons)
            {
                ArtInfo artInfo = JsonUtility.FromJson<ArtInfo>(json);
                artsInfo.Add(artInfo);
            }
        }
    }


    public List<string> LoadJson(string path)
    {
        List<string> jsons = new List<string>();
        // 检查文件是否存在
        string[] jsonFiles = Directory.GetFiles(path, "*.json", SearchOption.AllDirectories);
        if (jsonFiles.Length > 0)
        {
            Debug.Log("JSON file(s) found:");
            foreach (string jsonFile in jsonFiles)
            {
                string json = File.ReadAllText(jsonFile);
                jsons.Add(json);
            }
        }
        else
        {
            Debug.Log("No JSON files found in the directory.");
        }
        return jsons;
    }

    public void SetLayout()
    {
        // 获取画廊的UI位置
        GameObject gallery = GameObject.Find("ArtContents");

        int artCount = arts.Count;

        if (disby=="time")
        {
            artsInfo.OrderBy(art => art.time);
        }else if (disby=="score")
        {
            artsInfo.OrderBy(art => art.score);
        }else if (disby=="drawTimes")
        {
            artsInfo.OrderBy(art => art.drawTimes);
        }else if (disby=="price")
        {
            artsInfo.OrderBy(art => art.price);
        }

        // 生成prefab
        for (int i = 0; i < artCount; i++)
        {
            GameObject h_content = null;
            if (i%3 == 0) {
                h_content = new GameObject();
                h_content.name = "h_content" + i;
                h_content.transform.parent = gallery.transform;
                h_content.AddComponent<HorizontalLayoutGroup>();
            }
            GameObject art = Instantiate(artPrefab);
            art.transform.parent = h_content.transform;
            art.name = artsInfo[i].artName;
            art.GetComponent<PlaceArtItem>().artInfo = artsInfo[i];
            art.GetComponent<PlaceArtItem>().DisPlayArt();
        }

    }

}

[Serializable]
public struct ArtInfo
{
    public string artName;
    public int score;
    public int drawTimes;
    public float price;
    public List<string> contributors;
    public string artPath;
    public string PUID;
    public string time;

    public ArtInfo(string artName, int score, int drawTimes, float price, List<string> contributors, string artPath, string PUID)
    {
        this.artName = artName;
        this.score = score;
        this.drawTimes = drawTimes;
        this.price = price;
        this.contributors = contributors;
        this.artPath = artPath;
        this.PUID = PUID;
        this.time = DateTime.Now.ToString();
    }
}