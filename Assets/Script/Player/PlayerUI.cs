using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI HpGUI;
    [SerializeField] private TextMeshProUGUI InkGUI;

    void Update()
    {
        if (InkManager.main != null)
            InkGUI.text = "Ink : " + InkManager.main.ink;

        if (Player.main != null)
            HpGUI.text = "HP : " + Player.main.CurrentHP;
    }
}

