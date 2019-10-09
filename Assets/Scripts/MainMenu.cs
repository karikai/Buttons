using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private int highScore;
    [SerializeField]
    private TextMeshProUGUI highScoreText;
    [SerializeField]
    private int coins;
    [SerializeField]
    private TextMeshProUGUI coinsText;


    // Start is called before the first frame update
    void Start()
    {
        GetHighScore();
        GetCoins();
        GameObject highScoreObject = GameObject.FindGameObjectWithTag("HiScoreText");
        highScoreText = highScoreObject.GetComponent<TextMeshProUGUI>();
        GameObject coinsObject = GameObject.FindGameObjectWithTag("CoinsText");
        coinsText = coinsObject.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayCoins();
        DisplayHighScore();
        if (Input.GetMouseButtonDown(0))
        {
            NavigationListener();
        }
    }

    // Save & Load Functions

    void GetHighScore()
    {
        if (PlayerPrefs.HasKey("highScore"))
        {
            this.highScore = PlayerPrefs.GetInt("highScore");
        }
        else
        {
            PlayerPrefs.SetInt("highScore", 0);
            this.highScore = 0;
        }
    }

    void GetCoins()
    {
        if (PlayerPrefs.HasKey("coins"))
        {
            this.coins = PlayerPrefs.GetInt("coins");
        }
        else
        {
            PlayerPrefs.SetInt("coins", 0);
            this.coins = 0;
        }
    }

    void DisplayCoins()
    {
        coinsText.SetText(this.coins.ToString("0"));
    }

    void DisplayHighScore()
    {
        highScoreText.SetText("Hi Score: " + this.highScore.ToString());
    }

    // Touch Functions

    string idOfTappedElement()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit != null && hit.collider != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(hit.collider.name.ToString());
                return hit.collider.name.ToString();
            }
            else
            {
                return "0";
            }
        }
        else
        {
            return "0";
        }
    }

    void NavigateToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    void NavigationListener()
    {
        switch (idOfTappedElement())
        {
            case "Start":
                NavigateToScene("GameplayPrototype");
                break;
            case "Buttons":
                NavigateToScene("Buttons");
                break;
            case "Podium":
                NavigateToScene("HiScores");
                break;
            case "Bell":
                break;
            default:
                break;
        }
    }
}
