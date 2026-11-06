using UnityEngine;
using UnityEngine.EventSystems;


public class PlacementManager : MonoBehaviour
{
    // 道具下拉選單, 從它接收拖曳事件
    [SerializeField] private PropDropdown dropdown;

    // 用來把滑鼠位置轉成射線的相機, 留空則自動用 MainCamera
    [SerializeField] private Camera targetCamera;

    // 射線只會打這個 Layer, 請勾選 Ground
    [SerializeField] private LayerMask groundMask;

    // 目前拖曳中的預覽物件, 沒在拖曳時是 null
    private GameObject ghost;

    private void Awake()
    {
        // 沒手動指定相機就用場景裡的 MainCamera
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    // 啟用時, 告訴下拉選單: 有拖曳事件請通知我
    private void OnEnable()
    {
        dropdown.DragBegan += OnDragBegan;
        dropdown.Dragging += OnDragging;
        dropdown.DragEnded += OnDragEnded;
    }

    // 停用時取消訂閱, 避免物件消失後還被呼叫
    private void OnDisable()
    {
        dropdown.DragBegan -= OnDragBegan;
        dropdown.Dragging -= OnDragging;
        dropdown.DragEnded -= OnDragEnded;
    }

    // 第一步: 開始拖曳 -> 生成預覽, 先隱藏
    private void OnDragBegan(Proplist data, PointerEventData e)
    {
        if (data == null || data.Prefab == null) return;

        ghost = Instantiate(data.Prefab);
        ghost.SetActive(false);
    }

    // 第二步: 拖曳中 -> 有效落點就顯示並移過去, 否則隱藏
    private void OnDragging(Proplist data, PointerEventData e)
    {
        if (ghost == null) return;

        if (TryGetGroundPoint(e, out Vector3 point))
        {
            ghost.transform.position = point;
            ghost.SetActive(true);
        }
        else
        {
            ghost.SetActive(false);
        }
    }

    // 第三步: 放開 -> 預覽一律刪除, 落點有效才生成正式道具
    private void OnDragEnded(Proplist data, PointerEventData e)
    {
        if (ghost != null)
            Destroy(ghost);

        // 落在 UI 上或沒打到地形: 直接結束, 什麼都不生成
        if (!TryGetGroundPoint(e, out Vector3 point)) return;

        GameObject obj = Instantiate(data.Prefab, point, Quaternion.identity);

        // 掛上 PlaceableProp 並記錄資料, 步驟 5 的選取與刪除會用到
        PlaceableProp prop = obj.GetComponent<PlaceableProp>();
        if (prop == null) prop = obj.AddComponent<PlaceableProp>();
        prop.Initialize(data);
    }

    /// <summary>
    /// 判斷滑鼠位置是不是有效落點, 有效就把地形上的座標放進 point。
    /// 有效 = 滑鼠不在 UI 上, 而且射線打到 Ground。
    /// </summary>
    private bool TryGetGroundPoint(PointerEventData e, out Vector3 point)
    {
        point = Vector3.zero;

        // 滑鼠在 UI 上: 無效
        if (EventSystem.current.IsPointerOverGameObject())
            return false;

        // 從相機透過滑鼠位置往場景發射一條射線
        Ray ray = targetCamera.ScreenPointToRay(e.position);

        // 只檢查 Ground, 打到就取得碰撞點
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask))
        {
            point = hit.point;
            return true;
        }

        return false;
    }
}