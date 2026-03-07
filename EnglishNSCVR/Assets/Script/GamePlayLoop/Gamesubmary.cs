using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Gamesubmary : MonoBehaviour
{
    public int currentScore;
    public List<EvaluationData> statusSumary = new List<EvaluationData>();
    public int converCount;

    public int SuitableWordScore;
    public int maxServiceScore;
    public int currentServiceScore; // if serve the wrong menu this score will be decrese

    [Header("UI")]
    public TMP_Text rank; // rank all conver
    public TMP_Text rankText;
    public GameObject gameSubmaryUI; // all submary UI -> close || open
    [Header("mini ui")]
    public GameObject recabTabBtnParent;
    public GameObject recabBtnPrefab;
    public GameObject gameRecabPanel; // submary all game score

    public List<GameObject> recab_BTNs = new List<GameObject>();
    [Header("one recab tab")]
    public GameObject recabTab; // submary each conversation score
    public TMP_Text title;
    public TMP_Text conver_rank; // rank each conver
    public TMP_Text strengths;
    public TMP_Text improvements;
    public TMP_Text next_rank_tip;
    public TMP_Text serviceScoreUI;
    public Button backToOverview;


    

    void OnEnable()
    {

        GameEvent.OnGameEnding += UISubmaryAll;
        gameSubmaryUI.SetActive(false);
    }
    void OnDisable()
    {
        GameEvent.OnGameEnding -= UISubmaryAll;
    }
    void Start()
    {
        currentServiceScore = maxServiceScore;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void addItem(string rank, string strengths, string improvements, string next_rank_tip)
    {
        
        EvaluationData newEvaluation = new EvaluationData(rank, strengths, improvements, next_rank_tip);
        if(newEvaluation.rank == null) return;
        statusSumary.Add(newEvaluation);
        ScoreCalculate(newEvaluation.rank);
        createSubmaryUI();
        converCount += 1;
        foreach (EvaluationData evaluation in statusSumary)
        {
            Debug.Log(evaluation.rank + " " + evaluation.strengths + " " + evaluation.improvements + " " + evaluation.next_rank_tip);
        }
    }

    public void ScoreCalculate(string rank)
    {
        switch (rank)
        {
            case "S":
                currentScore += 100;
                break;
            case "A":
                currentScore += 90;
                break;
            case "B":
                currentScore += 80;
                break;
            case "C":
                currentScore += 70;
                break;
            case "D":
                currentScore += 60;
                break;
            case "E":
                currentScore += 50;
                break;
            case "F":
                currentScore += 40;
                break;
        }
    }
    public string RankCalculate(int score)
    {
        int avgScore = (score / converCount);
        Debug.Log(avgScore);
        if (avgScore == 100)
        {
            return "S";
        }
        else if (avgScore >= 90)
        {
            return "A";
        }

        else if (avgScore >= 80)
        {
            return "B";
        }
        else if (avgScore >= 70)
        {
            return "C";
        }
        else if (avgScore >= 60)
        {
            return "D";
        }
        else if (avgScore >= 50)
        {
            return "E";
        }
        else
        {
            return "F";
        }
    }

    

    public void createSubmaryUI()
    {
        GameObject newRecab_btn = Instantiate(recabBtnPrefab);
        newRecab_btn.transform.parent = recabTabBtnParent.transform;
        recab_BTNs.Add(newRecab_btn);
        int thisConver = recab_BTNs.Count;
        Button btn = newRecab_btn.GetComponent<Button>();
        TMP_Text btnText = newRecab_btn.GetComponentInChildren<TMP_Text>();
        if(btn != null)
        {
            btn.onClick.AddListener(() => OpenThisPanel(thisConver));
            btnText.text = thisConver.ToString();
        }
    }

    public void OpenThisPanel(int page)
    {
        gameRecabPanel.SetActive(false);
        recabTab.SetActive(true);
        EvaluationData thisPageData = statusSumary[page - 1];
        title.text = "Coversation : " + page;
        conver_rank.text = thisPageData.rank;
        strengths.text = "strengths : " + thisPageData.strengths;
        improvements.text = "improvements : " + thisPageData.improvements;
        next_rank_tip.text = "next_rank_tip : " + thisPageData.next_rank_tip;
    }

    public void UISubmaryAll()
    {
        gameSubmaryUI.SetActive(true);
        recabTab.SetActive(false);
        string endRank = RankCalculate(currentScore);
        Debug.Log("rank : " + endRank + " score : " + currentScore);
        rank.text = endRank;
        rankText.text = "Your english  skill is rank : " + endRank;
        serviceScoreUI.text = "Service Score : " + currentServiceScore;
    }
    public void BackToOverview()
    {
        gameRecabPanel.SetActive(true);
        recabTab.SetActive(false);
    }
}
