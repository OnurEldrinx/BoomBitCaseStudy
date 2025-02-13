using System.Globalization;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public Image timeBar;
    public TextMeshProUGUI timeText;
    public float countdownTime;

    private float _timeRemaining;
    private bool _isCounting;
    
    private void OnEnable()
    {
        GameManager.Instance.OnGameStart += () =>
        {
            StartCountdown(countdownTime);
        };

        GameManager.Instance.OnNextLevel += Reset;
    }

    private void StartCountdown(float duration)
    {
        _timeRemaining = duration;
        _isCounting = true;
    }

    private void Update()
    {
        if (_isCounting && _timeRemaining > 0)
        {
            _timeRemaining -= Time.deltaTime;
            timeText.text = Mathf.Ceil(_timeRemaining).ToString(CultureInfo.CurrentCulture); // Display whole seconds
            
            timeBar.fillAmount = _timeRemaining/countdownTime;
            
        }
        else if (_isCounting)
        {
            _isCounting = false;
            timeText.text = "0";
            OnCountdownEnd();
        }
    }

    private void OnCountdownEnd()
    {
        print("Countdown Finished!");
        //TimeIsUp?.Invoke();
        GameManager.Instance.SetState(GameState.Win);
    }

    private void Reset()
    {
        _timeRemaining = 0;
        _isCounting = false;
        timeBar.fillAmount = 1;
    }
}