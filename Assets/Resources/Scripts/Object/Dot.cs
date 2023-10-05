using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.U2D;

public class Dot : MonoBehaviour
{

    Vector2 firstTouchPosition;
    Vector2 finalTouchPosition;

    float swipeAngle;

    public int value;
    public IObjectPool<GameObject> dottPool { get; set; }
    SpriteAtlas spAtlas;
    SpriteRenderer spriteRenderer;

    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(firstTouchPosition);
    }
    private void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log(finalTouchPosition);
        CalculateAngle();
    }

    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * Mathf.Rad2Deg;
        Debug.Log(swipeAngle);
    }


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spAtlas = Resources.Load<SpriteAtlas>("Textures/Dot");
    }

    // Update is called once per frame
    void Update()
    {
        switch(value)
        {
            case 0:
                spriteRenderer.sprite = spAtlas.GetSprite("RedDot");
                break; 
            case 1:
                spriteRenderer.sprite = spAtlas.GetSprite("YellowDot");
                break;
            case 2:
                spriteRenderer.sprite = spAtlas.GetSprite("GreenDot");
                break;
            case 3:
                spriteRenderer.sprite = spAtlas.GetSprite("BlueDot");
                break;
            case 4:
                spriteRenderer.sprite = spAtlas.GetSprite("PurpleDot");
                break;
        }
    }
}
