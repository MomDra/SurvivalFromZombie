using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    [SerializeField] int mapX;
    [SerializeField] int mapZ;

    Vector3 randomPos;

    [SerializeField] GameObject boxPrefab;
    Rigidbody rigid;

    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        int x = Random.Range(-mapX + 5, mapX - 5);
        int z = Random.Range(-mapZ + 5, mapZ - 5);

        randomPos = new Vector3(x, transform.position.y, z);

        transform.position = new Vector3(x, transform.position.y, -mapZ - 20);
    }
    
    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + transform.forward * 10 * Time.deltaTime);

        if(Vector3.Distance(rigid.position, randomPos) <= 0.001f)
        {
            Instantiate(boxPrefab, randomPos - Vector3.down * 3, Quaternion.identity);
        }

        if(rigid.position.z >= 80)
        {
            Destroy(gameObject);
        }
    }
}
