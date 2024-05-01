using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public float currHP;

    int remainingTime;
    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        currHP = 200;
    }

    public void ReduceHP(float amount)
    {
        currHP = Mathf.Clamp(currHP - amount, 0, 100);

        slider.value = currHP;

        if (currHP == 0)
        {
            UIManager.Instance.SetFailPopup();

            StartCoroutine(GameManager.Instance.ToGameOverScene());
        }
    }

    public void IncreaseHP(int amount)
    {
        currHP = Mathf.Clamp(currHP + amount, 0, 100);

        slider.value = currHP;
    }
}
