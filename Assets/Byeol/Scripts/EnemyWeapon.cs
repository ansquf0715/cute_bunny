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
        //speed = 5;
        //DestroyTime = 1.0f;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Destroy(gameObject, DestroyTime);
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 dir = Enemy.GetComponent<Enemy>().GetToPlayer();
        //transform.position += dir * speed * Time.deltaTime;

        Vector3 player_pos = GameObject.FindWithTag("Player").GetComponent<Player>().getPos() +
            Enemy.GetComponent<Enemy>().GetEnemyPos();
        this.transform.position = Vector3.MoveTowards(this.transform.position,
            player_pos, speed * Time.deltaTime);
        Vector3 toPlayer = player_pos - transform.position;
        transform.rotation = Quaternion.LookRotation(toPlayer).normalized;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Player")
        {
            player.setHealth(-Epower);
        }
    }

    //IEnumerator attackDelay()
    //{
    //    yield return new WaitForSeconds(1);
    //}

}
