using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private GameState currentState;

    public Action OnGameStart;
    public Action OnWin;
    public Action OnFail;
    public Action OnNextLevel;
    public Action OnRestartLevel;
    

    private void Start()
    {
        SetState(GameState.Start);
    }

    public void SetState(GameState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case GameState.Start:
                OnGameStart?.Invoke();
                break;
            case GameState.Win:
                OnWin?.Invoke();
                break;
            case GameState.Fail:
                // Fail
                OnFail?.Invoke();
                break;
            case GameState.NextLevel:
                OnNextLevel?.Invoke();
                SetState(GameState.Start);
                break;
            case GameState.LevelRestart:
                OnRestartLevel?.Invoke();
                break;
        }
    }
}

public enum GameState { Start,Win,Fail,NextLevel,LevelRestart }
