using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementGenerator : MonoBehaviour
{
    [SerializeField] public GameObject elementPrefab;
    [SerializeField] public GameObject parentObject;
    [SerializeField] public bool hasLife = true;
    [SerializeField] public int lifeLeftF = 100;
    [Tooltip(">1.0 is able; 5.0 means 5 per flame")]
    [SerializeField] public float generatePossibility = 1.0f;

    void Update()
    {
        Generate();

        lifeLeftF--;
        if(hasLife && (lifeLeftF <= 0))
        {
            Destroy(this.gameObject);
        }
    }

    private void Generate()
    {
        //if possibility 4.7 => 4 must generated + one more by 0.7 chance
        int generate_n = (int)generatePossibility;
        if(Random.Range(0,1) < generatePossibility)
        {
            generate_n++;
        }

        //without direction
        for(int i = 0; i < generate_n; i++)
        {
            GameObject elementObject = Instantiate(elementPrefab,
                new Vector3(0,0,0),
                Quaternion.identity,
                parentObject.transform);
            elementObject.transform.position = this.transform.position;
        }
    }
}
