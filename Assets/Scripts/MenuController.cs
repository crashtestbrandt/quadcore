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

    public void OnHotseatSelect()
    {
        GameState.GameMode = GameController.GameModeType.LOCAL_MULTIPLAYER;
        SceneManager.LoadScene(1);

    }

    public void OnVsAISelect()
    {
        GameState.GameMode = GameController.GameModeType.LOCAL_AI;
        SceneManager.LoadScene(1);
    }
}
