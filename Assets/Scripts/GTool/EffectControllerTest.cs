using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class EffectControllerTest : MonoBehaviour
{
    public GameObject slowEffect;
    public GameObject runEffect_1;
    public GameObject runEffect_2;
    public GameObject shieldsEffect_1;
    public GameObject shieldsEffect_2;
    public GameObject shieldsEffect_3;
    public GameObject tornadoEffect;
    public GameObject electricityEffect;
    public GameObject strikeEffect;

    public void PlaySlowEffect()
    {
        slowEffect.SetActive(true);
        Invoke("CloseSlowEffect",2f);
    }

    public void CloseSlowEffect()
    {
        slowEffect.SetActive(false);
    }

    public void PlayRunEffect_1()
    {
        runEffect_1.SetActive(true);
        Invoke("CloseRunEffect_1", 2f);
    }

    public void CloseRunEffect_1()
    {
        runEffect_1.SetActive(false);
    }

    public void PlayRunEffect_2()
    {
        runEffect_2.SetActive(true);
        Invoke("CloseRunEffect_2", 2f);
    }

    public void CloseRunEffect_2()
    {
        runEffect_2.SetActive(false);
    }

    public void PlayShieldsEffect_1()
    {
        shieldsEffect_1.SetActive(true);
        Invoke("CloseShieldsEffect_1", 2f);
    }

    public void CloseShieldsEffect_1()
    {
        shieldsEffect_1.SetActive(false);
    }

    public void PlayShieldsEffect_2()
    {
        shieldsEffect_2.SetActive(true);
        Invoke("CloseShieldsEffect_2", 2f);
    }

    public void CloseShieldsEffect_2()
    {
        shieldsEffect_2.SetActive(false);
    }

    public void PlayShieldsEffect_3()
    {
        shieldsEffect_3.SetActive(true);
        Invoke("CloseShieldsEffect_3", 2f);  //测试时自动关闭
    }

    public void CloseShieldsEffect_3()
    {
        shieldsEffect_3.SetActive(false);
    }

    /// ////////////////////////////////////////////////////////////////////////////////////龙卷风效果不是单纯的开启关闭

    public float tornadoRange = 5f;
    public void PlayTornadoEffect()
    {
        //tornadoEffect.SetActive(true);
        //Invoke("CloseTornadoEffect", 2f);  //测试时自动关闭
        
        //首先知道要生成几股龙卷风 随机获得
        int num = Random.Range(2, 5);
        for (int i = 0; i < num; i++)
        {
            float dur = Random.Range(3f, 3.8f);//获得持续时间
            float xdir = Random.Range(-1f, 1f);
            float zdir = Random.Range(-1f, 1f);//分别获得两个方向的
            Vector3 targetPos = transform.position + new Vector3(xdir, 0, zdir).normalized * tornadoRange; //当前位置加上目标方向乘以距离就是目标位置
            GameObject tornado = Instantiate(tornadoEffect, transform.position, Quaternion.identity);
            tornado.SetActive(true);
            tornado.transform.DOMove(targetPos, dur).OnComplete(() =>
            {
               Destroy(tornado.gameObject); 
            });
        }
    }

    public void CloseTornadoEffect()
    {
        tornadoEffect.SetActive(false);
    }
    /// ////////////////////////////////////////////////////////////////////////////////////
    public void PlayElectricityEffect()
    {
        electricityEffect.SetActive(true);
        Invoke("CloseElectricityEffect", 2f);  //测试时自动关闭
    }

    public void CloseElectricityEffect()
    {
        electricityEffect.SetActive(false);
    }

    public void PlayStrikeEffect()
    {
        strikeEffect.SetActive(true);
        Invoke("CloseStrikeEffect", 2f);  //测试时自动关闭
    }

    public void CloseStrikeEffect()
    {
        strikeEffect.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))//龙卷风摧毁停车场
        {
            PlayTornadoEffect();
        }

        if (Input.GetKeyDown(KeyCode.X)) //电磁范围
        {
            PlayElectricityEffect();
        }

        if (Input.GetKeyDown(KeyCode.C)) //雷劈
        {
            PlayStrikeEffect();
        }

        if (Input.GetKeyDown(KeyCode.V)) //减速
        {
            PlaySlowEffect();
        }

        if (Input.GetKeyDown(KeyCode.B)) //奔跑1
        {
            PlayRunEffect_1();
        }

        if (Input.GetKeyDown(KeyCode.N)) //奔跑2
        {
            PlayRunEffect_2();
        }

        if (Input.GetKeyDown(KeyCode.M)) //护盾1
        {
            PlayShieldsEffect_1();
        }

        if (Input.GetKeyDown(KeyCode.Comma)) //护盾2
        {
            PlayShieldsEffect_2();
        }

        if (Input.GetKeyDown(KeyCode.Period)) //护盾3
        {
            PlayShieldsEffect_3();
        }
    }
}
