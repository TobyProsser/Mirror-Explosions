using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineScript : MonoBehaviour
{
    public int Team;

    private void Awake()
    {
        if (this.transform.position.y < 0)
        {
            this.GetComponent<Rigidbody2D>().gravityScale = 9.81f;
        }
        else
        {
            this.GetComponent<Rigidbody2D>().gravityScale = -9.81f;
        }
    }
}
