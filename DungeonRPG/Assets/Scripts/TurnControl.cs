using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnControl : MonoBehaviour
{
    enum Turn
    {
        Interval,
        Player,
        Enemy,
    }
    [SerializeField] Turn _turn;
    [SerializeField] Turn _next;

    const int INTERVAL_MAX = 20;

    int _interval;

    // Start is called before the first frame update
    void Start()
    {
        _turn = Turn.Player;
        _interval = INTERVAL_MAX;
    }

    // Update is called once per frame
    void Update()
    {
        if(_turn == Turn.Interval)
        {
                --_interval;
                if(_interval < 0)
                {
                    _turn = _next;
                }
        }
    }

    public bool IsPlayerTurn()
    {
        if(Turn.Player == _turn)
            return true;
        return false;
    }
    public void EndPlayerTurn()
    {
        _next = Turn.Enemy;
        _turn = Turn.Interval;
        _interval = INTERVAL_MAX;
    }

    public bool IsEnemyTurn()
    {
        if(Turn.Enemy == _turn)
            return true;
        return false;
    }
    public void EndEnemyTurn()
    {
        _next = Turn.Player;
        _turn = Turn.Interval;
        _interval = INTERVAL_MAX;
    }
}
