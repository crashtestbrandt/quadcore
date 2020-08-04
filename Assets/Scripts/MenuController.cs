using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Threading;
using System.Threading.Tasks;

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
        GameState.GameMode = GameController.GameModeType.NETWORK_MULTIPLAYER;
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public async void OnCreateConfirm()
    {
        GameState.GameMode = GameController.GameModeType.NETWORK_MULTIPLAYER;
        await NetworkController.CreateNewMatchAsync();
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void OnCopyMatchID(Text textObject)
    {
        TextEditor matchID = new TextEditor();
        matchID.text = NetworkController.MatchID;
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
