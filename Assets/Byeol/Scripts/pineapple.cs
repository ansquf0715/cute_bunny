using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pineapple : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("player collision");
            other.gameObject.GetComponent<Player>().setHealth(-8f);

            GameObject pineapplePlayer = Resources.Load<GameObject>("pineapplePlayer");
            GameObject clonedPineapplePlayer = Instantiate(
                pineapplePlayer, this.gameObject.transform.position, Quaternion.identity);

            Destroy(clonedPineapplePlayer, 0.8f);
            Destroy(this.gameObject, 1f);
        }
        else if(other.gameObject.CompareTag("floor"))
        {
            GameObject pineappleWater = Resources.Load<GameObject>("pineappleFloor");
            GameObject clonedPineappleWater = Instantiate(
                pineappleWater, this.gameObject.transform.position, Quaternion.identity);

            Destroy(clonedPineappleWater, 1f);
            Destroy(this.gameObject, 1.3f);
        }
    }
}
