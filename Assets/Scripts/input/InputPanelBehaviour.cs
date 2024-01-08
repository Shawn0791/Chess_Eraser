using UnityEngine.EventSystems;
using UnityEngine;


public class InputPanelBehaviour : UIBehaviour, IEventSystemHandler, IPointerClickHandler
{
    public static InputPanelBehaviour instance { get; private set; }
    public RectTransform canvasTrans;

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        InputService.instance.InputPanelClick(eventData);
    }
}