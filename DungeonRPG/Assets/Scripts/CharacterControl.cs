using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    // キャラの向き
    protected enum Dir
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    protected Dir _dir;

    // キャラの状態
    protected enum Status
    {
        Wait,
        Move,
        Attack,
        Skill,
    }
    protected Status _status;

    protected MapCreate _map;
    protected TurnControl _turn;

    [SerializeField] protected Sprite Up;
    [SerializeField] protected Sprite Down;
    [SerializeField] protected Sprite Left;
    [SerializeField] protected Sprite Right;
    protected SpriteRenderer _sprite;

    protected Vector2Int _pos;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
