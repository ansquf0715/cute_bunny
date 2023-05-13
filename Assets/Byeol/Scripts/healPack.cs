using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class healPack : MonoBehaviour
{
    public GameObject emptyParticle;
    public GameObject packParticle;

    public GameObject bossPotion;
    public GameObject potionParticle;
    Vector3 potionPos;

    Dictionary<GameObject, int> emptyObjectCounts = new Dictionary<GameObject, int>();
    Dictionary<GameObject, int> packObjectCounts = new Dictionary<GameObject, int>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void bulletHitsEmptyObjects(GameObject obj, Vector3 objPos)
    {
        if(!emptyObjectCounts.ContainsKey(obj))
        {
            emptyObjectCounts.Add(obj, 1);
        }
        else
        {
            emptyObjectCounts[obj]++;

            if(emptyObjectCounts[obj] >= 3)
            {
                Debug.Log("empty objects count >= 3");

                Vector3 newPos = objPos;
                newPos.y += 3;

                Destroy(obj, 1f);
            }
        }

        foreach (var kvp in emptyObjectCounts)
        {
            Debug.Log("key : " + kvp.Key + "value : " + kvp.Value);
        }
    }

    public void bulletHitsHealObjects(GameObject obj, Vector3 objPos)
    {
        if(!packObjectCounts.ContainsKey(obj))
        {
            packObjectCounts.Add(obj, 1);
        }
        else
        {
            packObjectCounts[obj]++;

            if(packObjectCounts[obj] >= 3)
            {
                Vector3 newPos = objPos;
                newPos.y += 5;
                potionPos = newPos;
                potionPos.y -= 2;

                GameObject clonedHealParticle = Instantiate(
                    packParticle, newPos, Quaternion.identity);
                clonedHealParticle.GetComponent<ParticleSystem>().Play();

                Destroy(clonedHealParticle, 1f);
                Destroy(obj, 1.5f);

                Invoke("clonePotion", 1f);
            }
        }
    }

    void clonePotion()
    {
        GameObject clonedBossPotion = Instantiate(
            bossPotion, potionPos, Quaternion.identity);
        GameObject clonedBossPotionParticle = Instantiate(
            potionParticle, potionPos, Quaternion.identity);

        clonedBossPotion.GetComponent<bossPotion>().setParticle(clonedBossPotionParticle);
    }

}
