using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour
{
    private Rigidbody2D MissileRB;
    private float MissileSpeed = 560;
    private float ScreenWidth;
    private float ParcialScreenWidth;
    public int Team;

    private int PassTimes = 0;

    public GameObject Explosion2;

    private void Awake()
    {
        ScreenWidth = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).x - .5f);
        ParcialScreenWidth = ScreenWidth / 60;
    }

    void Start()
    {
        MissileRB = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (this.transform.position.x >= ScreenWidth / 2 + ParcialScreenWidth)        //When Missile goes above screen spawn on other side
        {
            float PosX = ScreenWidth / 2;
            Vector3 NewPos = new Vector3(-PosX, this.transform.position.y * -1, 0);
            this.transform.position = NewPos;
            PassTimes += 1;
        }

        if (this.transform.position.x <= -ScreenWidth / 2 - ParcialScreenWidth)
        {
            float PosX = ScreenWidth / 2;
            Vector3 NewPos = new Vector3(PosX, this.transform.position.y * -1, 0);
            this.transform.position = NewPos;

            PassTimes += 1;
        }

        if (PassTimes >= 2)                   //After it goes across the other side, delete object
        {
            Destroy(this.gameObject);
        }
    }

    private void LateUpdate()
    {
        MissileRB.AddForce(transform.up * MissileSpeed, ForceMode2D.Force);
    }

    private void OnTriggerEnter2D(Collider2D collision)
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
