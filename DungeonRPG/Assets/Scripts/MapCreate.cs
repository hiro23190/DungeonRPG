using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreate : MonoBehaviour
{
    struct Room
    {
        public Vector2Int UpperLeft;   // 部屋の左上座標
        public Vector2Int Center;      // 部屋の中心座標
        public int Hight;              // 部屋の高さ
        public int Width;              // 部屋の幅
    }

    List<Room> _Rooms;
    int[] _Tiles;

    public GameObject _floor;
    public GameObject _wall;
    
    // Start is called before the first frame update
    void Start()
    {
        _Rooms = new List<Room>();
        _Tiles = new int[2500];
        // 初期化
        for (int i = 0; i < _Tiles.Length; ++i)
        {
            _Tiles[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.M))
        {
            Create();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            // すべての子オブジェクトを取得
            foreach (Transform n in this.transform)
            {
                GameObject.Destroy(n.gameObject);
            }
            for (int i = 0; i < _Tiles.Length; ++i)
            {
                var x = i / 50;
                var y = -i % 50;
                Vector3 pos = new Vector3(x, y);

                var origin = _Tiles[i] != 0 ? _floor : _wall;
                var tile = Instantiate(origin, this.transform);
                tile.transform.position = pos;
            }
            Debug.Log(_Tiles.Length);
        }
    }

    void Create()
    {
        // 初期化
        for (int i = 0; i < _Tiles.Length; ++i)
        {
            _Tiles[i] = 0;
        }
        _Rooms.Clear();

        for (int i = 0; i < 12; ++i)
        {
            var r = new Room();
            r.UpperLeft.x   = Random.Range(0, 50);
            r.UpperLeft.y   = Random.Range(0, 50);
            r.Hight         = Random.Range(1, 20);
            r.Width         = Random.Range(1, 20);

            if (r.UpperLeft.x + r.Width > 50) continue;
            if (r.UpperLeft.y + r.Hight > 50) continue;

            bool flag = false;
            for (int h = 0; h < r.Hight; ++h)
            {
                for (int w = 0; w < r.Width; ++w)
                {
                    var id = r.UpperLeft.x * r.UpperLeft.y + w + h * 50;
                    if (_Tiles[id] != 0)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (flag) continue;

            for (int h = 0; h < r.Hight; ++h)
            {
                for (int w = 0; w < r.Width; ++w)
                {
                    var id = r.UpperLeft.x * r.UpperLeft.y + w + h * 50;
                    _Tiles[id] = 1;
                }
            }

            _Rooms.Add(r);
        }

        Debug.Log(_Rooms.Count);
    }
}
