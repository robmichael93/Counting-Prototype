using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject titleScreen;
    [SerializeField] GameObject ball;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI ballsLeftText;
    [SerializeField] TextMeshProUGUI ballsCaughtText;
    [SerializeField] TextMeshProUGUI jackpotText;
    [SerializeField] Button restartButton;
    [SerializeField] AudioClip buttonSound;
    [SerializeField] AudioSource gameAudio;
    [SerializeField] AudioClip dropSound;
    [SerializeField] GameObject surface;
    [SerializeField] ParticleSystem scoreSplash;
    [SerializeField] float zRange;
    [SerializeField] List<ParticleSystem> jackpotSplashes;
    [SerializeField] int ballsLeft;
    [SerializeField] int ballsCaught;
    [SerializeField] float timeLeft;
    [SerializeField] bool isGameActive;
    [SerializeField] bool platformShouldRotate;
    [SerializeField] int numberOfActiveBalls;
    [SerializeField] bool hitJackpot;

    void Start()
    {
        surface = GameObject.FindGameObjectWithTag("Surface");
        zRange = surface.GetComponent<BoxCollider>().bounds.size.z / 2;
        gameAudio = GameObject.FindGameObjectWithTag("Platform").GetComponent<AudioSource>();
    }

    public void UpdateBallsCaught()
    {
        ballsCaught++;
        ballsCaught = Mathf.Min(ballsCaught, 50);
        ballsCaughtText.text = "Balls Caught: " + ballsCaught;
    }

    public void UpdateBallsLeft()
    {
        ballsLeft--;
        ballsLeft = Mathf.Max(ballsLeft, 0);
        ballsLeftText.text = "Balls Left: " + ballsLeft;
        numberOfActiveBalls = FindObjectsOfType<Ball>().Length;
    }

    public void GameOver()
    {
        isGameActive = false;
        platformShouldRotate = false;
        int numberOfSplashes = Mathf.FloorToInt(ballsCaught / 10);
        for (int i = 0; i < numberOfSplashes; i++)
        {
            StartCoroutine(PlaySplashes(i));
        }
       if (ballsCaught == 50)
        {
            hitJackpot = true;
            StartCoroutine(PlayJackpotSplashes());
        }
        StartCoroutine(ShowGameOverUI(hitJackpot));
    }

    public void RestartGame()
    {
        gameAudio.PlayOneShot(buttonSound, 1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame()
    {
        isGameActive = false;
        platformShouldRotate = true;
        hitJackpot = false;
        ballsLeft = 50;
        ballsCaught = 0;
        timeLeft = 60;
        titleScreen.gameObject.SetActive(false);
        timerText.gameObject.SetActive(true);
        ballsLeftText.gameObject.SetActive(true);
        ballsCaughtText.gameObject.SetActive(true);
        gameAudio.PlayOneShot(buttonSound, 1);
    }

    void UpdateTime(float timeRemaining)
    {
        timerText.text = "Time: " + Mathf.FloorToInt(timeLeft % 60);
    }

    void CountdownTimer()
    {
        timeLeft -= Time.deltaTime;
        timeLeft = Mathf.Max(timeLeft, 0);
        UpdateTime(timeLeft);
        numberOfActiveBalls = FindObjectsOfType<Ball>().Length;        
        if (timeLeft <= 0 && numberOfActiveBalls == 0)
        {
            GameOver();
        }
    }

    public bool ShouldRotate()
    {
        return platformShouldRotate;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGameActive)
        {
            CountdownTimer();
            if (ballsLeft == 0 && numberOfActiveBalls == 0)
            {
                GameOver();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) || (Input.GetMouseButtonDown(0) && platformShouldRotate))
        {
            if (!isGameActive)
            {
                isGameActive = true;
            }
            if (ballsLeft > 0 && timeLeft > 0)
            {
                Instantiate(ball, new Vector3(4, 4, 0), transform.rotation);
                gameAudio.PlayOneShot(dropSound, 2);
                UpdateBallsLeft();
            }
        }
    }

    IEnumerator PlaySplashes(float delay)
    {
        yield return new WaitForSeconds(1 + delay * 0.5f);
        float xPos = surface.transform.position.x;
        float yPos = surface.transform.position.y + 2;
        float zPos = Random.Range(-zRange, zRange);
        Instantiate(scoreSplash, new Vector3(xPos, yPos, zPos), scoreSplash.transform.rotation);
    }

    IEnumerator PlayJackpotSplashes()
    {
        yield return new WaitForSeconds(4);
        int numberOfSplashes = Random.Range(10, 15);
        for (int i = 0; i < numberOfSplashes; i++)
        {
            float xPos = surface.transform.position.x;
            float yPos = surface.transform.position.y + Random.Range(-2, 3);
            float zPos = Random.Range(-zRange, zRange);

            int splashIndex = Random.Range(0, jackpotSplashes.Count);
            ParticleSystem splash = Instantiate(jackpotSplashes[splashIndex], new Vector3(xPos, yPos, zPos), scoreSplash.transform.rotation);
            yield return new WaitForSeconds(0.1f);
        }
        jackpotText.gameObject.SetActive(true);
    }

    IEnumerator ShowGameOverUI(bool hitJackpot)
    {
        int delay;
        if (hitJackpot)
        {
            delay = 6;
        }
        else
        {
            delay = 4;
        }
        yield return new WaitForSeconds(delay);
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
    }
}
