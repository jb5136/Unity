using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatingText : MonoBehaviour
{
    private float moveSpeed;
    private float destroyTime = 0.0f;
    private float scaleTime = 0.0f;

    public Text text;
     
    private Vector3 vector;

    private Vector3 startPos = Vector3.zero;
    private Vector3 pass = new Vector3(5, 3, 0);
    private Vector3 end = Vector3.zero;
    private float f = 0.1f;

    private Vector3 s = Vector3.zero;
    private Vector3 l = Vector3.zero;

    private Queue<Vector3> movepos = new Queue<Vector3>();
    float time;
    private Vector3 endPos;
    public void SetEndPos(Vector3 pos)
    {
        endPos = pos;
    }
    private void Start()
    {
        time = Time.time;
        startPos = transform.position;
        //end = new Vector3(startPos.x + 1, startPos.y, startPos.z + 1);
        pass = new Vector3((startPos.x + end.x) / 2, (startPos.y + end.y), (startPos.z + end.z) / 2);
        for (int i = 0; i < 5; i++)
        {
            Vector3 vec = BezierQuadratic(f, startPos, pass, end);
            movepos.Enqueue(vec);
        }
    }
    // Update is called once per frame
    void Update()
    {
        float temp = transform.position.y - startPos.y;
        scaleTime += Time.deltaTime;
        destroyTime += Time.deltaTime;

        if (temp <= 80.0f)
        {
            if (destroyTime < 0.4f)
                moveSpeed = 200.0f;
            else
                moveSpeed = 40.0f;

            vector.Set(text.transform.position.x, text.transform.position.y + (moveSpeed * Time.deltaTime), text.transform.position.z);
        }

        text.transform.position = vector;

        if(destroyTime >= 1.3f)
        {
            Destroy(this.gameObject);
        }
        //if (scaleTime <= 0.5f)
        //{
        //    transform.localScale = Vector3.one * (2 + scaleTime);
        //}
    }

    public static Vector3 BezierQuadratic(float t, Vector3 P0, Vector3 P1, Vector3 P2)
    {
        float t2 = (1 - t) * (1 - t);
        return t2 * P0 + 2 * t * (1 - t) * P1 + t * t * P2;
    }
}
