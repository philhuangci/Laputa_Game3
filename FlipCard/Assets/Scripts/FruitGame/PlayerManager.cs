using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FruitGame
{
    public class PlayerManager : PlayerController
    {
        public List<GameObject> Fruits;

        [HideInInspector]
        public int index = -1;

        private Text Score;

        void Start()
        {
            
            GameObject Text3D = Instantiate(Resources.Load("FruitGame/Prefabs/Text3D")) as GameObject;
            Score = Text3D.GetComponent<Text>();
            Score.text = "Score: 0 ";
            Score.transform.SetParent(GameObject.Find("Text3DNode").transform, false);
            Text3DScript text3DScript = gameObject.AddComponent<Text3DScript>();
            text3DScript.UI3D = Text3D;
            text3DScript.Object3D = gameObject;
            text3DScript.Offset = new Vector3(-1f, 1.5f,0);
        }

        // Update is called once per frame
        void Update()
        {
            Score.text = "Score: " + (index+1).ToString();

            UpdateInput();
        }

        public int GetScore()
        {
            return index+1;
        }

        public bool IsWin()
        {
            return index == 16;
        }

        public void AddFruit()
        {
            if(index < 16)
            {
                index++;
                Fruits[index].SetActive(true);
            }
        }

        public void AddBomb()
        {
            int count = 3;
            while(count > 0 && index >=0)
            {
                Fruits[index--].SetActive(false);
                count--;
            }
            
        }
    }
}
