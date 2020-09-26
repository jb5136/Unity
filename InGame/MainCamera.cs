using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainCamera : MonoBehaviour
{
    public static MainCamera instance;

    GameObject cameraTarget;
    public float rotateSpeed = 25.0f;
    float rotate = 0.0f;
    public float offsetDistance = 6.0f;
    public float offsetHeight = 4.0f;
    public float smoothing;
    public Vector3 offset;
    Vector3 lastPosition;

    public float xSpeed = 220.0f;
    public float ySpeed = 100.0f;
    public float x = 0.0f;
    public float y = 0.0f;

    public Quaternion rotation;

    private bool cameraRotationCheck = false;
    private bool cameraMoveCheck = false;
    //camera Shake
    [SerializeField, Range(0, 1)]
    private float m_shakeAmount = 0.2f;
    private float shakeTime = 0.2f;
    Vector3 initialPosition;
    [SerializeField]
    private bool isShake = false;
    private bool isPositionCheck = false;

    //Boss
    private GameObject m_boss;
    public bool isAppearBoss = false;
    private Transform m_appearBossCameraPos;

    private Button m_cameraRotateLeft;
    private Button m_cameraRotateRight;
    private bool isLeft = false;
    private bool isRight = false;

    public bool isSceneMove = false;
    public bool SetIsShake
    {
        set
        {
            isShake = value;
        }
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        cameraTarget = GameObject.FindGameObjectWithTag("Player");
        lastPosition = new Vector3(cameraTarget.transform.position.x, cameraTarget.transform.position.y + offsetHeight, cameraTarget.transform.position.z - offsetDistance);


        xAngle = 0;
        yAngle = 50;
        //transform.rotation = Quaternion.Euler(yAngle, xAngle, 0);

        m_cameraRotateLeft = GameObject.Find("Canvas").transform.Find("UI/CameraRotate/Left").GetComponent<Button>();
        m_cameraRotateRight = GameObject.Find("Canvas").transform.Find("UI/CameraRotate/Right").GetComponent<Button>();
        m_cameraRotateLeft.onClick.AddListener(RotateLeft);
        m_cameraRotateRight.onClick.AddListener(RotateRight);

        //transform.parent.position = cameraTarget.transform.position;
        //rotation = Quaternion.Euler(0, -90, 0);
        offset = new Vector3(0.0f, offsetHeight,- offsetDistance);
        //offset = Quaternion.AngleAxis(rotate * rotateSpeed, Vector3.up) * offset;
        //transform.position = cameraTarget.transform.position + Vector3.back;
        //transform.position = new Vector3(0, 5, -offsetDistance) + cameraTarget.transform.position;
        //transform.LookAt(cameraTarget.transform);
        //transform.position = cameraTarget.transform.position + offset;
        //transform.position = new Vector3((cameraTarget.transform.position.x + offset.x), (cameraTarget.transform.position.y + offset.y),           (cameraTarget.transform.position.z + offset.z));
    }
    private void RotateLeft()
    {
        isLeft = true;
        rotate = -1.0f;
    }
    private void RotateRight()
    {
        isRight = true;
        rotate = 1.0f;
    }
    private Vector3 m_oriPos;
    Vector3 FirstPoint;
    Vector3 SecondPoint;
    float xAngle;
    float yAngle;
    float xAngleTemp;
    float yAngleTemp;
    void Update()
    {
        if (isAppearBoss)
        {
            m_boss = GameObject.FindGameObjectWithTag("Boss");
            m_appearBossCameraPos = GameObject.Find("AppearBossCameraPos").transform;

            m_oriPos = transform.position;
            transform.position = m_appearBossCameraPos.position;
            transform.LookAt(m_boss.transform.position);
        }
        else
        {
            if (isShake)
            {
                if (!isPositionCheck)
                {
                    initialPosition = transform.position;
                    isPositionCheck = true;
                }
                if (shakeTime > 0)
                {
                    transform.position = Random.insideUnitSphere * m_shakeAmount + initialPosition;
                    shakeTime -= Time.deltaTime;
                }
                else
                    isShake = false;
            }
            else
            {
                shakeTime = 0.3f;
                //transform.position = initialPosition;
                isPositionCheck = false;
#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR
            //Touch touch = Input.GetTouch(0);
            //if(!EventSystem.current.IsPointerOverGameObject(touch.fingerId) && Input.touchCount > 0)
            //{
            //    if (Input.GetTouch(0).phase == TouchPhase.Began)
            //    {
            //        FirstPoint = Input.GetTouch(0).position;
            //        xAngleTemp = xAngle;
            //        yAngleTemp = yAngle;
            //    }
            //    if (Input.GetTouch(0).phase == TouchPhase.Moved)
            //    {
            //        SecondPoint = Input.GetTouch(0).position;
            //        xAngle = xAngleTemp + (SecondPoint.x - FirstPoint.x) * 180 / Screen.width;
            //        yAngle = yAngleTemp - (SecondPoint.y - FirstPoint.y) * 90 * 3f / Screen.height; // Y값 변화가 좀 느려서 3배 곱해줌.
 
            //        // 회전값을 40~85로 제한
            //        if (yAngle < 40f)
            //            yAngle = 40f;
            //        if (yAngle > 85f)
            //            yAngle = 85f;
 
            //        transform.rotation = Quaternion.Euler(yAngle, xAngle, 0.0f);
            //    }
                
            // }
            //else
            //{
                
            //}
                if (isLeft)
                {
                    rotate = -2.0f;
                    isLeft = false;
                }
                else if (isRight)
                {
                    rotate = 2.0f;
                    isRight = false;
                }
                else
                    rotate = 0.0f;
                if (!isSceneMove)
                {
                    offset = Quaternion.AngleAxis(rotate * rotateSpeed, Vector3.up) * offset;
                    transform.position = cameraTarget.transform.position + offset;
                    transform.position = new Vector3(Mathf.Lerp(lastPosition.x, cameraTarget.transform.position.x + offset.x, smoothing * Time.deltaTime),
                        Mathf.Lerp(lastPosition.y, cameraTarget.transform.position.y + offset.y, smoothing * Time.deltaTime),
                        Mathf.Lerp(lastPosition.z, cameraTarget.transform.position.z + offset.z, smoothing * Time.deltaTime));
                    transform.LookAt(cameraTarget.transform.position);
                }
                //Vector3 position = rotation * new Vector3(0, 5, -offsetDistance) + cameraTarget.transform.position;
                //    if (position.y < 0.0f)
                //        position.y = 0.0f;
                //    transform.position = Vector3.Lerp(transform.position, position, smoothing * Time.deltaTime);


                //if (Input.touchCount > 0)
                //{
                //    if (Input.GetTouch(0).phase == TouchPhase.Began || Input.GetTouch(0).phase == TouchPhase.Moved)
                //    {
                //        if (!EventSystem.current.IsPointerOverGameObject())
                //            cameraRotationCheck = true;
                //        else if (EventSystem.current.IsPointerOverGameObject())
                //            cameraMoveCheck = true;
                //    }
                //    if (Input.GetMouseButtonUp(0))
                //    {
                //        cameraRotationCheck = false;
                //        //x = 0.0f;
                //        //y = 0.0f;
                //    }
                //}
#else
                if (Input.GetMouseButtonDown(0))
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                        cameraRotationCheck = true;
                    else if (EventSystem.current.IsPointerOverGameObject())
                        cameraMoveCheck = true;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    cameraRotationCheck = false;
                    //x = 0.0f;
                    //y = 0.0f;
                }
                if (isLeft)
                {
                    rotate = -2.0f;
                    isLeft = false;
                }
                else if (isRight)
                {
                    rotate = 2.0f;
                    isRight = false;
                }
                else
                    rotate = 0.0f;

                if (!isSceneMove)
                {
                    offset = Quaternion.AngleAxis(rotate * rotateSpeed, Vector3.up) * offset;
                    transform.position = new Vector3(Mathf.Lerp(lastPosition.x, cameraTarget.transform.position.x + offset.x, smoothing * Time.deltaTime),
                        Mathf.Lerp(lastPosition.y, cameraTarget.transform.position.y + offset.y, smoothing * Time.deltaTime),
                        Mathf.Lerp(lastPosition.z, cameraTarget.transform.position.z + offset.z, smoothing * Time.deltaTime));
                }
                if (cameraRotationCheck)
                {
                    //CameraRotation();
                }
                else
                {
                    //if (!test)
                    //{
                    //    offset = Quaternion.AngleAxis(rotate * rotateSpeed, Vector3.up) * offset;
                    //    transform.position = cameraTarget.transform.position + offset;
                    //    transform.position = new Vector3(Mathf.Lerp(lastPosition.x, cameraTarget.transform.position.x + offset.x, smoothing * Time.deltaTime),
                    //        Mathf.Lerp(lastPosition.y, cameraTarget.transform.position.y + offset.y, smoothing * Time.deltaTime),
                    //        Mathf.Lerp(lastPosition.z, cameraTarget.transform.position.z + offset.z, smoothing * Time.deltaTime));
                    //}
                    //else
                    //{
                    //}
                    
                    //Vector3 position = rotation * new Vector3(0, 5, -offsetDistance) + cameraTarget.transform.position;
                    //if (position.y < 0.0f)
                    //    position.y = 0.0f;
                    //transform.position = Vector3.Lerp(transform.position, position, smoothing * Time.deltaTime);
                }
                transform.LookAt(cameraTarget.transform.position);
#endif
            }

        }
    }
    void LateUpdate()
    {
        lastPosition = transform.position;
    }
    private void CameraRotation()
    {

        x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

        rotation = Quaternion.Euler(y, x, 0);
        Vector3 position = rotation * new Vector3(0, 5, -offsetDistance) + cameraTarget.transform.position;
        if (position.y < 0.0f)
            position.y = 0.0f;
        transform.position = position;
    }
}