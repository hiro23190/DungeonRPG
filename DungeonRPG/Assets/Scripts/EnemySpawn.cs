using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    const int SPAWN_INTERVAL = 5;
    const int ENEMY_MAX = 10;

    [SerializeField] GameObject _original;

    MapCreate _map;
    TurnControl _turn;
    PlayerControl _player;

    List<GameObject> _enemys;

    int _spawnInterval;

    // Start is called before the first frame update
    void Start()
    {
        _map = transform.parent.GetComponent<MapCreate>();
        _turn = transform.parent.GetComponent<TurnControl>();

        _enemys = new List<GameObject>();

        for (int i = 0; i < 10; ++i)
        {
            Spawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(_turn.IsEnemyTurn())
        {
            if(_enemys.Count < ENEMY_MAX)
            {
                //Spawn();
            }

            foreach(GameObject ene in _enemys)
            {
                ene.GetComponent<EnemyControl>().EnemyTurn(_player.GetPos());
            }

            _turn.EndEnemyTurn();
        }
    }

    void Spawn()
    {
        var x = Random.Range(0, _map._Tiles.GetLength(1));
        var y = Random.Range(0, _map._Tiles.GetLength(0));

        if (_map._Tiles[y, x] == 1 && _map._Charactor[y, x] == 0)
        {
            var enemy = Instantiate(_original, new Vector3(x, -y), new Quaternion(), this.transform.parent);
            _map._Charactor[y, x] = 2;
            enemy.GetComponent<EnemyControl>().SetPos(x, y);
            _enemys.Add(enemy);
        }
        for (int i = 0; i < _map._Rooms.Count; ++i)
        {
            //_player.transform.position
            //for (int h = 0; h < _map._Rooms[i].Hight; ++h) 
            //{
            //    for (int w = 0; w < _map._Rooms[i].Width; ++w)
            //    {
            //        _map._Charactor[,]
            //    }
            //}
        }
    }

    public void SetPlayer(GameObject pl)
    {
        _player = pl.GetComponent<PlayerControl>();
    }
}
