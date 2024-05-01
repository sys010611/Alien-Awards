using Unity.Burst.CompilerServices;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Word : MonoBehaviour
{
    private int _wordIdx;
    public int wordIdx => _wordIdx;

    Vector3 startPos;
    Vector3 endPos;
    LineRenderer lr;
    Camera cam;
    Vector3 camOffset = new Vector3(0, 0, 5);
    TextMesh textMesh;

    private void Awake()
    {
        lr = gameObject.GetComponent<LineRenderer>();
    }

    void Start()
    {
        cam = Camera.main;

        MeshRenderer mesh = GetComponent<MeshRenderer>();
        mesh.sortingLayerName = "CardContents";
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // 우클릭으로 선 지우기 가능
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.transform == this.transform)
            {
                lr.enabled = false;
            }
        }
    }

    private void OnMouseDown()
    {
        //Debug.Log("CLICKED WORD");

        lr.useWorldSpace = false;

        lr.enabled = true;
        lr.positionCount = 2;

        startPos = transform.InverseTransformPoint(transform.GetChild(0).position - camOffset);
        
        lr.SetPosition(0, startPos);

        //endPos = cam.ScreenToWorldPoint(Input.mousePosition) + camOffset;
        //lr.SetPosition(1, endPos);

    }

    private void OnMouseDrag()
    {
        //lr.useWorldSpace = true;
        endPos = transform.InverseTransformPoint(cam.ScreenToWorldPoint(Input.mousePosition) + camOffset);
        lr.SetPosition(1, endPos);
    }
    private void OnMouseUp()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit == true && hit.transform.CompareTag("Meaning"))
        {
            //Debug.Log("DRAW LINE");
            //lr.useWorldSpace = false;
            endPos = transform.InverseTransformPoint(hit.transform.GetChild(0).position - camOffset);
            lr.SetPosition(1, endPos);
        }
        else
        {
            Debug.Log("ERASE LINE");
            lr.enabled = false;
        }
    }

    public void SetLine(Mean targetMean)
    {
        lr.useWorldSpace = false;

        lr.enabled = true;

        startPos = transform.InverseTransformPoint(transform.GetChild(0).position - camOffset);

        lr.SetPosition(0, startPos);

        endPos = transform.InverseTransformPoint(targetMean.transform.GetChild(0).position - camOffset);
        lr.SetPosition(1, endPos);

        GetComponent<BoxCollider2D>().enabled = false;

        lr.startColor = Color.red;
        lr.endColor = Color.red;
        textMesh.color = Color.red;

        //Debug.Log("SET LINE");
    }

    public void SetWord(int idx)
    {
        _wordIdx = idx;

        textMesh = GetComponent<TextMesh>();
        Debug.Assert(textMesh != null);
        textMesh.text = GameManager.Instance.idxToWord[_wordIdx].ToString();
    }
}
