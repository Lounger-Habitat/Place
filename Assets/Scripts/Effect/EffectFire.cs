using AllIn1VfxToolkit.Demo.Scripts;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;
using System.Security.Cryptography.X509Certificates;

public class EffectFire : MonoBehaviour
{
    [SerializeField] private Transform endPosBottom;
    [SerializeField] private Transform endPosTop;
    [SerializeField] private All1VfxDemoEffect currDemoEffect;
    [SerializeField] private GameObject projectileBasePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    public int debugx = 0;
    public int debugy = 0;

    public void PlayEffect(int x,int y)
    {
        // 获取 frame 宽 高
        int boradWidth = PlaceBoardManager.Instance.width;
        int boradHeight = PlaceBoardManager.Instance.height;

        // 检测 x,y 小于 width height, 超出部分做截断处理
        x = Mathf.Clamp(x, 0, boradWidth);
        y = Mathf.Clamp(y, 0, boradHeight);


        //get end pos
        Vector3 frame = endPosTop.position - endPosBottom.position;
        double pixelWidth = frame.x / boradWidth;
        double pixelHeight = frame.y / boradHeight;
        Vector3 delta = new Vector3((float)(x * pixelWidth), (float)(y * pixelHeight), 0f);
        Debug.Log("boradWidth: " + boradWidth + " boradHeight: " + boradHeight);
        Debug.Log("x: " + x + " y: " + y);
        Debug.Log("frame: " + frame);
        Debug.Log("pixelWidth: " + pixelWidth + " pixelHeight: " + pixelHeight);
        Debug.Log("delta: " + delta);

        Vector3 aimPos = endPosBottom.position + delta;
        Debug.Log(aimPos);
        
        Transform tempTransform = null;
        tempTransform = Instantiate(currDemoEffect.muzzleFlashPrefab, projectileSpawnPoint.position, Quaternion.identity).transform;
        tempTransform.localRotation = Quaternion.identity;
        tempTransform.forward = projectileSpawnPoint.forward;
        tempTransform.parent = transform;
        tempTransform.localScale *= currDemoEffect.scaleMultiplier;
        
        
        
        Transform projectileBase = Instantiate(projectileBasePrefab, projectileSpawnPoint.position, Quaternion.identity).transform;
        projectileBase.LookAt(aimPos);
        projectileBase.parent = transform;
        projectileBase.localRotation = Quaternion.identity;
        
        
        tempTransform = Instantiate(currDemoEffect.projectilePrefab, projectileSpawnPoint.position, Quaternion.identity).transform;
        tempTransform.localRotation = Quaternion.identity;
        tempTransform.forward = projectileBase.forward;
        tempTransform.parent = projectileBase;
        
        
        AllIn1DemoProjectile tempProjectileInstance = projectileBase.GetComponent<AllIn1DemoProjectile>();
        tempProjectileInstance.Initialize(transform, projectileSpawnPoint.forward, currDemoEffect.projectileSpeed, currDemoEffect.impactPrefab, currDemoEffect.scaleMultiplier);

        projectileBase.DOMove(aimPos, 3.0f).SetEase(Ease.OutQuint);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // 随机生成一个坐标
            // int x = Random.Range(0, 500);
            // int y = Random.Range(0, 500);
            PlayEffect(debugx,debugy);
        }
    }
}