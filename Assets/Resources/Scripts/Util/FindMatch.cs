using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class FindMatch : MonoBehaviour
{
    private Board board;
    
    public List<GameObject> match = new List<GameObject>();
    public List<GameObject> bombMatch = new List<GameObject>();
    
    public bool isUpdown;
    public bool isLeftRight;


    // Start is called before the first frame update
    void Start()
    {
        board = FindObjectOfType<Board>();
        StartCoroutine(FindMatches());
        
    }

    public void MatchFinder()
    {
        StartCoroutine(FindMatches());
    }

    IEnumerator FindMatches()
    {
        yield return new WaitForSeconds(.1f);
        for (int i = 0; i < board.Width; i++)
        {
            for (int j = 0; j < board.Height; j++)
            {
                GameObject dot = board.allDots[i, j]; // 원래 각 도트가 주인공이여서 상관 없었는데 이제 전체 체킹을 돌아욤 비교적 버그가 덜 하네요
                if (dot != null)
                {
                    var dots = dot.GetComponent<Dot>();

                    if (i > 0 && i < board.Width - 1) // column 가져온 거에용 Side 체크 그거에용
                    {
                        GameObject left = board.allDots[i - 1, j];
                        GameObject right = board.allDots[i + 1, j];
                        if (left != null && right != null)
                        {
                            if (left.GetComponent<Dot>().value == dots.value && right.GetComponent<Dot>().value == dots.value)
                            {
                                //left.GetComponent<Dot>().isMatch = true;
                                //right.GetComponent<Dot>().isMatch = true;
                                //dots.isMatch = true;
                                //Debug.Log(left.name + ", " + right.name + ", " + dots.gameObject.name);
                                AddListCheck(left, right, dots.gameObject);
                                isLeftRight = true;
                                isUpdown = false;
                            }
                        }
                    }
                    if (j > 0 && j < board.Height - 1) // row 그거에용 Up Down 체크 그거에용 그대로 가져왔어용
                    {
                        GameObject up = board.allDots[i, j + 1];
                        GameObject down = board.allDots[i, j - 1];
                        if (up != null && down != null)
                        {
                            if (up.GetComponent<Dot>().value == dots.value && down.GetComponent<Dot>().value == dots.value)
                            {
                                //up.GetComponent<Dot>().isMatch = true;
                                //down.GetComponent<Dot>().isMatch = true;
                                //dots.isMatch = true;
                                //Debug.Log(up.name + ", " + down.name + ", " + dots.gameObject.name);
                                AddListCheck(up, down, dots.gameObject);
                                isUpdown = true;
                                isLeftRight = false;
                            }
                        }
                    }
                }
            }
        }
        //Debug.Log("match 최대 수 : " + match.Count);
        for (int i = 0; i < match.Count; i++)
        {
            //Debug.Log($"match{i}의 이름 : " + match[i].name);
        }
        yield return new WaitForSeconds(.4f);
        board.DestroyCheck();
    }

    public void bombAddListColumn(int col, int row)
    {
        var dots = board.allDots[col, row];
        if (dots != null)
        {
            for (int i = 0; i < board.Width; i++)
            {
                if (!bombMatch.Contains(board.allDots[i, row]))
                {
                    bombMatch.Add(board.allDots[i, row]);
                }
                board.allDots[i, row].GetComponent<Dot>().isMatch = true;
            }
        }
    }

    public void bombAddListRow(int col, int row)
    {
        var dots = board.allDots[col, row];
        if (dots != null)
        {
            for (int i = 0; i < board.Height; i++)
            {
                if (!bombMatch.Contains(board.allDots[col, i]))
                {
                    bombMatch.Add(board.allDots[col, i]);
                }
                board.allDots[col, i].GetComponent<Dot>().isMatch = true;
            }
        }
    }

    void AddListCheck(GameObject leftup, GameObject rightdown, GameObject dots)
    {

        if(!match.Contains(leftup))
        {
            match.Add(leftup);
        }
        leftup.GetComponent<Dot>().isMatch = true;

        if (!match.Contains(rightdown))
        {
            match.Add(rightdown);
        }
        rightdown.GetComponent<Dot>().isMatch = true;

        if (!match.Contains(dots))
        {
            match.Add(dots);
        }
        dots.GetComponent<Dot>().isMatch = true;
    }

    public void checkBomb()
    {

        if (board.currentDot != null)
        {
            if (board.currentDot.isMatch)
            {
                if (isLeftRight)
                {
                    board.currentDot.changeColumnbomb();
                    board.currentDot.bombType = Enum.Enum.Bomb.columnBomb;
                    isLeftRight = false;
                }
                else if (isUpdown)
                {
                    board.currentDot.changeRowbomb();
                    board.currentDot.bombType = Enum.Enum.Bomb.rowBomb;
                    isUpdown = false;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
