using UnityEngine;

public class GridPosition : MonoBehaviour
{
    [SerializeField] private int _x;
    [SerializeField] private int _y;

    private void OnMouseDown() {
        Debug.Log(_x + ", "+ _y);
        GameManager.Instance.ClickedOnGridPositionRpc(_x, _y, GameManager.Instance.GetLocalPlayerType());
    }
}
