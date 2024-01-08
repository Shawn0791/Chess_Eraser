using UnityEngine;
using UnityEngine.EventSystems;

public class InputService : MonoBehaviour
{
    public static InputService instance;

    private void Awake()
    {
        instance = this;
    }

    public void InputPanelClick(PointerEventData eventData)
    {
        var chess = GetPointerChess(eventData);
        if (chess != null)
        {
            chess.OnClick();
            return;
        }
    }

    private SlotBehaviour GetPointerChess(PointerEventData eventData)
    {
        RaycastHit raycastHit;
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);

        if (Physics.Raycast(ray, out raycastHit, 100f))
        {
            if (raycastHit.transform != null)
            {
                var go = raycastHit.transform.gameObject;
                foreach (var instance in SlotBehaviour.instances)
                {
                    if (go.transform == instance.transform)
                    {
                        return instance;
                    }
                }
            }
        }

        return null;
    }

    public bool canShowInputPanel
    {
        get
        {
            if (isPlayingAnimation())
            {
                return false;
            }
            if (isGameEnd())
            {
                return false;
            }
            return true;
        }
    }

    public bool canInput
    {
        get
        {
            if (isPlayingAnimation())
            {
                return false;
            }
            if (isGameEnd())
            {
                return false;
            }
            return true;
        }
    }

    private bool isPlayingAnimation()
    {
        return false;
    }

    private bool isGameEnd()
    {
        return false;
    }
}
