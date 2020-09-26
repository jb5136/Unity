using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class Joystick : MonoBehaviour, IPointerDownHandler,IPointerUpHandler, IDragHandler 
{
    private Image m_btn;
    private Image m_background;
    private Vector3 m_position = Vector3.zero;
    private Vector3 m_eventPos = Vector3.zero;
    private Vector2 m_dir = Vector3.zero;
    private bool moveCheck = false;

    public Vector2 Dir { get { return m_dir;  } }
    public bool GetmoveCheck { get { return moveCheck; } }
    void Start()
    {
        m_btn = transform.Find("Btn").GetComponent<Image>();
        m_background = GetComponent<Image>();
        m_position = m_btn.transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        m_btn.transform.position = m_position;
        m_dir = Vector2.zero;
        moveCheck = false;
    }

    public void OnDrag(PointerEventData eventData){
        moveCheck = true;
        Vector2 localPosition = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(m_background.rectTransform, eventData.position,eventData.pressEventCamera,out localPosition );
        if( localPosition.magnitude > 40 )
        {
            m_dir = localPosition.normalized;
            localPosition.Normalize();
            m_btn.transform.localPosition = m_dir * 40;
        }
        else
        {
            m_btn.transform.localPosition = localPosition;
            m_dir = localPosition.normalized;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
