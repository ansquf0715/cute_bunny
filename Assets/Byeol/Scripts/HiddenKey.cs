using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenKey : MonoBehaviour
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
        if(other.gameObject.tag == "Player")
        {
            FindObjectOfType<QuestManager>().foundKeyForBird();
            Destroy(this.gameObject, 1f);
        }
    }
}
