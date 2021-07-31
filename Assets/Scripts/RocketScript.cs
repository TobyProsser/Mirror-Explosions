using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    private Rigidbody2D RocketRB;
    private float RocketSpeed = 200;
    private float ScreenHeight;

    public GameObject Explosion2;

    public int Team;

    private void Awake()
    {
        ScreenHeight = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).y - .5f);
    }

    void Start()
    {
        RocketRB = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Team == 0)
        {
            if (this.transform.position.y >= ScreenHeight / 2)        //When Missile goes above screen spawn on other side
            {
                float PosY = ScreenHeight / 2;
                Vector3 NewPos = new Vector3(this.transform.position.x, -PosY, 0);
                this.transform.position = NewPos;
            }
        }
        else
        {
            if (this.transform.position.y <= -ScreenHeight / 2)        //When Missile goes below screen spawn on other side
            {
                float PosY = ScreenHeight / 2;
                Vector3 NewPos = new Vector3(this.transform.position.x, PosY, 0);
                this.transform.position = NewPos;
            }
        }
    }

    private void LateUpdate()
    {
        RocketRB.AddForce(transform.up * RocketSpeed, ForceMode2D.Force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            AudioManager.instance.Play("Explosion1");
            GameObject CurExplosion = Instantiate(Explosion2, this.transform.position, collision.transform.rotation);
            Destroy(CurExplosion, .4f);
            Destroy(this.gameObject);
        }
    }
}
