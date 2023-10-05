using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.U2D;

public class Dot : MonoBehaviour
{

    Vector2 firstTouchPosition;
    Vector2 finalTouchPosition;
    Vector2 tempPosition;

    float swipeAngle;

    public int value;
    public int column;
    public int row;
    public int targetX, targetY;

    public bool isMatch;

    Board board;

    GameObject otherDot;

    public IObjectPool<GameObject> dottPool { get; set; }
    public SpriteAtlas spAtlas;
    SpriteRenderer spriteRenderer;

    private void OnMouseDown()
    {
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(firstTouchPosition);
    }
    private void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CalculateAngle();
    }
    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * Mathf.Rad2Deg;
        Debug.Log(swipeAngle);
        MoveDots();
    }
    void MoveDots()
    {
        if (swipeAngle > 45 && swipeAngle <= 135 && row < board.Height - 1) //윗 방향
        {
            otherDot = board.allDots[column, row+1]; //윗 방향 도트 가져오기
            otherDot.GetComponent<Dot>().row -= 1; // 가져온 도트를 밑으로 내려놓기
            row += 1;
        }
        else if(swipeAngle > -45 && swipeAngle <= 45 && column < board.Width - 1) // 오른쪽 방향
        {
            otherDot = board.allDots[column + 1, row]; // 같음
            otherDot.GetComponent<Dot>().column -= 1; //같음
            column += 1;
        }
        else if (swipeAngle >= -135 && swipeAngle < -45 && row > 0) // 아랫 방향
        {
            otherDot = board.allDots[column, row - 1];
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
        }
        else if (swipeAngle > 135 || swipeAngle <= -135 && column > 0) // 왼쪽 방향 여기는 각도가 끝의 각이라서 ||로 처리한다.
        {
            otherDot = board.allDots[column - 1, row];
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
        }
        else
        {

        }
    }

    void CheckMath()
    {
        if(column > 0 && column < board.Width - 1)
        {
            GameObject left = board.allDots[column-1, row];
            GameObject right = board.allDots[column +1, row];
            if(left.GetComponent<Dot>().value == value && right.GetComponent<Dot>().value == value)
            {
                left.GetComponent<Dot>().isMatch = true;
                right.GetComponent<Dot>().isMatch = true;
                isMatch = true;
            }
        }

        if (row > 0 && row < board.Height - 1)
        {
            GameObject up = board.allDots[column, row +1];
            GameObject down = board.allDots[column , row -1];
            if (up.GetComponent<Dot>().value == this.value && down.GetComponent<Dot>().value == this.value)
            {
                up.GetComponent<Dot>().isMatch = true;
                down.GetComponent<Dot>().isMatch = true;
                isMatch = true;
            }
        }
    }




    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spAtlas = Resources.Load<SpriteAtlas>("Textures/dotAtlas");

        board = FindObjectOfType<Board>();
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        column = targetX;
        row = targetY;
    }

    void colorCheck()
    {
        switch (value)
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

    // Update is called once per frame
    void Update()
    {
        colorCheck();
        if (isMatch)
        {
            spriteRenderer.color = new Color(0f, 0f, 0f);
        }
        targetX = column;
        targetY = row;

        // Mathf.Abs 절대값 구하기에요
        float x = targetX - transform.position.x;
        float y = targetY - transform.position.y;
        if(x < 0)
        {
            x = x * -1;
        }
        if( y < 0)
        {
            y = y * -1;
        }

        //자꾸 체킹한다. 혹시나 MoveDots에서 row나 column이 바뀌면 해당 위치로 이동한다~~
        //어차피 x,y 좌표가 1,2,3 이런 식이니깐 바뀐 row나 column으로 좌표를 바꾸면 된다.
        if(x > 0.1f) //제자리만 아니면 이동한다고요~
        {
            tempPosition = new Vector2(targetX, transform.position.y); //바뀐 column위치로 저장~
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.4f); // 천천히 이동하도록 해요
        }
        else // 제자리면 암것도 하지 말어요
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.allDots[column, row] = this.gameObject;
        }

        if (y > 0.1f) //y도 같아요
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, 0.4f);
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.allDots[column, row] = this.gameObject;
        }

        CheckMath();
    }
}
