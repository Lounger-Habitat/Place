using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBoradAnchor : MonoBehaviour
{
    public RectTransform spaceUIBoard;
    public BoxCollider hitCollider;
    public Transform origin;
    public Transform destination;
    // Start is called before the first frame update
    void Start()
    {
        GetBoardSpaceInfo();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GetBoardSpaceInfo()
    {
        if (spaceUIBoard != null)
        {
            // 获取屏幕空间位置
            Vector3[] worldCorners = new Vector3[4];
            spaceUIBoard.GetWorldCorners(worldCorners);

            origin.position = worldCorners[0];
            destination.position = worldCorners[2];
            Vector2 size = worldCorners[2] - worldCorners[0];
            // 计算中心
            Vector3 center = (worldCorners[0] + worldCorners[2]) / 2;
            hitCollider.transform.position = center;
            hitCollider.size = new Vector3(size.x, size.y, 0.1f);
        }
        else
        {
            Debug.LogError("No RectTransform assigned.");
        }
    }
}
