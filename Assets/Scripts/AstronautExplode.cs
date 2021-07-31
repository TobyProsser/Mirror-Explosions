using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstronautExplode : MonoBehaviour
{
    public GameObject[] Pieces;
    public int Team = 1;

    public bool ShowPieces = false;

    void Start()
    {
        if (ShowPieces)
        {
            for (int i = 0; i < Pieces.Length; i++)
            {
                Vector3 RandomDir = Vector3.zero;
                GameObject CurPiece = Instantiate(Pieces[i], this.transform.position, this.transform.rotation);
                if (Team == 0)
                {
                    RandomDir = new Vector3(Random.Range(-100, 100), Random.Range(4, 100), 0);
                    CurPiece.GetComponent<Rigidbody2D>().gravityScale = 1f;
                }
                else
                {
                    RandomDir = new Vector3(Random.Range(-100, 100), Random.Range(-100, -4), 0);
                    CurPiece.GetComponent<Rigidbody2D>().gravityScale = -1f;
                }

                CurPiece.GetComponent<Rigidbody2D>().AddForce(RandomDir * Random.Range(3, 7));
                Destroy(CurPiece, i * .6f);
            }
        }
    }
}
