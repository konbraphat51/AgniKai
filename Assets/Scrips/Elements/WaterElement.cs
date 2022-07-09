using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterElement : Element
{
    void Update()
    {
        lifeLeft--;
        if(lifeLeft <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
