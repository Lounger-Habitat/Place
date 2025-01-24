using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanMuUIManager : MonoBehaviour
{

    public static DanMuUIManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public List<string>  happyNewYearTexts= new() {"新年快乐！","财源滚滚！","万事如意！","恭喜发财！","祝你暴富！","健康平安！"};

    public DanMuItem prefab;

    public void ShowHappyNewYearText(User user)
    {
        var item = Instantiate(prefab, transform);
        //新建一个vector2,他的y值取随机值，范围是0~600
        Vector2 randomVector = new Vector2(0, Random.Range(-600, 0));
        (item.transform as RectTransform).anchoredPosition = randomVector;//取随机位置
        var text = happyNewYearTexts[Random.Range(0, happyNewYearTexts.Count)];
        item.Initialize(user, text);

    }
}
