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

    public bool isStart = false;

    WaitForSeconds waitCameraSpeed;

    void Start()
    {
        waitCameraSpeed = new WaitForSeconds(5f * Time.deltaTime);
    }

    public void OnTouchStart()
    {
        isStart = true;
        StartBtn.SetActive(false);
        StartCoroutine(CameraRotationCoroutine(() => {
            FindObjectOfType<MapManager>().OnStartMap();
        }));
        
    }
    IEnumerator CameraRotationCoroutine(Action callback = null)
    {
        Transform mainCameraTransform = Camera.main.transform;
        float addY = Mathf.Abs(mainCameraTransform.rotation.y / 10f);

        bool isMoving = true;
        while (isMoving)
        {
            if (mainCameraTransform.rotation.y >= -0.01f) isMoving = false;
            mainCameraTransform.rotation = Quaternion.Lerp(mainCameraTransform.rotation, StartPoint.transform.rotation, 10f * Time.deltaTime);
            yield return waitCameraSpeed;
        }
        mainCameraTransform.rotation = Quaternion.Euler(0f, 0f, 0f);
        callback?.Invoke();
    }
}
