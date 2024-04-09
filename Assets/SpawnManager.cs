using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SpawnManager : MonoBehaviour
{
    public List<Transform> spawnPoints;
    public GameObject spawnPrefab;
    public float radius = 50;
    // Start is called before the first frame update
    void Start()
    {
        // 如果没有指定生成点，就找到所有的生成点
        if (spawnPoints.Count == 0)
        {
            spawnPoints = new List<Transform>();
            // 以 0 0 为中心 ，30为 半径，生成 8个 生成点
            for (int i = 0; i < 8; i++)
            {
                float angle = i * Mathf.PI * 2 / 8;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                Vector3 pos = new Vector3(x, 0, z);
                // GameObject go = new GameObject("SpawnPoint" + i);
                GameObject go = Instantiate(spawnPrefab, pos, Quaternion.identity);
                go.transform.SetParent(transform);
                spawnPoints.Add(go.transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
