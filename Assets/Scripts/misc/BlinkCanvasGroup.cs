using UnityEngine;

public class BlinkCanvasGroup : MonoBehaviour
{
    public float TurnTime;
    private float _timer;
    public bool Loop;
    public float StartAlpha;
    public float EndAlpha;
    public bool Pingpong;
    public bool StartAwake;
    public CanvasGroup cg;
    private bool _paused;

    void Start()
    {
        if (cg == null)
        {
            cg = GetComponent<CanvasGroup>();
        }
        _paused = true;
        if (StartAwake)
        {
            Play();
        }
    }

    public void Play()
    {
        cg.alpha = StartAlpha;
        _timer = TurnTime;
        _paused = false;
    }

    public void Stop()
    {
        cg.alpha = 0;
        _paused = true;
    }

    void Update()
    {
        if (_paused)
        {
            return;
        }
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            if (Loop)
            {
                Play();
            }
            else
            {
                if (Pingpong)
                {
                    cg.alpha = StartAlpha;
                }
                else
                {
                    cg.alpha = EndAlpha;
                }
                _paused = true;
            }
        }
        else
        {
            var f = _timer / TurnTime;
            var t = 1 - f;
            if (Pingpong)
            {
                t = 1 - Mathf.Abs(1 - 2 * f);
            }
            cg.alpha = Mathf.Lerp(StartAlpha, EndAlpha, t);
        }
    }
}