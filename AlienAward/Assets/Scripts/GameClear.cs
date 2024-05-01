using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameClear : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txt_score;
    

    private void Start()
    {
        SetScore();
    }

    public void ToTitle()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void SetScore()
    {
        int currScore = GameManager.Instance.GetScore();

        txt_score.SetText("Score : " + currScore);
        
    }
}
