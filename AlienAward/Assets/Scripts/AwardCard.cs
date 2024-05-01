using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwardCard : MonoBehaviour
{
    TextMesh content;

    float moveDist = 6;
    bool isMoving = false;
    bool isShown = false;

    public bool IsShown => isShown;

    private void Awake()
    {
        content = transform.GetChild(0).GetComponent<TextMesh>();
        content.GetComponent<MeshRenderer>().sortingLayerName = "CardContents";
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

    public void SetContent(Alien alienInfo)
    {
        char[] contentArr = new char[6];

        Debug.Assert(GameManager.Instance.idxToWord != null);

        contentArr[0] = GameManager.Instance.idxToWord[alienInfo.colorIdx + GameInfo.colorIdxOffset];
        contentArr[1] = GameManager.Instance.idxToWord[alienInfo.headIdx + GameInfo.headIdxOffset];
        contentArr[2] = GameManager.Instance.idxToWord[alienInfo.hairIdx + GameInfo.hairIdxOffset];
        contentArr[3] = GameManager.Instance.idxToWord[alienInfo.eyeIdx + GameInfo.eyeIdxOffset];
        contentArr[4] = GameManager.Instance.idxToWord[alienInfo.itemIdx + GameInfo.itemIdxOffset];
        contentArr[5] = GameManager.Instance.idxToWord[alienInfo.legIdx + GameInfo.legIdxOffset];

        content.text = "";
        for (int i=0;i<6;i+=3)
        {
            content.text += contentArr[i] + "   " + contentArr[i+1] + "   " + contentArr[i+2] + "\n";
        }
    }
}
