using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool gameOver = false;
    [SerializeField]
    private bool isPaused = false;
    [SerializeField]
    private int score = 0;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private float timeElapsed = 0.0f;
    [SerializeField]
    private float nextCheckpoint;
    [SerializeField]
    private float initialTimeForNextCheckpoint;
    [SerializeField]
    private Text timerText;
    [SerializeField]
    private int correctColorId = 1;
    [SerializeField]
    private LinkedList<ColorObject> colorList;
    [SerializeField]
    private GameObject[] buttonSkins = new GameObject[3];
    [SerializeField]
    private GameObject orb;
    [SerializeField]
    private GameObject restartButton;
    [SerializeField]
    private GameObject playButton;
    [SerializeField]
    private GameObject quitButton;
    private LinkedList<GameObject> orbList;
    SpriteRenderer m_SpriteRenderer;
    [SerializeField]
    private int numberOfTaps;
    [SerializeField]
    private int highScore;
    [SerializeField]
    private Text highScoreText;
    [SerializeField]
    private int coins;
    [SerializeField]
    private Text coinsText;
    private string selectedButton;
    

    // Start is called before the first frame update
    void Start()
    {
        selectedButton = PlayerPrefs.GetString("selectedButton");
        switch (selectedButton)
        {
            case "default":
                break;
            case "krazyButton":
                orb = buttonSkins[1];
                break;
            case "starButton":
                orb = buttonSkins[2];
                break;
            default:
                break;
        }
        colorList = new LinkedList<ColorObject>();
        orbList = new LinkedList<GameObject>();

        // Fetch the SpriteRenderer from the GameObject
        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        GetHighScore();
        GetCoins();

        timeElapsed = 0.0f;
        nextCheckpoint = timeElapsed + 3.5f;
        initialTimeForNextCheckpoint = nextCheckpoint;

        initializeColor();
        GenerateOrbs();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            if (!hasTimeRunOut())
            {
                if (!isPaused)
                {
                    timeElapsed += Time.deltaTime;
                    UpdateScore();
                    UpdateTime();
                    updateColorBar();
                    DisplayHighScore();
                    DisplayCoins();
                    UpdateHighScore();
                    UpdateColorBarSize();
                    quitButton.SetActive(false);
                    playButton.SetActive(false);
                    restartButton.SetActive(false);

                    if (Input.GetMouseButtonDown(0))
                    {
                        string colorId = this.correctColorId.ToString();
                        if (colorId.Equals(idOfTappedElement()))
                        {
                            CorrectOrb();
                        }
                        else if (idOfTappedElement().Equals("0"))
                        {
                            Debug.Log("Did nothing");
                        }else if (idOfTappedElement().Equals("Pause"))
                        {
                            isPaused = true;
                        }
                        else
                        {
                            IncorrectOrb();
                        }
                    }
                }
                else
                {
                    quitButton.SetActive(true);
                    playButton.SetActive(true);
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (idOfTappedElement().Equals("Play"))
                        {
                            isPaused = false;
                        }
                        else if (idOfTappedElement().Equals("remove"))
                        {
                            SceneManager.LoadScene("Main Menu");
                        }
                        else
                        {
                        }
                    }
                }

    
            } else
            {
                restartButton.SetActive(true);
                quitButton.SetActive(true);
                if (Input.GetMouseButtonDown(0))
                {
                    if (idOfTappedElement().Equals("replay"))
                    {
                        IncorrectOrb();
                        timeElapsed = 0.0f;
                        nextCheckpoint = timeElapsed + 3.5f;
                        score = 0;

                        initializeColor();
                        GenerateOrbs();
                        this.gameOver = false;
                    }
                    else if (idOfTappedElement().Equals("remove"))
                    {
                        SceneManager.LoadScene("Main Menu");
                    }
                    else { }
                }
            }
        }
        else
        {
            restartButton.SetActive(true);
            quitButton.SetActive(true);
            if (Input.GetMouseButtonDown(0))
            {
                if (idOfTappedElement().Equals("replay"))
                {
                    IncorrectOrb();
                    timeElapsed = 0.0f;
                    nextCheckpoint = timeElapsed + 3.5f;
                    score = 0;

                    initializeColor();
                    GenerateOrbs();
                    this.gameOver = false;
                }
                else if (idOfTappedElement().Equals("remove"))
                {
                    SceneManager.LoadScene("Main Menu");
                }
                else {}
            }
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

    void SetHighScore(int score)
    {
        PlayerPrefs.SetInt("highScore", score);
        this.highScore = score;
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

    void SetCoins(int coins)
    {
        PlayerPrefs.SetInt("coins", coins);
    }

    // Coin Funcions

    void addCoins()
    {
        this.coins ++;
    }

    void UpdateCoins()
    {
        SetCoins(score);
    }

    void DisplayCoins()
    {
        coinsText.text = this.coins.ToString("0");
    }

    // Score Functions

    void UpdateScore()
    {
        scoreText.text = this.score.ToString("0");
    }

    void addScore()
    {
        this.score += (this.score / 10) + 2;
    }

    void UpdateHighScore()
    {
        if (score > highScore)
        {
            SetHighScore(score);
        }
    }

    void DisplayHighScore()
    {
        highScoreText.text = "Hi: " + this.highScore.ToString("0");
    }

    // Time Functions

    bool hasTimeRunOut()
    {
        if (timeElapsed >= nextCheckpoint)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    float TimeUntilNextCheckpoint()
    {
        return (nextCheckpoint - timeElapsed);
    }

    void ResetTime()
    {
        nextCheckpoint = 3.5f;
        float subtractionFactor;
        float extraTime = Random.Range(1, 3) / 10;

        if (score < 100)
        {
            subtractionFactor = (1 / 25f * score) * 0.5f;
        } else if (score > 100 && score < 350)
        {
            subtractionFactor = 2;
            subtractionFactor += (1 / 200f * score) * 0.5f;
        }
        else
        {
            subtractionFactor = 2.75f;
        }

        nextCheckpoint -= subtractionFactor;
        nextCheckpoint += timeElapsed;
        nextCheckpoint += extraTime;
        initialTimeForNextCheckpoint = TimeUntilNextCheckpoint();
    }

    void UpdateTime()
    {
        timerText.text = TimeUntilNextCheckpoint().ToString("0.0") + "s";
    }

    float GetBarNumerator()
    {
        return TimeUntilNextCheckpoint();
    }

    float GetBarDenomenator()
    {
        return initialTimeForNextCheckpoint;
    }

    // Color Picker Functions

    void GenerateRandomColors()
    {
        Color[] randomColors = { Color.red, Color.blue, Color.cyan, Color.green, Color.magenta };

        this.correctColorId = Random.Range(1, GetNumberOfOrbs() + 1);

        for (int i = 0; i < GetNumberOfOrbs(); i++)
        {
            Color randomColor = randomColors[i];
            ColorObject newColor = new ColorObject(randomColor, i + 1);
            this.colorList.AddFirst(newColor);
        }
    }

    void initializeColor()
    {
        Color[] randomColors = { Color.red, Color.blue, Color.cyan, Color.green, Color.magenta };
        Color randomColor = randomColors[Random.Range(1, 5)];
        this.colorList = new LinkedList<ColorObject>();
        this.correctColorId = 1;
        ColorObject newColor = new ColorObject(randomColor, 1);
        this.colorList.AddFirst(newColor);
    }

    void updateColorBar()
    {
        foreach (ColorObject i in colorList)
        {
            if (i.id == correctColorId)
            {
                m_SpriteRenderer.color = i.color;
            }
        }
    }

    void clearColors()
    {
        this.colorList = new LinkedList<ColorObject>();
    }

    int GetNumberOfOrbs()
    {
        int numberOfOrbs;

        if (score < 25)
        {
            numberOfOrbs = 1;
        } else if (score >= 25 && score < 75)
        {
            numberOfOrbs = 2;
        }
        else if (score >= 75 && score < 150)
        {
            numberOfOrbs = 3;
        }
        else if (score >= 150 && score < 250)
        {
            numberOfOrbs = 4;
        } else
        {
            numberOfOrbs = 5;
        }

        return numberOfOrbs;
    }

    void UpdateColorBarSize()
    {
        Vector3 temp = transform.localScale;
        temp.x = (GetBarNumerator() / GetBarDenomenator());
        transform.localScale = temp;
    }


    // Orb Functions

    void GenerateOrbs()
    {
        foreach (ColorObject i in colorList)
        {
            GameObject tempOrb = Instantiate(orb, GenerateRandomPosition(), Quaternion.identity);
            Orb tempOrbScript = tempOrb.GetComponent<Orb>();
            tempOrbScript.color = i.color;
            tempOrbScript.id = i.id;
            if (this.correctColorId == tempOrbScript.id)
            {
                tempOrbScript.isCorrectColor = true;
            }
            this.orbList.AddLast(tempOrb);
        }
    }

    Vector3 GenerateRandomPosition()
    {
        bool doesCollide = false;
        float xMin = -1.25f;
        float xMax = 1.25f;
        float yMin = -2f;
        float yMax = 1.5f;
        float randomX = (float)System.Math.Round(Random.Range(xMin, xMax), 2);
        float randomY = (float)System.Math.Round(Random.Range(yMin, yMax), 2);
        Vector3 orbPosition = new Vector3(randomX, randomY, 0);

        foreach (GameObject orb in orbList)
        {
            if (doesOrbOverlap(orb.GetComponent<CircleCollider2D>(), orbPosition))
            {
                doesCollide = true;
            }
        }

        if (doesCollide)
        {
            return GenerateRandomPosition();
        }
        else
        {
            return orbPosition;
        }
    }

    bool doesOrbOverlap(CircleCollider2D c, Vector3 point)
    {
        Vector3 closest = c.ClosestPoint(point);
        // Because closest=point if point is inside - not clear from docs I feel
        return closest == point;
    }

    void clearOrbs()
    {
        foreach (GameObject i in orbList)
        {
            Destroy(i);
        }
        this.orbList = new LinkedList<GameObject>();
    }

    void CorrectOrb()
    {
        ResetTime();
        addScore();
        addCoins();
        clearColors();
        clearOrbs();
        GenerateRandomColors();
        GenerateOrbs();
    }

    void IncorrectOrb()
    {
        SetCoins(this.coins);
        clearColors();
        clearOrbs();
        this.gameOver = true;
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
                return hit.collider.name.ToString();
            } else
            {
                return "0";
            }
        }
        else
        {
            return "0";
        }
    }
}

public class ColorObject
{
    public ColorObject(Color color, int id)
    {
        this.color = color;
        this.id = id;
    }

    public Color color;
    public int id;
}