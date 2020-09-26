using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keybowd : MonoBehaviour
{
    public InputField InputText;
    public void Text(Text text)
    {
        text.text = InputText.text;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            
        }
    }
}
