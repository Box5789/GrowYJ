using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileReader : MonoBehaviour
{
    public Dictionary<int, EventData> EventDataSet = new Dictionary<int, EventData>();
    public class EventData
    {
        string 이름;
        string 설명;
        string 대사;
        int 포만감;
        int 피로도;
        int 매력;
        int 아드레날린;
        int 인기;
        int 주머니사정;
        int 지식;
        int 지지도;
        int 시간;
        int 합산;

        public EventData(string 이름, string 설명, string 대사, int 포만감, int 피로도, int 매력, int 아드레날린, int 인기, int 주머니사정, int 지식, int 지지도, int 시간, int 합산)
        {
            this.이름 = 이름;
            this.설명 = 설명;
            this.대사 = 대사;
            this.포만감 = 포만감;
            this.피로도 = 피로도;
            this.매력 = 매력;
            this.아드레날린 = 아드레날린;
            this.인기 = 인기;
            this.주머니사정 = 주머니사정;
            this.지식 = 지식;
            this.지지도 = 지지도;
            this.시간 = 시간;
            this.합산 = 합산;
        }
    }

    public void Start()
    {
        EventDataSet = test();
    }

    public Dictionary<int,EventData> test()
    {
        Dictionary<int, EventData> result = new Dictionary<int, EventData>();

        StreamReader sr = new StreamReader(Application.dataPath + "/Resources/공부.csv");

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
                string 이름 = data_values[i++];
                string 설명 = data_values[i++];
                string 대사 = data_values[i++];
                int 포만감 = int.Parse(data_values[i++]);
                int 피로도 = int.Parse(data_values[i++]);
                int 매력 = int.Parse(data_values[i++]);
                int 아드레날린 = int.Parse(data_values[i++]);
                int 인기 = int.Parse(data_values[i++]);
                int 주머니사정 = int.Parse(data_values[i++]);
                int 지식 = int.Parse(data_values[i++]);
                int 지지도 = int.Parse(data_values[i++]);
                int 시간 = int.Parse(data_values[i++]);
                int 합산 = int.Parse(data_values[i++]);
                result.Add(key, new EventData(이름, 설명, 대사, 포만감, 피로도, 매력, 아드레날린, 인기, 주머니사정, 지식, 지지도, 시간, 합산));
            }
            else first = false;
        }

        return result;
    }
}
