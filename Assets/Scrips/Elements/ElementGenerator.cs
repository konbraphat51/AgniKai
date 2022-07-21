using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementGeneratorSettings
{
    public GameObject elementPrefab;
    public GameObject parentObject;
    public bool hasLife = true;
    public int lifeLeftF = 100;
    public float generatePossibility = 1.0f;
    
    public ElementGenerator.Option option = ElementGenerator.Option.quiet;

    public ElementSettings elementSettings;
}

public class ElementGenerator : MonoBehaviour
{
    [SerializeField] private GameObject elementPrefab;

    public ElementGeneratorSettings settings;

    public enum Option
    {
        quiet,
        spreadRandom
    }

    private void Start()
    {
        //if no prefab
        if(settings.elementPrefab == null)
        {
            settings.elementPrefab = elementPrefab;
        }
    }

    void Update()
    {
        Generate();

        if (settings.hasLife)
        {
            settings.lifeLeftF--;
            if (settings.hasLife && (settings.lifeLeftF <= 0))
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void Generate()
    {
        //if possibility 4.7 => 4 must generated + one more by 0.7 chance
        int generate_n = (int)settings.generatePossibility;
        if(Random.Range(0,1) < settings.generatePossibility)
        {
            generate_n++;
        }

        for(int i = 0; i < generate_n; i++)
        {
            GameObject elementObject = Instantiate(settings.elementPrefab,
                new Vector3(0,0,0),
                Quaternion.identity,
                settings.parentObject.transform);
            elementObject.transform.position = this.transform.position;
            Element element = elementObject.GetComponent<Element>();
            element.settings = settings.elementSettings;

            switch (settings.option)
            {
                case (Option.spreadRandom):
                    Vector3 projectionSpeed = new Vector3(Random.RandomRange(0f, 1f),0,0);
                    Quaternion angle = Quaternion.EulerAngles(0,0, Random.RandomRange(0f, 360f));
                    elementObject.GetComponent<Rigidbody2D>().velocity = angle * projectionSpeed;
                    break;
            }
        }
    }
}

