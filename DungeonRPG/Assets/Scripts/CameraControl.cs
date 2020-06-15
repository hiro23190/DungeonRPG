using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    GameObject _target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        var pos = _target.transform.position;
        pos.z = transform.position.z;
        transform.position = pos;
    }

    public void SetTarget(GameObject pl)
    {
        _target = pl;
    }
}
