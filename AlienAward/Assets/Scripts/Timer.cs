using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    int remainingTime;
    Image fillImage;
    Coroutine timerCoroutine;
    [SerializeField] HP hp;

    private void Awake()
    {
        fillImage = transform.GetChild(0).GetComponent<Image>();
        remainingTime = 300;
    }

    // Start is called before the first frame update
    void Start()
    {
        timerCoroutine = StartCoroutine(RunTimer());
    }

    IEnumerator RunTimer()
    { 
        while(remainingTime > 0)
        {
            yield return new WaitForSeconds(1f);
            remainingTime--;

            if(GameManager.Instance.CurrTurn <= 3)
                hp.ReduceHP(1f);
            else if (GameManager.Instance.CurrTurn <= 10)
                hp.ReduceHP(1.2f);
            else
                hp.ReduceHP(1.5f);

            fillImage.fillAmount = (float)remainingTime / 100f;
        }

        GameManager.Instance.TimeOut();
        StopCoroutine(timerCoroutine);
    }

    public int GetRemainingTime()
    {
        return remainingTime;
    }

}
