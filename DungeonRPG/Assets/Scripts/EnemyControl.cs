using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : CharacterControl
{
    const int EFFECT_MAX = 15;

    private GameObject _player;

    [SerializeField]
    GameObject  _hitEffect_prefab; // 攻撃の軌跡のプレハブ
    GameObject  _hitEffect_obj;    // 攻撃の軌跡
    private int _effectCount;

    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _map = transform.parent.GetComponent<MapCreate>();

        _effectCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_effectCount > 0)
        {
            --_effectCount;
            return;
        }
        if (_hitEffect_obj != null)
        {
            Destroy(_hitEffect_obj);
        }
    }

    public void EnemyTurn()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) <= 1.0f)
        {
            Attack();
        }
        else
        {
            Move();
        }
    }

    void Attack()
    {
        _hitEffect_obj = Instantiate(_hitEffect_prefab);

        Vector3 dis = _player.transform.position - transform.position;
        var pos = transform.position;
        _effectCount = EFFECT_MAX;

        if (dis.x != 0)
        {
            _sprite.sprite = dis.x < 0 ? Left : Right;
            pos.x = dis.x < 0 ? pos.x - 0.5f : pos.x + 0.5f;
        }
        else
        {
            _sprite.sprite = dis.y < 0 ? Down : Up;
            pos.y = dis.y < 0 ? pos.y - 0.5f : pos.y + 0.5f;
        }
        _hitEffect_obj.transform.position = pos;
    }

    void Move()
    {
        Vector3 dis = _player.transform.position - transform.position;
        float dis_X = Mathf.Abs(dis.x);
        float dis_Y = Mathf.Abs(dis.y);

        if(dis_X >= dis_Y)
        {
            transform.Translate(transform.right * (dis.x / dis_X));

            _sprite.sprite = (dis.x / dis_X) < 0 ? Left : Right;
        }
        else
        {
            transform.Translate(transform.up * (dis.y / dis_Y));

            _sprite.sprite = (dis.y / dis_Y) < 0 ? Down : Up;
        }
    }

    public void SetPos(int x, int y)
    {
        _pos = new Vector2Int(x, y);
    }

    public void SetPlayer(GameObject pl)
    {
        _player = pl;
    }
}
