using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Time_Slider : NetworkBehaviour
{
    /*
        1. �ǽð����� Ÿ�̸� ���� (������� ���� �ʿ�)
        2. ���� �Ŵ����� �޷��ִ� Turn_Index�� �޶��������� ���� �����̴� setactive false ��븸 �����̴� true
        3. OnDisable �ݹ� �޼ҵ带 �̿��ؼ� ��Ȱ��ȭ �ɶ��� ���� �����̴��� ������� 0���� �ʱ�ȭ

        �� �� �Ѿ�� Turn_Index�� �޶����Ƿ� �ǵ帱�ʿ� ���� �� ?
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
    // ���������� ȣ��Ǵ� �Լ��� Ŭ���̾�Ʈ�� ����ȭ�� ���� ����
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

    // ��� �����̴� ���� �ʱ�ȭ
    [Server]
    public void ResetSliderValues()
    {
        Debug.Log("Resetting slider values to 0");
        SetP1SliderValue(0);
        SetP2SliderValue(0);
    }

    // ���������� ����Ǵ� Ÿ�̸� �ڷ�ƾ
    [Server]
    private IEnumerator ServerTimer()
    {
        float exitTime = 0;
        Debug.Log("Coroutine Start");
        while (exitTime < TimeLimit)
        {
            if (Server_Manager.instance.Turn_Index == 1)
            {
                // �������� �����̴� ���� ����
                SetP1SliderValue(p1SliderValue + 0.1f);
            }
            else if (Server_Manager.instance.Turn_Index == 2)
            {
                // �������� �����̴� ���� ����
                SetP2SliderValue(p2SliderValue + 0.1f);
            }
            yield return new WaitForSeconds(0.1f);
            exitTime += Time.deltaTime;
        }

        // Coroutine�� ����Ǹ� isCoroutineRunning�� false�� ����
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
            // MonoBehaviour�� ��Ȱ��ȭ�� �� ���� ���� �ڷ�ƾ�� ����
            StopCoroutine(serverTimerCoroutine);
            serverTimerCoroutine = null;

            // �ð��� ����Ǹ� �ʱ�ȭ
            ResetSliderValues();
        }
    }
    #endregion

    #region Hook Method
    // SyncVar�� ������ ������ �� ȣ��Ǵ� �ݹ� �Լ��Դϴ�.
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
            // ���� ����Ǿ��� ���� ���� ���� �ڷ�ƾ�� �����ϰ� ���ο� �ڷ�ƾ ����
            if (currentTurnIndex != previousTurnIndex)
            {
                if (serverTimerCoroutine != null)
                {
                    StopCoroutine(serverTimerCoroutine);
                    // �ð��� ����Ǹ� �ʱ�ȭ
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
            // ���� ������� �ʾ��� ���� �ڷ�ƾ�� ��� ����
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
