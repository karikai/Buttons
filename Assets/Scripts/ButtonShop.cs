using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonShop : MonoBehaviour
{
    [SerializeField]
    private int index;
    private int coins;
    [SerializeField]
    private GameObject classicButton;
    [SerializeField]
    private GameObject krazyButton;
    [SerializeField]
    private GameObject starButton;
    [SerializeField]
    private GameObject buyButton;
    [SerializeField]
    private GameObject selectButton;
    [SerializeField]
    private TextMeshProUGUI coinsText;
    [SerializeField]
    private TextMeshProUGUI costText;
    private GameObject costObject;
    private string buttonPrefix = "hasPurchased";
    private string buttonSuffix = "Button";

    // Start is called before the first frame update
    void Start()
    {
        GameObject coinsObject = GameObject.FindGameObjectWithTag("CoinsText");
        coinsText = coinsObject.GetComponent<TextMeshProUGUI>();

        costObject = GameObject.FindGameObjectWithTag("costText");
        costText = costObject.GetComponent<TextMeshProUGUI>();

        coins = PlayerPrefs.GetInt("coins");
        coinsText.SetText(coins.ToString());

        index = 1;
        classicButton = Instantiate(classicButton, new Vector3(0,0,0), Quaternion.identity);
        classicButton.GetComponent<Orb>().color = Color.grey;
        classicButton.GetComponent<Orb>().id = 1;


        krazyButton = Instantiate(krazyButton, new Vector3(0, 0, 0), Quaternion.identity);
        krazyButton.GetComponent<Orb>().color = Color.grey;
        krazyButton.GetComponent<Orb>().id = 2;

        starButton = Instantiate(starButton, new Vector3(0, 0, 0), Quaternion.identity);
        starButton.GetComponent<Orb>().color = Color.grey;
        starButton.GetComponent<Orb>().id = 3;
    }

    // Update is called once per frame
    void Update()
    {
        DisplayButtons();
        coinsText.SetText(coins.ToString());


        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(idOfTappedElement());
            if ("left-chevron".Equals(idOfTappedElement()))
            {
                previous();
            }
            else if (idOfTappedElement().Equals("right-chevron"))
            {
                next();
            }
            else if (idOfTappedElement().Equals("Select"))
            {
                select();
            }
            else if (idOfTappedElement().Equals("Buy"))
            {
                buy();
            }
            else
            {

            }
        }
    }

    void DisplayButtons()
    {
        classicButton.SetActive(false);
        krazyButton.SetActive(false);
        starButton.SetActive(false);
        selectButton.SetActive(false);
        buyButton.SetActive(false);
        costObject.SetActive(false);

        switch (index)
        {
            case 1:
                classicButton.SetActive(true);
                this.selectButton.SetActive(true);
                break;
            case 2:
                krazyButton.SetActive(true);
                if (PlayerPrefs.GetInt(buttonPrefix + "Krazy" + buttonSuffix) == 1)
                {
                    this.selectButton.SetActive(true);
                }
                else
                {
                    this.buyButton.SetActive(true);
                    costText.SetText("- 2500");
                    costObject.SetActive(true);
                }
                break;
            case 3:
                starButton.SetActive(true);
                if (PlayerPrefs.GetInt(buttonPrefix + "Star" + buttonSuffix) == 1)
                {
                    this.selectButton.SetActive(true);
                }
                else
                {
                    this.buyButton.SetActive(true);
                    costText.SetText("- 500");
                    costObject.SetActive(true);
                }
                break;
            default:
                starButton.SetActive(true);
                break;
        }
    }

    void select()
    {
        switch (index)
        {
            case 1:
                PlayerPrefs.SetString("selectedButton", "default");
                break;
            case 2:
                PlayerPrefs.SetString("selectedButton", "krazyButton");
                break;
            case 3:
                PlayerPrefs.SetString("selectedButton", "starButton");
                break;
            default:
                break;
        }

        Debug.Log(PlayerPrefs.GetString("selectedButton"));
    }

    void buy()
    {
        switch (index)
        {
            case 2:
                if (coins > 2500)
                { 
                    coins -= 2500;
                    PlayerPrefs.SetInt("coins", coins);
                    PlayerPrefs.SetInt(buttonPrefix + "Krazy" + buttonSuffix, 1);
                }
                break;
            case 3:
                if (coins > 500)
                { 
                    coins -= 500;
                    PlayerPrefs.SetInt("coins", coins);
                    PlayerPrefs.SetInt(buttonPrefix + "Star" + buttonSuffix, 1);
                }
                break;
            default:
                break;
        }
    }

    void initializeButtonsSaves()
    {
        if (!PlayerPrefs.HasKey(buttonPrefix + "Krazy" + buttonSuffix))
        {
            PlayerPrefs.SetInt(buttonPrefix + "Krazy" + buttonSuffix, 0);
        }

        if (!PlayerPrefs.HasKey(buttonPrefix + "Star" + buttonSuffix))
        {
            PlayerPrefs.SetInt(buttonPrefix + "Star" + buttonSuffix, 0);
        }

        if (!PlayerPrefs.HasKey("selectedButton"))
        {
            PlayerPrefs.SetString("selectedButton", "default");
        }
    }

    string idOfTappedElement()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit != null && hit.collider != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
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


    void next()
    {
        if (index == 3)
        {
            index = 1;
        }
        else
        {
            index ++;
        }
    }

    void previous()
    {
        if (index == 1)
        {
            index = 3;
        }
        else
        {
            index --;
        }
    }
}
