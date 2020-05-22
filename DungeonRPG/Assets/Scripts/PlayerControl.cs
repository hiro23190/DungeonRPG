using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : CharacterControl
{
    const int LIFE_MAX  = 3;
    const int COUTN_MAX = 15;

    [SerializeField]
    GameObject  _attack_prefab; // 攻撃の軌跡のプレハブ
    GameObject  _attack_obj;    // 攻撃の軌跡

    [SerializeField]
    GameObject      _skill_prefab; // 特技の軌跡のプレハブ
    GameObject[]    _skill_obj;    // 特技の軌跡

    private int _life = 2;

    private int _count;

    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _map    = transform.parent.GetComponent<MapCreate>();
        _turn   = transform.parent.GetComponent<TurnControl>();

        _skill_obj = new GameObject[4];

        _status = Status.Wait;
        _dir    = Dir.DOWN;
        _count  = COUTN_MAX;

        // playerの初期位置設定
        var room = _map._Rooms[0];
        var w = Random.Range(0, room.Width);
        var h = Random.Range(0, room.Hight);
        _pos = new Vector2Int(room.UpperLeft.x + w, room.UpperLeft.y + h);
        _map._Charactor[_pos.y, _pos.x] = 1;
        transform.position = new Vector3(_pos.x, -_pos.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (_turn.IsPlayerTurn())
        {
            if (PlayerTurn())
            {
                _turn.EndPlayerTurn();
            }
        }
        transform.position = new Vector3(_pos.x, -_pos.y);

        if (_count > 0)
        {
            --_count;
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
        if (_attack_obj != null)
        {
            Destroy(_attack_obj);
        }
        if (_skill_obj != null)
        {
            for (int i = 0; i < _skill_obj.Length; ++i)
            {
                Destroy(_skill_obj[i]);
            }
        }
    }

    bool PlayerTurn()
    {
        // 移動
        if (Input.GetKey(KeyCode.UpArrow))
        {
            _dir = Dir.UP;
            _sprite.sprite = Up;

            if (_pos.y - 1 < 0) return false;
            if (_map._Tiles[_pos.y-1,_pos.x] == 0) return false;

            _status = Status.Move;

            _map._Charactor[_pos.y, _pos.x] = 0;
            _pos.y -= 1;
            _map._Charactor[_pos.y, _pos.x] = 1;

            return true;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            _dir = Dir.DOWN;
            _sprite.sprite = Down;

            if (_pos.y + 1 >= _map._Tiles.GetLength(0)) return false;
            if (_map._Tiles[_pos.y + 1, _pos.x] == 0) return false;

            _status = Status.Move;

            _map._Charactor[_pos.y, _pos.x] = 0;
            _pos.y += 1;
            _map._Charactor[_pos.y, _pos.x] = 1;

            return true;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            _dir = Dir.LEFT;
            _sprite.sprite = Left;

            if (_pos.x - 1 < 0) return false;
            if (_map._Tiles[_pos.y, _pos.x - 1] == 0) return false;

            _status = Status.Move;

            _map._Charactor[_pos.y, _pos.x] = 0;
            _pos.x -= 1;
            _map._Charactor[_pos.y, _pos.x] = 1;

            return true;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _dir = Dir.RIGHT;
            _sprite.sprite = Right;

            if (_pos.x + 1 >= _map._Tiles.GetLength(1)) return false;
            if (_map._Tiles[_pos.y, _pos.x + 1] == 0) return false;

            _status = Status.Move;

            _map._Charactor[_pos.y, _pos.x] = 0;
            _pos.x += 1;
            _map._Charactor[_pos.y, _pos.x] = 1;

            return true;
        }

        // 攻撃
        if (Input.GetKeyDown(KeyCode.X))
        {
            _attack_obj = Instantiate(_attack_prefab);

            _count = COUTN_MAX;
            _status = Status.Attack;

            var pos = transform.position;

            switch (_dir)
            {
                case Dir.UP:
                    pos.y += 0.5f;
                    _attack_obj.transform.rotation = Quaternion.Euler(0, 0, 90);
                    break;
                case Dir.DOWN:
                    pos.y -= 0.5f;
                    _attack_obj.transform.rotation = Quaternion.Euler(0, 0, -90);
                    break;
                case Dir.LEFT:
                    pos.x -= 0.5f;
                    _attack_obj.transform.rotation = Quaternion.Euler(0, 0, 180);
                    break;
                case Dir.RIGHT:
                    pos.x += 0.5f;
                    _attack_obj.transform.rotation = Quaternion.Euler(0, 0, 0);
                    break;
            }
            _attack_obj.transform.position = pos;

            return true;
        }

        // 特技
        if (Input.GetKeyDown(KeyCode.Z))
        {
            _count = COUTN_MAX;
            _status = Status.Skill;

            var pos = transform.position;

            for (int i = 0; i < _skill_obj.Length; ++i)
            {
                _skill_obj[i] = Instantiate(_skill_prefab);
            }
            _skill_obj[0].transform.position = pos + Vector3.up;
            _skill_obj[0].transform.rotation = Quaternion.Euler(0, 0, 90);
            _skill_obj[1].transform.position = pos - Vector3.up;
            _skill_obj[1].transform.rotation = Quaternion.Euler(0, 0, -90);
            _skill_obj[2].transform.position = pos - Vector3.right;
            _skill_obj[2].transform.rotation = Quaternion.Euler(0, 0, 180);
            _skill_obj[3].transform.position = pos + Vector3.right;
            _skill_obj[3].transform.rotation = Quaternion.Euler(0, 0, 0);

            return true;
        }

        // アイテム
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (_life < LIFE_MAX)
            {
                ++_life;
            }
            return true;
        }

        return false;
    }
}
