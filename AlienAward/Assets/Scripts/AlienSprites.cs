using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienSprites : MonoBehaviour
{
    public static AlienSprites Instance;

    public Sprite[] bodySprites;
    public Sprite[] eyeSprites;
    public Sprite[] hairSprites;
    public Sprite[] itemSprites;
    public Sprite[] legSprites;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
