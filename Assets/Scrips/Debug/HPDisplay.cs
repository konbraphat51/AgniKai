using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DebugD
{
    public class HPDisplay : MonoBehaviour
    {
        [SerializeField] private GameObject playerObj;
        private Player player;

        private Text text;

        private void Start()
        {
            text = GetComponent<Text>();
            player = playerObj.GetComponent<Player>();
        }

        private void Update()
        {
            text.text = "HP : " + player.hp.ToString();
        }
    }
}