using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    const int MOVE_INTERVAL     = 30;   // 移動後の入力受付間隔
    const int ATTACK_INTERVAL   = 15;   // 攻撃後の入力受付間隔
    const int SKILL_INTERVAL    = 15;   // 特技後の入力受付間隔
    const int LIFE_MAX          = 3;

    // キャラの向き
    enum Dir
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    private Dir _dir;

    // キャラの状態
    enum Status
    {
        Wait,
        Move,
        Attack,
        Skill,
    }
    private Status _status;

    public Sprite Up;
    public Sprite Down;
    public Sprite Left;
    public Sprite Right;
    private SpriteRenderer _sprite;

    public  GameObject  _attack_prefab; // 攻撃の軌跡のプレハブ
    private GameObject  _attack_obj;    // 攻撃の軌跡

    public  GameObject      _skill_prefab; // 特技の軌跡のプレハブ
    private GameObject[]    _skill_obj;    // 特技の軌跡

    private int _intervalCnt;

    private int _life = 2;

    // Start is called before the first frame update
    void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
        _skill_obj = new GameObject[4];

        _status = Status.Wait;
        _dir    = Dir.DOWN;

        _intervalCnt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerTurn();
    }

    public void PlayerTurn()
    {
        ++_intervalCnt;

        switch (_status)
        {
            case Status.Move:
                if (_intervalCnt < MOVE_INTERVAL) return;
                else _status = Status.Wait;
                break;
            case Status.Attack:
                if (_intervalCnt < ATTACK_INTERVAL) return;
                else
                {
                    Destroy(_attack_obj);
                    _status = Status.Wait;
                }
                break;
            case Status.Skill:
                if (_intervalCnt < SKILL_INTERVAL) return;
                else
                {
                    for (int i = 0; i < _skill_obj.Length; ++i)
                    {
                        Destroy(_skill_obj[i]);
                    }
                    _status = Status.Wait;
                }
                break;
            default:
                break;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(transform.up);
            _sprite.sprite = Up;

            _dir = Dir.UP;
            _status = Status.Move;
            _intervalCnt = 0;

            return;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(-transform.up);
            _sprite.sprite = Down;

            _dir = Dir.DOWN;
            _status = Status.Move;
            _intervalCnt = 0;

            return;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(-transform.right);
            _sprite.sprite = Left;

            _dir = Dir.LEFT;
            _status = Status.Move;
            _intervalCnt = 0;

            return;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(transform.right);
            _sprite.sprite = Right;

            _dir = Dir.RIGHT;
            _status = Status.Move;
            _intervalCnt = 0;

            return;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            _attack_obj = Instantiate(_attack_prefab);

            _intervalCnt = 0;
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

            return;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            _intervalCnt = 0;
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

            return;
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            _intervalCnt = 0;

            if (_life < LIFE_MAX)
            {
                ++_life;
            }
            return;
        }
    }
}
