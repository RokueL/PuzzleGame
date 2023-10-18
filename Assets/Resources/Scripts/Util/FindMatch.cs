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
        //StartCoroutine(FindMatches());
    }

    public void MatchFinder()
    {
        if (board.state == Enum.Enum.State.Check)
        {
            StartCoroutine(FindMatches());
        }
    }

    IEnumerator FindMatches() //state == check
    {
        yield return new WaitForSeconds(.1f);
        isLeftRight = false; isUpdown = false;
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
                                AddListCheck(up, down, dots.gameObject);
                                isUpdown = true;
                                isLeftRight = false;
                            }
                        }
                    }
                }
            }
        }

        yield return new WaitForSeconds(.4f);
        board.CheckdestroyDelay();
    }

    public void MatchDown()
    {
        if (board.state == Enum.Enum.State.Down || board.state == Enum.Enum.State.Spawn)
        {
            StartCoroutine(downCheck());
        }
    }

    IEnumerator downCheck()
    {
        yield return new WaitForSeconds(.6f); // 내려온 후 매칭 딜레이
        isLeftRight = false; isUpdown = false;
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
                                board.currentDot = dots;
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
                                board.currentDot = dots;
                                AddListCheck(up, down, dots.gameObject);
                                isUpdown = true;
                                isLeftRight = false;
                            }
                        }
                    }
                }
            }
        }

        yield return new WaitForSeconds(.4f);
        board.CheckdestroyDelay();
    }


    public void bombAddListColumn(int col, int row)
    {
        var dots = board.allDots[col, row];

        var obj = ObjectPoolManager.Instance.E_ColBombPool.Get();
        obj.transform.position = dots.gameObject.transform.position;
        obj.GetComponent<EffectsColBomb>().DestroySelf();

        dots.GetComponent<Dot>().isMatch = true;
        if (dots != null)
        {
            for (int i = 0; i < board.Width; i++)
            {

                if (board.allDots[i, row] != null)
                {
                    if (!bombMatch.Contains(board.allDots[i, row]))
                    {
                        bombMatch.Add(board.allDots[i, row]);
                    }
                    board.allDots[i, row].GetComponent<Dot>().isMatch = true;
                    if (board.allDots[i, row].GetComponent<Dot>().isBomb == false)
                    {
                        board.makeEffect(board.allDots[i, row].GetComponent<Dot>().value, i, row);
                        board.allDots[i, row].GetComponent<Dot>().dottPool.Release(board.allDots[i, row]);
                        board.allDots[i, row] = null;
                    }
                }
            }
        }
    }

    public void bombAddListRow(int col, int row)
    {
        var dots = board.allDots[col, row];

        var obj = ObjectPoolManager.Instance.E_RowBombPool.Get();
        obj.transform.position = dots.gameObject.transform.position;
        obj.GetComponent<EffectsRowBomb>().DestroySelf();

        dots.GetComponent<Dot>().isMatch = true;
        if (dots != null)
        {
            for (int i = 0; i < board.Height; i++)
            {
                if (board.allDots[col, i] != null)
                {
                    if (!bombMatch.Contains(board.allDots[col, i]))
                    {
                        bombMatch.Add(board.allDots[col, i]);
                    }
                    board.allDots[col, i].GetComponent<Dot>().isMatch = true;
                    if (board.allDots[col, i].GetComponent<Dot>().isBomb == false)
                    {
                        board.makeEffect(board.allDots[col, i].GetComponent<Dot>().value, col, i);
                        board.allDots[col, i].GetComponent<Dot>().dottPool.Release(board.allDots[col, i]);
                        board.allDots[col, i] = null;
                    }
                }
            }
        }
    }

    public void bombAddListArea(int col, int row)
    {
        var dots = board.allDots[col, row];

        var obj = ObjectPoolManager.Instance.E_AreaBombPool.Get();
        obj.transform.position = dots.gameObject.transform.position;
        obj.GetComponent<EffectsAreaBomb>().DestroySelf();

        dots.GetComponent<Dot>().isMatch = true;
        int c = 0;
        int r = 0;
        if (dots != null)
        {
            for (int i = -2; i <= 2; i++)
            {
                for (int j = -2; j <= 2; j++)
                {
                    if(col + i >= board.Width)
                    {
                        c = board.Width - 1;
                    }
                    else if(row + j >= board.Height)
                    {
                        r = board.Height - 1;
                    }
                    else if(col + i < 0)
                    {
                        c = 0;
                    }
                    else if(row + j < 0)
                    {
                        r = 0;
                    }
                    else
                    {
                        c = col + i;
                        r = row + j;
                    }
                    if (board.allDots[c, r] != null)
                    {
                        if (!bombMatch.Contains(board.allDots[c, r]))
                        {
                            bombMatch.Add(board.allDots[c, r]);
                        }
                        board.allDots[c, r].GetComponent<Dot>().isMatch = true;
                        if (board.allDots[c, r].GetComponent<Dot>().isBomb == false)
                        {
                            board.makeEffect(board.allDots[c, r].GetComponent<Dot>().value, c, r);
                            board.allDots[c, r].GetComponent<Dot>().dottPool.Release(board.allDots[c, r]);
                            board.allDots[c, r] = null;
                        }
                    }
                }
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

    public void checkColRowBomb()
    {

        if (board.currentDot != null)
        {
            if (board.isMakeBomb)
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
    public void checkAreaBomb()
    {

        if (board.currentDot != null)
        {
            if (board.isMakeBomb)
            {
                board.currentDot.changeAreabomb();
                board.currentDot.bombType = Enum.Enum.Bomb.areaBomb;
                isLeftRight = false;
                isUpdown = false;
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
