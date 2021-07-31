using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlOpacity : MonoBehaviour
{
    public GameObject[] Buttons = new GameObject[3];

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "TopPlayer" || collision.tag == "Item" || collision.tag == "BottomPlayer")
        {
            for (int i = 0; i < Buttons.Length; i++)
            {
                Buttons[i].GetComponent<Image>().color = new Color(1, 1, 1, .1f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "TopPlayer" || collision.tag == "Item" || collision.tag == "BottomPlayer")
        {
            for (int i = 0; i < Buttons.Length; i++)
            {
                Buttons[i].GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
        }
    }
}
