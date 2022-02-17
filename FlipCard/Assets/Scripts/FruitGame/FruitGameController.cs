using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FruitGame
{

    public class FruitGameController : MonoBehaviour
    {
        public PlayerManager Chicken;
        public PlayerManager Dog;
        public PlayerManager Duck;
        public PlayerManager Rabbit;

        public UIGameInfo UIGameInfo;

        public Text ChickenScore;
        public Text DogScore;
        public Text DuckScore;
        public Text RabbitScore;

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            CheckGameOver();
            UpdatePlayerScore();
        }

        void UpdatePlayerScore()
        {
            ChickenScore.text = Chicken.GetScore().ToString();
            DogScore.text = Dog.GetScore().ToString();
            DuckScore.text = Duck.GetScore().ToString();
            RabbitScore.text = Rabbit.GetScore().ToString();
        }

        void CheckGameOver()
        {
            string text = "";
            text += Chicken.IsWin() ? "Chicken" : "";
            text += Dog.IsWin() ? "Dog" : "";
            text += Duck.IsWin() ? "Duck" : "";
            text += Rabbit.IsWin() ? "Rabit" : "";

            if(text!= "")
            {
                UIGameInfo.ShowQuickStart(text);
            }
            

        }
    }
}
