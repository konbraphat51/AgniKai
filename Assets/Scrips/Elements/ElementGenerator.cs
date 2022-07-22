using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ElementGeneratorSettings
    {
        public GameObject elementPrefab;
        public GameObject parentObject;
        public bool hasLife = true;
        public int lifeLeftF = 100;
        public float generatePossibility = 1.0f;
        public float generateAreaR = 0.2f;

        public ElementSettings elementSettings;
    }

    public class ElementGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject elementPrefab;

        public ElementGeneratorSettings settings;

        private void Start()
        {
            //if no prefab
            if (settings.elementPrefab == null)
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
            if (Random.Range(0, 1) < settings.generatePossibility)
            {
                generate_n++;
            }

            for (int i = 0; i < generate_n; i++)
            {
                Quaternion generateAngle = Quaternion.Euler(0, 0, Random.Range(0, 360));
                Vector3 generateR = new Vector3(settings.generateAreaR, 0, 0);
                GameObject elementObject = Instantiate(settings.elementPrefab,
                    generateAngle * generateR,
                    Quaternion.identity,
                    settings.parentObject.transform);
                elementObject.transform.position = this.transform.position;
                Element element = elementObject.GetComponent<Element>();
                element.settings = settings.elementSettings;
            }
        }
    }


}
