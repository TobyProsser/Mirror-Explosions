using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public int TopPlayers;
    public int BottomPlayers;

    private int GamePlayers;

    public static int TopWins = 0;
    public static int BottomWins = 0;
    public static int Rounds;

    private bool GameRunning;

    private bool UnlockedMaps = true;
    public List<GameObject> Maps = new List<GameObject>();

    public GameObject[] ControllerCanvases = new GameObject[3];
    public GameObject[] Players = new GameObject[4];

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        GameRunning = true;

        for (int i = 0; i < ControllerCanvases.Length; i++)
        {
            ControllerCanvases[i].SetActive(false);
        }

        for (int i = 0; i < Players.Length; i++)
        {
            Players[i].SetActive(false);
        }

        if (BeforePlayScript.Players == 2)
        {
            ControllerCanvases[2].SetActive(true);
            Players[0].SetActive(true);
            Players[1].SetActive(true);
            TopPlayers = 1;
            BottomPlayers = 1;
            GamePlayers = 2;
        }
        else if (BeforePlayScript.Players == 3)
        {
            ControllerCanvases[1].SetActive(true);
            Players[0].SetActive(true);
            Players[1].SetActive(true);
            Players[3].SetActive(true);
            TopPlayers = 2;
            BottomPlayers = 1;
            GamePlayers = 3;
        }
        else if (BeforePlayScript.Players == 4)
        {
            Players[0].SetActive(true);
            Players[1].SetActive(true);
            Players[2].SetActive(true);
            Players[3].SetActive(true);
            ControllerCanvases[0].SetActive(true);
            TopPlayers = 2;
            BottomPlayers = 2;
            GamePlayers = 4;
        }
    }

    private void Start()
    {
        if (UnlockedMaps)
        {
            int Map = Random.Range(0, Maps.Count -1);

            for (int i = 0; i < Maps.Count; i++)
            {
                Maps[i].SetActive(false);
            }

            Maps[1].SetActive(true);
        }
    }
    private void Update()
    {
        if (GameRunning)
        {
            if (TopPlayers == 0)
            {
                BottomWin();
            }
            else if (BottomPlayers == 0)
            {
                TopWin();
            }
        }
    }

    private void TopWin()
    {
        GameRunning = false;
        TopWins += 1;
        Rounds -= 1;
        if (Rounds == 0)
        {
            GameEnd();
        }
        StartCoroutine(OpenRoundsScene(2));
    }

    private void BottomWin()
    {
        GameRunning = false;
        BottomWins += 1;
        Rounds -= 1;
        if (Rounds == 0)
        {
            GameEnd();
        }
        StartCoroutine(OpenRoundsScene(2));
    }

    IEnumerator OpenRoundsScene(float WaitTime)
    {
        yield return new WaitForSeconds(WaitTime);

        SceneManager.LoadScene("InBetweenRounds");
    }

    private void GameEnd()
    {
        if (BottomWins > TopWins)
        {
            //Bottomwins
        }
        else
        {
            //TopWins
        }
    }
}
