using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bulletPool bullets;

    public float speed = 5; //총알속도
    public float DestroyTime = 10.0f; //사라지는 시간
    public Player player;

    public ParticleSystem particle;

    // Start is called before the first frame update
    void Start()
    {
        bullets = GameObject.Find("GameObject").GetComponent<bulletPool>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        //Destroy(gameObject, DestroyTime);
        Invoke("returnObject", DestroyTime);

        particle.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void returnObject()
    {
        bullets.ReturnBullet(this.gameObject);
    }

    public void startParticle()
    {
        particle.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("floor")
            || collision.gameObject.CompareTag("FightingZonePlane"))
        {
            Invoke("returnObject", 0.3f);
        }

        if (collision.gameObject.CompareTag("empty"))
        {
            returnObject();
            GameObject emptyObject = collision.gameObject;
            FindObjectOfType<healPack>().bulletHitsEmptyObjects(collision.gameObject, collision.gameObject.transform.position);
        }

        if(collision.gameObject.CompareTag("treasure"))
        {
            //Destroy(this.gameObject);
            returnObject();
            GameObject healObject = collision.gameObject;
            FindObjectOfType<healPack>().bulletHitsHealObjects(collision.gameObject, collision.gameObject.transform.position);
        }
    }
}