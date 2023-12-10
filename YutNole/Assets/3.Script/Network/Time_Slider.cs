using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Time_Slider : NetworkBehaviour
{
    /*
        1. 실시간으로 타이머 증가 (밸류값은 조정 필요)
        2. 서버 매니저에 달려있는 Turn_Index가 달라질때마다 본인 슬라이더 setactive false 상대만 슬라이더 true
        3. OnDisable 콜백 메소드를 이용해서 비활성화 될때는 본인 슬라이더의 밸류값을 0으로 초기화

        즉 턴 넘어갈때 Turn_Index가 달라지므로 건드릴필요 없을 듯 ?
    */

    [SerializeField] private Slider P1_Slider;
    [SerializeField] private Slider P2_Slider;

    public float TimeLimit = 30; 
    public float ThrowTime = 0f;
    private Coroutine serverTimerCoroutine;
    private int previousTurnIndex = -1;
    private bool isCoroutineRunning = false;

    #region SyncVar
    [SyncVar(hook = nameof(OnP1SliderValueChanged))]
    private float p1SliderValue;

    [SyncVar(hook = nameof(OnP2SliderValueChanged))]
    private float p2SliderValue;
    #endregion

    #region Server
    // 서버에서만 호출되는 함수로 클라이언트에 동기화된 값을 설정
    [Server]
    public void SetP1SliderValue(float value)
    {
        p1SliderValue = value;
    }

    [Server]
    public void SetP2SliderValue(float value)
    {
        p2SliderValue = value;
    }

    // 모든 슬라이더 값을 초기화
    [Server]
    public void ResetSliderValues()
    {
        Debug.Log("Resetting slider values to 0");
        SetP1SliderValue(0);
        SetP2SliderValue(0);
    }

    // 서버에서만 실행되는 타이머 코루틴
    [Server]
    private IEnumerator ServerTimer()
    {
        float exitTime = 0;
        Debug.Log("Coroutine Start");
        while (exitTime < TimeLimit)
        {
            if (Server_Manager.instance.Turn_Index == 1)
            {
                // 서버에서 슬라이더 값을 설정
                SetP1SliderValue(p1SliderValue + 0.1f);
            }
            else if (Server_Manager.instance.Turn_Index == 2)
            {
                // 서버에서 슬라이더 값을 설정
                SetP2SliderValue(p2SliderValue + 0.1f);
            }
            yield return new WaitForSeconds(0.1f);
            exitTime += Time.deltaTime;
        }

        // Coroutine이 종료되면 isCoroutineRunning을 false로 설정
        isCoroutineRunning = false;
    }
    #endregion

    #region Client
    #endregion

    #region Command
    #endregion

    #region ClientRPC
    #endregion

    #region Unity Callback
    private void Update()
    {
        Start_Timer();
    }

    private void OnDisable()
    {
        if (isServer)
        {
            Debug.Log("OnDisable - Resetting slider values");
            // MonoBehaviour가 비활성화될 때 실행 중인 코루틴을 중지
            StopCoroutine(serverTimerCoroutine);
            serverTimerCoroutine = null;

            // 시간이 종료되면 초기화
            ResetSliderValues();
        }
    }
    #endregion

    #region Hook Method
    // SyncVar에 변경이 생겼을 때 호출되는 콜백 함수입니다.
    private void OnP1SliderValueChanged(float oldValue, float newValue)
    {
        P1_Slider.value = newValue;
    }

    private void OnP2SliderValueChanged(float oldValue, float newValue)
    {
        P2_Slider.value = newValue;
    }
    #endregion

    private void Start_Timer()
    {
        int currentTurnIndex = Server_Manager.instance.Turn_Index;
        if (isServer)
        {
            // 턴이 변경되었을 때만 실행 중인 코루틴을 중지하고 새로운 코루틴 시작
            if (currentTurnIndex != previousTurnIndex)
            {
                if (serverTimerCoroutine != null)
                {
                    StopCoroutine(serverTimerCoroutine);
                    // 시간이 종료되면 초기화
                    ResetSliderValues();
                }
                if (currentTurnIndex == 1)
                {
                    P1_Slider.gameObject.SetActive(true);
                    P2_Slider.gameObject.SetActive(false);
                }
                else if (currentTurnIndex == 2)
                {
                    P1_Slider.gameObject.SetActive(false);
                    P2_Slider.gameObject.SetActive(true);
                }

                serverTimerCoroutine = StartCoroutine(ServerTimer());
                previousTurnIndex = currentTurnIndex;
                isCoroutineRunning = true;
            }
            // 턴이 변경되지 않았을 때는 코루틴을 계속 실행
            else if (serverTimerCoroutine == null && !isCoroutineRunning)
            {
                serverTimerCoroutine = StartCoroutine(ServerTimer());
                isCoroutineRunning = true;
            }
        }
        else if (isClient)
        {
            if (currentTurnIndex != previousTurnIndex)
            {
                if (currentTurnIndex == 1)
                {
                    P1_Slider.gameObject.SetActive(true);
                    P2_Slider.gameObject.SetActive(false);
                }
                else if (currentTurnIndex == 2)
                {
                    P1_Slider.gameObject.SetActive(false);
                    P2_Slider.gameObject.SetActive(true);
                }
            }
        }
    }
}
