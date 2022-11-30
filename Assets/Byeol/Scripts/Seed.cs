using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : MonoBehaviour
{
    public GameObject[] trees;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject selectTree()
    {
        Debug.Log("selec Tree »£√‚µ ");

        int randomItemCount = Random.Range(0, trees.Length);
        GameObject selectedPrefab = trees[randomItemCount];
        return selectedPrefab;
    }
}
