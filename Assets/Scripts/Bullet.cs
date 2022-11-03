using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 5; //총알속도
    public float DestroyTime = 2.0f; //사라지는 시간

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, DestroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Vector3.forward;
        transform.position += dir * speed * Time.deltaTime;
    }

}
