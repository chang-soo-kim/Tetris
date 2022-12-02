using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[System.Serializable]
public class UserScore
{
    public string name;
    public string date;
    public int highscore;
}
[System.Serializable]
public class UserScoreList
{
    public UserScore[] list = null;
}

public class UIRanking : MonoBehaviour
{
    [SerializeField] ScrollRect srUserScoreList = null;
    [SerializeField] UIUserScore prefabUserScore = null;

    public bool IsShowUI()
    {
        return this.gameObject.activeSelf;
    }
    
    public void Init()
    {
        Debug.Log("## init");
        this.gameObject.SetActive(true);
        //서버에서 유저 점수 목록을 받아서
        // 정상적으로 처리됐으면 ui 활성화
        StartCoroutine(processGetDataFromServer());
    }

    IEnumerator processGetDataFromServer()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://localhost:8080/userscorelist"))
        {
            //완료될때까지 알아서 기다림
            yield return request.SendWebRequest();

            Debug.Log(request.downloadHandler.text); // json 포멧으로 저장되어 있을거다...

            UserScoreList userScoreList =  JsonUtility.FromJson<UserScoreList>("{\"list\":" + request.downloadHandler.text + "}");

            //기존에 생성되어 있는  User Score 객체를 지운다.
            foreach(Transform child in srUserScoreList.content.transform)
            {
                Destroy(child.gameObject);
            }

            //서버에서 받은 데이터 만큼  User Score 객체를 생성한다ㅏ.
            for (int i = 0; i < userScoreList.list.Length; i++)
            {
                // UIUserSocre 객체를 하나 생성하고..
                UIUserScore instUIObj = Instantiate<UIUserScore>(prefabUserScore, srUserScoreList.content.transform);
                // 생성된 UI객체에 유저점수를 넘겨준다.
                instUIObj.Init(userScoreList.list[i].name, userScoreList.list[i].date, userScoreList.list[i].highscore);
                

            }

        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }


}
