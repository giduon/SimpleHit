using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager instance = null;
    private void Awake() { instance = this; }

    public int magazineCnt = 99;

    [SerializeField]
    [Header("?μ°½ λ°μ€")]
    public GameObject magazineBox;

    [SerializeField]
    [Header("?μ°½ κ°μ")]
    public Text magazineCntText;

    [SerializeField]
    [Header("κ²μ ?μ λ²νΌ")]
    GameObject StartBtn;

    [SerializeField]
    [Header("κ²μ ?μ ?μΉ")]
    Transform StartPoint;

    [SerializeField]
    [Header("κ²μ ?λ© ?μΉ")]
    Transform EndPoint;

    public bool isStart = false;

    [ContextMenuItem("κ²μ μ’λ£", "EndGame")]
    public string clear = "<- ?€λ₯Έμͺ?λ²νΌ ?΄λ¦­";

    WaitForSeconds waitCameraSpeed;

    Vector3 originCameraPos;
    void Start()
    {
        waitCameraSpeed = new WaitForSeconds(5f * Time.deltaTime);
        magazineCntText.text = magazineCnt.ToString();
        originCameraPos = Camera.main.transform.localPosition;
    }

    public void OnPause()
    {
        Debug.Log("OnPause");
    }

    public void OnTouchStart()
    {
        isStart = true;
        magazineBox.SetActive(true);
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
            mainCameraTransform.rotation = Quaternion.Lerp(mainCameraTransform.rotation, StartPoint.transform.rotation, 10f * Time.deltaTime);
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
        Camera.main.transform.rotation = Quaternion.Euler(0f, -100f, 0f);
        FindObjectOfType<MapManager>().ClearWall();

        StartCoroutine(EndCameraRotationCoroutine(() =>
        {
            FindObjectOfType<MapManager>().ClearWall();
        }));
    }
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

    public void OnGameOver()
    {
        StartCoroutine(Shake(1f, 0.2f,() =>{ Invoke(nameof(EndGame), 1f); }));
        
    }
    public IEnumerator Shake(float _amount, float _duration, Action callback = null)
    {
        float timer = 0;
        while (timer <= _duration)
        {
            Camera.main.transform.localPosition = (Vector3)UnityEngine.Random.insideUnitCircle * _amount + originCameraPos;
            timer += Time.deltaTime;
            yield return new WaitForSeconds(0.1f);
        }
        Camera.main.transform.localPosition = originCameraPos;

        callback?.Invoke();
    }
}