using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    const int COUNT_MAX = 30;

    private int _count = 0; // 削除までのカウント

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ++_count;

        if(COUNT_MAX < _count)
        {
            Destroy(this);
        }
    }
}
