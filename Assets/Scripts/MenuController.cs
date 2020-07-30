using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameState
{
    public static GameController.GameModeType GameMode;
}
public class MenuController : MonoBehaviour
{
    public Canvas MainMenu;
    public Canvas OnlineMenu;

    Canvas back = null;
    Canvas current = null;

    private void Start() {
        current = MainMenu;

        OnlineMenu.gameObject.SetActive(false);
        MainMenu.gameObject.SetActive(true);
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

        MainMenu.gameObject.SetActive(false);
        OnlineMenu.gameObject.SetActive(true);
        current = OnlineMenu;
        back = MainMenu;
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
}
