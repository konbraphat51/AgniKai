using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class ElementSettings
{
    //which side this is.
    public int playerN = 1;

    ////settings
    public bool bullet = false;     //has damage
    public bool rigid = false;      //has rigidbody2D
    public bool collidable = true; //destroy at colliding with another collidable
    public bool hasLife = true;

    ////parameters
    //element life
    public int lifeLeft = 50;
    //bullet
    public float damageToPlayer = 1.0f;

    public bool destroyHittingWall = true;
}

public class Element : MonoBehaviour
{
    public ElementSettings settings = new ElementSettings();

    int hp;

    private void Start()
    {
        hp = settings.lifeLeft;

        if (settings.rigid)
        {
            gameObject.AddComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        //when has life
        if (settings.hasLife)
        {
            hp--;
            if (hp <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    public virtual void Damage(int damage)
    {

    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject colliderObject = collision.gameObject;
        string colliderTag = collision.gameObject.tag;

        // vs wall
        if (settings.destroyHittingWall
            &&TagCommon.Contains(colliderTag, "Wall"))
        {
            Destroy(this.gameObject);
        }

        //when bullet
        if (settings.bullet)
        {
            //vs Bullet
            if (TagCommon.Contains(colliderTag, "Bullet")
                && (settings.playerN != colliderObject.GetComponent<Element>().settings.playerN))
            {
                Destroy(colliderObject);
                Destroy(this.gameObject);
            }

            //vs Player
            if (TagCommon.Contains(colliderTag, "Player")
                && (settings.playerN != colliderObject.GetComponent<Player>().number))
            {
                Player player = colliderObject.GetComponent<Player>();
                player.Damage(settings.damageToPlayer);
                Destroy(this.gameObject);
            }
        }
    }
}
