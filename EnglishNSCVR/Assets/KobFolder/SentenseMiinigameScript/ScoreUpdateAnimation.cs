using UnityEngine;
using TMPro;
using System.Collections;

public class ScoreUpdateAnimation : MonoBehaviour
{
    public Animator anim;
    public TMP_Text scoreChange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void PlayScoreUpdate(int scoreUpdate)
    {
        if (scoreUpdate > 0)
        {
            scoreChange.text = "+" + scoreUpdate;
            scoreChange.color = Color.green;
        }
        else
        {
            scoreChange.text = scoreUpdate.ToString();
            scoreChange.color = Color.red;
        }

        anim.ResetTrigger("ScoreUpdate"); // ✅ รีเซ็ตก่อน
        anim.SetTrigger("ScoreUpdate");
    }

    IEnumerator PunchScale()
    {
        Vector3 original = transform.localScale;

        transform.localScale = original * 1.5f;
        yield return new WaitForSeconds(0.1f);

        transform.localScale = original;
    }
}
