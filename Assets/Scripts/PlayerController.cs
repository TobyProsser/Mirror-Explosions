using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine;

public class PlayerController : MonoBehaviour, IPointerDownHandler
{
    public bool TopPlayer;
    private int Team;

    private Rigidbody2D ThisRB;
    private float PlayerSpeed = 600;
    public float JumpForce;
    private bool CanInteract = true;

    private float ItemInHand = 0;  //0 = nothing, 1 = mine, 2 = missile, 3 = rocket

    private float Dir = 1;

    public GameObject Mine;
    public GameObject Rocket;
    public GameObject Missile;

    public GameObject MineExplosion;
    public GameObject Explosion2;

    private float ScreenWidth;

    public CameraShake cameraShake;
    public GameController GameCon;

    float fJumpPressedRemember = 0;
    [SerializeField]
    float fJumpPressedRememberTime = 0.2f;

    float fGroundedRemember = 0;
    [SerializeField]
    float fGroundedRememberTime = 0.25f;

    [SerializeField]
    float fJumpVelocity = .005f;

    private bool bGrounded;
    private Transform GroundedObject;

    private float Dir1;

    private bool InElevator = false;
    private bool InDownvator = false;

    private float addForceUp = 2500;
    private float addForceDown = 1500;

    private void Awake()
    {
        ScreenWidth = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).x - .5f);
        ThisRB = this.GetComponent<Rigidbody2D>();

        GroundedObject = this.transform.GetChild(0);

        if (!TopPlayer)
        {
            ThisRB.gravityScale = -9.81f;
            Team = 1;                          //Bottom player is team 1
        }
        else
        {
            ThisRB.gravityScale = 9.81f;
            Team = 0;
        }

        if (Team == 1)
        {
            Dir1 = 1;                        //BottomPlayer
        }
        else
        {
            Dir1 = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.x >= ScreenWidth / 2)        //When Missile goes above screen spawn on other side
        {
            float PosX = ScreenWidth / 2;
            Vector3 NewPos = new Vector3(-PosX, this.transform.position.y , 0);
            this.transform.position = NewPos;
        }
        else if (this.transform.position.x <= -ScreenWidth / 2)
        {
            float PosX = ScreenWidth / 2;
            Vector3 NewPos = new Vector3(PosX, this.transform.position.y, 0);
            this.transform.position = NewPos;
        }

        // Jumping Mechanics
        
        RaycastHit2D hit = Physics2D.Raycast(GroundedObject.position, Vector2.up * Dir1, 0.1f);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Floor")
            {
                bGrounded = true;
            }
            else
            {
                bGrounded = false;
            }
        }
        else
        {
            bGrounded = false;
        }

        fGroundedRemember -= Time.deltaTime;
        if (bGrounded)
        {
            fGroundedRemember = fGroundedRememberTime;
        }

        fJumpPressedRemember -= Time.deltaTime;

        if ((fJumpPressedRemember > 0) && (fGroundedRemember > 0))
        {
            this.GetComponent<ParticleSystem>().Play();
            AudioManager.instance.Play("Jump");
            fJumpPressedRemember = 0;
            fGroundedRemember = 0;
            //ThisRB.velocity = new Vector2(ThisRB.velocity.x, fJumpVelocity);       //Try And make this work
            ThisRB.AddForce(transform.up * JumpForce, ForceMode2D.Force);
        }
    }

    private void LateUpdate()
    {
        ThisRB.velocity = new Vector3(3 * Dir, ThisRB.velocity.y, 0);

        if (InElevator)
        {
            ThisRB.AddForce(transform.up * addForceUp);    //Dir1 makes sure values are positive or negative depending on if player is on top or bottom
        }
        else if (InDownvator)
        {
            ThisRB.AddForce(transform.up * -addForceDown);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ItemTop" || collision.gameObject.tag == "ItemBottom")
        {
            if (ItemInHand == 0)
            {
                AudioManager.instance.Play("PickUp");
                ItemInHand = collision.GetComponent<ItemScript>().Item;
                Destroy(collision.gameObject);
            }
        }

        if (collision.gameObject.tag == "Missile")
        {
            int TeamCheck = collision.GetComponent<MissileScript>().Team;
            if (TeamCheck != Team)
            {
                AudioManager.instance.Play("Explosion1");
                StartCoroutine(cameraShake.Shake(.3f, .1f));
                CanInteract = false;

                GameObject CurExplosion = Instantiate(Explosion2, this.transform.position, collision.transform.rotation);
                CurExplosion.GetComponent<AstronautExplode>().Team = Team;
                CurExplosion.GetComponent<AstronautExplode>().ShowPieces = true;
                Destroy(collision.gameObject);
                Destroy(CurExplosion, .5f);

                Death();
            }
        }

        if (collision.gameObject.tag == "Mine")
        {
            int TeamCheck = collision.GetComponent<MineScript>().Team;
            if (TeamCheck != Team)
            {
                AudioManager.instance.Play("Explosion2");
                CanInteract = false;
                StartCoroutine(cameraShake.Shake(.3f, .1f));
                Vector3 SpawnPos;

                if (Team == 0)
                {
                    SpawnPos = new Vector3(collision.transform.position.x, collision.transform.position.y + .4f, 0);
                }
                else
                {
                    SpawnPos = new Vector3(collision.transform.position.x, collision.transform.position.y - .4f, 0);
                }
                
                GameObject CurExplosion = Instantiate(MineExplosion, SpawnPos, collision.transform.rotation);
                CurExplosion.GetComponent<AstronautExplode>().Team = Team;
                CurExplosion.GetComponent<AstronautExplode>().ShowPieces = true;
                Destroy(collision.gameObject);
                Destroy(CurExplosion, .69f);
                Death();
            }
        }

        if (collision.gameObject.tag == "Rocket")
        {
            int TeamCheck = collision.GetComponent<RocketScript>().Team;
            if (TeamCheck != Team)
            {
                AudioManager.instance.Play("Explosion3");
                CanInteract = false;
                StartCoroutine(cameraShake.Shake(.3f, .1f));

                GameObject CurExplosion = Instantiate(Explosion2, this.transform.position, collision.transform.rotation);
                CurExplosion.GetComponent<AstronautExplode>().Team = Team;
                CurExplosion.GetComponent<AstronautExplode>().ShowPieces = true;
                Destroy(collision.gameObject);
                Destroy(CurExplosion, .5f);
                Death();
            }
        }

        if (collision.gameObject.tag == "Elevator")
        {
            InElevator = true;
        }

        if (collision.gameObject.tag == "Downvator")
        {
            InDownvator = true;
        }

        if (collision.gameObject.tag == "Spike")
        {
            AudioManager.instance.Play("Explosion3");

            Destroy(collision.gameObject);

            GameObject CurExplosion = Instantiate(Explosion2, this.transform.position, collision.transform.rotation);
            CurExplosion.GetComponent<AstronautExplode>().Team = Team;
            CurExplosion.GetComponent<AstronautExplode>().ShowPieces = true;
            Destroy(CurExplosion, .5f);

            Death();
        }


    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Elevator")
        {
            InElevator = false;
        }

        if (collision.gameObject.tag == "Downvator")
        {
            InDownvator = false;
        }
    }

    public void Jump()
    {
        if (CanInteract)
        {
            fJumpPressedRemember = fJumpPressedRememberTime;
        }
    }

    public void Switch()
    {
        if (CanInteract)
        {
            AudioManager.instance.Play("Turn");
            if (Dir == 1)
            {
                Dir = -1;
                this.transform.Rotate(0, 180, 0);
            }
            else
            {
                Dir = 1;
                this.transform.Rotate(0, 180, 0);
            }
        }
    }

    public void UseItem()
    {
        if (CanInteract)
        {
            if (ItemInHand == 1)
            {
                AudioManager.instance.Play("MinePlace");
                ItemInHand = 0;
                GameObject CurMine = Instantiate(Mine, this.transform.position, Mine.transform.rotation);

                float newY = -this.transform.position.y;
                Vector3 newMinePos = new Vector3(this.transform.position.x, newY, this.transform.position.z);
                CurMine.transform.position = newMinePos;

                if (Team == 1)
                {
                    CurMine.GetComponent<MineScript>().Team = 1;
                }
                else
                {
                    CurMine.transform.Rotate(180, 0, 0);
                    CurMine.GetComponent<MineScript>().Team = 0;
                }
            }
            else if (ItemInHand == 2)
            {
                AudioManager.instance.Play("ShootRocket");
                ItemInHand = 0;

                Vector3 NewPos = Vector3.zero;
                if (Team == 1)
                {
                    NewPos = new Vector3(this.transform.position.x, this.transform.position.y + -1f, 0); //Spawns above player so it doesnt make contact with floor which would destroy it
                }
                else
                {
                    NewPos = new Vector3(this.transform.position.x, this.transform.position.y + 1f, 0);
                }

                GameObject CurRocket = Instantiate(Rocket, NewPos, Rocket.transform.rotation);

                if (Team == 1)
                {
                    CurRocket.GetComponent<RocketScript>().Team = 1;
                    CurRocket.transform.Rotate(180, 0, 0);
                }
                else
                {
                    CurRocket.GetComponent<RocketScript>().Team = 0;
                }

            }
            else if (ItemInHand == 3)
            {
                AudioManager.instance.Play("ShootMissile");
                ItemInHand = 0;
                GameObject CurMissile = Instantiate(Missile, this.transform.position, Missile.transform.rotation);

                if (Dir == 1)
                {
                    CurMissile.transform.Rotate(180, 0, 0);
                }

                if (Team == 1)
                {
                    CurMissile.GetComponent<MissileScript>().Team = 1;
                }
                else
                {
                    CurMissile.GetComponent<MissileScript>().Team = 0;
                }
            }
        }
    }

    public void Death()
    {
        Destroy(this.gameObject);
        if (Team == 0)
        {
            GameCon.TopPlayers -= 1;
        }
        else
        {
            GameCon.BottomPlayers -= 1;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        print("OnPointerDown");
    }
}

//COMPUTER CONTROLS
/*
if (Input.GetKeyDown("space"))      //Switch Direction
{
    if (Dir == 1)
    {
        Dir = -1;
    }
    else
    {
        Dir = 1;
    }
}

if (Input.GetMouseButtonDown(0))         //Jump
{
    ThisRB.AddForce(transform.up * JumpForce * GravityDir);
}

if (Input.GetMouseButtonDown(1))           //Use Item
{
    if (ItemInHand == 1)
    {
        ItemInHand = 0;
        GameObject CurMine = Instantiate(Mine, this.transform.position, Mine.transform.rotation);
        float newY = -this.transform.position.y;

        Vector3 newMinePos = new Vector3(this.transform.position.x, newY, this.transform.position.z);
        CurMine.transform.position = newMinePos;
    }
}
*/
