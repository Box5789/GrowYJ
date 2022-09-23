using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

namespace 이벤트
{
    public class 이벤트관리 : MonoBehaviour
    {
        [Header("- 버튼")]
        [SerializeField] private GameObject 공부버튼;
        [SerializeField] private GameObject 여가버튼;
        [SerializeField] private GameObject 사랑버튼;
        [SerializeField] private GameObject 돈버튼;

        private 스탯관리 스탯;

        public List<Dictionary<int, 이벤트데이터>> 데이터 = new List<Dictionary<int, 이벤트데이터>>();


        public enum 버튼{ 공부,여가,사랑, 돈}

        private void Awake()
        {
            for(int i=0; i < System.Enum.GetValues(typeof(버튼)).Length; i++)
            {
                Read(버튼.공부.ToString());
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            공부버튼.GetComponent<Button>().onClick.AddListener(delegate { 이벤트버튼((int)버튼.공부); });
            여가버튼.GetComponent<Button>().onClick.AddListener(delegate { 이벤트버튼((int)버튼.여가); });
            사랑버튼.GetComponent<Button>().onClick.AddListener(delegate { 이벤트버튼((int)버튼.사랑); });
            돈버튼.GetComponent<Button>().onClick.AddListener(delegate { 이벤트버튼((int)버튼.돈); });
            스탯 = GetComponent<스탯관리>();
        }

        private void Read(string path_name)
        {
            Dictionary<int, 이벤트데이터> 임시데이터 = new Dictionary<int, 이벤트데이터>();

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
                    임시데이터.Add(key, new 이벤트데이터(이름, 설명, 대사, 포만감, 피로도, 매력, 아드레날린, 인기, 주머니사정, 지식, 지지도, 시간, 합산));

                    /* 이렇게 해도 되나
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

        private void 이벤트버튼(int num)
        {
            //랜덤값 생성
            int rand = Random.Range(0, 데이터[num].Count);

            //스탯 조정
            스탯.이벤트발생(데이터[num][rand].Get포만감(), 데이터[num][rand].Get피로도(), 데이터[num][rand].Get지식(),
                데이터[num][rand].Get지지도(), 데이터[num][rand].Get아드레날린(), 데이터[num][rand].Get인기(), 
                데이터[num][rand].Get매력(), 데이터[num][rand].Get주머니사정());

            //설명 창 띄우고 대사 등 애니메이션&화면 효과
        }
    }
}