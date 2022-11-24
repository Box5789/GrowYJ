using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public delegate void EventHandler();

public class player_controller : MonoBehaviour
{
    //�̺�Ʈ ������
    enum ��ư { ����, ����, ���, �� }
    GameObject[] �̺�Ʈ��ư�� = new GameObject[4];
    public List<Dictionary<int, EventData>> �̺�Ʈ������ = new List<Dictionary<int, EventData>>();

    GameObject ������;
    player_view ������UI;
    player_model ������Model;

    private void Awake()
    {
        ������ = GameObject.FindGameObjectWithTag("������");
        ������UI = new player_view();
        ������Model = new player_model();

        for (int i = 0; i < System.Enum.GetValues(typeof(��ư)).Length; i++)
        {
            �̺�Ʈ_������_�о����(((��ư)i).ToString());
            �̺�Ʈ��ư��[i] = GameObject.FindGameObjectsWithTag("�̺�Ʈ��ư")[i + 2];
            int x = i;
            �̺�Ʈ��ư��[i].GetComponent<Button>().onClick.AddListener(delegate { 
                ������Model.�̺�Ʈ�߻�();
            });
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void �̺�Ʈ_������_�о����(string path)
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
}
