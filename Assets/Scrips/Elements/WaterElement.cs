using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;

namespace ElementFunc
{
    public static class WaterElement
    {
        public static class Orbit
        {
            public static void CShoot(Game.Element element)
            {
                float power = 2f;

                Rigidbody2D rigid = element.gameObject.GetComponent<Rigidbody2D>();
                if (element.settings.toRight)
                {
                    rigid.AddForce(new Vector2(power, 0));
                }
                else
                {
                    rigid.AddForce(new Vector2(-power, 0));
                }
            }
        }
    }
}