using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossPotion : MonoBehaviour
{
    GameObject effect;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setParticle(GameObject particle)
    {
        effect = particle;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<Player>().setHealth(5f);
            Destroy(this.gameObject);
            Destroy(effect);
        }
    }
}
