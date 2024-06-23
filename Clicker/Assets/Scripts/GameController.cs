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

    private bool _isGameStarted;
    private float _currentTimer;
    private float _currentClicks;

    private void Awake()
    {
        SetupData();
    }

    private void Update()
    {
        if(_isGameStarted && _currentTimer < maxTime
            && _currentTimer > 0)
        {
            _currentTimer -= Time.deltaTime;
            timeCounter.text = GetFormattedTime();

            if(_currentTimer <= 0)
            {
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

    private string GetFormattedTime()
    {
        int seconds = (int) _currentTimer;
        int milliseconds = (int)((_currentTimer - seconds) * 100);

        return string.Format("{0:00}:{1:00}", seconds, milliseconds);
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(2);
        _isGameStarted = false;
    }
}
