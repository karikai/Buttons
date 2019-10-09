using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            NavigationListener();
        }
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
            case "Back":
                NavigateToScene("Main Menu");
                break;
            default:
                break;
        }
    }
}
