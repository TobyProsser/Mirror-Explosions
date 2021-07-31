using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    public Sprite[] ItemSprites = new Sprite[3];
    public float Item = 0;  //0 = nothing, 1 = mine, 2 = Rocket, 3 = Missile
    public int Team;

    private void Start()
    {
        if (Item == 1)
        {
            this.GetComponent<SpriteRenderer>().sprite = ItemSprites[0];
        }
        else if (Item == 2)
        {
            this.GetComponent<SpriteRenderer>().sprite = ItemSprites[1];
        }
        else if (Item == 3)
        {
            this.GetComponent<SpriteRenderer>().sprite = ItemSprites[2];
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Floor")
        {
            if (Team == 0)
            {
                this.transform.position += new Vector3(0, .175f, 0);         //Makes sure Item doesnt Spawn in anything.
            }
            else
            {
                this.transform.position += new Vector3(0, -.175f, 0);         //Makes sure Item doesnt Spawn in anything.
            }
            
        }
    }
}
