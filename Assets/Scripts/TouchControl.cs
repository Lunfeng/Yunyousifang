using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchControl : MonoBehaviour {

    public bool isTracking = false;
    public GameObject main;
    public GameObject center;
    public float screenWidth;
    public float screenHight;
    public float scale = 1f;
    public int viewMode = 1;
    float dis;
    private Touch nullTouch;
    public Vector2 startPos;

    private Touch oldTouch1;  //上次触摸点1(手指1)  
    private Touch oldTouch2;  //上次触摸点2(手指2)  

    float velX = 0;
    float velY = 0;

    // Use this for initialization
    void Start () {
        main = GameObject.Find("main");
        screenHight = Screen.height;
        screenWidth = Screen.width;
        dis = (transform.position - center.transform.position).magnitude;
        nullTouch = new Touch();
        Input.multiTouchEnabled = true;
        center.transform.eulerAngles = transform.eulerAngles;
    }

    void Update()
    {
        switch (viewMode)
        {
            case 1:
                mode1();
                break;
            case 2:
                mode2();
                break;
        }
    }

    public void setMode(int mode)
    {
        viewMode = mode;
    }

    void mode1()
    {
        if (!DefaultTrackableEventHandler.status)
        {
            main.transform.localScale = Vector3.one * 15;
            if (Input.touchCount == 1)// && Input.GetTouch(0).phase == TouchPhase.Moved
            {
                Touch touch = Input.GetTouch(0);
                Vector3 angle = transform.eulerAngles + new Vector3(-touch.deltaPosition.y / 5, touch.deltaPosition.x / 5, 0);
                angle.z = 0;
                angle.x = Mathf.Clamp(angle.x, 0f, 88f);
                //angle.x = angle.x < 90 ? Mathf.Clamp(angle.x, -10f, 88f) : Mathf.Clamp(angle.x, 272f, 366f);
                Debug.Log("AngleX: " + angle.x);
                transform.eulerAngles = angle;
            }
            else if(Input.touchCount >= 1)
            {
                //多点触摸, 放大缩小  
                Touch newTouch1 = Input.GetTouch(0);
                Touch newTouch2 = Input.GetTouch(1);

                //第2点刚开始接触屏幕, 只记录，不做处理
                if (newTouch2.phase == TouchPhase.Began)
                {
                    oldTouch2 = newTouch2;
                    oldTouch1 = newTouch1;
                    return;
                }

                //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型  
                float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
                float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

                //两个距离之差，为正表示放大手势， 为负表示缩小手势  
                float offset = newDistance - oldDistance;

                //放大因子， 一个像素按 0.01倍来算(100可调整)  
                float scaleFactor = offset / 2000f;
                Vector3 localScale = main.transform.localScale;
                scale -= scaleFactor;

                //记住最新的触摸点，下次使用  
                oldTouch1 = newTouch1;

                oldTouch2 = newTouch2;
            }
            transform.position = center.transform.position - transform.forward * dis * scale;
        }
        else
        {
            dis = (transform.position - center.transform.position).magnitude;
        }
    }

    // Update is called once per frame
    void mode2()
    {
        //Input.simulateMouseWithTouches = true;
        if (!DefaultTrackableEventHandler.status)
        {
            Touch leftTouch = nullTouch;
            Touch rightTouch = nullTouch;

            for (int i = 0; i < Input.touches.Length; i++)
            {
                if (Input.touches[i].position.x < screenWidth / 2)
                {
                    leftTouch = Input.touches[i];
                    if (leftTouch.phase == TouchPhase.Began)
                    {
                        startPos = leftTouch.position;
                        break;
                    }
                    else
                    {
                        var deltaposition = leftTouch.position - startPos;
                        transform.Translate((transform.forward * deltaposition.y + transform.right * deltaposition.x) * 0.04f, Space.World);
                        Debug.Log("l: " + leftTouch.position);
                        break;
                    }
                }
            }

            for (int i = 0; i < Input.touches.Length; i++)
            {
                if (Input.touches[i].position.x > screenWidth / 2)
                {
                    rightTouch = Input.touches[i];
                    var deltaposition = rightTouch.deltaPosition;
                    transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
                    transform.eulerAngles += new Vector3(-deltaposition.y / 10, deltaposition.x / 10, 0f);
                    Debug.Log("r: " + rightTouch.position);
                    break;
                }
            }


            //StopAllCoroutines();
            //StartCoroutine(lookAt(main));
            //transform.LookAt(main.transform);
            //没有触摸  
            /*
            if (Input.touchCount <= 0)
            {
                return;
            }

            //单点触摸， 水平上下移动
            if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                var deltaposition = Input.GetTouch(0).deltaPosition;
                Debug.Log(Input.GetTouch(0).position);
                //transform.Rotate(new Vector3(-deltaposition.y * Time.deltaTime, 0f, deltaposition.x * Time.deltaTime));
                //Vector3 forward = transform.forward * deltaposition.y * 0.1f;
                //forward.x = 0;
                //Vector3 right = transform.up * deltaposition.x * 0.1f;
                //transform.Translate(forward+right);
                //transform.Translate(deltaposition.y * 0.1f, 0f, deltaposition.x * 0.1f, Space.World);
                transform.Translate((transform.forward * deltaposition.y + transform.right * deltaposition.x) * 0.1f, Space.World);
                
                //transform.eulerAngles = new Vector3(-deltaposition.y * 0.1f, 0f, deltaposition.x * 0.1f);
            }
            //单点触摸， 水平上下旋转  
            if (Input.touchCount == 3)
            {
                //Touch touch = Input.GetTouch(0);-deltaposition.y * 0.1f, 0f, deltaposition.x * 0.1f, Space.World
               //Vector2 deltaPos = touch.deltaPosition;
                //transform.Rotate(Vector3.down * deltaPos.x * 0.1f, Space.World);
                //transform.Rotate(Vector3.right * deltaPos.y * 0.1f, Space.World);
            }
            */
        }
    }

    IEnumerator lookAt(GameObject obj)
    {
        float velX = 0;
        float velY = 0;
        while (true)
        {
            Vector3 dir = (transform.position - obj.transform.position);
            Vector2 flatDir = new Vector2(dir.x, dir.z);
            float routeX = Mathf.Rad2Deg * Mathf.Atan2(dir.y, flatDir.magnitude);
            float routeY = Mathf.Rad2Deg * Mathf.Atan(dir.x / dir.z);
            routeY = (dir.x < 0 && dir.z > 0) ? (routeY + 180f) : routeY;
            routeY = (dir.x > 0 && dir.z > 0) ? (routeY - 180f) : routeY;
            float x = Mathf.SmoothDampAngle(transform.eulerAngles.x, routeX, ref velX, 0.1f);
            float y = Mathf.SmoothDampAngle(transform.eulerAngles.y, routeY, ref velY, 0.1f);
            transform.eulerAngles = new Vector3(x, y, 0f);
            yield return new WaitForFixedUpdate();
        }
    }

}
