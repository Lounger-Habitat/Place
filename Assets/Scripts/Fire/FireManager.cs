using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    public static FireManager Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public List<GameObject> firePrefabs;

    public void Fire(int id, float time, Vector3 pos)
    {
        float timeFloat = time;
        GameObject fire = Instantiate(firePrefabs[Random.Range(0, firePrefabs.Count)], pos, Quaternion.identity);
        var particleSystem = fire.GetComponent<ParticleSystem>();
        particleSystem.Stop();
        var psMain = particleSystem.main;
        psMain.loop = false;
        psMain.duration = timeFloat;
        var psd = fire.AddComponent<PSDestory>();
        psd.ps = particleSystem;
        psd.Init();
        particleSystem.Play();
    }


}
