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
    public Canvas OnlineMenu;
    public Canvas CreateMenu;
    public Canvas JoinMenu;

    public Text MatchIDText;

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

    public void OnOnlineSelect()
    {
        GameState.GameMode = GameController.GameModeType.NETWORK_MULTIPLAYER;
        UpdateMenuCanvasTo(OnlineMenu);
    }

    public void OnJoinSelect()
    {
        UpdateMenuCanvasTo(JoinMenu);
    }

    public void OnJoinConfirm()
    {

    }

    public void OnCreateSelect()
    {
        UpdateMenuCanvasTo(CreateMenu);
    }

    public void OnCreateConfirm()
    {

    }

    public void OnCopyMatchID()
    {
        TextEditor matchID = new TextEditor();
        matchID.text = Guid.NewGuid().ToString();
        matchID.SelectAll();
        matchID.Copy();
        MatchIDText.text = "Match ID copied!";
    }

    public void OnBackSelected()
    {
        if (current != null && back != null)
        {
            current.gameObject.SetActive(false);
            back.gameObject.SetActive(true);
            Canvas temp = back;
            back = current;
            current = temp;
        }
    }

    private void UpdateMenuCanvasTo(Canvas canvas)
    {
        back = current ?? null;
        current = canvas;

        back?.gameObject.SetActive(false);
        current.gameObject.SetActive(true);

    }
}
