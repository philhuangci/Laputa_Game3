using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public ParticleSystem BoomParticle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag != "Fruit")
        {
            Instantiate(BoomParticle, transform.position, Quaternion.identity);
            GameObject.Destroy(gameObject);
        }
    }
}
