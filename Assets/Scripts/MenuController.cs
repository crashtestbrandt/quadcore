using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public static class GameState
{
    public static GameController.GameModeType GameMode;
}
public class MenuController : MonoBehaviour
{
    public Canvas MainMenu;

    Canvas back = null;
    Canvas current = null;

    private void Start() {
        UpdateMenuCanvasTo(MainMenu);
    }

    public void OnHotseatSelect()
    {
        GameState.GameMode = GameController.GameModeType.LOCAL_MULTIPLAYER;
        SceneManager.LoadScene(1, LoadSceneMode.Single);

    }

    public void OnVsAISelect()
    {
        GameState.GameMode = GameController.GameModeType.LOCAL_AI;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void OnJoinConfirm()
    {

    }

    public void OnCreateConfirm()
    {

    }

    public void OnCopyMatchID(Text textObject)
    {
        TextEditor matchID = new TextEditor();
        matchID.text = Guid.NewGuid().ToString();
        matchID.SelectAll();
        matchID.Copy();
        textObject.text = "Match ID copied!";
    }

    public void UpdateMenuCanvasTo(Canvas canvas)
    {
        back = current ?? null;
        current = canvas;

        back?.gameObject.SetActive(false);
        current.gameObject.SetActive(true);

    }
}
