using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class Element : MonoBehaviour
{
    [SerializeField] protected int lifeLeft = 50;
    
    //which side this is.
    public int playerN = 1;

    public bool destroyHittingWall = true;

    public virtual void Damage(int damage)
    {

    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject colliderObject = collision.gameObject;
        string colliderTag = collision.gameObject.tag;

        Debug.Log(colliderTag);

        //Bullet vs Bullet
        if (TagCommon.Contains(this.gameObject.tag, "Bullet")
            && TagCommon.Contains(colliderTag, "Bullet")
            && (this.playerN != colliderObject.GetComponent<Element>().playerN))
        {
            
            Destroy(colliderObject);
            Destroy(this.gameObject);
        }

        // vs wall
        if (TagCommon.Contains(colliderTag, "Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}
