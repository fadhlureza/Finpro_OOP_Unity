using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using SgLib;

public class UIManager : MonoBehaviour
{
    public static bool firstLoad = true;
    public GameManager gameManager;
    public Text score;
    public Text scoreInScoreBg;
    public Text bestScore;
    public GameObject buttons;
    public Button muteBtn;
    public Button unMuteBtn;
    public Text pauseText; // Changed to Text component

    private Animator scoreAnimator;
    private bool hasCheckedGameOver = false;
    private bool isPaused = false;

    void OnEnable()
    {
        ScoreManager.ScoreUpdated += OnScoreUpdated;
    }

    void OnDisable()
    {
        ScoreManager.ScoreUpdated -= OnScoreUpdated;
    }

    void Start()
    {
        scoreAnimator = score.GetComponent<Animator>();
        score.gameObject.SetActive(false);
        scoreInScoreBg.text = ScoreManager.Instance.Score.ToString();
        pauseText.gameObject.SetActive(false);

        if (!firstLoad)
        {
            HideAllButtons();
        }
    }

    void Update()
    {
        score.text = ScoreManager.Instance.Score.ToString();
        bestScore.text = ScoreManager.Instance.HighScore.ToString();
        UpdateMuteButtons();

        if (gameManager.gameOver && !hasCheckedGameOver)
        {
            hasCheckedGameOver = true;
            Invoke("ShowButtons", 1f);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    void OnScoreUpdated(int newScore)
    {
        scoreAnimator.Play("NewScore");
    }

    public void HandlePlayButton()
    {
        if (!firstLoad)
        {
            StartCoroutine(Restart());
        }
        else
        {
            HideAllButtons();
            gameManager.StartGame();
            gameManager.CreateBall();
            firstLoad = false;
        }
    }

    public void ShowButtons()
    {
        buttons.SetActive(true);
        score.gameObject.SetActive(false);
        scoreInScoreBg.text = ScoreManager.Instance.Score.ToString();
    }

    public void HideAllButtons()
    {
        buttons.SetActive(false);
        score.gameObject.SetActive(true);
    }

    public void FinishLoading()
    {
        if (firstLoad)
        {
            ShowButtons();
        }
        else
        {
            HideAllButtons();
        }
    }

    void UpdateMuteButtons()
    {
        if (SoundManager.Instance.IsMuted())
        {
            unMuteBtn.gameObject.SetActive(false);
            muteBtn.gameObject.SetActive(true);
        }
        else
        {
            unMuteBtn.gameObject.SetActive(true);
            muteBtn.gameObject.SetActive(false);
        }
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToggleSound()
    {
        SoundManager.Instance.ToggleMute();
    }

    void Pause()
    {
        isPaused = true;
        pauseText.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    void Resume()
    {
        isPaused = false;
        pauseText.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }
}