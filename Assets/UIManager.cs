using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI HighScoreVal = null;
    [SerializeField] TextMeshProUGUI CurScoreVal = null;
    [SerializeField] TextMeshProUGUI txtPlayerName = null;

    [Header("[게임오버]")]
    [SerializeField] GameObject objGameOver = null;
    [Header("[랭킹]")]
    [SerializeField] UIRanking uIRanking = null; // 랭킹 UI

    [SerializeField]GameObject objInputName = null; //이름입력 받는
    [SerializeField] TMP_InputField InputName = null;   //

    public void UpdateScore(int HighScoreVal, int CurScoreVal)
    {
        this.HighScoreVal.text = $"{HighScoreVal}";
        this.CurScoreVal.text = $"{CurScoreVal}";
    }

    public void ShowGameOver()
    {
        //Time.timeScale = 0;
        objGameOver.SetActive(true);
    }

    //게임리스타트
    public void Onclick_Restart()
    {
        SceneManager.LoadScene("SampleScene", 0);
    }

    public void Onclick_Ranking()
    {
        if (uIRanking.gameObject.activeSelf == false)
            uIRanking.Init();
        else
            uIRanking.OnClick_Close();
    }
    // 랭킹 UI 보여주기
    public void ShowRankingUI()
    {
        uIRanking.Init();
    }
    public bool IsShowRankingUI()
    {
        return uIRanking.IsShowUI();
    }

    public bool InputNameResult { get; private set; } = false;
    public void ShowInputName(bool show = true)
    {
        objInputName.SetActive(show);
    }


    public void OnClick_Confirm()
    {
        if (string.IsNullOrEmpty(InputName.text))
            return;

        InputNameResult = true;
        PlayerPrefs.SetString("myname", InputName.text);
        SetPlayerName(InputName.text);
    }

    public void SetPlayerName(string name)
    {
        txtPlayerName.text = name;
    }

}