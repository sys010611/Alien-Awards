using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    public bool isAnswer = false;
    public bool isClickable = true;

    int _headIdx;
    int _eyeIdx;
    int _itemIdx;
    int _legIdx;
    int _hairIdx;
    int _colorIdx;

    public int eyeIdx => _eyeIdx;
    public int itemIdx => _itemIdx;
    public int legIdx => _legIdx;
    public int hairIdx => _hairIdx;
    public int colorIdx => _colorIdx;
    public int headIdx => _headIdx;

    SpriteRenderer bodyRenderer;
    SpriteRenderer hairRenderer;
    SpriteRenderer legRenderer;
    SpriteRenderer itemRenderer;
    SpriteRenderer eyeRenderer;
    SpriteRenderer armRenderer; //not random

    Vector3 defaultEyePos;
    Vector3 defaultHairPos;
    Vector3 defaultItemPos;

    Vector3 hair2Offset = new Vector3(1f, -1.01f);

    
    static Color[] colorList = new Color[3];

    private void Awake()
    {
        Color color1, color2, color3;
        ColorUtility.TryParseHtmlString("#a9a2ff", out color1);
        ColorUtility.TryParseHtmlString("#339d41", out color2);
        ColorUtility.TryParseHtmlString("#e3484f", out color3);
        colorList[0] = color1;
        colorList[1] = color2;
        colorList[2] = color3;


        bodyRenderer = this.GetComponent<SpriteRenderer>();

        hairRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        legRenderer = transform.GetChild(1).GetComponent<SpriteRenderer>();
        itemRenderer = transform.GetChild(2).GetComponent<SpriteRenderer>();
        eyeRenderer = transform.GetChild(3).GetComponent<SpriteRenderer>();
        armRenderer = transform.GetChild(4).GetComponent<SpriteRenderer>();

        defaultEyePos = eyeRenderer.transform.position;
        defaultHairPos = hairRenderer.transform.position;
        defaultItemPos = itemRenderer.transform.position;
    }

    public void Randomize()
    {
        _headIdx = UnityEngine.Random.Range(0, 3);
        _eyeIdx = UnityEngine.Random.Range(0, 3);
        _itemIdx = UnityEngine.Random.Range(0, 5);
        _legIdx = UnityEngine.Random.Range(0, 3);
        _hairIdx = UnityEngine.Random.Range(0, 3);

        _colorIdx = UnityEngine.Random.Range(0, colorList.Length);


        bodyRenderer.sprite = AlienSprites.Instance.bodySprites[_headIdx];

        eyeRenderer.sprite = AlienSprites.Instance.eyeSprites[eyeIdx];

        if (_headIdx == 1)
            eyeRenderer.transform.position = defaultEyePos + Vector3.down * 0.8f;
        else
            eyeRenderer.transform.position = defaultEyePos;

        if (_hairIdx == 2)
            hairRenderer.transform.position = defaultHairPos + hair2Offset;
        else
            hairRenderer.transform.position = defaultHairPos;

        legRenderer.sprite = AlienSprites.Instance.legSprites[legIdx];
        hairRenderer.sprite = AlienSprites.Instance.hairSprites[hairIdx];
        itemRenderer.sprite = AlienSprites.Instance.itemSprites[itemIdx];

        if(_itemIdx == 2 || _itemIdx == 4)
            itemRenderer.transform.position = defaultItemPos + Vector3.down * 0.8f;
        else
            itemRenderer.transform.position = defaultItemPos;

        bodyRenderer.color = colorList[colorIdx];
        hairRenderer.color = colorList[colorIdx];
        legRenderer.color = colorList[colorIdx];
        eyeRenderer.color = colorList[colorIdx];
        itemRenderer.color = colorList[colorIdx];
        armRenderer.color = colorList[colorIdx];
    }

    public void SetAsAnswer()
    {
        GameManager.Instance.SetAnswerAlienInfo(this);
        GameManager.Instance.SetUpAwardCard();
        

        this.isAnswer = true;
    }

    private void OnMouseDown()
    {
        CheckAnswer();
    }

    private void CheckAnswer()
    {
        if (isClickable)
        {
            //Debug.Log("CLICKED ALIEN");
            if (isAnswer) // 성공
            {
                GameManager.Instance.Success();
            }
            else // 실패
            {
                GameManager.Instance.Fail();
            }
        }
    }
}
