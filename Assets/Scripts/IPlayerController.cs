using UnityEngine.UI;

public interface IPlayerController
{
    void SetDebugTextObject(Text textObject);
    void SetPlayerNumber(int num);
    void StartTurn();
}