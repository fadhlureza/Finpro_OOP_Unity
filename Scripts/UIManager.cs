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

    Animator scoreAnimator;
    bool hasCheckedGameOver = false;

    void OnEnable()
    {
        ScoreManager.ScoreUpdated += OnScoreUpdated;
    }

    void OnDisable()
    {
        ScoreManager.ScoreUpdated -= OnScoreUpdated;
    }

    // Use this for initialization
    void Start()
    {
        scoreAnimator = score.GetComponent<Animator>();
        score.gameObject.SetActive(false);
        scoreInScoreBg.text = ScoreManager.Instance.Score.ToString();
            

        if (!firstLoad)
        {
            HideAllButtons();
        }
    }

    // Update is called once per frame
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
}

