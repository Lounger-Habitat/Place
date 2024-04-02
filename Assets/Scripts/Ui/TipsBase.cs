using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//提示面板基类
public class TipsBase : MonoBehaviour
{
    public TipsType panelType;

    public virtual void SetData(TipsItem tips) { }
    
    public virtual void MoveTipsPanel(bool isOn = true){}
}
