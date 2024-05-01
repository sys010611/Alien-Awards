using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class AlienManager : MonoBehaviour
{
    public static AlienManager Instance;

    public List<int> hintList;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        //this.Invoke(() => transform.DOLocalMoveY(transform.position.y + 10f, 0.5f)
        //    .OnComplete(()=>SetClickable(true)), 0.2f);

        transform.DOLocalMoveY(transform.position.y + 10f, 0.5f)
            .OnComplete(() => SetClickable(true));

        SetUpAliens();

    }

    public void SetUpAliens()
    {
        Alien[] aliens = GetComponentsInChildren<Alien>();

        foreach (Alien alien in aliens)
        {
            alien.Randomize();
            alien.isAnswer = false;
        }

        int answerIdx = UnityEngine.Random.Range(0, aliens.Length);
        aliens[answerIdx].SetAsAnswer();
        Debug.Log(answerIdx);

        int currTurn = GameManager.Instance.CurrTurn;
        int minSimilarity = 0;
        int maxSimilarity = 0;

        if (currTurn <= 6)
        {
            minSimilarity = 0;
            maxSimilarity = 1;
        }
        else if (currTurn <= 9)
        {
            minSimilarity = 3;
            maxSimilarity = 3;
        }
        else if (currTurn <= 12)
        {
            minSimilarity = 2;
            maxSimilarity = 2;
        }
        else
        {
            minSimilarity = 3;
            maxSimilarity = 3;
        }

        // 정답과의 유사도 검사, 낮을 시 랜덤 다시 돌리기
        for (int i = 0; i < aliens.Length; i++)
        {
            if (i == answerIdx)
                continue;

            int similarity = CheckSimilarity(aliens[i], aliens[answerIdx]);
            while (!( minSimilarity <= similarity  && similarity <= maxSimilarity))
            {
                aliens[i].Randomize();
                similarity = CheckSimilarity(aliens[i], aliens[answerIdx]);
            }
        }

        // 유사 특성들을 뽑아서 힌트 리스트에 저장
        hintList = new List<int>();

        for (int i = 0; i < aliens.Length; i++)
        {
            for (int j = i + 1; j < aliens.Length; j++)
            {
                if (aliens[i].eyeIdx == aliens[j].eyeIdx)
                    hintList.Add(aliens[i].eyeIdx + GameInfo.eyeIdxOffset);

                if (aliens[i].hairIdx == aliens[j].hairIdx)
                    hintList.Add(aliens[i].hairIdx + GameInfo.hairIdxOffset);

                if (aliens[i].headIdx == aliens[j].headIdx)
                    hintList.Add(aliens[i].headIdx + GameInfo.headIdxOffset);

                if (aliens[i].colorIdx == aliens[j].colorIdx)
                    hintList.Add(aliens[i].colorIdx + GameInfo.colorIdxOffset);

                if (aliens[i].legIdx == aliens[j].legIdx)
                    hintList.Add(aliens[i].legIdx + GameInfo.legIdxOffset);

                if (aliens[i].itemIdx == aliens[j].itemIdx)
                    hintList.Add(aliens[i].itemIdx + GameInfo.itemIdxOffset);
            }
        }

        // 중복 제거
        hintList = hintList.Distinct().ToList();

        //Debug.Log("HINT LIST SIZE : " + hintList.Count);

        GameManager.Instance.SetUpClueCard();

    }

    int CheckSimilarity(Alien a, Alien b)
    {
        int count = 0;
        if (a.eyeIdx == b.eyeIdx)
            count++;
        if (a.itemIdx == b.itemIdx)
            count++;
        if (a.legIdx == b.legIdx)
            count++;
        if (a.hairIdx == b.hairIdx)
            count++;
        if (a.colorIdx == b.colorIdx)
            count++;
        if (a.headIdx == b.headIdx)
            count++;

        return count;
    }

    public void SetClickable(bool flag)
    {
        foreach(var alien in GetComponentsInChildren<Alien>())
        {
            alien.isClickable = flag;
        }
    }

    public void MoveAliens(bool isToShow)
    {
        if(isToShow)
        {
            transform.DOLocalMoveY(transform.position.y + 10f, 0.5f)
                .OnComplete(()=>SetClickable(true));
        }
        else
        {
            SetClickable(false);
            transform.DOLocalMoveY(transform.position.y - 10f, 0.5f);
        }
    }
}
