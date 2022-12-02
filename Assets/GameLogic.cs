using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
public class GameLogic : MonoBehaviour
{
    //------------------------------------------
    #region Singleton
    static public GameLogic Instance = null;
    static public GameLogic instance
    {
        get
        {
            if(Instance == null)
            {
                Instance = FindObjectOfType<GameLogic>();
                if (Instance == null)
                {
                    Instance = new GameObject("GameLogic").AddComponent<GameLogic>();
                }
            }
            return Instance;
        }
    }

    #endregion
    //------------------------------------------


    public bool IsGameStart = false; // ���� ���� ����..

    UIManager myUIManager = null;
    string myName = null;

    IEnumerator Start()
    {
        myUIManager = FindObjectOfType<UIManager>();

        // �Էµ� �̸��� �ִ��� üũ
        string myname = PlayerPrefs.GetString("myname");
        if(string.IsNullOrEmpty(myname) == true)
        {
            //�̸��� �Է� ����.
            myUIManager.ShowInputName();
            yield return new WaitUntil(() => myUIManager.InputNameResult);
            myUIManager.ShowInputName(false);
        }
        else
        {
            myUIManager.SetPlayerName(myname);
        }

        

        IsGameStart = true;
    }

    public void GameOver(int curScore, int highScore)
    {
        StartCoroutine(processGameOver(curScore, highScore));
    }

    IEnumerator processGameOver(int curScore, int highScore)
    {
        if(curScore >= highScore)
        {
           yield return StartCoroutine(processSaveHighScore(curScore));
        }
        myUIManager.ShowRankingUI();
        yield return new WaitUntil(() => !myUIManager.IsShowRankingUI());

        myUIManager.ShowGameOver();
    }
    IEnumerator processSaveHighScore(int newHighScore)
    {
        UserScore userScore = new UserScore() { name = myName, date = DateTime.Today.ToString("d") , highscore = newHighScore};
        string jsonData = JsonUtility.ToJson(userScore);
        using (UnityWebRequest request = UnityWebRequest.Post("http://localhost:8080/savemyscore",jsonData))
        {
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
      
            


        }
    }


}
