using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolSprites : MonoBehaviour
{
    public static SymbolSprites Instance;

    [SerializeField] public List<Sprite> sprites;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        else
            Destroy(this);
    }
}
