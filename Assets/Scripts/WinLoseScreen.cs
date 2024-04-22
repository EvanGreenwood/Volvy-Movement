using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Framework;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class WinLoseScreen : SingletonBehaviour<WinLoseScreen>
{
    [SerializeField] CanvasGroup _winLoseCanvasGroup;
    [SerializeField] TMP_Text _winLoseText;
    [SerializeField] GameObject _restartButton;

    public void EndGame(bool hasWon)
    {
        Time.timeScale = 0;
        _winLoseCanvasGroup.alpha = 1;
        _winLoseCanvasGroup.interactable = true;

        EventSystem.current.SetSelectedGameObject(_restartButton);

        if (hasWon)
            _winLoseText.text = "Volvy Survived";
        else
            _winLoseText.text = "Volvy Starved";

    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1.0f;
    }
}
