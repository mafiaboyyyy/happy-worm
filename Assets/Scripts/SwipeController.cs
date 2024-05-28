using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SwipeController : MonoBehaviour
{
    public Transform btnPrev;
    public Transform btnNext;
    [SerializeField] int maxPage;
    int currentPage;
    Vector3 targetPos;
    [SerializeField] Vector3 pageStep;
    [SerializeField] RectTransform levelPagesRect;
    [SerializeField] float tweenTime;
    [SerializeField] LeanTweenType tweenType;
    public AudioClip click, changePage;

    public void Awake()
    {
        currentPage = 1;
        targetPos = levelPagesRect.localPosition;
        btnPrev.transform.GetComponent<Button>().interactable = false;
    }
    public void Next()
    {
        transform.GetComponentInParent<LevelManager>().playSound(click);
        if (currentPage < maxPage)
        {
            currentPage++;
            if (currentPage == maxPage)
            {
                btnNext.transform.GetComponent<Button>().interactable = false;
            } else
            {
                btnPrev.transform.GetComponent<Button>().interactable = true;
            }
            targetPos += pageStep;
            MovePage();
        } 
    }

    public void Prev()
    {
        transform.GetComponentInParent<LevelManager>().playSound(click);
        if (currentPage > 1)
        {
            currentPage--;
            if (currentPage == 1)
            {
                btnPrev.transform.GetComponent<Button>().interactable = false;
            } else
            {
                btnNext.transform.GetComponent<Button>().interactable = true;
            }
            targetPos -= pageStep;
            MovePage();
        } 
    }

    void MovePage()
    {
        transform.GetComponentInParent<LevelManager>().playSound(changePage);
        levelPagesRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
    }
}
