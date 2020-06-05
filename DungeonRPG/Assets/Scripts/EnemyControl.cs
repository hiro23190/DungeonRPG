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
            if (Vector2Int.Distance(pl, _pos) < 7)
            {
                Chase(pl);
            }
            else
            {
                Wander(pl);
            }
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

    void Wander(Vector2Int pl)
    {
        var check = false;
        while (!check)
        {
            var d = Random.Range(0, 3);

            switch (d)
            {
                case 0:
                    if (_map._Tiles[_pos.y - 1, _pos.x] == 1 &&
                        _map._Charactor[_pos.y - 1, _pos.x] == 0)
                    {
                        _dir = Dir.UP;
                        check = true;
                    }
                    break;
                case 1:
                    if (_map._Tiles[_pos.y + 1, _pos.x] == 1 &&
                        _map._Charactor[_pos.y + 1, _pos.x] == 0)
                    {
                        _dir = Dir.DOWN;
                        check = true;
                    }
                    break;
                case 2:
                    if (_map._Tiles[_pos.y, _pos.x - 1] == 1 &&
                        _map._Charactor[_pos.y, _pos.x - 1] == 0)
                    {
                        _dir = Dir.LEFT;
                        check = true;
                    }
                    break;
                case 3:
                    if (_map._Tiles[_pos.y, _pos.x + 1] == 1 &&
                        _map._Charactor[_pos.y, _pos.x + 1] == 0)
                    {
                        _dir = Dir.RIGHT;
                        check = true;
                    }
                    break;
                default:
                    break;
            }
        }

        Move();
    }

    void Chase(Vector2Int pl)
    {
        var up = int.MaxValue;
        if (_map._Tiles[_pos.y - 1, _pos.x] == 1 &&
            _map._Charactor[_pos.y - 1, _pos.x] == 0)
        {
            var x = Mathf.Abs(pl.x - _pos.x);
            var y = Mathf.Abs(pl.y - (_pos.y - 1));

            up = x + y;
        }
        var down = int.MaxValue;
        if (_map._Tiles[_pos.y + 1, _pos.x] == 1 &&
            _map._Charactor[_pos.y + 1, _pos.x] == 0)
        {
            var x = Mathf.Abs(pl.x - _pos.x);
            var y = Mathf.Abs(pl.y - (_pos.y + 1));

            down = x + y;
        }
        var left = int.MaxValue;
        if (_map._Tiles[_pos.y, _pos.x - 1] == 1 &&
            _map._Charactor[_pos.y, _pos.x - 1] == 0)
        {
            var x = Mathf.Abs(pl.x - (_pos.x - 1));
            var y = Mathf.Abs(pl.y - _pos.y);

            left = x + y;
        }
        var right = int.MaxValue;
        if (_map._Tiles[_pos.y, _pos.x + 1] == 1 &&
            _map._Charactor[_pos.y, _pos.x + 1] == 0)
        {
            var x = Mathf.Abs(pl.x - (_pos.x + 1));
            var y = Mathf.Abs(pl.y - _pos.y);

            right = x + y;
        }

        _dir = Dir.UP;
        var min = up;
        if (min > down)
        {
            _dir = Dir.DOWN;
            min = down;
        }
        if (min > left)
        {
            _dir = Dir.LEFT;
            min = left;
        }
        if (min > right)
        {
            _dir = Dir.RIGHT;
            min = right;
        }

        Move();
    }

    void Move()
    {
        switch(_dir)
        {
            case Dir.UP:
                _sprite.sprite = Up;
                _map._Charactor[_pos.y, _pos.x] = 0;
                _pos.y -= 1;
                _map._Charactor[_pos.y, _pos.x] = 2;
                break;
            case Dir.DOWN:
                _sprite.sprite = Down;
                _map._Charactor[_pos.y, _pos.x] = 0;
                _pos.y += 1;
                _map._Charactor[_pos.y, _pos.x] = 2;
                break;
            case Dir.LEFT:
                _sprite.sprite = Left;
                _map._Charactor[_pos.y, _pos.x] = 0;
                _pos.x -= 1;
                _map._Charactor[_pos.y, _pos.x] = 2;
                break;
            case Dir.RIGHT:
                _sprite.sprite = Right;
                _map._Charactor[_pos.y, _pos.x] = 0;
                _pos.x += 1;
                _map._Charactor[_pos.y, _pos.x] = 2;
                break;
            default:
                break;
        }

        transform.position = new Vector3(_pos.x, -_pos.y);
    }

    public void SetPos(int x, int y)
    {
        _pos = new Vector2Int(x, y);
    }
}
