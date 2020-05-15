using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    // キャラの向き
    public enum Dir
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }
    protected Dir _dir;

    // キャラの状態
    public enum Status
    {
        Wait,
        Move,
        Attack,
        Skill,
    }
    protected Status _status;

    public Sprite Up;
    public Sprite Down;
    public Sprite Left;
    public Sprite Right;
    protected SpriteRenderer _sprite;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
