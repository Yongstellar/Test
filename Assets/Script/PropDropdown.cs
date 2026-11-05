using UnityEngine;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PropDropdown : MonoBehaviour
{
    [Header("UI References")]
    // 標題按鈕:點擊切換展開/收合
    [SerializeField] private Button toggleButton;

    // 標題按鈕上的文字,用來顯示「Props [+] / [-]」
    [SerializeField] private TMP_Text titleLabel;

    // 清單面板:預設隱藏,展開時顯示。清單項目會產生在它底下
    [SerializeField] private RectTransform listPanel;

    // 單一清單項目的 Prefab(掛有 PropListItem)
    [SerializeField] private PropListItem itemPrefab;

    [Header("Data")]
    // 清單要顯示的道具資料,把六個 PropDefinition 拖進來
    [SerializeField] private List<Proplist> props = new List<Proplist>();

    [Header("Debug")]
    // 勾選後會在 Console 印出拖曳事件,步驟 3 用來驗證;步驟 4 完成後可關閉
    [SerializeField] private bool logEvents = true;

    // 轉發給外部(放置系統)的事件
    public event Action<Proplist, PointerEventData> DragBegan;
    public event Action<Proplist, PointerEventData> Dragging;
    public event Action<Proplist, PointerEventData> DragEnded;

    // 目前是否展開
    public bool IsExpanded => listPanel != null && listPanel.gameObject.activeSelf;

    private void Awake()
    {
        BuildItems();
        SetExpanded(false); // 一開始收合,點了才展開(任務需求)
        toggleButton.onClick.AddListener(Toggle);
    }
    private void BuildItems()
    {
        foreach (var data in props)
        {
            if (data == null) continue; // 略過沒填的空欄位,避免報錯

            PropListItem item = Instantiate(itemPrefab, listPanel);
            item.name = "Item_" + data.DisplayName;
            item.Setup(data);

            item.DragBegan += OnItemDragBegan;
            item.Dragging += OnItemDragging;
            item.DragEnded += OnItemDragEnded;
        }
    }

    /// <summary>切換展開/收合。</summary>
    public void Toggle()
    {
        SetExpanded(!IsExpanded);
    }

    /// <summary>收合清單(放置系統需要時可呼叫)。</summary>
    public void Collapse()
    {
        SetExpanded(false);
    }

    /// <summary>顯示或隱藏清單面板,同時更新標題文字。</summary>
    private void SetExpanded(bool expanded)
    {
        listPanel.gameObject.SetActive(expanded);

        if (titleLabel != null)
            titleLabel.text = expanded ? "Props [-]" : "Props [+]";
    }

    // 以下三個方法只是把項目的事件原封不動轉發出去
    private void OnItemDragBegan(Proplist data, PointerEventData e)
    {
        if (logEvents) Debug.Log($"[Dropdown] Drag began: {data.DisplayName}");
        DragBegan?.Invoke(data, e);
    }

    private void OnItemDragging(Proplist data, PointerEventData e)
    {
        Dragging?.Invoke(data, e);
    }

    private void OnItemDragEnded(Proplist data, PointerEventData e)
    {
        if (logEvents) Debug.Log($"[Dropdown] Drag ended: {data.DisplayName} at {e.position}");
        DragEnded?.Invoke(data, e);
    }
}
