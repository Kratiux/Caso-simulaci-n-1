using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    [SerializeField]
    GameObject spawningObjectPrefab;

    [SerializeField]
    int spawningAmount = 2;

    [SerializeField]
    float spawningRange = 50.0F;

    [SerializeField]
    float spawningSafeRange = 10.0F;

    public Transform playerTarget;




    List<GameObject> spawningObjects = new List<GameObject>();

    Vector3 spawningPoint;


    void Start()
    {
        for (int i = 0; i < spawningAmount; i++)
        {
            GetSpawningPoint();
            while (Vector2.Distance(spawningPoint, Vector3.zero) < spawningSafeRange)
            {
                GetSpawningPoint();
            }

            GameObject spawningObject = Instantiate(spawningObjectPrefab, spawningPoint, Quaternion.Euler(Random.Range(0.0F, 360.0F), Random.Range(0.0F, 360.0F), Random.Range(0.0F, 360.0F)));


            spawningObject.transform.parent = this.transform;
            spawningObjects.Add(spawningObject);

            EnemyController _enemycontroller = spawningObject.GetComponent<EnemyController>();

            _enemycontroller.playerTarget = playerTarget;
        }
    }



    void GetSpawningPoint()
    {
        spawningPoint = new Vector3(Random.Range(-1.0F, 1.0F), Random.Range(-1.0F, 1.0F), Random.Range(-1.0F, 1.0F));

        if (spawningPoint.magnitude > 1.0F)
        {
            spawningPoint.Normalize();
        }
        spawningPoint *= spawningRange;
    }
}
