using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class ElementBullet : Element
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        GameObject colliderObject = collision.gameObject;
        string colliderTag = collision.gameObject.tag;

        //Bullet vs Bullet
        if (TagCommon.Contains(this.gameObject.tag, "Bullet")
            && TagCommon.Contains(colliderTag, "Bullet")
            && (this.playerN != colliderObject.GetComponent<Element>().playerN))
        {

            Destroy(colliderObject);
            Destroy(this.gameObject);
        }
    }
}
