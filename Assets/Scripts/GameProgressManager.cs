using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameProgressManager : MonoBehaviour
{
    public static GameProgressManager instance = null;
    private void Awake() { instance = this; }

    public bool isPause = false;
    public int magazineCnt = 99;

    [SerializeField]
    [Header("??????? ??????")]
    public GameObject magazineBox;

    [SerializeField]
    [Header("??????? ??????")]
    public Text magazineCntText;

    [SerializeField]
    [Header("?????? ??????? ??????")]
    GameObject StartBtn;

    [SerializeField]
    [Header("?????? ??????? ???????")]
    Transform StartPoint;

    [SerializeField]
    [Header("?????? ??????? ???????")]
    Transform EndPoint;

    public bool isStart = false;

    [ContextMenuItem("?????? ??????", "EndGame")]
    public string clear = "<- ???????????????? ???????";

    [SerializeField]
    GameObject pauseBox;

    [SerializeField]
    Slider BGMSlider;

    [SerializeField]
    Slider SFXSlider;

    [SerializeField]
    GameObject GameSettingBox;

    WaitForSeconds waitCameraSpeed;

    Vector3 originCameraPos;
    void Start()
    {
        Debug.Log("> Start GameProgressManager");
        waitCameraSpeed = new WaitForSeconds(5f * Time.deltaTime);
        magazineCntText.text = magazineCnt.ToString();
        originCameraPos = Camera.main.transform.localPosition;
        BGMSlider.value = SoundManager.instance.BGMcurrentVolume;
        SFXSlider.value = SoundManager.instance.SFXcurrentVolume;
    }

    public void OnPause()
    {
        Time.timeScale = 0;
        pauseBox.transform.Find("On").gameObject.SetActive(false);
        pauseBox.transform.Find("Off").gameObject.SetActive(true);
        isPause = true;
        GameSettingBox.SetActive(true);
    }
    public void OnPlay()
    {
        Time.timeScale = 1;
        pauseBox.transform.Find("On").gameObject.SetActive(true);
        pauseBox.transform.Find("Off").gameObject.SetActive(false);
        isPause = false;
        GameSettingBox.SetActive(false);
    }

    public void OnSliderBMG()
    {
        SoundManager.instance.SetBGMVolume(BGMSlider.value);
    }
    public void OnSliderSFX()
    {
        SoundManager.instance.SetSFXVolume(SFXSlider.value);
    }


    public void OnTouchStart()
    {
        isStart = true;
        magazineBox.SetActive(true);
        StartBtn.SetActive(false);
        SoundManager.instance.StartGameBGM();
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
            mainCameraTransform.rotation = Quaternion.Lerp(mainCameraTransform.rotation, StartPoint.transform.rotation, 20f * Time.deltaTime);
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
        StartCoroutine(Shake(0.2f, 0.2f,() =>{ Invoke(nameof(EndGame), 1f); }));
        
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