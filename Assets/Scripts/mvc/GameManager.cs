using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static GameManager;

public class GameManager : MonoBehaviour
{
    [Header("- �̺�Ʈ ���")]
    public int day;
    public string result_text;
    public string result_state;

    [Header("- ���� ������")]
    public string endingKey = null;
    public Dictionary<string, ������> ����������;
    public Dictionary<string, ������> ����������;

    [Header("- ��� ����")]
    public string deathKey = null;
    public Dictionary<string, ������> ���������;
    public Dictionary<string, ������> ���������;


    public enum ����num { ����, ������, �Ƶ巹����, �α�, �ŷ�, �ָӴϻ��� };
    public class ������
    {
        public bool �޼�����;
        string �̸�;
        Sprite ����;
        public string ��¥;
        public ������(bool c, string n, Sprite p, string d) { �޼����� = c; �̸� = n; ���� = p; ��¥ = d; }
    }
    public class ������
    {
        string �̸�;
        public string ���;
        public ������(string n, string t) { �̸� = n; ��� = t; }
    }


    private static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
                return null;
            return instance;
        }
    }

    private void Start()
    {
        ���������� = new Dictionary<string, ������>();
        ��������� = new Dictionary<string, ������>();
        ���������� = new Dictionary<string, ������>();
        ��������� = new Dictionary<string, ������>();
        �������о����(����������, ����������, "ending.csv");
        �������о����(���������, ���������, "death.csv");
        ����о����(����������, "EndingFile.json");
        ����о����(���������, "DeathFile.json");//���� �Ƿε� ������ �߰�
    }

    void �������о����(Dictionary<string, ������> dic, Dictionary<string, ������> ����, string fileName)//���� csv�����̶� ��� ����
    {
        StreamReader sr = new StreamReader(Application.dataPath + "/Resources/" + fileName);

        bool end = false;
        bool first = true;

        while (!end)
        {
            string data_String = sr.ReadLine();

            if (data_String == null)
            {
                end = true;
            }
            else if (!first)
            {
                var data_values = data_String.Split('/');

                string key = data_values[0];
                string name = data_values[1];
                string text = data_values[2];
                Sprite pic = Resources.Load<Sprite>("������/" + key);//���� ��ο� ���� �߰�(���� �̸� key�� �Ұ��� name���� �Ұ��� Ȯ��
                Debug.Log(name + text);
                dic.Add(key, new ������(name, text));
                ����.Add(key, new ������(false, name, pic, "?"));
            }
            else first = false;
        }
    }
    void ����о����(Dictionary<string, ������> dic, string fileName)
    {
        string filePath = Application.persistentDataPath + fileName;

        if(File.Exists(filePath))
            dic = JsonUtility.FromJson<Dictionary<string, ������>>(File.ReadAllText(filePath));
        else
        {
            string ToJsonData = JsonUtility.ToJson(dic);
            File.WriteAllText(filePath, ToJsonData);
        }
    }
    public void ����������߰�()
    {
        ���������[deathKey].�޼����� = true;
        ���������[deathKey].��¥ = DateTime.Now.ToString("yyyy.MM.dd");

        string filePath = Application.persistentDataPath + "DeathFile.json";
        string ToJsonData = JsonUtility.ToJson(���������);
        File.WriteAllText(filePath, ToJsonData);

        string GameData = Application.persistentDataPath + "GameDataFile.json";
        File.Delete(GameData);
    }
    public void �����������߰�()
    {
        ����������[endingKey].�޼����� = true;
        ����������[endingKey].��¥ = DateTime.Now.ToString("yyyy.MM.dd");

        string filePath = Application.persistentDataPath + "EndingFile.json";
        string ToJsonData = JsonUtility.ToJson(���������);
        File.WriteAllText(filePath, ToJsonData);

        string GameData = Application.persistentDataPath + "GameDataFile.json";
        File.Delete(GameData);
    }

    public void �̺�Ʈ���(int d, string t, string s)
    {
        day = d;
        result_text = t;
        result_state = s;
    }
}
