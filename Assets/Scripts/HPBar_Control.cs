using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar_Control : MonoBehaviour
{
    public Transform obj;
    public GameObject hp_bar;

    Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        hp_bar.transform.position = obj.position;
       
    }

    // Update is called once per frame
    void Update()
    {
        hp_bar.transform.position
            = cam.WorldToScreenPoint(obj.position + new Vector3(0, 5f, 0));
    }
}
