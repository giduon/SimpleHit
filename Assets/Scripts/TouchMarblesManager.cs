using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMarblesManager : MonoBehaviour
{
    [SerializeField]
    GameObject marbles;
    
    public int marbleCnt = 10;
    
    List<GameObject> marblesList;

    [SerializeField]
    GameObject palyer;

    public float zCnt = 1f;
    
    void Start()
    {
        marblesList = new List<GameObject>();
        for (int i = 0; i < marbleCnt; i++)
		{
            marblesList.Add(Instantiate(marbles));
		}
    }

    void OnChangeIsShot()
	{
        isShot = true;
    }
    bool isShot = true;
    Vector3 touchPos;
    void Update()
    {
		if (GameProgressManager.instance.isStart && isShot && Input.GetMouseButtonDown(0))
		{
            Debug.Log("Touch");
            isShot = false;
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -(Camera.main.transform.position.z + zCnt))); //zCnt = 25
			foreach (GameObject marbles in marblesList)
			{
				if (!marbles.activeSelf)
				{
                    // marbles.transform.position = new Vector3 (worldPos.x * 1920, worldPos.y * 1080, palyer.transform.position.z);
                    // marbles.transform.position = new Vector3 (worldPos.x * Screen.width, worldPos.y * Screen.height, palyer.transform.position.z) * Time.deltaTime;
                    // marbles.transform.position = new Vector3 (worldPos.x, worldPos.y, palyer.transform.position.z) * Time.deltaTime;
                    marbles.transform.position = worldPos;
					marbles.SetActive(true);
                    GameProgressManager.instance.magazineCntText.text = (--GameProgressManager.instance.magazineCnt).ToString();
                    if (GameProgressManager.instance.magazineCnt < 1)
                    {
                        GameProgressManager.instance.isStart = false;
                        GameProgressManager.instance.OnGameOver();
                    }
                    break;
				}
			}

            /*
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                foreach (GameObject marbles in marblesList)
                {
                    if (!marbles.activeSelf)
                    {
                        marbles.SetActive(true);
                        marbles.transform.position = new Vector3(hit.point.x, hit.point.y, palyer.transform.position.z);
                        break;
                    }
                }
            }
            */
            Invoke(nameof(OnChangeIsShot), 1f);
        }
    }
}
