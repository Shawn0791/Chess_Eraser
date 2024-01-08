using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControler : MonoBehaviour
{
    private float origanRotateX;
    private float origanTranX;
    private float targetRotateX;
    private float targetTranX;

    private float rand1;
    private float rand2;
    private float lastRotateX;
    private float timer = 0.3f;

    public float rotateSpeed;
    public float rotate1;
    public float rotate2;
    public float horizontalSpeed;
    public float horizontal1;
    public float horizontal2;

    void Start()
    {
        origanRotateX = targetRotateX = transform.rotation.eulerAngles.x;
        origanTranX = targetTranX = transform.position.x;
    }


    void Update()
    {
        SwayCamera();
        ChangeRotateTarget();
        ChangeTransformTarget();
    }

    private void SwayCamera()
    {
        //rotation的X变化
        transform.rotation = Quaternion.RotateTowards(transform.rotation, 
            Quaternion.Euler(targetRotateX, 0, 0), Time.deltaTime * rotateSpeed);
        //transform的X变化
        transform.position = new Vector3(Mathf.MoveTowards(transform.position.x,
            targetTranX, Time.deltaTime * horizontalSpeed), transform.position.y, transform.position.z);
    }

    private void ChangeRotateTarget()
    {
        //x = sin(Y/2)sin(Z/2)cos(X/2)+cos(Y/2)cos(Z/2)sin(X/2)

        //w = cos(Y/2)cos(Z/2)cos(X/2)-sin(Y/2)sin(Z/2)sin(X/2)
        //if (Mathf.Abs(transform.rotation.eulerAngles.x - targetRotateX) < 0.1f) 
        //{
        //    rand1 = Random.Range(-2f, 2f);
        //    targetRotateX = origanRotateX + rand1;
        //}

        timer -= Time.deltaTime;
        var rotateX = transform.rotation.eulerAngles.x;
        //每0.3s检测一次镜头rotateX是否滞留
        if(timer <= 0)
        {
            if (lastRotateX != rotateX)
            {
                lastRotateX = rotateX;
            }
            else
            {
                rand1 = Random.Range(rotate1, rotate2);
                targetRotateX = origanRotateX + rand1;
            }

            timer = 0.3f;
        }
    }

    private void ChangeTransformTarget()
    {
        if (Mathf.Approximately(transform.position.x, targetTranX))
        {
            rand2 = Random.Range(horizontal1, horizontal2);
            targetTranX = origanTranX + rand2;
        }
    }
}
