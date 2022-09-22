using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileReader : MonoBehaviour
{
    public Dictionary<int, EventData> EventDataSet = new Dictionary<int, EventData>();
    public class EventData
    {
        string �̸�;
        string ����;
        string ���;
        int ������;
        int �Ƿε�;
        int �ŷ�;
        int �Ƶ巹����;
        int �α�;
        int �ָӴϻ���;
        int ����;
        int ������;
        int �ð�;
        int �ջ�;

        public EventData(string �̸�, string ����, string ���, int ������, int �Ƿε�, int �ŷ�, int �Ƶ巹����, int �α�, int �ָӴϻ���, int ����, int ������, int �ð�, int �ջ�)
        {
            this.�̸� = �̸�;
            this.���� = ����;
            this.��� = ���;
            this.������ = ������;
            this.�Ƿε� = �Ƿε�;
            this.�ŷ� = �ŷ�;
            this.�Ƶ巹���� = �Ƶ巹����;
            this.�α� = �α�;
            this.�ָӴϻ��� = �ָӴϻ���;
            this.���� = ����;
            this.������ = ������;
            this.�ð� = �ð�;
            this.�ջ� = �ջ�;
        }
    }

    public void Start()
    {
        EventDataSet = test();
    }

    public Dictionary<int,EventData> test()
    {
        Dictionary<int, EventData> result = new Dictionary<int, EventData>();

        StreamReader sr = new StreamReader(Application.dataPath + "/Resources/����.csv");

        bool end = false;
        bool first = true;
        while (!end)
        {
            string data_String = sr.ReadLine();
            if (data_String == null)
            {
                end = true;
                break;
            }
            else if (!first)
            {
                var data_values = data_String.Split(',');
                int i = 0;
                int key = int.Parse(data_values[i++]);
                string �̸� = data_values[i++];
                string ���� = data_values[i++];
                string ��� = data_values[i++];
                int ������ = int.Parse(data_values[i++]);
                int �Ƿε� = int.Parse(data_values[i++]);
                int �ŷ� = int.Parse(data_values[i++]);
                int �Ƶ巹���� = int.Parse(data_values[i++]);
                int �α� = int.Parse(data_values[i++]);
                int �ָӴϻ��� = int.Parse(data_values[i++]);
                int ���� = int.Parse(data_values[i++]);
                int ������ = int.Parse(data_values[i++]);
                int �ð� = int.Parse(data_values[i++]);
                int �ջ� = int.Parse(data_values[i++]);
                result.Add(key, new EventData(�̸�, ����, ���, ������, �Ƿε�, �ŷ�, �Ƶ巹����, �α�, �ָӴϻ���, ����, ������, �ð�, �ջ�));
            }
            else first = false;
        }

        return result;
    }
}
