using TMPro;
using UnityEngine;

public class HowToController : MonoBehaviour
{
    private int pageNum;
    private int curPage;
    [SerializeField]
    private TMP_Text numDisplay;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        pageNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        numDisplay.text = (pageNum + 1).ToString();
    }

    public void nextPage()
    {
        curPage = pageNum;
        if (pageNum == 7)
        {
            pageNum = 0;
        }
        else
        { 
            pageNum++;
        }
        transform.GetChild(curPage).gameObject.SetActive(false);
        transform.GetChild(pageNum).gameObject.SetActive(true);
    }

    public void previousPage()
    {
        curPage = pageNum;
        if (pageNum == 0)
        {
            pageNum = 7;
        }
        else
        {
            pageNum--;
        }
        transform.GetChild(curPage).gameObject.SetActive(false);
        transform.GetChild(pageNum).gameObject.SetActive(true);
    }
}
