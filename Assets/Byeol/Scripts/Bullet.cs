using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed = 5; //�Ѿ˼ӵ�
    public float DestroyTime = 2.0f; //������� �ð�
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
        transform.position += player.GetMoveVec() * speed * Time.deltaTime;
    }

}
