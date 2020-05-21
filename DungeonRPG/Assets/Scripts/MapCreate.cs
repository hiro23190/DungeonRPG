using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCreate : MonoBehaviour
{
    const int Line = 40;    // 行数
    const int Row = 50;     // 列数

    struct Room
    {
        public Vector2Int UpperLeft;   // 部屋の左上座標
        public Vector2Int Center;      // 部屋の中心座標
        public int Hight;              // 部屋の高さ
        public int Width;              // 部屋の幅
    }

    List<Room> _Rooms;
    public int[,] _Tiles;
    public int[,] _Charactor;

    [SerializeField] GameObject _tiles;
    [SerializeField] GameObject _number;
    [SerializeField] GameObject _player;
    [SerializeField] GameObject _spawner;

    // Start is called before the first frame update
    void Start()
    {
        _Rooms = new List<Room>();
        _Tiles      = new int[Line, Row];
        _Charactor  = new int[Line, Row];

        Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            // mapの生成
            Create();
            // すべての子オブジェクトを取得
            foreach (Transform n in this.transform)
            {
                // 削除
                GameObject.Destroy(n.gameObject);
            }
            // タイルの設置
            Instantiate(_tiles, this.transform).GetComponent<TilesControl>().Set();

            // 部屋番号の表示
            for (int i = 0; i < _Rooms.Count; ++i) 
            {
                var number = Instantiate(_number, this.transform);
                number.GetComponent<TextMesh>().text = i.ToString();
                Vector3 pos = new Vector3(_Rooms[i].Center.x, -_Rooms[i].Center.y);
                number.transform.position = pos;
            }

            // player
            var pl = Instantiate(_player, this.transform);
            // spawner
            var sp = Instantiate(_spawner, this.transform);
            sp.GetComponent<EnemySpawn>().SetPlayer(pl);
        }
    }

    void Create()
    {
        Init();
        _Rooms.Clear();

        // 部屋をランダム生成
        for (int i = 0; i < 20; ++i)
        {
            var room = new Room();
            room.UpperLeft.x   = Random.Range(1, Row);
            room.UpperLeft.y   = Random.Range(1, Line);
            room.Hight         = Random.Range(6, 16);
            room.Width         = Random.Range(6, 16);
            room.Center         = new Vector2Int(room.UpperLeft.x + room.Width / 2, room.UpperLeft.y + room.Hight / 2);

            if (room.UpperLeft.x + room.Width > Row)  continue;
            if (room.UpperLeft.y + room.Hight > Line) continue;

            // 部屋同士の間隔をあける
            var l_min = Mathf.Max(room.UpperLeft.y - 3, 0);
            var r_min = Mathf.Max(room.UpperLeft.x - 3, 0);
            var l_max = Mathf.Min(room.UpperLeft.y + room.Hight + 3, Line - 1);
            var r_max = Mathf.Min(room.UpperLeft.x + room.Width + 3, Row - 1);

            // 生成可能かチェック
            bool flag = false;
            for (int l = l_min; l < l_max; ++l)
            {
                for (int r = r_min; r < r_max; ++r)
                {
                    if (_Tiles[l,r] != 0)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (flag) continue;

            // 部屋の設置
            for (int y = room.UpperLeft.y; y < room.UpperLeft.y + room.Hight; ++y)
            {
                for (int x = room.UpperLeft.x; x < room.UpperLeft.x + room.Width; ++x)
                {
                    _Tiles[y,x] = 1;
                }
            }
            _Rooms.Add(room);
        }
        Debug.Log(_Rooms.Count);

        // 通路の設定
        for (int i = 0; i < _Rooms.Count; ++i)
        {
            int room_num = i;               // 最寄りの部屋番号
            float dis_min = float.MaxValue; // 最寄りの部屋との距離

            for (int j = 0; j < _Rooms.Count; ++j)
            {
                // 同じ部屋は飛ばす
                if (i == j) continue;
                // 部屋同士の距離
                var d = Vector2Int.Distance(_Rooms[i].Center, _Rooms[j].Center);
                // 最寄りの距離更新
                if(dis_min > d)
                {
                    dis_min = d;
                    room_num = j;
                }
            }
            var str = ("%d & %d", i, room_num);
            Debug.Log(str);

            var dis_y = _Rooms[room_num].Center.y - _Rooms[i].Center.y;
            var dis_x = _Rooms[room_num].Center.x - _Rooms[i].Center.x;

            for (int x = 0; x < Mathf.Abs(dis_x); ++x)
            {
                var r = _Rooms[i].Center.x + (x + 1) * (dis_x / Mathf.Abs(dis_x));
                _Tiles[_Rooms[i].Center.y, r] = 1;
            }
            for (int y = 0; y < Mathf.Abs(dis_y); ++y)
            {
                var l = _Rooms[room_num].Center.y - (y + 1) * (dis_y / Mathf.Abs(dis_y));
                _Tiles[l, _Rooms[room_num].Center.x] = 1;
            }
        }

        // キャラ用マップの生成
        _Charactor[_Rooms[0].Center.y, _Rooms[0].Center.x] = 1;
    }

    void Init()
    {
        // 初期化
        for (int i = 0; i < Line; ++i)
        {
            for (int j = 0; j < Row; ++j)
            {
                _Tiles[i, j] = 0;
                _Charactor[i, j] = 0;
            }
        }
    }

    public Vector2Int GetStartPos()
    {
        var x = _Rooms[0].UpperLeft.x + Random.Range(0, _Rooms[0].Width);
        var y = _Rooms[0].UpperLeft.y + Random.Range(0, _Rooms[0].Hight);

        return new Vector2Int(x, y);
    }
}
