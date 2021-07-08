using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{

    enum ObstacleType
    {
       None,
       Window,
    }

    [System.Serializable]
    class ObstacleInfo
    {
        public Transform point;
        public ObstacleType type;
    }
    [SerializeField]
    ObstacleInfo[] obstacleInfos;

    [SerializeField]
    GameObject window;

    public Transform parentPos;

    public void CreatedObstacle()
    {
        GameObject go;
        foreach(ObstacleInfo info in obstacleInfos)
        {
            if (info.type == ObstacleType.Window)
            {
                go = Instantiate(window);
                if(parentPos != null) go.transform.SetParent(parentPos);
                go.transform.position = info.point.position;
            }
        }
    }

}
