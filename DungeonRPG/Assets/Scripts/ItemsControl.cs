using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsControl : MonoBehaviour
{
    [SerializeField] GameObject _coin;
    [SerializeField] GameObject _flasks;

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
        var items = transform.parent.GetComponent<MapCreate>()._Items;

        // すべての子オブジェクトを取得
        foreach (Transform n in this.transform)
        {
            // 削除
            GameObject.Destroy(n.gameObject);
        }
        // アイテムの設置
        for (int i = 0; i < items.GetLength(0); ++i)
        {
            for (int j = 0; j < items.GetLength(1); ++j)
            {
                Vector3 pos = new Vector3(j, -i);

                GameObject origin = null;
                switch(items[i,j])
                {
                    case 1:
                        origin = _coin;
                        break;
                    case 2:
                        origin = _flasks;
                        break;
                    default:
                        origin = null;
                        break;
                }
                if (origin == null) continue;

                var item = Instantiate(origin, this.transform);
                item.transform.position = pos;
            }
        }
    }

    public void GetItem(Vector2Int pos)
    {
        foreach (Transform n in this.transform)
        {
            if (n.position.x == pos.x && n.position.y == -pos.y)
            {
                // 削除
                GameObject.Destroy(n.gameObject);
            }
        }
    }
}
