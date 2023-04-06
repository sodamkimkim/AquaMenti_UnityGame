using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_InGameDebugManager : MonoBehaviour
{
    private TextMeshProUGUI tmpGUI = null;

    private void Awake()
    {
        tmpGUI = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {


         List<Dictionary<string, object>> data_info = CSVReader.Read("Datas/GameInfo/SectionStory");
        for (int i = 0; i < data_info.Count; i++)
        {

            if (data_info[i]["MapNumber"].ToString() == "1")
            {
                tmpGUI.text += $"\n �Ƿ��� : {data_info[i]["Intermediary"].ToString()} ��ġ :  {data_info[i]["Location"].ToString()}" +
                    $" ������ : {data_info[i]["SectionTitle"].ToString()} \n" +
                    $"�̾߱���� :  {data_info[i]["StoryBegin"].ToString()}\n �̾߱ⳡ :  {data_info[i]["StoryEnd"].ToString()} \n ";
            }

        }
    }
}
