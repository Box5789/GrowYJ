using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class controller : MonoBehaviour
{
    //���ӵ����� ���� ���
    string fileName = "GameDataFile.json";

    //model
    public delegate void void�Լ�();
    public delegate void int�Լ�(int i);
    public static event void�Լ� �ʱ����;
    public static event int�Լ� �̺�Ʈ;
    public static event void�Լ� ��Ա�;
    public static event void�Լ� ���ڱ�;
    public static event void�Լ� ����;

    //view
    public delegate void ��(player.GameData ���);
    public static event �� ������;

    //����â ��ư
    public Button �λ����¹�ư;
    public Button ��������;

    //������
    enum ��ư { ����, ����, ���, �� }
    public static List<Dictionary<int, EventData>> �̺�Ʈ������ = new List<Dictionary<int, EventData>>();
    player ������Model;
    player_ui ������View;

    private void Awake()
    {
        //�̺�Ʈ�Լ� ���̱�
        ������Model = new player();
        ������View = GetComponent<player_ui>();

        player.�̺�Ʈ�Ϸ� += ���ӵ���������;
        player.�̺�Ʈ�Ϸ� += �̺�Ʈ���̵�;
        player.���ĿϷ� += ���ӵ���������;
        player.���ĿϷ� += �����;
        player.���¿Ϸ� += ���ӵ���������;
        player.���¿Ϸ� += ���ξ��̵�;
        player.���ÿϷ� += �����;

        �ʱ���� += ������Ʈ����;
        �ʱ���� += ������View.�����;
        �ʱ���� += ������Model.�����ͼ���;
        �ʱ���� += ������Model.�����ͼ��ø�����;
        ������ += ������View.������;

        Debug.Log("controller Ŭ���� ���� : ");

        �ʱ����();
    }
    void ������Ʈ����()
    {
        GameObject[] �̺�Ʈ��ư = GameObject.FindGameObjectsWithTag("�̺�Ʈ��ư");
        for (int i = 0; i < System.Enum.GetValues(typeof(��ư)).Length; i++)
        {
            �̺�Ʈ_������_�о����(((��ư)i).ToString());
            int x = i;
            �̺�Ʈ��ư[i].GetComponent<Button>().onClick.AddListener(delegate { �̺�Ʈ(x); });
        }
        GameObject.Find("��").gameObject.GetComponent<Button>().onClick.AddListener(delegate { ��Ա�(); });
        GameObject.Find("��").gameObject.GetComponent<Button>().onClick.AddListener(delegate { ���ڱ�(); });
        �λ����¹�ư.onClick.AddListener(delegate { ����(); });
        ��������.onClick.AddListener(delegate { SceneManager.LoadScene("Main Scene"); });

        Debug.Log("controller : ������Ʈ ����");
    }


    void ���ӵ���������(player.GameData ���ӵ�����)
    {
        string filePath = Application.persistentDataPath + fileName;
        string ToJsonData = JsonUtility.ToJson(���ӵ�����);
        File.WriteAllText(filePath, ToJsonData);

        Debug.Log("������ ����");
    }
    void �̺�Ʈ���̵�(player.GameData tmp) { SceneManager.LoadScene("Event Scene"); }
    void ���ξ��̵�(player.GameData tmp) { SceneManager.LoadScene("Main Scene"); }
    void �����(player.GameData ���)
    {/*
        if (GameManager.Instance.endingKey == null && GameManager.Instance.deathKey == null)
            SceneManager.LoadScene("Event Scene");
        else
            */

        ������(���);
    }


    void �̺�Ʈ_������_�о����(string path)//�̺�Ʈ ��Ʈ �ϼ��Ǹ� ���� ����
    {
        Dictionary<int, EventData> �ӽõ����� = new Dictionary<int, EventData>();
        StreamReader sr = new StreamReader(Application.dataPath + "/Resources/" + path + ".csv");

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
                int i = 0;
                int[] ���� = new int[6];

                var data_values = data_String.Split('/');

                int key = int.Parse(data_values[i++]);
                string �̸� = data_values[i++];
                string ���� = data_values[i++];
                string ��� = data_values[i++];
                int ������ = int.Parse(data_values[i++]);
                int �Ƿε� = int.Parse(data_values[i++]);
                for (int j = 0; j < ����.Length; j++)
                    ����[j] = int.Parse(data_values[i++]);
                int �ð� = int.Parse(data_values[i++]);

                �ӽõ�����.Add(key, new EventData(�̸�, ����, ���, ������, �Ƿε�, ����, �ð�));
            }
            else first = false;
        }

        �̺�Ʈ������.Add(�ӽõ�����);
    }

    private void OnDestroy()
    {
        player.�̺�Ʈ�Ϸ� -= ���ӵ���������;
        player.�̺�Ʈ�Ϸ� -= �̺�Ʈ���̵�;
        player.���ĿϷ� -= ���ӵ���������;
        player.���ĿϷ� -= �����;
        player.���¿Ϸ� -= ���ӵ���������;
        player.���¿Ϸ� -= ���ξ��̵�;
        player.���ÿϷ� -= �����;

        �ʱ���� -= ������Ʈ����;
        �ʱ���� -= ������View.�����;
        �ʱ���� -= ������Model.�����ͼ���;
        �ʱ���� -= ������Model.�����ͼ��ø�����;
        ������ -= ������View.������;

        ������Model.�̺�Ʈ����();
    }
}

