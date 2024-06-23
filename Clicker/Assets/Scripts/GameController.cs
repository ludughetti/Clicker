using System.Collections;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private float maxTime = 10f;
    [SerializeField] private TMP_Text startGameInstruction;
    [SerializeField] private TMP_Text timeCounter;
    [SerializeField] private TMP_Text clicksCounter;
    [SerializeField] private TMP_Text highScoreCounter;
    [SerializeField] private GameObject creditsScreen;
    [SerializeField] private AdsController ads;

    private static string _prefKey_highScore = "HighScore";

    private bool _isTimerRunning;
    private float _currentTimer;
    private float _tempMaxTime;
    private int _currentClicks;
    private int _highestScoreSaved;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        SetupData();
        FetchHighScore();
    }

    private void Update()
    {
        if(_isTimerRunning)
        {
            _currentTimer -= Time.deltaTime;
            timeCounter.text = GetFormattedTimeToDisplay();
            highScoreCounter.text = GetHighestScoreToDisplay();

            if(_currentTimer <= 0)
                ExecuteEndgame();
        }
    }

    // Start playing or add score
    public void Click() {
        if (!_isTimerRunning)
        {
            _isTimerRunning = true;
            _currentTimer -= Time.deltaTime;
            startGameInstruction.gameObject.SetActive(false);
        }

        if (_currentTimer < _tempMaxTime
            && _currentTimer > 0)
        {
            _currentClicks++;
            clicksCounter.text = _currentClicks.ToString();
        }
    }

    private void ExecuteEndgame()
    {
        _currentTimer = 0;
        UpdateHighScore();
        StartCoroutine(Cooldown());
    }

    public bool IsCreditScreenActive()
    {
        return creditsScreen.activeSelf;
    }

    public void ToggleCreditsScreen()
    {
        creditsScreen.SetActive(!creditsScreen.activeSelf);
    }
    public void AddTime(float timeToAdd)
    {
        _currentTimer += timeToAdd;
        _tempMaxTime += timeToAdd;
        timeCounter.text = GetFormattedTimeToDisplay();
    }

    private void SetupData()
    {
        _isTimerRunning = false;
        _tempMaxTime = maxTime;
        _currentTimer = _tempMaxTime;
        _currentClicks = 0;
        timeCounter.text = GetFormattedTimeToDisplay();
    }

    private string GetFormattedTimeToDisplay()
    {
        int seconds = (int) _currentTimer;
        int milliseconds = (int)((_currentTimer - seconds) * 100);

        return string.Format("{0:00}:{1:00}", seconds, milliseconds);
    }

    private string GetHighestScoreToDisplay()
    {
        return _currentClicks > _highestScoreSaved 
            ? _currentClicks.ToString() 
            : _highestScoreSaved.ToString();
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1);
        SetupData();
        startGameInstruction.gameObject.SetActive(true);
        ads.ShowAdsAfterTurnEnded();
    }

    private void FetchHighScore()
    {
        _highestScoreSaved = PlayerPrefs.HasKey(_prefKey_highScore) 
            ?  PlayerPrefs.GetInt(_prefKey_highScore) 
            : 0;
    }

    private void UpdateHighScore()
    {
        if(_currentClicks > _highestScoreSaved)
        {
            _highestScoreSaved = _currentClicks;
            PlayerPrefs.SetInt(_prefKey_highScore, _currentClicks);
            PlayerPrefs.Save();
        }
    }
}
