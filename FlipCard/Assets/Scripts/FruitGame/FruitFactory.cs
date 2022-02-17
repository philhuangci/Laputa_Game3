using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FruitGame
{
    public class FruitFactory : MonoBehaviour
    {
        public Transform LB;
        public Transform LF;
        public Transform RF;
        public Transform RB;

        public GameObject[] FruitPrefabs;
        public GameObject ContainerBottom;
        public GameObject Ground;
        // Start is called before the first frame update
        public void Start()
        {
            StartCoroutine(CreateFruit());
        }

        // Update is called once per frame
        public void Update()
        {

        }

        IEnumerator CreateFruit()
        {
            int count = 0;
            while (true)
            {
                int i = count % 8;
                GameObject go = Instantiate(FruitPrefabs[i],
                    new Vector3(Random.Range(LB.position.x, RB.position.x),
                    LB.position.y, Random.Range(LB.position.z, LF.position.z)),
                    Quaternion.Euler(0.0f, 0.0f, 0.0f));

                go.transform.localScale = new Vector3(4.0f, 4.0f, 4.0f);
                Fruit fruit = go.AddComponent<Fruit>();
                fruit.Ground = Ground;
                fruit.ContainerBottom = ContainerBottom;
                if (count == 7)
                {
                    fruit.IsBomb = true;
                    go.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                }

                yield return new WaitForSeconds(0.4f);
                i++;
                count = i;

            }


        }
    }
}
