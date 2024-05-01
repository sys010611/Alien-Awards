using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ClueCard : MonoBehaviour
{
    float moveDist = 6;
    bool isMoving = false;
    bool isShown = false;
    Alien answerAlienInfo;

    [SerializeField] Transform wordList;
    [SerializeField] Transform meanList;

    [SerializeField] List<Word> words;
    List<Mean> means;
    List<int> answerInfoList;
    List<int> answerInCommonList;
    [SerializeField] GameObject meanPrefab;

    public bool IsShown => isShown;

    LineRenderer[] lines;

    private void Awake()
    {
        lines = GetComponentsInChildren<LineRenderer>();
    }

    public void SetUpCard()
    {
        GetAnswerInfo();

        List<int> hintList = AlienManager.Instance.hintList;

        answerInCommonList = new List<int>();

        for (int i = answerInfoList.Count - 1; i >= 0; i--)
        {
            int currAttrib = answerInfoList[i];
            if (hintList.Contains(currAttrib))
            {
                answerInCommonList.Add(currAttrib);
                answerInfoList.Remove(currAttrib);
            }
        }

        int currTurn = GameManager.Instance.CurrTurn;

        int wordCount = 6;
        if (currTurn >= 1 && currTurn <= 3) //1������
        {
            wordCount = 6;
        }
        else if (currTurn <= 9) // 2, 3������
        {
            wordCount = 5;
        }
        else // �� �� ������
        {
            wordCount = 4;
        }

        SetWords(currTurn, wordCount);

        int providingAnswerCount = 0;

        if (currTurn <= 3) // 1������
            providingAnswerCount = 5;
        else if (currTurn <= 9) // 2, 3������
            providingAnswerCount = 4;
        else if (currTurn <= 12) //4������
            providingAnswerCount = 2;
        else//5������, 6������
            providingAnswerCount = 0;

        SetMeans(hintList, providingAnswerCount);

        //List<Vector3> wordPositions = new List<Vector3>();

        //var activeWords = GetComponentsInChildren<Word>();

        //for (int i = 0; i < activeWords.Length; i++)
        //    wordPositions.Add(activeWords[i].transform.position);

        //wordPositions.Shuffle();

        //for (int i = 0; i < activeWords.Length; i++)
        //    activeWords[i].transform.position = wordPositions[i];

        int lineCount = 0;
        bool IsDone = false;

        foreach (var word in wordList.GetComponentsInChildren<Word>())
        {
            foreach (var mean in meanList.GetComponentsInChildren<Mean>())
            {
                if (word.wordIdx == mean.wordIdx)
                {
                    word.SetLine(mean);
                    lineCount++;

                    if (lineCount >= providingAnswerCount)
                    {
                        IsDone = true;
                        break;
                    }
                }
            }
            if (IsDone)
                break;
        }

        foreach (var mean in GetComponentsInChildren<Mean>())
        {
            Destroy(mean);
        }
    }

    private void GetAnswerInfo()
    {
        answerInfoList = new List<int>();

        answerAlienInfo = GameManager.Instance.GetAnswerAlienInfo();

        answerInfoList.Add(answerAlienInfo.eyeIdx + GameInfo.eyeIdxOffset);
        answerInfoList.Add(answerAlienInfo.headIdx + GameInfo.headIdxOffset);
        answerInfoList.Add(answerAlienInfo.hairIdx + GameInfo.hairIdxOffset);
        answerInfoList.Add(answerAlienInfo.legIdx + GameInfo.legIdxOffset);
        answerInfoList.Add(answerAlienInfo.colorIdx + GameInfo.colorIdxOffset);
        answerInfoList.Add(answerAlienInfo.itemIdx + GameInfo.itemIdxOffset);
    }

    private void SetMeans(List<int> hintList, int providingAnswerCount)
    {
        if (means != null)
        {
            for (int i = 0; i < meanList.childCount; i++)
                Destroy(meanList.GetChild(i).gameObject);
        }
        means = new List<Mean>();

        // �ܾ�鸶�� �´� �� ����
        List<Word> activeWords = new List<Word>();

        for (int i = 0; i < words.Count; i++)
        {
            if (words[i].gameObject.activeSelf)
                activeWords.Add(words[i]);
        }

        for(int i=0;i<activeWords.Count;i++)
        { 
            // �ٰ� ������ �н�
            if (answerInfoList.Count == 0 && hintList.Count == 0)
            {
                Debug.Log("�ܾ ���� ������ ����");
                activeWords[i].gameObject.SetActive(false);
                continue;
            }

            Word word = activeWords[i];
            if (i < providingAnswerCount) // ���� ���� �ֱ�
            {
                if(answerInfoList.Count>0)
                {
                    int currTurn = GameManager.Instance.CurrTurn;
                    if (currTurn >= 13 && currTurn <= 15) // 5������
                    {
                        Debug.Log("5������");
                        if (i <= 2) // ���� �ܰ��ΰ� ��ġ�� �ܾ� 3�� ����
                        {
                            int infoIdx = Random.Range(0, answerInCommonList.Count);
                            word.SetWord(answerInCommonList[infoIdx]);
                            hintList.Remove(answerInCommonList[infoIdx]);
                            answerInCommonList.RemoveAt(infoIdx);
                        }
                        else // ���� ���� �ܰ��� �ܾ� �� 1�� ����
                        {
                            int infoIdx = Random.Range(0, answerInfoList.Count);
                            word.SetWord(answerInfoList[infoIdx]);
                            answerInfoList.RemoveAt(infoIdx);
                        }
                    }
                    else if (currTurn >= 16) // 6������
                    {
                        Debug.Log("6������");
                        if (i <= 4) // ���� �ܰ����� Ư�������� �ٸ� �͵��ϰ� ��ġ�� �� 4�� ��������
                        {
                            int infoIdx = Random.Range(0, answerInCommonList.Count);
                            word.SetWord(answerInCommonList[infoIdx]);
                            hintList.Remove(answerInCommonList[infoIdx]);
                            answerInCommonList.RemoveAt(infoIdx);
                        }
                        else // ���� �ܰ��ΰ� ��ġ�� �ʴ� �ܾ�� ��������
                        {
                            int infoIdx = Random.Range(0, hintList.Count);
                            word.SetWord(hintList[infoIdx]);
                            hintList.RemoveAt(infoIdx);
                        }
                    }
                    else // �� �� �� �������
                    {
                        //Debug.Log("���ݺ� ������");
                        int infoIdx = Random.Range(0, answerInfoList.Count);
                        word.SetWord(answerInfoList[infoIdx]);
                        answerInfoList.RemoveAt(infoIdx);
                    }
                }
                else
                {
                    if(hintList.Count == 0)
                    {
                        word.gameObject.SetActive(false);
                        continue;
                    }
                    int infoIdx = Random.Range(0, hintList.Count);
                    word.SetWord(hintList[infoIdx]);
                    answerInfoList.Remove(hintList[infoIdx]);
                    hintList.RemoveAt(infoIdx);
                }
            }
            else
            {
                if(hintList.Count == 0)
                {
                    word.gameObject.SetActive(false);
                    continue;
                }
                int infoIdx = Random.Range(0, hintList.Count);
                word.SetWord(hintList[infoIdx]);
                answerInfoList.Remove(hintList[infoIdx]);
                hintList.RemoveAt(infoIdx);                
            }

            //Debug.Log(word.transform.position);

            GameObject mean =
                Instantiate(meanPrefab, word.transform.position + Vector3.down * 2f, Quaternion.identity);

            means.Add(mean.GetComponent<Mean>());

            mean.GetComponent<Mean>().SetMeaning(word.wordIdx);

            mean.transform.SetParent(meanList, true);
        }
    }

    private void SetWords(int currTurn, int wordCount)
    {
        for(int i=0;i<wordList.childCount;i++)
        {
            wordList.GetChild(i).gameObject.SetActive(false);
        }
        for (int i = 0; i < wordCount; i++)
        {
            words[i].gameObject.SetActive(true);
        }
    }

    private void OnMouseDown()
    {
        MoveCard();
    }

    public void MoveCard()
    {
        if (!isMoving)
        {
            if (!isShown)
            {
                float movePosY = transform.position.y + moveDist;
                transform.DOLocalMoveY(movePosY, 0.5f);

                isShown = true;
            }
            else
            {
                float movePosY = transform.position.y - moveDist;

                transform.DOLocalMoveY(movePosY, 0.5f);

                isShown = false;
            }
            isMoving = true;

            this.Invoke(() => isMoving = false, 0.5f);
        }
    }
}
