using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum YutState
{
    Do = 0,
    Gae,
    Geol,
    Yut,
    Mo,
    Backdo,
    Nack
}

public class PlayingYut : MonoBehaviour
{
    /*
     * Yut拭 魚献 巴傾戚嬢 戚疑 貢 獄動 戚疑, 鉢詞妊 持失 貢 鉢詞妊 喚袈聖 獣 巴傾戚嬢 持失
        Trun昔 position拭 杏軒檎 Transform 壕伸聖 郊蚊爽奄
     */
    public Transform[] pos1;
    public Transform[] pos2;
    public Transform[] pos3;
    public Transform[] pos4;

    public Transform[] playerArray; // player亜 背雁馬澗 pos array
    public RectTransform[] yutButton; // 亀, 鯵, 杏, 牲, 乞, 鯖亀 獄動

    [SerializeField] private GameObject player; // 切重税 源
    public GameObject[] playerButton; // character, return
    
    public int currentIndex = 0; // Button 昔畿什
    public int resultIndex = 0; // 獄動 是帖拝 昔畿什
    public List<int> yutResultIndex = new List<int>(); // yut 衣引拭 企廃 収切, 戚疑 獄動 適遣 獣 Remove, Nack戚檎 Add 照敗
    private int[] yutArray = { 1, 2, 3, 4, 5, -1, 0 }; // 亀 鯵 杏 牲 乞 鯖亀 開

    // 牲 衣引 亜閃神奄
    public Yut_Gacha_Test yutGacha; // 蟹掻拭 Yut_Gacha稽 郊蚊爽奄
    public string yutResult;
    public YutState type;

    public GameObject goalButton; // goal button, resultIndex左陥 適 凶

    [SerializeField] private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();    //蟹掻拭 陥献号狛生稽 凧繕馬奄
        yutGacha = FindObjectOfType<Yut_Gacha_Test>(); // 蟹掻拭 Yut_Gacha稽 郊蚊爽奄
        playerArray = pos1;
    }
    
    public void PlayingYutPlus()
    { // 牲 揮走奄 獄動 適遣 獣
        yutResult = yutGacha.ThrowResult; // 牲 揮遭 衣引
        type = (YutState)Enum.Parse(typeof(YutState), yutResult);

        if (!yutResult.Equals("Nack") && !(yutResult.Equals("Backdo") && currentIndex == 0))
        {
            yutResultIndex.Add(yutArray[(int)type]); // yutResult拭 魚虞 List拭 戚疑拝 幻鏑税 収切 蓄亜
            playerButton[0].SetActive(true);
        }

        for (int i = 0; i < yutResultIndex.Count; i++)
        {
            Debug.Log($"YutResultIndex{i} : {yutResultIndex[i]}");
        }
    }

    private void YutButtonPosition()
    {
        // 獄動 醗失鉢 貢 是帖 竺舛
        playerButton[0].gameObject.SetActive(true); // 蝶遣斗 識澱 獄動 醗失鉢
        for (int i = 0; i < yutResultIndex.Count; i++)
        {  // 牲 揮霜 凶 原陥 乞窮 獄動 Canvas 鉱生稽
            yutButton[i].transform.position = Camera.main.WorldToScreenPoint(yutButton[i].transform.parent.position);
        }
        if (yutResult != "Nack")
        {
            // 開戚 焼諌 凶
            if (yutResult == "BackDo" && currentIndex == 0)
            {
                // 戚凶澗 鯖亀亜 災亜管
                return;
            }
            else
            {
                PositionIn();
            }
        }
        else
        { // Nack
            PositionIn();
        }
    }

    public void CharacterButtonClick()
    { // Canvas - CharacterButton
        YutButtonPosition();
        playerButton[1].SetActive(true);
        playerButton[0].SetActive(false);
    }

    public void ReturnButtonClick()
    { // Canvas - ReturnButton
        playerButton[0].SetActive(true);
        playerButton[1].SetActive(false);
        PositionOut();
    }

    public void YutButtonClick(string name)
    { // Canvas - YutObject - 亀鯵杏牲乞鯖亀
        playerButton[0].SetActive(false);
        playerButton[1].SetActive(false);

        YutState yutName = (YutState)Enum.Parse(typeof(YutState), name);
        GameObject btn = yutButton[(int)yutName].gameObject;
        Vector3 screen = Camera.main.WorldToScreenPoint(btn.transform.parent.position); // Canvas 鉱生稽
        btn.transform.position = screen; // 蟹紳 牲拭 限澗 獄動 匂走芝 竺舛

        yutResultIndex.Remove(yutArray[(int)yutName]); // 軒什闘 肢薦

        Debug.Log($"YutResultIndex Remove : {yutArray[(int)yutName]}");

        currentIndex += yutArray[(int)yutName]; // 薄仙 昔畿什 痕井
        TurnPosition(playerArray, currentIndex); // 薄仙 是帖 壕伸 痕井
        PositionOut();
        goalButton.SetActive(false);

        if (yutResultIndex.Count > 0)
        {
            PositionIn();
        }
    }

    public void GoalButtonClick()
    {
        Debug.Log("せせせせせせせせせせせせせせせ");
        // Goal Count++
        PositionOut();
        playerButton[0].SetActive(false);
       playerButton[1].SetActive(false);
        StartCoroutine(playerMovement.Move_Co());
        goalButton.SetActive(false);
        // StartCoroutine(Goal_Co());
    }

    private IEnumerator Goal_Co()
    {
        yield return new WaitForSeconds(0.2f);
        player.SetActive(false);
        goalButton.SetActive(false);
    }

    private void PositionOut()
    { // Button Position out
        for (int i = 0; i < yutButton.Length; i++)
        {  // 牲 揮霜 凶 原陥 乞窮 獄動 Canvas 鉱生稽
            yutButton[i].transform.position = Camera.main.WorldToScreenPoint(yutButton[i].transform.parent.position);
        }
        goalButton.SetActive(false);
    }

    public void PositionIn()
    { // Button Position in
        YutState yutType = YutState.Backdo;
        for (int i = 0; i < yutResultIndex.Count; i++)
        {
            resultIndex = currentIndex + yutResultIndex[i];
            
            if (!yutResult.Equals("Nack"))
            {
                if (yutResultIndex[i] == -1)
                {
                    yutType = YutState.Backdo; // 5
                }
                else
                {
                    yutType = (YutState)(yutResultIndex[i] - 1);
                }
            }
            Debug.Log("YutType: " + yutType);
            if (playerArray.Length > resultIndex)
            {
                Vector3 screen = Camera.main.WorldToScreenPoint(playerArray[resultIndex].transform.position);
                yutButton[(int)yutType].transform.position = screen; // 蟹紳 牲拭 限澗 獄動 匂走芝 竺舛, Backdo = 5
            } 
            else if (playerArray.Length == resultIndex)
            {
                Vector3 screen = Camera.main.WorldToScreenPoint(playerArray[0].transform.position);
                yutButton[(int)yutType].transform.position = screen;
                goalButton.SetActive(true);
            }
            else
            {
                goalButton.SetActive(true);
            }
        }
    }

    public void TurnPosition(Transform[] pos, int num)
    {
        if (pos == pos1)
        {
            if (num == 5)
            { // pos1 -> pos3, 5
                playerArray = pos3;
                currentIndex = 5;
            } else if (num == 10)
            { // pos1 -> pos2, 10
                playerArray = pos2;
                currentIndex = 10;
            }
        } else if (pos == pos3)
        {
            if (num == 8)
            { // pos3 -> pos4, 8(22 是帖)
                playerArray = pos4;
                currentIndex = 8;
            }
        }
        // Catch 雁梅聖 凶 playerArray = pos1稽 痕井
    }
}
