using UnityEngine;

[CreateAssetMenu(fileName = "NewProp", menuName = "PropPlacement/Prop list")]
public class Proplist : ScriptableObject
{
    // 顯示在清單上的名稱
    [SerializeField] private string displayName = "New Prop";

    // 要生成的道具 Prefab(Pivot 在底部中心)
    [SerializeField] private GameObject prefab;

    // 清單項目上的縮圖,可留空(之後可用文字代替)
    [SerializeField] private Sprite icon;

    // 對外唯讀屬性:其他腳本只能讀,不能亂改資料
    public string DisplayName => displayName;
    public GameObject Prefab => prefab;
    public Sprite Icon => icon;
}
