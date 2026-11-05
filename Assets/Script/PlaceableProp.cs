using UnityEngine;

public class PlaceableProp : MonoBehaviour
{
    // 此道具所用的資料定義,生成時由放置系統填入(可用於顯示名稱等)
    public Proplist Data { get; private set; }

    /// <summary>
    /// 放置系統生成道具後呼叫,記錄它是由哪個 Proplist 產生的。
    /// </summary>
    public void Initialize(Proplist data)
    {
        Data = data;
    }

    /// <summary>
    /// 刪除這個道具。集中在這裡,之後若要加音效或動畫,只改這一處。
    /// </summary>
    public void Remove()
    {
        Destroy(gameObject);
    }
}
