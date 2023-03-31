using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_DebugManager : MonoBehaviour
{
    [SerializeField] private UI_Manager uI_Manager = null;
    [SerializeField] private UI_MenuManager uI_MenuManager = null;
    private TextMeshProUGUI tmpGUI = null;

    private void Awake()
    {
        tmpGUI = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        tmpGUI.text = uI_Manager.GetScriptInfo();
    }
}
