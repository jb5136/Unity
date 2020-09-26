using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class CSSecneMng : MonoBehaviour
{
    private GameObject m_camera;

    private Transform m_startPos;
    private Transform m_endPos;
    private Transform m_lookPos;

    private Transform m_CharacSelection;

    private float m_cameramoveTime = 0.0f;

    private bool isBack = false;

    //UI
    private GameObject m_selectUI;
    private GameObject m_characInfo;
    //charInfo UI
    private Button m_backBtn;
    private Text m_job;
    private Button m_charCreateBtn;
    private Text m_inputField;

    //test
    private GameObject m1;
    private GameObject m2;
    private GameObject m3;

    void Start()
    {
        m_camera = GameObject.FindGameObjectWithTag("MainCamera");

        GameObject g = GameObject.Find("Canvas");
        if(g != null)
        {
            m_selectUI = g.transform.Find("CharacterSelecetUI").gameObject;
            m_selectUI.SetActive(false);
            m_characInfo = g.transform.Find("CharacterInfo").gameObject;
            m_characInfo.SetActive(false);
        }

        GameObject t = GameObject.Find("Position");
        if(t != null)
        {
            m_startPos = t.transform.Find("StartPos");
            m_endPos = t.transform.Find("EndPos");
            m_lookPos = t.transform.Find("Pos");
        }

        m_backBtn = m_characInfo.transform.Find("Back").GetComponent<Button>();
        m_backBtn.onClick.AddListener(BackBtn);
        m_job = m_characInfo.transform.Find("CharJob/Text").GetComponent<Text>();
        m_inputField = m_characInfo.transform.Find("InputField/Text").GetComponent<Text>();
        m_charCreateBtn = m_characInfo.transform.Find("Create").GetComponent<Button>();
        //m_charCreateBtn.onClick.AddListener(CreateCharacter);


        m1 = GameObject.Find("Canvas").transform.Find("1").gameObject;
        m2 = GameObject.Find("Canvas").transform.Find("2").gameObject;
        m3 = GameObject.Find("Canvas").transform.Find("3").gameObject;
    }
    public void CreateCharacter()
    {
        if (m_inputField.text != string.Empty)
        {
            
            int i = 1;

            TextAsset txtFile = Resources.Load("Files/CharStateBasic") as TextAsset;
            string test = txtFile.text;
            string[] line2 = test.Split('\n');
            string line = line2[1];
            string[] info = line.Split(',');
            string level = info[0];
            string str = info[1];
            string def = info[2];
            string cri = info[3];
            string hp = info[4];
            string SL1 = info[5];
            string SL2 = info[6];
            string SL3 = info[7];
            string SL4 = info[8];
            string SL5 = info[9];
            string exp = info[10];
            string sp = info[11];
            
            FileStream fs = new FileStream(Application.persistentDataPath + "/" + "CharState.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamWriter sw = new StreamWriter(fs);
            //sw.WriteLine("CharID,CharName,CharLev,CharStr,CharDef,CharCri,CharHp,firstSL,secondSL,thirdSL,firthSL,fifthSL,Exp,SP,14");
            line = i.ToString() + ',' + m_inputField.text + ',' + level + ',' + str + ',' + def + ',' + cri + ',' + hp + ',' + SL1 + ',' + SL2 + ',' + SL3 + ',' + SL4 + ',' + SL5 + ',' + exp + ',' + sp;
            sw.WriteLine(line);
            sw.Close();
            FileStream fs2 = new FileStream(Application.persistentDataPath + "/" + "CharState.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite);
            StreamReader sr = new StreamReader(fs2);
            sr.Close();


            //TextAsset txtFile = Resources.Load("Files/CharState") as TextAsset;
            //string test = txtFile.text;
            //string[] line2 = test.Split('\n');
            //int i = 1;
            //for (int j = 1; j < line2.Length; j++)
            //{
            //    if (line2[j] == null)
            //        break;
            //    else
            //        ++i;
            //}
            //FileStream fs = new FileStream(Application.persistentDataPath + "/" + "TestFileName.txt", FileMode.OpenOrCreate, FileAccess.Read);



            //string path = Application.dataPath + "/Resources/Files/CharState.txt";
            //StreamReader sr = new StreamReader(fs);
            //sr.ReadLine();
            //int i = 1;
            //while (true)
            //{
            //    string cline = sr.ReadLine();
            //    if (cline == null)
            //        break;
            //    else
            //        ++i;
            //}
            //sr.Close();

            //TextAsset txtFile = Resources.Load("Files/CharStateBasic") as TextAsset;
            //string test = txtFile.text;
            //string[] line2 = test.Split('\n');
            //path = Application.dataPath + "/Resources/Files/CharStateBasic.txt";
            //sr = new StreamReader(path);
            //sr.ReadLine();
            //string line = line2[1];
            //string[] info = line.Split(',');
            //string level = info[0];
            //string str = info[1];
            //string def = info[2];
            //string cri = info[3];
            //string hp = info[4];
            //string SL1 = info[5];
            //string SL2 = info[6];
            //string SL3 = info[7];
            //string SL4 = info[8];
            //string SL5 = info[9];
            //string exp = info[10];
            //string sp = info[11];
            //sr.Close();
            //path = Application.dataPath + "/Resources/Files/CharState.txt";
            //StreamWriter sw = new StreamWriter(path);
            //sw.WriteLine("CharID,CharName,CharLev,CharStr,CharDef,CharCri,CharHp,firstSL,secondSL,thirdSL,firthSL,fifthSL,Exp,SP,14");
            //line = i.ToString() + ',' + m_inputField.text + ',' + level + ',' + str + ',' + def + ',' + cri + ',' + hp + ',' + SL1 + ',' + SL2 + ',' + SL3 + ',' + SL4 + ',' + SL5 + ',' + exp + ',' + sp;
            //sw.WriteLine(line);
            //sw.Close();


            LoadingSceneMng.LoadScene("Village");
        }
    }
    private void BackBtn()
    {
        m_characInfo.SetActive(false);
        isBack = true;
    }
    float time = 0.0f;
    float backTime = 0.0f;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitobj;
            if (Physics.Raycast(ray, out hitobj, 100))
            {
                if (hitobj.transform.tag == "Character")
                {
                    m_CharacSelection = hitobj.transform;
                    m_CharacSelection.GetComponent<Animator>().SetTrigger("Spellcast");
                }

            }
        }
        if(m_CharacSelection != null)
        {
            
            if (isBack)
            {
                backTime += Time.deltaTime;
                m_camera.transform.position = Vector3.Lerp(m_camera.transform.position, m_endPos.position, backTime);
                if(backTime >= 1.0f)
                {
                    isBack = false;
                    backTime = 0.0f;
                    m_CharacSelection = null;
                    time = 0.0f;
                }
            }
            else
            {
                m_selectUI.SetActive(false);
                m_characInfo.SetActive(true);
                if (time <= 0.3f)
                    time += Time.deltaTime;
                Vector3 Pos = new Vector3(m_CharacSelection.position.x, m_CharacSelection.position.y + 1.0f, m_CharacSelection.position.z);
                m_camera.transform.position = Vector3.Lerp(m_endPos.position, Pos, time);
                m_camera.transform.LookAt(Pos);
            }
        }
        else
        {
            m_cameramoveTime += Time.deltaTime / 2;
            m_camera.transform.position = Vector3.Slerp(m_startPos.position, m_endPos.position, m_cameramoveTime);
            m_camera.transform.LookAt(m_lookPos.position);
            if(m_cameramoveTime >= 1.0f)
            {
                m_selectUI.SetActive(true);
                

            }

        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
        }
    }
}
