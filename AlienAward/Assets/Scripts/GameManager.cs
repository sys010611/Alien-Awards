using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    Dictionary<char, int> _wordToIdx;
    public Dictionary<char, int> wordToIdx => _wordToIdx;

    Dictionary<int, char> _idxToWord;
    public Dictionary<int, char> idxToWord => _idxToWord;


    [SerializeField] private Timer timer;
    [SerializeField] private HP hp;
    private int score = 0;

    [HideInInspector] private Alien answerAlienInfo;
    [SerializeField] AwardCard awardCard;
    [SerializeField] ClueCard clueCard;

    int currTurn = 1;
    public int CurrTurn => currTurn;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        SetUpDic();
    }


    void SetUpDic() // A ~ Z 까지의 문자를 숫자 0~26에 랜덤으로 매핑
    {
        _wordToIdx = new Dictionary<char, int>();
        _idxToWord = new Dictionary<int, char>();

        char word = 'A';

        List<int> meanList = new List<int>();
        for (int i = 0; i < 20; i++)
            meanList.Add(i);

        //meanList.Shuffle();

        for (int i = 0; i < 20; i++)
        {
            _wordToIdx[word] = meanList[i];
            _idxToWord[i] = word;

            word++;
        }

        Debug.Log(wordToIdx.Count);
        Debug.Log(idxToWord.Count);
    }

    public void TimeOut()
    {
        Debug.Log("Time Out!!");
        hp.ReduceHP(100);

        Fail();
    }

    public void Success()
    {
        score += 100;
        score += (int)hp.currHP;
        hp.IncreaseHP(25);

        if (currTurn == 20) // 게임 끝
        {
            AlienManager.Instance.MoveAliens(false);

            UIManager.Instance.SetSuccessPopup();

            if (score > PlayerPrefs.GetInt("HighScore"))
            {
                PlayerPrefs.SetInt("HighScore", score);
                Debug.Log("HighScore : " + PlayerPrefs.GetInt("HighScore"));
            }

            PlayerPrefs.SetInt("LastScore", score);

            StartCoroutine(ToGameClearScene());
        }
        else
        {
            currTurn++;

            Debug.Log("이번 턴 : " + currTurn);

            // 매핑 정보 초기화
            //SetUpDic();

            // 외계인 초기화, 상장 카드 초기화
            AlienManager.Instance.MoveAliens(false);

            this.Invoke(() => AlienManager.Instance.SetUpAliens(), 0.5f);
            this.Invoke(()=>AlienManager.Instance.MoveAliens(true), 1f);

            if (clueCard.IsShown)
                clueCard.MoveCard();

            if (awardCard.IsShown)
                awardCard.MoveCard();
        }
    }
    IEnumerator ToGameClearScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameClear");
    }

    public void Fail()
    {
        score -= 50;

        hp.ReduceHP(20);

        if (clueCard.IsShown)
            clueCard.MoveCard();

        if (awardCard.IsShown)
            awardCard.MoveCard();

        // 매핑 정보 초기화
        //SetUpDic();

        // 외계인 초기화, 상장 카드 초기화
        AlienManager.Instance.MoveAliens(false);

        this.Invoke(() => AlienManager.Instance.SetUpAliens(), 0.5f);
        this.Invoke(() => AlienManager.Instance.MoveAliens(true), 1f);
    }

    public IEnumerator ToGameOverScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("GameOver");
    }

    public void SetUpAwardCard()
    {
        awardCard.SetContent(answerAlienInfo);
    }

    public void SetUpClueCard()
    {
        clueCard.SetUpCard();
    }

    public bool IsAllCardOff()
    {
        return !awardCard.IsShown && !clueCard.IsShown;
    }

    public int GetScore()
    {
        return this.score;
    }

    public Alien GetAnswerAlienInfo()
    {
        return answerAlienInfo;
    }
    public void SetAnswerAlienInfo(Alien info)
    {
        answerAlienInfo = info;
    }
}
