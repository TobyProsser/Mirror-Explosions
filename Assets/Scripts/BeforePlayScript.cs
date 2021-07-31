using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BeforePlayScript : MonoBehaviour
{
    public static int Players;
    private int Rounds;
    public static int Items;

    public TextMeshProUGUI PlayersText;
    public TextMeshProUGUI RoundsText;
    public TextMeshProUGUI ItemsText;

    private void Awake()
    {
        Players = 2;
        Rounds = 3;
        Items = 3;

        UpdateText();

        GameController.TopWins = 0;
        GameController.BottomWins = 0;
    }

    private void UpdateText()
    {
        PlayersText.text = Players.ToString();
        RoundsText.text = Rounds.ToString();
        ItemsText.text = Items.ToString();
    }

    public void PlayersUp()
    {
        if (Players < 4)
        {
            AudioManager.instance.Play("Click");
            Players += 1;
        }
        UpdateText();
    }

    public void PlayersDown()
    {
        if (Players > 2)
        {
            AudioManager.instance.Play("Click");
            Players -= 1;
        }
        UpdateText();
    }

    public void RoundsUp()
    {
        if (Rounds < 7)
        {
            AudioManager.instance.Play("Click");
            Rounds++;
        }
        UpdateText();
    }

    public void RoundsDown()
    {
        if (Rounds > 2)
        {
            AudioManager.instance.Play("Click");
            Rounds--;
        }
        UpdateText();
    }

    public void ItemsUp()
    {
        if (Items < 5)
        {
            AudioManager.instance.Play("Click");
            Items++;
        }
        UpdateText();
    }

    public void ItemsDown()
    {
        if (Items > 2)
        {
            AudioManager.instance.Play("Click");
            Items--;
        }
        UpdateText();
    }

    public void Play()
    {
        AudioManager.instance.Play("Click");
        GameController.Rounds = Rounds;
        SceneManager.LoadScene("GameScene");
    }

}
