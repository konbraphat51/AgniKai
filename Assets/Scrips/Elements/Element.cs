using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using ElementFunc;

namespace Game
{
    public class ElementSettings
    {
        //which side this is.
        public int playerN = 1;

        ////settings
        public bool bullet = false;     //has damage
        public bool rigid = false;      //has rigidbody2D
        public bool collidable = true; //destroy at colliding with another collidable
        public bool hasLife = true;
        public bool isTrigger = false;
        public bool destroyHittingWall = true;
        public Element.Option option = Element.Option.quiet;
        public Element.Orbit orbit = Element.Orbit.free;
        public bool toRight = true;     //for orbit
        public float startTimeF = 0f;   //time when born (frame)
        public GameObject[] collisionIgnored;

        ////parameters
        //element life
        public int lifeLeft = 50;
        //bullet
        public float damageToPlayer = 1.0f;
        //option
        public float optionA = 1f;
    }

    public class Element : MonoBehaviour
    {
        public enum Option
        {
            quiet,
            spreadRandom,    //rigidbody needed
            waterCshoot
        }

        public enum Orbit
        {
            free,
            waterCshoot
        }

        public ElementSettings settings = new ElementSettings();

        int hp;

        private void Start()
        {
            //init life
            hp = settings.lifeLeft;

            //init rigid
            if (settings.rigid)
            {
                gameObject.AddComponent<Rigidbody2D>();
            }

            //init time
            settings.startTimeF = Time.frameCount;

            //init trigger
            gameObject.GetComponent<Collider2D>().isTrigger = settings.isTrigger;

            //ignored collision
            if(settings.collisionIgnored != null)
            {
                foreach (GameObject obj in settings.collisionIgnored)
                {
                    Physics2D.IgnoreCollision(this.gameObject.GetComponent<Collider2D>(),
                                                obj.GetComponent<Collider2D>());
                }
            }
            //option
            switch (settings.option)
            {
                case Option.spreadRandom:
                    Debug.Log(settings.optionA);
                    //REQUIRE: rigidbody
                    Vector3 speed = new Vector3(Random.Range(0f, settings.optionA), 0f, 0f);
                    Quaternion angle = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
                    gameObject.GetComponent<Rigidbody2D>().velocity = angle * speed;
                    break;
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

            //orbit
            MoveOnOrbit();
        }

        private void MoveOnOrbit()
        {
            switch (settings.orbit)
            {
                case Orbit.waterCshoot:
                    WaterElement.Orbit.CShoot(this);
                    break;
            }
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            GameObject colliderObject = collision.gameObject;
            string colliderTag = collision.gameObject.tag;

            // vs wall
            if (settings.destroyHittingWall
                && TagCommon.Contains(colliderTag, "Wall"))
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
}