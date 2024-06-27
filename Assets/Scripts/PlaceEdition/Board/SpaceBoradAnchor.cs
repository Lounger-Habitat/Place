using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBoradAnchor : MonoBehaviour
{
    public RectTransform spaceUIBoard;
    public RectTransform team1SpaceUIBoard;
    public RectTransform team2SpaceUIBoard;
    public BoxCollider hitCollider;
    public BoxCollider team1HitCollider;
    public BoxCollider team2HitCollider;
    public Transform origin;
    public Transform destination;
    public Transform team1Destination;
    public Transform team1Origin;
    public Transform team2Destination;
    public Transform team2Origin;

    
    // Start is called before the first frame update
    void Start()
    {
        hitCollider.gameObject.SetActive(false);
        team1HitCollider.gameObject.SetActive(false);
        team2HitCollider.gameObject.SetActive(false);
        GetBoardSpaceInfo();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GetBoardSpaceInfo()
    {
        if (GameSettingManager.Instance.mode == GameMode.Competition)
        {
            if (team1SpaceUIBoard != null)
            {
                Vector3[] team1WorldCorners = new Vector3[4];
                team1SpaceUIBoard.GetWorldCorners(team1WorldCorners);

                team1Origin.position = team1WorldCorners[0];
                team1Destination.position = team1WorldCorners[2];
                Vector2 size = team1WorldCorners[2] - team1WorldCorners[0];
                // 计算中心
                Vector3 center = (team1WorldCorners[0] + team1WorldCorners[2]) / 2;
                team1HitCollider.gameObject.SetActive(true);
                team1HitCollider.transform.position = center;
                team1HitCollider.size = new Vector3(size.x, size.y, 0.01f);
            }
            if (team2SpaceUIBoard != null)
            {
                Vector3[] team2WorldCorners = new Vector3[4];
                team2SpaceUIBoard.GetWorldCorners(team2WorldCorners);

                team2Origin.position = team2WorldCorners[0];
                team2Destination.position = team2WorldCorners[2];
                Vector2 size = team2WorldCorners[2] - team2WorldCorners[0];
                // 计算中心
                Vector3 center = (team2WorldCorners[0] + team2WorldCorners[2]) / 2;
                team2HitCollider.gameObject.SetActive(true);
                team2HitCollider.transform.position = center;
                team2HitCollider.size = new Vector3(size.x, size.y, 0.01f);
            }


        }
        else
        {
            // 获取屏幕空间位置
            Vector3[] worldCorners = new Vector3[4];
            spaceUIBoard.GetWorldCorners(worldCorners);

            origin.position = worldCorners[0];
            destination.position = worldCorners[2];
            Vector2 size = worldCorners[2] - worldCorners[0];
            // 计算中心
            Vector3 center = (worldCorners[0] + worldCorners[2]) / 2;
            hitCollider.gameObject.SetActive(true);
            hitCollider.transform.position = center;
            hitCollider.size = new Vector3(size.x, size.y, 0.01f);
        }
    }
}
