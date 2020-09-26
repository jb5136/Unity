using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilHelper
{
    // 입력받은 transform의 하위 오브젝트를 찾을 때 사용하는 함수입니다.
    public static T Find<T>(Transform transform,
                            string path,
                            bool callInit = false)
                            where T : UnityEngine.Component
    {
        if (transform == null) return null;
        Transform findObj = transform.Find(path);
        if (findObj != null)
        {
            // callInit이 true일경우 Init 함수를 호출하도록 처리합니다.
            if (callInit)
                findObj.SendMessage("Init",
                    SendMessageOptions.DontRequireReceiver);

            return findObj.GetComponent<T>();
        }

        return null;
    }

    // 따로 함수를 만든 이유 : 편의성을 제공하기 위해서입니다.
    // 아래의 함수를 사용하게 되면, 같은 계층에 붙어 있는 타입이 다른 컴포넌트를
    // 얻어올 수 있게 됩니다.
    public static T FindObjectWithTag<T>(string tag)
        where T : UnityEngine.Component
    {
        GameObject obj = GameObject.FindGameObjectWithTag(tag);
        if (obj == null)
            return null;

        return obj.GetComponent<T>();
    }

    public static T Instantiate<T>(string path, Vector3 pos, Transform parent,
                                    Quaternion rot) where T : Component
    {
        T t = Resources.Load<T>(path);
        if (t != null)
        {
            t = GameObject.Instantiate(t, pos, rot, parent);
            Transform tr = t.GetComponent<Transform>();
            tr.localScale = Vector3.one;
            tr.localPosition = Vector3.zero;
            tr.localRotation = Quaternion.identity;
        }
        

        return t;
    }

}
