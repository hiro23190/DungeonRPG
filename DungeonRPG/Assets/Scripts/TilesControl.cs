using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilesControl : MonoBehaviour
{
    [SerializeField] GameObject _floor;
    [SerializeField] GameObject _wall;
    [SerializeField] GameObject _ladder;

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

                switch (tiles[i, j])
                {
                    case 0:
                        Instantiate(_wall, pos, new Quaternion(), this.transform);
                        break;
                    case 1:
                        Instantiate(_floor, pos, new Quaternion(), this.transform);
                        break;
                    case 2:
                        Instantiate(_floor, pos, new Quaternion(), this.transform);
                        Instantiate(_ladder, pos, new Quaternion(), this.transform);
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
