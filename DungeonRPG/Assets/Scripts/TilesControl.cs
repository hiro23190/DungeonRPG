using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesControl : MonoBehaviour
{
    [SerializeField] GameObject _floor;
    [SerializeField] GameObject _wall;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Set()
    {
        var tiles = transform.parent.GetComponent<MapCreate>()._Tiles;

        // すべての子オブジェクトを取得
        foreach (Transform n in this.transform)
        {
            // 削除
            GameObject.Destroy(n.gameObject);
        }
        // タイルの設置
        for (int i = 0; i < tiles.GetLength(0); ++i)
        {
            for (int j = 0; j < tiles.GetLength(1); ++j)
            {
                Vector3 pos = new Vector3(j, -i);

                var origin = tiles[i, j] != 0 ? _floor : _wall;
                var tile = Instantiate(origin, this.transform);
                tile.transform.position = pos;
            }
        }
    }
}
