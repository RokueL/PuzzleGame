using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class FindMatch : MonoBehaviour
{
    private Board board;
    public List<GameObject> match = new List<GameObject>();


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
                GameObject dot = board.allDots[i, j]; // ���� �� ��Ʈ�� ���ΰ��̿��� ��� �����µ� ���� ��ü üŷ�� ���ƿ� ���� ���װ� �� �ϳ׿�
                if (dot != null)
                {
                    var dots = dot.GetComponent<Dot>();

                    if (i > 0 && i < board.Width - 1) // column ������ �ſ��� Side üũ �װſ���
                    {
                        GameObject left = board.allDots[i - 1, j];
                        GameObject right = board.allDots[i + 1, j];
                        if (left != null && right != null)
                        {
                            if (left.GetComponent<Dot>().value == dots.value && right.GetComponent<Dot>().value == dots.value)
                            {
                                left.GetComponent<Dot>().isMatch = true;
                                right.GetComponent<Dot>().isMatch = true;
                                dots.isMatch = true;
                            }
                        }
                    }
                    if (j > 0 && j < board.Height - 1) // row �װſ��� Up Down üũ �װſ��� �״�� �����Ծ��
                    {
                        GameObject up = board.allDots[i, j + 1];
                        GameObject down = board.allDots[i, j - 1];
                        if (up != null && down != null)
                        {
                            if (up.GetComponent<Dot>().value == dots.value && down.GetComponent<Dot>().value == dots.value)
                            {
                                up.GetComponent<Dot>().isMatch = true;
                                down.GetComponent<Dot>().isMatch = true;
                                dots.isMatch = true;
                            }
                        }
                    }
                }
            }
        }
        yield return new WaitForSeconds(.4f);
        board.DestroyCheck();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
