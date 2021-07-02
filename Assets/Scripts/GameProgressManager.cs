using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager instance = null;
    private void Awake() { instance = this; }

    [SerializeField]
    [Header("게임 시작 버튼")]
    GameObject StartBtn;

    [SerializeField]
    [Header("게임 시작 위치")]
    Transform StartPoint;

    [SerializeField]
    [Header("게임 끝나고 돌아갈 위치")]
    Transform EndPoint;

    public bool isStart = false;

    [ContextMenuItem("로비로 가기", "EndGame")]
    public string clear = "<- 오른쪽 버튼으로 실행";

    WaitForSeconds waitCameraSpeed;

    void Start()
    {
        waitCameraSpeed = new WaitForSeconds(5f * Time.deltaTime);
    }

    public void OnTouchStart()
    {
        isStart = true;
        StartBtn.SetActive(false);
        StartCoroutine(StartCameraRotationCoroutine(() => {
            FindObjectOfType<MapManager>().OnStartMap();
        }));

    }
    IEnumerator StartCameraRotationCoroutine(Action callback = null)
    {
        Transform mainCameraTransform = Camera.main.transform;
        float addY = Mathf.Abs(mainCameraTransform.rotation.y / 10f);

        bool isMoving = true;
        bool isCreatWall = true;
        while (isMoving)
        {
            if (mainCameraTransform.rotation.y >= -0.01f) isMoving = false;
            mainCameraTransform.rotation = Quaternion.Lerp(mainCameraTransform.rotation, StartPoint.transform.rotation, 50f * Time.deltaTime);
            yield return waitCameraSpeed;

            // Debug.Log($" Camera : {mainCameraTransform.rotation.y}, Start : {StartPoint.transform.rotation.y }");
            if (isCreatWall && mainCameraTransform.rotation.y > -0.5f)
            {
                isCreatWall = false;
                FindObjectOfType<MapManager>().CreatStartWall();
            }
        }
        //CreatStartWall
        mainCameraTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
        callback?.Invoke();
    }

    public void EndGame()
    {
        Transform mainCameraTransform = Camera.main.transform;
        mainCameraTransform.rotation = Quaternion.Euler(0f, -100f, 0f);
        FindObjectOfType<MapManager>().ClearWall();

        //StartCoroutine(EndCameraRotationCoroutine(() => {
        //    FindObjectOfType<MapManager>().ClearWall();
        //}));
    }
    /*
    IEnumerator EndCameraRotationCoroutine(Action callback = null)
    {
        Transform mainCameraTransform = Camera.main.transform;
        float addY = Mathf.Abs(mainCameraTransform.rotation.y / 10f);
        yield return waitCameraSpeed;
        //bool isMoving = true;
        //while (isMoving)
        //{
        //    if (mainCameraTransform.rotation.y <= -100f) isMoving = false;
        //    mainCameraTransform.rotation = Quaternion.Lerp(mainCameraTransform.rotation, EndPoint.transform.rotation, 10f * Time.deltaTime);
        //    yield return waitCameraSpeed;
        //}
        mainCameraTransform.rotation = Quaternion.Euler(0f, -100f, 0f);
        callback?.Invoke();
    }
    */

}