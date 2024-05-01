using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] GameObject successPopup;
    [SerializeField] GameObject failPopup;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void SetSuccessPopup()
    {
        successPopup.SetActive(true);
    }

    public void SetFailPopup()
    {
        failPopup.SetActive(true);
    }
}
