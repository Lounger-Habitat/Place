using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSDestory : MonoBehaviour
{
    public ParticleSystem ps;

    public void Init()
    {
        var mainModule = ps.main;
        mainModule.stopAction = ParticleSystemStopAction.Callback;
    }

    void OnParticleSystemStopped()
    {
        Debug.Log("粒子系统播放完成！");
        var psAudio = gameObject.GetComponent<ParticleEndSound>();
        psAudio.isDestroy = true;
        psAudio.StopAllCoroutines();
        gameObject.SetActive(false);
        // 在这里编写粒子播放结束时需要执行的代码
        Destroy(gameObject);
    }
}
