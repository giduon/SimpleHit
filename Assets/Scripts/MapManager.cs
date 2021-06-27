using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [System.Serializable]
    class WallPrefab
    {
        public GameObject wall;
    }
    [SerializeField]
    [Header("벽 프리팹 배열")]
    WallPrefab[] wallPrefabs;

    [SerializeField]
    [Header("첫번째 벽 위치")]
    Transform startWallPoint;

    [SerializeField]
    [Header("생성 되기전 첫 벽 위치")]
    Transform[] fistWalls;

    [SerializeField]
    [Header("첫 시작 벽")]
    GameObject startWall;

    [SerializeField]
    [Header("벽 생성 위치")]
    Transform createPoint;

    [Header("벽 이동 속도")]
    public float wallMoveSpeed = 5f;

    [Header("벽 생성 속도")]
    public float wallCreatSpeed = 3f;

    [SerializeField]
    [Header("벽 보관 박스")]
    GameObject wallBox;

    [SerializeField]
    [Header("벽 사라질 위치")]
    Transform endWallPoint;

    public bool isMove = false;

    WaitForSeconds waitWallMove;

    void Start()
    {
        waitWallMove = new WaitForSeconds(0.1f * Time.deltaTime);
    }

    public void CreatStartWall()
    {
        isMove = true;
        GameObject wall = CreateWall(startWall);
        wall.transform.position = startWallPoint.transform.position;
        OnMoveMap(wall);
    }

    public void OnStartMap()
    {
        foreach (Transform fistWall in fistWalls)
        {
            OnSpecifyPositionWall(fistWall);
        }

        StartCoroutine(OnCreatWallCoroutine());
    }

    GameObject CreateWall(GameObject bringWall = null)
    {
        if (!isMove) return null;
        GameObject wall = Instantiate(bringWall == null ? wallPrefabs[Random.Range(0, wallPrefabs.Length)].wall: bringWall);
        wall.transform.SetParent(wallBox.transform, false);
        return wall;
    }

    void OnSpecifyPositionWall(Transform fistWall = null)
    {
        if (!isMove) return;
        GameObject wall = CreateWall();
        if (fistWall != null) wall.transform.position = fistWall.transform.position;
        else wall.transform.position = createPoint.transform.position;
        OnMoveMap(wall);
    }
    IEnumerator OnCreatWallCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(wallCreatSpeed);
            //yield return waitWallCreate;
            OnSpecifyPositionWall();
        }
    }

    void OnMoveMap(GameObject wall)
    {
        if(wall != null) StartCoroutine(WallMoveCoroutine(wall));
    }
    IEnumerator WallMoveCoroutine(GameObject wall)
    {
       
        while (true)
        {
            yield return waitWallMove;
            yield return new WaitUntil(()=> isMove);
            if (wall == null) break;
                 
            if (wall.transform.position.z == endWallPoint.position.z)
            {
                Destroy(wall);
            }
            else
            {
                wall.transform.position = Vector3.MoveTowards(wall.transform.position, endWallPoint.transform.position, wallMoveSpeed * Time.deltaTime);
            }
        }
    }
   
    public void ClearWall()
    {
        isMove = false;
        for (int i =0; i < wallBox.transform.childCount; i++)
        {
            GameObject wall = wallBox.transform.GetChild(i).gameObject;
            if(wall != null) Destroy(wall);
        }
    }
}
