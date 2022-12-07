using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    //public GameObject player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
    }

    //private void LateUpdate()
    //{
    //    Vector3 direction = (player.transform.position - transform.position).normalized;
    //    RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, Mathf.Infinity,
    //        1 << LayerMask.NameToLayer("EnvironmentObject"));

    //}

}
