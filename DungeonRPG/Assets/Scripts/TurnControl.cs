using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnControl : MonoBehaviour
{
    public enum Turn
    {
        Interval,
        Player,
        Enemy,
    }
    public Turn _turn;

    const int INTERVAL_MAX = 60;

    public GameObject _player;
    public GameObject _enemys;

    public int _interval;

    // Start is called before the first frame update
    void Start()
    {
        _turn = Turn.Player;

        _interval = INTERVAL_MAX;
    }

    // Update is called once per frame
    void Update()
    {
        switch(_turn)
        {
            case Turn.Interval:
                --_interval;
                if(_interval < 0)
                {
                    _turn = Turn.Enemy;
                }
                break;
            case Turn.Player:
                // プレイヤーの処理
                var pc = _player.GetComponent<PlayerControl>();

                if(pc.PlayerTurn())
                {
                    _interval = INTERVAL_MAX;
                    _turn = Turn.Interval;
                }
                break;
            case Turn.Enemy:
                _interval = INTERVAL_MAX;
                _turn = Turn.Player;

                // 敵の処理
                var childCount = _enemys.transform.childCount;

                for (int i = 0; i < childCount; ++i)
                {
                    var ec = _enemys.transform.GetChild(i).GetComponent<EnemyControl>();

                    ec.EnemyTurn();
                }
                break;
            default:
                break;
        }
    }
}
