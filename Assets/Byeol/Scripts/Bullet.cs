using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 5; //총알속도
    public float DestroyTime = 10.0f; //사라지는 시간
    public Player player;

    public ParticleSystem particle;

    Dictionary<GameObject, int> emptyObjectCounts = new Dictionary<GameObject, int>();

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Destroy(gameObject, DestroyTime);
        particle.Play();
    }

    // Update is called once per frame
    void Update()
    {

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
            Destroy(this.gameObject, 0.3f);
        }

        if (collision.gameObject.CompareTag("empty"))
        {
            Destroy(this.gameObject);
            GameObject emptyObject = collision.gameObject;
            FindObjectOfType<healPack>().bulletHitsEmptyObjects(collision.gameObject, collision.gameObject.transform.position);
        }

        if(collision.gameObject.CompareTag("treasure"))
        {
            Destroy(this.gameObject);
            GameObject healObject = collision.gameObject;
            FindObjectOfType<healPack>().bulletHitsHealObjects(collision.gameObject, collision.gameObject.transform.position);
        }
    }
}