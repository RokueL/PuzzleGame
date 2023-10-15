using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.U2D;
using Enum;

public class Dot : MonoBehaviour
{
    public Enum.Enum.Bomb bombType = Enum.Enum.Bomb.None;

    Vector2 firstTouchPosition;
    Vector2 finalTouchPosition;
    Vector2 tempPosition;

    float swipeAngle;
    float swipeSpeed = 0.2f;

    public int value;
    public int column;
    public int row;
    public int targetX, targetY;
    private int exrow, excol;

    public bool isBomb;
    public bool isMatch;

    Board board;
    FindMatch findMatch;

    GameObject otherDot;

    public IObjectPool<GameObject> dottPool { get; set; }
    public SpriteAtlas spAtlas;
    SpriteRenderer spriteRenderer;

    private void OnMouseDown()
    {
        if (board.state == Enum.Enum.State.Move)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (isBomb)
            {
                isMatch = true;
                board.DestroyCheck();
            }
        }
        //Debug.Log(firstTouchPosition);
    }
    private void OnMouseUp()
    {
        if (board.state == Enum.Enum.State.Move)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CalculateAngle();
        }
    }
    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * Mathf.Rad2Deg;
        Debug.Log(swipeAngle);
        board.currentDot = this;
        MoveDots();
        board.state = Enum.Enum.State.Wait;
    }
    void MoveDots()
    {
        if (swipeAngle > 45 && swipeAngle <= 135 && row < board.Height - 1) //�� ����
        {
            otherDot = board.allDots[column, row + 1]; //�� ���� ��Ʈ ��������
            exrow = row;
            excol = column;
            otherDot.GetComponent<Dot>().row -= 1; // ������ ��Ʈ�� ������ ��������
            row += 1;
        }
        else if (swipeAngle > -45 && swipeAngle <= 45 && column < board.Width - 1) // ������ ����
        {
            otherDot = board.allDots[column + 1, row]; // ����
            exrow = row;
            excol = column;
            otherDot.GetComponent<Dot>().column -= 1; //����
            column += 1;
        }
        else if (swipeAngle >= -135 && swipeAngle < -45 && row > 0) // �Ʒ� ����
        {
            otherDot = board.allDots[column, row - 1];
            exrow = row;
            excol = column;
            otherDot.GetComponent<Dot>().row += 1;
            row -= 1;
        }
        else if (swipeAngle > 135 || swipeAngle <= -135 && column > 0) // ���� ���� ����� ������ ���� ���̶� ||�� ó���Ѵ�.
        {
            otherDot = board.allDots[column - 1, row];
            exrow = row;
            excol = column;
            otherDot.GetComponent<Dot>().column += 1;
            column -= 1;
        }
        else
        {
            
        }
        StartCoroutine(CheckMoveGo());
    }


    public IEnumerator CheckMoveGo()
    {
        yield return new WaitForSeconds(0.5f);
        if(otherDot != null)
        {
            if(!isMatch && !otherDot.GetComponent<Dot>().isMatch)
            {
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().column = column;
                row = exrow;
                column = excol;

                yield return new WaitForSeconds(0.5f);
                board.currentDot = null;

                findMatch.match.Clear();
                findMatch.bombMatch.Clear();

                board.state = Enum.Enum.State.Move;
            }
            else {
                if(isBomb)
                {
                    isMatch = true;
                }
                board.DestroyCheck();
            }

            otherDot = null;
        }
    }

    public void changeAreabomb()
    {
        Debug.Log("������ź�����Ϸ�");
        spriteRenderer.sprite = spAtlas.GetSprite("AreaBombDot");
        isMatch = false;
        isBomb = true;
    }
    public void changeRowbomb()
    {
        Debug.Log("������ź�����Ϸ�");
        spriteRenderer.sprite = spAtlas.GetSprite("RowBombDot");
        isMatch = false;
        isBomb = true;
    }
    public void changeColumnbomb()
    {
        Debug.Log("������ź�����Ϸ�");
        spriteRenderer.sprite = spAtlas.GetSprite("ColBombDot");
        isMatch = false;
        isBomb = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spAtlas = Resources.Load<SpriteAtlas>("Textures/dotAtlas");

        board = FindObjectOfType<Board>();
        findMatch = FindObjectOfType<FindMatch>();

        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        //column = targetX;
        //row = targetY;
       // exrow = row;
        //excol = column;
    }

    public void colorCheck()
    {
        if (!isBomb)
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
    }



    // Update is called once per frame
    void Update()
    {
        colorCheck();

        targetX = column;
        targetY = row;

        // Mathf.Abs ���밪 ���ϱ⿡��
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

        //�ڲ� üŷ�Ѵ�. Ȥ�ó� MoveDots���� row�� column�� �ٲ�� �ش� ��ġ�� �̵��Ѵ�~~
        //������ x,y ��ǥ�� 1,2,3 �̷� ���̴ϱ� �ٲ� row�� column���� ��ǥ�� �ٲٸ� �ȴ�.
        if(x > 0.1f) //���ڸ��� �ƴϸ� �̵��Ѵٰ��~
        {
            tempPosition = new Vector2(targetX, transform.position.y); //�ٲ� column��ġ�� ����~
            transform.position = Vector2.Lerp(transform.position, tempPosition, swipeSpeed); // õõ�� �̵��ϵ��� �ؿ�
            if (board.allDots[column,row] != this.gameObject) // ���� �Ű����� ������Ʈ�� ��Ī�� �ȵǸ� ���� ��Ͻ�Ų��.
            {
                board.allDots[column, row] = this.gameObject;
            }
            findMatch.MatchFinder();
        }
        else // ���ڸ��� �ϰ͵� ���� �����
        {
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            // ���� �ִ� �༮�� ���� �ǹ̰� �������� ���� ġ����
        }

        if (y > 0.1f) //y�� ���ƿ�
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = Vector2.Lerp(transform.position, tempPosition, swipeSpeed);
            if (board.allDots[column, row] != this.gameObject)
            {
                board.allDots[column, row] = this.gameObject;
            }
            findMatch.MatchFinder();
        }
        else
        {
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
        }

    }
}
