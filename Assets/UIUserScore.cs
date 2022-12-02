using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIUserScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtName = null;
    [SerializeField] TextMeshProUGUI txtDate = null;
    [SerializeField] TextMeshProUGUI txtScore = null;

    public void Init(string name, string strDate, int score)
    {
        txtName.text = name;
        txtDate.text = strDate;
        txtScore.text = $"{score}";


    }
}
