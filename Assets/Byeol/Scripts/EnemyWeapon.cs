using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    public GameObject Enemy;
    public float speed = 10f;
    public float DestroyTime = 7.0f;
    public float Epower;

    public Player player;
    
    // Start is called before the first frame update
    void Start()
    {
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
        if (other.gameObject.tag == "Player")
        {
            player.setHealth(-Epower);
            Destroy(this.gameObject);
        }

        if(other.gameObject.tag == "floor")
        {
            Destroy(this.gameObject, 0.5f);
        }
    }

}
