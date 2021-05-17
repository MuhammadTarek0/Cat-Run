using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Enums
    public enum GameState { MainMenu, SettingsMenu, Game, EndLevel}
    public static GameState GAME_STATE = GameState.MainMenu;
    #endregion

    #region Public Events
    public delegate void OnNullEventTrigger();
    public static event OnNullEventTrigger GAME_STATE_CHANGED = delegate { };
    #endregion

    #region Unity Callbacks
    private void OnEnable()
    {
        UIManager.FIRST_INPUT += ManageFirstInput;
        WinTrigger.PLAYER_WON += EndGameWin;
    }

    private void OnDisable()
    {
        UIManager.FIRST_INPUT -= ManageFirstInput;
        WinTrigger.PLAYER_WON -= EndGameWin;
    }
    #endregion

    #region Private Methods
    private void ManageFirstInput()
    {
        GAME_STATE = GameState.Game;
        GAME_STATE_CHANGED();
    }

    private void EndGameWin()
    {
        SaveManager.INSTANCE.AdvanceLevel();
        GAME_STATE = GameState.EndLevel;
        GAME_STATE_CHANGED();
    }
    #endregion

    #region Public Methods
    public void LoadNextLevel()
    {
        try
        {
            SaveManager.INSTANCE.LoadSavedScene();
        }
        catch(Exception exception)
        {
            int index = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(index);
        }
    }
    #endregion
}
