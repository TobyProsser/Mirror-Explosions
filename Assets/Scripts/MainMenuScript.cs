using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject PlayButtonObject;
    public GameObject StoreButtonObject;
    public GameObject ExitButtonObject;

    private void Start()
    {
        StartCoroutine(ButtonAnimLoop(1f));
    }

    public void PlayButton()
    {
        SceneManager.LoadScene("BeforePlayScene");
        PlayButtonObject.GetComponent<Animator>().SetTrigger("ButtonClick");
        AudioManager.instance.Play("Click");
    }

    public void StoreButton()
    {
        StoreButtonObject.GetComponent<Animator>().SetTrigger("ButtonClick");
        AudioManager.instance.Play("Click");
    }

    public void ExitButton()
    {
        AudioManager.instance.Play("Click");
        Application.Quit();
        ExitButtonObject.GetComponent<Animator>().SetTrigger("ButtonClick");
    }

    private IEnumerator ButtonAnimLoop(float waitTime)
    {
        while (true)
        {
            ExitButtonObject.GetComponent<Animator>().ResetTrigger("ButtonClick");
            PlayButtonObject.GetComponent<Animator>().SetTrigger("ButtonClick");
            yield return new WaitForSeconds(waitTime);
            PlayButtonObject.GetComponent<Animator>().ResetTrigger("ButtonClick");
            StoreButtonObject.GetComponent<Animator>().SetTrigger("ButtonClick");
            yield return new WaitForSeconds(waitTime);
            StoreButtonObject.GetComponent<Animator>().ResetTrigger("ButtonClick");
            ExitButtonObject.GetComponent<Animator>().SetTrigger("ButtonClick");
            yield return new WaitForSeconds(waitTime);

        }
    }
}
