using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public int id;
    public Color color;
    public Color flickerColor;
    public bool isCorrectColor;
    public bool isFlickering = false;
    public float timeUntilFlicker;
    public float elapsedTime;
    SpriteRenderer m_SpriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        m_SpriteRenderer.color = this.color;
        gameObject.name = this.id.ToString();
        flickerColor = LightenColor(this.color);
        elapsedTime = 0.0f;
        timeUntilFlicker += 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        elapsedTime += Time.deltaTime;
        if (isCorrectColor)
        {
            FlickerCheck();
            UpdateFlicker();
        }

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit != null && hit.collider != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (transform.name == hit.collider.name.ToString())
                {
                    ChangeCorrectColor();
                }
            }
        }
    }

    void ChangeCorrectColor()
    {
        if (isCorrectColor)
        {
            isCorrectColor = false;
        }
        else
        {
            isCorrectColor =  true;
        }
    }

    Color LightenColor(Color color)
    {
        color.r += 0.75f;
        color.g += 0.75f;
        color.b += 0.75f;

        return color;
    }

    void FlickerCheck()
    {
        if (elapsedTime > timeUntilFlicker)
        {
            ChangeFlicker();
            timeUntilFlicker += 0.25f;
        }
    }

    void UpdateFlicker()
    {
        if (isFlickering)
        {
            m_SpriteRenderer.color = this.flickerColor;
        }
        else
        {
            m_SpriteRenderer.color = this.color;
        }
    }

    void ChangeFlicker()
    {
        if (isFlickering)
        {
            isFlickering = false;
        }
        else
        {
            isFlickering = true;
        }
    }

    void ResetFlickerCooldown()
    {

    }
}
