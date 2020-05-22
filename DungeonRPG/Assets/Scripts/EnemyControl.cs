using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : CharacterControl
{
    const int EFFECT_MAX = 15;

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
        EffectDestroy();
    }

    private void OnDestroy()
    {
        EffectDestroy();
    }

    void EffectDestroy()
    {
        if (_hitEffect_obj != null)
        {
            Destroy(_hitEffect_obj);
        }
    }

    public void EnemyTurn(Vector2Int pl)
    {
        if (Vector2Int.Distance(pl, _pos) > 1)
        {
            Move(pl);
        }
        else
        {
            Attack(pl);
        }
    }

    void Attack(Vector2Int pl)
    {
        _hitEffect_obj = Instantiate(_hitEffect_prefab);

        var dis = pl - _pos;
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

    void Move(Vector2Int pl)
    {
        var dis     = pl - _pos;
        var dis_X   = Mathf.Abs(dis.x);
        var dis_Y   = Mathf.Abs(dis.y);

        if (dis_X >= dis_Y)
        {
            _dir = Dir.RIGHT;
            if (dis_X < 0) _dir = Dir.LEFT;
        }
        else
        {
            _dir = Dir.UP;
            if (dis_Y > 0) _dir = Dir.DOWN; 
        }

        bool seach = false;
        while (!seach)
        {
            switch (_dir)
            {
                case Dir.UP:
                    break;
                case Dir.DOWN:
                    if (dis_X > dis_Y)
                    {
                        _dir = Dir.RIGHT;
                        break;
                    }
                    break;
                case Dir.LEFT:
                    break;
                case Dir.RIGHT:
                    break;
                default:
                    break;
            }
        }

        if(dis_X >= dis_Y)
        {
            if(dis.x > 0)
            {
                if (_pos.x + 1 >= _map._Tiles.GetLength(1)) return;
                if (_map._Tiles[_pos.y, _pos.x + 1] == 0) return;

                _map._Charactor[_pos.y, _pos.x] = 0;
                _pos.x += 1;
                _map._Charactor[_pos.y, _pos.x] = 2;

                _dir = Dir.RIGHT;
                _sprite.sprite = Right;
            }
            else
            {
                _pos.x -= 1;
                _sprite.sprite = Left;
            }
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
}
