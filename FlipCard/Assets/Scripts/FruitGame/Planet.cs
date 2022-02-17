using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FruitGame
{
    public class Planet : MonoBehaviour
    {
       

        private void OnTriggerEnter(Collider other)
        {
            Fruit fruit = other.gameObject.GetComponent<Fruit>();
            if (fruit )
            {
                Destroy(other.gameObject);
                PlayerManager playerManager = GetComponentInParent<PlayerManager>();
                if (fruit.IsBomb)
                {
                    playerManager.AddBomb();
                }
                else
                {
                    playerManager.AddFruit();
                }
                
            }

        }
    }
}