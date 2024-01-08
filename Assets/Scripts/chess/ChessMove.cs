using UnityEngine;
using System.Collections;

public class ChessMove : MonoBehaviour
{
    private float _timer;
    private float _time;
    private bool _isMovingAlongAxis;
    private bool _isMovingAlongRing;
    private Vector3 _posStart;
    private Vector3 _posEnd;
    private float _eularStart;
    private float _eularEnd;
    private float _radius;
    private bool _cw;
    private System.Action _endAction;

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Stop()
    {
        _timer = 0;
        _time = 0;
        _isMovingAlongAxis = false;
        _isMovingAlongRing = false;
        _endAction = null;
    }

    private void Move()
    {
        var cfg = GameService.instance.gameConfig;
        if (_isMovingAlongAxis)
        {
            _timer -= Time.deltaTime;
            var timeRatio = _timer / _time;
            if (timeRatio <= 0)
            {
                timeRatio = 0;
                _isMovingAlongAxis = false;
            }
            var timeRatioScaled = cfg.ac.Evaluate(1 - timeRatio);
            transform.position = Vector3.Lerp(_posStart, _posEnd, timeRatioScaled);

            if (timeRatio == 0)
            {
                _endAction?.Invoke();
            }
        }

        if (_isMovingAlongRing)
        {
            _timer -= Time.deltaTime;
            var timeRatio = _timer / _time;
            if (timeRatio <= 0)
            {
                timeRatio = 0;
                _isMovingAlongRing = false;
            }

            var timeRatioScaled = cfg.ac.Evaluate(1 - timeRatio);
            var eular = Mathf.Lerp(_eularStart, _eularEnd, timeRatioScaled);
            //Debug.Log("eular " + eular);
            var r = eular * Mathf.Deg2Rad;
            var x = Mathf.Sin(r);
            var z = Mathf.Cos(r);

            var centerPos = BoardService.instance.roundCenter.position;
            transform.position = centerPos + new Vector3(x * _radius, 0, z * _radius);

            if (timeRatio == 0)
            {
                _endAction?.Invoke();
            }
        }
    }

    public void MoveAlongRing(Vector3 posStart, Vector3 posEnd, bool cw, float time, System.Action onComplete)
    {
        _time = time;
        _timer = time;
        _isMovingAlongAxis = false;
        _isMovingAlongRing = true;
        _posStart = posStart;
        _posEnd = posEnd;
        _cw = cw;
        _endAction = onComplete;

        Debug.Log("MoveAlongRing");
        Debug.Log(cw);
        var cfg = GameService.instance.gameConfig;
        var centerPos = BoardService.instance.roundCenter.position;
        var r = centerPos - _posStart;
        r.y = 0;
        _radius = r.magnitude;

        var deltaPos1 = _posStart - centerPos;
        deltaPos1.y = 0;
        var rotation1 = Quaternion.LookRotation(deltaPos1, Vector3.up);

        var deltaPos2 = _posEnd - centerPos;
        deltaPos2.y = 0;
        var rotation2 = Quaternion.LookRotation(deltaPos2, Vector3.up);

        _eularStart = rotation1.eulerAngles.y;
        _eularEnd = rotation2.eulerAngles.y;
        if (cw && _eularEnd < _eularStart)
        {
            _eularEnd += 360;
        }
        if (!cw && _eularEnd > _eularStart)
        {
            _eularStart += 360;
        }
        Debug.Log("_eularStart " + _eularStart);
        Debug.Log("_eularEnd " + _eularEnd);
    }

    public void MoveAlongAxis(Vector3 posStart, Vector3 posEnd, float time, System.Action onComplete)
    {
        _time = time;
        _timer = time;
        _isMovingAlongAxis = true;
        _isMovingAlongRing = false;
        _posStart = posStart;
        _posEnd = posEnd;
        _endAction = onComplete;
    }
}
