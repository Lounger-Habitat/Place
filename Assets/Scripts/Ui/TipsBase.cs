using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//提示面板基类
public class TipsBase : MonoBehaviour
{
    public TipsType panelType;

    
    
    public virtual void MoveTipsPanel(bool isOn = true){}
    public virtual void TestMenu(){}
    //写一些通用方法，如赋值用户数据
    public TMP_Text userNameTextBase;//用户名称text组件
    public TMP_Text tipsTextBase;//提示消息text组件
    public Image userIconImageBase;//用户头像image

    public virtual void SetData(TipsItem tips)
    {
        if (userNameTextBase!=null)
        {
            userNameTextBase.text = tips.userName;
        }
        if (tipsTextBase!=null)
        {
            tipsTextBase.text = tips.text;
        }
        if (userIconImageBase!=null)
        {
            userIconImageBase.sprite = tips.icon;
        }
    }
}
