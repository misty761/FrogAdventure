using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // 박스 컬라이더 영역의 최소 초대값
    public BoxCollider2D bound;
    // bound
    Vector3 minBound;
    Vector3 maxBound;
    // player
    PlayerControl target;                       // 카메라가 따라갈 대상
    Vector3 targetPosition;                     // 대상의 현재 위치
    // 카메라의 반넓이와 반높이 값 변수
    float halfWidth;
    float halfHeight;
    float moveSpeed = 100f;                     // 카메라가 따라갈 속도
    // 반 높이를 구하기 위해 필요한 카메라 변수
    Camera theCamera;
    // position X of Camera
    float posCameraX;
    public float clampedX;
    float clampedY;
    bool bStart;
    public float clampedXInit;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<PlayerControl>();
        theCamera = GetComponent<Camera>();
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
        halfHeight = theCamera.orthographicSize;
        halfWidth = halfHeight * Screen.width / Screen.height;
        posCameraX = transform.position.x;
        bStart = false;
    }

    // Update is called once per frame
    void Update()
    {
        targetPosition.Set(target.transform.position.x, target.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, moveSpeed * Time.deltaTime);
        // position X of Camera
        posCameraX = transform.position.x;
        // camera only go forward
        if (target.transform.position.x >= posCameraX)
        {
            clampedX = Mathf.Clamp(transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
        }
        //clampedX = Mathf.Clamp(transform.position.x, minBound.x + halfWidth, maxBound.x - halfWidth);
        clampedY = Mathf.Clamp(transform.position.y, minBound.y + halfHeight, maxBound.y - halfHeight);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);

        if (!bStart)
        {
            bStart = true;
            clampedXInit = clampedX;
        }

        //print("unity Camera X : " + posCameraX);
        //print("unity Player X : " + target.transform.position.x);
        //print("unity clampedX : " + clampedX);
        //print("unity clampedY : " + clampedY);
    }

    public void SetBound(BoxCollider2D newBound)
    {
        bound = newBound;
        minBound = bound.bounds.min;
        maxBound = bound.bounds.max;
    }
}
