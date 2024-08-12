using UnityEngine;

public class PlaceFire : MonoBehaviour
{
    public Transform start;
    public Transform end;

    public int maxCount = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Fire(end.position);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            for (int i = 0; i < maxCount; i++)
            {
                Fire(end.position + new Vector3(i, 0, 0));
            }
        }
    }

    public void Fire(Vector3 endPos)
    {
        GameObject projGo = PlaceInkPoolManager.Instance.GetBlueInkProjectile();
        projGo.transform.position = start.position;
        ProjectileController projScript = projGo.GetComponent<ProjectileController>();
        projScript.Launch(endPos, 2.0f);
    }
    
}
