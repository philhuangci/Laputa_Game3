using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FruitGame
{
    public class Fruit : MonoBehaviour
    {
        public GameObject Ground;
        public GameObject ContainerBottom;

        [HideInInspector]
        public bool IsBomb = false;

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject == Ground)
            {
                StartCoroutine(DisappearFruit(0.2f));
            }

        }
        IEnumerator DisappearFruit(float time)
        {
            yield return new WaitForSeconds(time);
            Destroy(this.gameObject);
        }
    }
}
