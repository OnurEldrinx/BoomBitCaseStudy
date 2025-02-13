using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private TextMeshProUGUI finalKillCountText;
    [SerializeField] private TextMeshProUGUI killCounterText;
    [SerializeField] private Button nextLevelButton;
    

    
    private void OnEnable()
    {
        GameManager.Instance.OnWin += ShowWinScreen;
        GameManager.Instance.OnNextLevel += Reset;
        
        nextLevelButton.onClick.AddListener(() =>
        {
            GameManager.Instance.SetState(GameState.NextLevel);
        });
        
    }

    private void OnDisable()
    {
        GameManager.Instance.OnWin -= ShowWinScreen;
    }

    public void UpdateKillCounterText(int value)
    {
        killCounterText.text = value.ToString();
    }

    private async void ShowWinScreen()
    {
        try
        {
            await Task.Delay(2000);
            winScreen.SetActive(true);
            finalKillCountText.text = DataManager.Instance.GetEnemyCount().ToString();
        }
        catch (Exception)
        {
            // ignored
        }
    }

    private void Reset()
    {
        winScreen.SetActive(false);
        finalKillCountText.text = "0";
        killCounterText.text = "0";
    }
    
}
