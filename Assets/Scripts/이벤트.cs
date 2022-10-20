using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class 이벤트 : MonoBehaviour
{
    GameObject[] 이벤트버튼들 = new GameObject[4];
    플레이어 영준이;

    public List<Dictionary<int, EventData>> 데이터 = new List<Dictionary<int, EventData>>();
    public int 클릭버튼;
    public int 랜덤값;

    enum 버튼{ 공부,여가,사랑, 돈}

    private void Awake()
    {
        영준이 = GameObject.FindGameObjectWithTag("영준이").GetComponent<플레이어>();

        for (int i=0; i < System.Enum.GetValues(typeof(버튼)).Length; i++)
        {
            //Event DataSet읽어오기
            Read(((버튼)i).ToString());
            //이벤트 버튼 찾기
            이벤트버튼들[i] = GameObject.FindGameObjectsWithTag("이벤트버튼")[i+2];
            //이벤트 버튼에 클릭 함수 추가하기
            int x = i;
            이벤트버튼들[i].GetComponent<Button>().onClick.AddListener(delegate { 
                if(영준이.Get배터리() > 0) //배터리 없으면 이벤트 발생 X
                    이벤트발생(x); 
            });
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Read(string path_name)
    {
        Dictionary<int, EventData> 임시데이터 = new Dictionary<int, EventData>();

        StreamReader sr = new StreamReader(Application.dataPath + "/Resources/" + path_name + ".csv");

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
                var data_values = data_String.Split('/');
                int i = 0;
                int[] 스탯 = new int[6];

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

                /* 이렇게 해도 되나 >> 제대로 된 csv파일 만들어지면 테스트
                    임시데이터.Add(int.Parse(data_values[i++]), new EventData(data_values[i++], 
                data_values[i++], data_values[i++], int.Parse(data_values[i++]), 
                int.Parse(data_values[i++]), int.Parse(data_values[i++]), int.Parse(data_values[i++]), 
                int.Parse(data_values[i++]), int.Parse(data_values[i++]), int.Parse(data_values[i++]), 
                int.Parse(data_values[i++]), int.Parse(data_values[i++]), int.Parse(data_values[i++])));
                    */
            }
            else first = false;
        }

        데이터.Add(임시데이터);
    }

    public void 이벤트발생(int num)
    {
        클릭버튼 = num;
        
        //랜덤값 생성 - 추후 확률 조정
        랜덤값 = Random.Range(0, 데이터[num].Count);
        EventData 발생이벤트 = 데이터[클릭버튼][랜덤값];

        //플레이어 스탯 조정

        영준이.이벤트발생(발생이벤트);

        //시간 조정
        GetComponent<시간>().이벤트발생(발생이벤트.Get시간());

        //스탯 저장
        GetComponent<DataManager>().스탯저장();

        //설명 창 띄우고 대사 등 애니메이션&화면 효과
    }
}