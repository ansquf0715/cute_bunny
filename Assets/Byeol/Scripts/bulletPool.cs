using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletPool : MonoBehaviour
{
    public GameObject prefab;
    public int poolSize = 20;

    private Queue<GameObject> pool;
    private static bulletPool instance;

    public static bulletPool Instance
    {
        get { return instance; }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;

        prefab = Resources.Load<GameObject>("carrot");
        pool = new Queue<GameObject>();

        //초기에 풀에 총알을 생성하여 저장
        for(int i=0; i<poolSize; i++)
        {
            GameObject bullet = Instantiate(prefab);
            bullet.SetActive(false);
            pool.Enqueue(bullet);
        }
    }

    public GameObject GetBullet()
    {
        Debug.Log("get bullet function");
        if (pool.Count > 0)
        {
            GameObject bullet = pool.Dequeue();
            bullet.SetActive(true);
            return bullet;
        }
        else
        {
            //풀에 총알이 부족하면 새로 생성
            GameObject bullet = Instantiate(prefab);
            return bullet;
        }
    }

    //총알을 비활성 상태로 변경하고 풀에 추가하는 메서드
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        pool.Enqueue(bullet);
        Debug.Log("pool size" + pool.Count);
    }

}
