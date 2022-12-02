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
        //�������� ���� ���� ����� �޾Ƽ�
        // ���������� ó�������� ui Ȱ��ȭ
        StartCoroutine(processGetDataFromServer());
    }

    IEnumerator processGetDataFromServer()
    {
        using (UnityWebRequest request = UnityWebRequest.Get("http://localhost:8080/userscorelist"))
        {
            //�Ϸ�ɶ����� �˾Ƽ� ��ٸ�
            yield return request.SendWebRequest();

            Debug.Log(request.downloadHandler.text); // json �������� ����Ǿ� �����Ŵ�...

            UserScoreList userScoreList =  JsonUtility.FromJson<UserScoreList>("{\"list\":" + request.downloadHandler.text + "}");

            //������ �����Ǿ� �ִ�  User Score ��ü�� �����.
            foreach(Transform child in srUserScoreList.content.transform)
            {
                Destroy(child.gameObject);
            }

            //�������� ���� ������ ��ŭ  User Score ��ü�� �����Ѵ٤�.
            for (int i = 0; i < userScoreList.list.Length; i++)
            {
                // UIUserSocre ��ü�� �ϳ� �����ϰ�..
                UIUserScore instUIObj = Instantiate<UIUserScore>(prefabUserScore, srUserScoreList.content.transform);
                // ������ UI��ü�� ���������� �Ѱ��ش�.
                instUIObj.Init(userScoreList.list[i].name, userScoreList.list[i].date, userScoreList.list[i].highscore);
                

            }

        }
    }

    public void OnClick_Close()
    {
        this.gameObject.SetActive(false);
    }


}
