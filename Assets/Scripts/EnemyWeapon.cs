using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public GameObject Enemy;
    public float speed = 5;
    public float DestroyTime = 0.5f;
    public float Epower;

    public Player player;
    
    // Start is called before the first frame update
    void Start()
    {
        //speed = 5;
        //DestroyTime = 1.0f;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Destroy(gameObject, DestroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Enemy.GetComponent<Enemy>().GetToPlayer();
        transform.position += dir * speed * Time.deltaTime;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            float phealth = player.getHealth();
            phealth -= Epower;
            player.setHealth(phealth);
        }
    }

}
