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

        //�ʱ⿡ Ǯ�� �Ѿ��� �����Ͽ� ����
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
            //Ǯ�� �Ѿ��� �����ϸ� ���� ����
            GameObject bullet = Instantiate(prefab);
            return bullet;
        }
    }

    //�Ѿ��� ��Ȱ�� ���·� �����ϰ� Ǯ�� �߰��ϴ� �޼���
    public void ReturnBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        pool.Enqueue(bullet);
        Debug.Log("pool size" + pool.Count);
    }

}
