using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StatePattern;

public class BossTriggerHandler : MonoBehaviour
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
        if(other.gameObject.tag.Equals("Player"))
        {
            Boss.meetPlayerPos = other.transform.position;
        }
    }
}
