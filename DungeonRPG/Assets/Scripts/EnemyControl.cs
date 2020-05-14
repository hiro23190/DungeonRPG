using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(Vector3.Distance(_player.transform.position,transform.position) <= 1.0f)
            {
                Attack();
            }
            else
            {
                Move();
            }
        }
    }

    void Attack()
    {

    }

    void Move()
    {
        Vector3 dis = _player.transform.position - transform.position;
        float dis_X = Mathf.Abs(dis.x);
        float dis_Y = Mathf.Abs(dis.y);

        if(dis_X >= dis_Y)
        {
            transform.Translate(transform.right * (dis.x / dis_X));
        }
        else
        {
            transform.Translate(transform.up * (dis.x / dis_X));
        }
    }
}
