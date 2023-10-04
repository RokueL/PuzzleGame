using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.U2D;

public class Dot : MonoBehaviour
{
    int value;
    public IObjectPool<GameObject> dotPool { get; set; }
    SpriteAtlas spAtlas;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spAtlas = Resources.Load<SpriteAtlas>("Textures/Dot");
        value = Random.Range(0, 5);
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
