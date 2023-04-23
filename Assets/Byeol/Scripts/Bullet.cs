using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 5; //총알속도
    public float DestroyTime = 1.0f; //사라지는 시간
    public Player player;

    public ParticleSystem particle; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Destroy(gameObject, DestroyTime);
        particle.Play();

        //GetComponent<Rigidbody>().isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Shoot();
        transform.position += player.GetMoveVec() * speed * Time.deltaTime;
    }

    public void startParticle()
    {
        particle.Play();
    }

    public void Shoot()
    {
        //GetComponent<Rigidbody>().isKinematic = false;
        transform.position += player.GetMoveVec() * speed * Time.deltaTime
            + new Vector3(0, 0.0001f, 0.0001f);
    }

}
