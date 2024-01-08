using UnityEngine;

public class BlinkTipBehaviour : MonoBehaviour
{
    //DarkSouls like  tip
    public CanvasGroup Cg;
    public BlinkCanvasGroup Bcg;
    public UnityEngine.UI.Text text;

    public void Show(string s)
    {
        text.text = s;
        Cg.alpha = 1;
        Bcg.enabled = true;
    }

    public void Hide()
    {
        Cg.alpha = 0;
        Bcg.enabled = false;
    }
}
