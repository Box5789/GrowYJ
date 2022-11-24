using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public delegate void EventHandler();

public class player_controller : MonoBehaviour
{
    //이벤트 데이터
    enum 버튼 { 공부, 여가, 사랑, 돈 }
    GameObject[] 이벤트버튼들 = new GameObject[4];
    public List<Dictionary<int, EventData>> 이벤트데이터 = new List<Dictionary<int, EventData>>();

    GameObject 영준이;
    player_view 영준이UI;
    player_model 영준이Model;

    private void Awake()
    {
        영준이 = GameObject.FindGameObjectWithTag("영준이");
        영준이UI = new player_view();
        영준이Model = new player_model();

        for (int i = 0; i < System.Enum.GetValues(typeof(버튼)).Length; i++)
        {
            이벤트_데이터_읽어오기(((버튼)i).ToString());
            이벤트버튼들[i] = GameObject.FindGameObjectsWithTag("이벤트버튼")[i + 2];
            int x = i;
            이벤트버튼들[i].GetComponent<Button>().onClick.AddListener(delegate { 
                영준이Model.이벤트발생();
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

    void 이벤트_데이터_읽어오기(string path)
    {
        Dictionary<int, EventData> 임시데이터 = new Dictionary<int, EventData>();
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
                int[] 스탯 = new int[6];

                var data_values = data_String.Split('/');

                int key = int.Parse(data_values[i++]);
                string 이름 = data_values[i++];
                string 설명 = data_values[i++];
                string 대사 = data_values[i++];
                int 포만감 = int.Parse(data_values[i++]);
                int 피로도 = int.Parse(data_values[i++]);
                for (int j = 0; j < 스탯.Length; j++)
                    스탯[j] = int.Parse(data_values[i++]);
                int 시간 = int.Parse(data_values[i++]);

                임시데이터.Add(key, new EventData(이름, 설명, 대사, 포만감, 피로도, 스탯, 시간));
            }
            else first = false;
        }

        이벤트데이터.Add(임시데이터);
    }
}
