using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobBehaviour : MonoBehaviour
{
    public float timeAlive, duration = 0.75f;
    private bool spawnedChild = false;
    private int childNo = 30;
    public GameObject lavaPrefab;
    public bool fading = false;

    public void SetChildNo(int childNo)
    {
        this.childNo = childNo;
    }

    void OnTriggerStay2D(Collider2D coll)
    {
        GameObject obj = coll.gameObject;
        if (obj.tag == "Bear")
        {
            BearMovement bm = obj.GetComponent<BearMovement>();
            bm.BearHasBeenHit();
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeAlive += Time.deltaTime;
        if (timeAlive > 0.2 && spawnedChild == false && childNo > 0)
        {
            Vector3 offset = new Vector3(transform.position.x + 1f, transform.position.y, 0);
            GameObject child = GameObject.Instantiate(lavaPrefab, offset, transform.rotation);
            spawnedChild = true;
            child.GetComponent<BlobBehaviour>().SetChildNo(childNo - 1);
        }
        if (timeAlive > duration)
        {
            fading = true;
        }
        if (fading)
        {
            Color currentCol = gameObject.GetComponent<SpriteRenderer>().color;
            float newAlpha = currentCol.a - 0.02f;
            if (newAlpha < 0)
            {
                GameObject.Destroy(this);
            }
            else
            {
                currentCol.a = newAlpha;
                gameObject.GetComponent<SpriteRenderer>().color = currentCol;
            }
        }
    }
}
