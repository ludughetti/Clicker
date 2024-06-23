using System.Collections;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private TMP_Text startGameInstruction;
    [SerializeField] private TMP_Text timeCounter;
    [SerializeField] private TMP_Text clicksCounter;
    [SerializeField] private TMP_Text highScoreCounter;
    [SerializeField] private GameObject creditsScreen;
    [SerializeField] private float maxTime = 10f;

    private static string _prefKey_highScore = "HighScore";

    private bool _isGameStarted;
    private float _currentTimer;
    private int _currentClicks;
    private int _highestScoreSaved;

    private void Awake()
    {
        SetupData();
        FetchHighScore();
    }

    private void Update()
    {
        if(_isGameStarted && _currentTimer < maxTime
            && _currentTimer > 0)
        {
            _currentTimer -= Time.deltaTime;
            timeCounter.text = GetFormattedTimeToDisplay();
            highScoreCounter.text = GetHighestScoreToDisplay();

            if(_currentTimer <= 0)
            {
                UpdateHighScore();
                StartCoroutine(Cooldown());
                startGameInstruction.gameObject.SetActive(true);
            }
        }
    }

    // Start playing or add score
    public void Click() {
        if (!_isGameStarted)
        {
            SetupData();
            _isGameStarted = true;
            _currentTimer -= Time.deltaTime;
            startGameInstruction.gameObject.SetActive(false);
        }

        if (_currentTimer < maxTime
            && _currentTimer > 0)
        {
            _currentClicks++;
            clicksCounter.text = _currentClicks.ToString();
        }
    }

    public bool IsCreditScreenActive()
    {
        return creditsScreen.activeSelf;
    }

    public void ToggleCreditsScreen()
    {
        creditsScreen.SetActive(!creditsScreen.activeSelf);
    }

    private void SetupData()
    {
        _isGameStarted = false;
        _currentTimer = maxTime;
        _currentClicks = 0;
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
        yield return new WaitForSeconds(2);
        _isGameStarted = false;
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
