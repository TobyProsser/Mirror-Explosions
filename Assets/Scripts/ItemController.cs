using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private float ScreenWidth;
    private float ScreenHeight;
    public GameObject Item;

    private GameObject[] ItemsInSceneTop;
    private bool CanSpawnTop = true;

    private GameObject[] ItemsInSceneBottom;
    private bool CanSpawnBottom = true;

    private int SpawnableItemsNo = 3;
    // Start is called before the first frame update
    void Start()
    {
        ScreenWidth = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).x - .5f);
        ScreenHeight = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).y - .5f);

        SpawnableItemsNo = BeforePlayScript.Items;

        StartCoroutine(SpawnTop(5));
        StartCoroutine(SpawnBottom(5));
    }

    private void LateUpdate()
    {
        ItemsInSceneTop = GameObject.FindGameObjectsWithTag("ItemTop");
        if (ItemsInSceneTop.Length < SpawnableItemsNo - 1)
        {
            CanSpawnTop = true;
        }
        else
        {
            CanSpawnTop = false;
        }

        ItemsInSceneBottom = GameObject.FindGameObjectsWithTag("ItemBottom");
        if (ItemsInSceneBottom.Length < SpawnableItemsNo - 1)
        {
            CanSpawnBottom = true;
        }
        else
        {
            CanSpawnBottom = false;
        }
    }

    private IEnumerator SpawnTop(float waitTime)
    {
        while (true)
        {
            if (CanSpawnTop)
            {
                float randomItem = Random.Range(1, 4);
                yield return new WaitForSeconds(waitTime);

                float RandX = Random.Range(-6, 6);
                float RandY = Random.Range(.5f, 4.5f);
                Vector3 SpawnLoc = new Vector3(RandX, RandY, 0);
                GameObject CurItem = Instantiate(Item, SpawnLoc, Item.transform.rotation);
                CurItem.transform.tag = "ItemTop";
                CurItem.GetComponent<ItemScript>().Item = randomItem;
                CurItem.GetComponent<ItemScript>().Team = 0;
            }
            else
            {
                yield return null;
            }
        }
    }

    private IEnumerator SpawnBottom(float waitTime)
    {
        while (true)
        {
            if (CanSpawnBottom)
            {
                float randomItem1 = Random.Range(1, 4);
                yield return new WaitForSeconds(waitTime);

                float RandX = Random.Range(-6, 6);
                float RandY = Random.Range(-.5f, -4.5f);

                //Vector3 SpawnLoc1 = new Vector3(Random.Range(-ScreenWidth / 2, ScreenWidth / 2), Random.Range(0, (ScreenHeight / 2) * -1), 0); //Spawn Object within bottom half of screen.
                Vector3 SpawnLoc1 = new Vector3(RandX, RandY, 0);
                GameObject CurItem1 = Instantiate(Item, SpawnLoc1, Item.transform.rotation);
                CurItem1.transform.tag = "ItemBottom";
                CurItem1.GetComponent<ItemScript>().Item = randomItem1;
                CurItem1.GetComponent<ItemScript>().Team = 1;
            }
            else
            {
                yield return null;
            }
        }
    }
}
