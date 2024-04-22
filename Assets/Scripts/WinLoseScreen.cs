using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Framework;

public class WinLoseScreen : SingletonBehaviour<WinLoseScreen>
{
    [SerializeField] CanvasGroup _winLoseCanvasGroup;
    [SerializeField] TMP_Text _winLoseText;

    public void EndGame(bool hasWon)
    {
        Time.timeScale = 0;
        _winLoseCanvasGroup.alpha = 1;
        _winLoseCanvasGroup.interactable = true;

        if (hasWon)
            _winLoseText.text = "Volvy Survived";
        else
            _winLoseText.text = "Volvy Starved";

    }
}
