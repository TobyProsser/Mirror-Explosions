using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BetweenRoundsScript : MonoBehaviour
{
    private int TopWins;
    private int BottomWins;
    private int RoundsAmount;

    private bool CanContinue = false;

    public TextMeshProUGUI RoundsText;
    public TextMeshProUGUI TopWinText;
    public TextMeshProUGUI BottomWinText;

    private void Awake()
    {
        TopWins = GameController.TopWins;
        BottomWins = GameController.BottomWins;
        RoundsAmount = GameController.Rounds;
        RoundsText.text = RoundsAmount.ToString();
        TopWinText.text = TopWins.ToString();
        BottomWinText.text = BottomWins.ToString();
    }

    private void Start()
    {
        StartCoroutine(TillContinue(1));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && CanContinue)
        {
            AudioManager.instance.Play("Click");
            SceneManager.LoadScene("GameScene");
        }
    }

    private IEnumerator TillContinue(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        CanContinue = true;
    }
}
