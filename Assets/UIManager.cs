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

    [Header("[���ӿ���]")]
    [SerializeField] GameObject objGameOver = null;
    [Header("[��ŷ]")]
    [SerializeField] UIRanking uIRanking = null; // ��ŷ UI

    [SerializeField]GameObject objInputName = null; //�̸��Է� �޴�
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

    //���Ӹ���ŸƮ
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
    // ��ŷ UI �����ֱ�
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