using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mean : MonoBehaviour
{
    //TextMesh textMesh;

    int _wordIdx;
    public int wordIdx => _wordIdx;

    private void Awake()
    {
        //textMesh = GetComponentInChildren<TextMesh>();
    }

    public void SetMeaning(int idx)
    {
        _wordIdx = idx;
        //textMesh.text = GameManager.Instance.idxToWord[idx].ToString();
        //textMesh.GetComponent<MeshRenderer>().sortingLayerName = "CardContents";

        GetComponent<SpriteRenderer>().sprite = SymbolSprites.Instance.sprites[_wordIdx];            
    }
}
