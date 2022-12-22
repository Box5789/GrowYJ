using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    //게임 진행 상황 저장 파일
    string GameDataFileName = "GameDataFile.json";


    //이벤트
    [Header("- 이벤트")]
    public int 클릭버튼;
    public int 랜덤값;
    GameObject[] 이벤트버튼들 = new GameObject[4];
    GameObject 밥, 잠;
    플레이어 영준이;
    public List<Dictionary<int, EventData>> 이벤트데이터 = new List<Dictionary<int, EventData>>();

    enum 버튼 { 공부, 여가, 사랑, 돈 }

    [Header("- 시간 상수")]
    [SerializeField] private int 밥시간 = 60;
    [SerializeField] private int 잠시간 = 480;

    [Header("- 오브젝트")]
    [SerializeField] private GameObject 시간대이미지;
    [SerializeField] private GameObject 배경이미지;
    [SerializeField] private float 해달이속;
    [SerializeField] private float 배경이속;
    Coroutine 코루틴 = null;
    Transform 배경t;
    RectTransform 해달rt;
    bool 배경 = false, 해달 = false;


    //스탯 영준
    public enum 스탯enum { 지식, 지지도, 아드레날린, 인기, 매력, 주머니사정 }
    /*
    public struct 조건
    {
        int 미만, 이상;
        string 애니메이션;

        public 조건(int 미만, int 이상, string 애니메이션)
        {
            this.미만 = 미만;
            this.이상 = 이상;
            this.애니메이션 = 애니메이션;
        }

        public int Get미만() { return 미만; }
        public int Get이상() { return 이상; }
        public string Get애니() { return 애니메이션; }
    }*/
    //public Dictionary<스탯enum, List<조건>> 스탯영준데이터 = new Dictionary<스탯enum, List<조건>>();



    //데이터 클래스 & 싱글톤
    [Serializable]
    public class GameData
    {
        [Header("- 밥, 잠")]
        public int 밥카운트;
        public int 잠카운트;

        public int 배터리;
        public int 포만감;
        public int 피로도;

        [Header("- 스탯(지식 지지도 아드레날린 인기 매력 주머니사정)")]
        public int[] 스탯 = { 50, 50, 50, 50, 50, 50 };
        public string 상태 = "기본";

        [Header("- 시간")]
        public int 시;
        public int 분;
        public int 일;
        public TL 시간대 = TL.낮;
        public enum TL { 낮, 밤 };

        public void InitData()
        {
            밥카운트 = 0;
            잠카운트 = 0;

            배터리 = 5;
            포만감 = 10;
            피로도 = 10;

            스탯 = new int[6];
            for (int i = 0; i < 스탯.Length; i++)
                스탯[i] = 50;

            시 = 6;
            분 = 0;
            일 = 0;

            SetData();
        }

        public void SetData()
        {
            플레이어 영준이 = GameObject.FindGameObjectWithTag("영준이").GetComponent<플레이어>();

            영준이.밥카운트 = 밥카운트;
            영준이.잠카운트 = 잠카운트;

            영준이.Set배터리(배터리);
            영준이.Set포만감(포만감);
            영준이.Set피로도(피로도);

            영준이.Set스탯(스탯);

            스탯수치검사();
        }

        public void SaveData()
        {
            플레이어 영준이 = GameObject.FindGameObjectWithTag("영준이").GetComponent<플레이어>();

            밥카운트 = 영준이.밥카운트;
            잠카운트 = 영준이.잠카운트;

            배터리 = 영준이.Get배터리();
            포만감 = 영준이.Get포만감();
            피로도 = 영준이.Get피로도();

            스탯 = 영준이.Get스탯();

            스탯수치검사();
        }

        void 스탯수치검사()//영준이 변화 스탯 기준점 임시 : 20,80
        {
            Animator anim = GameObject.FindGameObjectWithTag("영준이").GetComponent<Animator>();

            List<List<int>> list = new List<List<int>>();
            for (int i = 0; i < 스탯.Length; i++)
                list.Add(new List<int> { i, 스탯[i] });
            var 정렬후 = list.OrderBy(x => x[1]).ToArray();

            int row = 정렬후[0][1], high = 정렬후[5][1];

            if (row > 100 - high && high >= 80)//최고점
            {
                if (!상태.Equals("기본"))
                    anim.SetBool(상태, false);
                상태 = ((스탯enum)정렬후[5][0]).ToString() + "_높음";
                anim.SetBool(상태, true);
            }
            else if (row < 100 - high && row < 20)//최저점
            {
                if (!상태.Equals("기본"))
                    anim.SetBool(상태, false);
                상태 = ((스탯enum)정렬후[0][0]).ToString() + "_낮음";
                anim.SetBool(상태, true);
            }
            else if (!상태.Equals("기본"))
            {
                anim.SetBool(상태, false);
                상태 = "기본";
            }
        }

        public bool 시간지남(int m)
        {
            시 += (분 + m) / 60;
            if (시 >= 24)
            {
                일 += (시 + (분 + m) / 60) / 24;
                시 %= 24;
            }
            분 = (분 + m) % 60;

            return true;
        }
        
    }
    [Header("- [ 저장 데이터 ] --------------------")]
    public GameData _gameData;
    public GameData gameData
    {
        get
        {
            if (_gameData == null)
            {
                스탯불러오기();
                스탯저장();
            }
            return _gameData;
        }
    }



    private void Awake()
    {
        영준이 = GameObject.FindGameObjectWithTag
            ("영준이").GetComponent<플레이어>();


        배경t = 배경이미지.GetComponent<Transform>();
        해달rt = 시간대이미지.GetComponent<RectTransform>();

        //밥&잠 버튼
        밥 = GameObject.FindGameObjectsWithTag("이벤트버튼")[0];
        잠 = GameObject.FindGameObjectsWithTag("이벤트버튼")[1];
        밥.GetComponent<Button>().onClick.AddListener(delegate { 밥잠(밥시간); });
        잠.GetComponent<Button>().onClick.AddListener(delegate { 밥잠(잠시간); });

        //이벤트 시트 데이터 로드
        for (int i = 0; i < System.Enum.GetValues(typeof(버튼)).Length; i++)
        {
            //Event DataSet읽어오기
            이벤트데이터Read(((버튼)i).ToString());
            //이벤트 버튼 찾기
            이벤트버튼들[i] = GameObject.FindGameObjectsWithTag("이벤트버튼")[i + 2];
            //이벤트 버튼에 클릭 함수 추가하기
            int x = i;
            이벤트버튼들[i].GetComponent<Button>().onClick.AddListener(delegate {
                if (영준이.Get배터리() > 0) //배터리 없으면 이벤트 발생 X
                    이벤트발생(x);
            });
        }

        //스탯에 따른 영준이 로드
        //스탯영준Read("스탯영준(테스트)");

        //스탯 로드
        스탯불러오기();

        //시간대 확인
        시간대확인();
    }



    private void 이벤트데이터Read(string path_name)
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

        이벤트데이터.Add(임시데이터);
    }

    /*
    private void 스탯영준Read(string path_name)
    {
        List<조건> 임시데이터 = new List<조건>();

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
                for (int i = 0; i < Enum.GetValues(typeof(스탯)).Length; i++)
                {
                    var data_values = data_String.Split('/');
                    while (data_values[0] == ((스탯)i).ToString())
                    {
                        조건 임시조건1 = new 조건(int.Parse(data_values[1]), int.Parse(data_values[2]), data_values[3]);
                        임시데이터.Add(임시조건1);
                        data_values = data_String.Split('/');
                    }
                    스탯영준데이터.Add((스탯)i, 임시데이터);

                    임시데이터.Clear();

                    조건 임시조건2 = new 조건(int.Parse(data_values[1]), int.Parse(data_values[2]), data_values[3]);
                    임시데이터.Add(임시조건2);
                }
                스탯영준데이터.Add(Enum.GetValues(typeof(스탯)).Cast<스탯>().Last(), 임시데이터);

                end = true;
            }
            else first = false;
        }
    }
    */



    void 시간대확인()
    {
        int 시간검사 = (_gameData.시 + 18) % 24;

        if (배경t.position.y > 0 && 시간검사 >= 12)
        {
            _gameData.시간대 = GameData.TL.밤;
            if (!해달 && !배경)
            {
                if (코루틴 != null)
                    StopCoroutine(코루틴);
                코루틴 = StartCoroutine(밤으로());
            }
        }
        else if (배경t.position.y < 0 && 시간검사 < 12)
        {
            _gameData.시간대 = GameData.TL.낮;
            if (!해달 && !배경)
            {
                if (코루틴 != null)
                    StopCoroutine(코루틴);
                StartCoroutine(낮으로());
            }
        }
    }
    IEnumerator 낮으로()
    {
        while (true)
        {
            //배경 이동
            //소수 계산 오류 때문에 int로 변환해서 사용
            int 배경위치 = (int)(배경t.position.y * 10f);
            int 배경목표 = (int)(배경t.localScale.y * 100f);

            if (배경위치 < 배경목표)
                배경t.position = Vector2.MoveTowards(배경t.position, new Vector2(0, (배경t.localScale.y * 3)), 배경이속);
            else 배경 = true;

            //해&달 변경
            if (해달rt.anchoredPosition.y > (해달rt.rect.height / -4))
                해달rt.anchoredPosition = Vector2.MoveTowards(해달rt.anchoredPosition, new Vector2(0, (해달rt.rect.height / -4)), 해달이속);
            else 해달 = true;

            if (해달 && 배경)
            {
                해달 = false;
                배경 = false;
                yield break;
            }
            else
                yield return new WaitForSecondsRealtime(0.01f);
        }
    }
    IEnumerator 밤으로()
    {
        while (true)
        {
            //배경 이동
            //소수 계산 오류 때문에 int로 변환해서 사용
            int 배경위치 = (int)(배경t.position.y * 10f);
            int 배경목표 = (int)(배경t.localScale.y * -100f);

            if (배경위치 > 배경목표)
                배경t.position = Vector2.MoveTowards(배경t.position, new Vector2(0, (배경t.localScale.y * -3)), 배경이속);
            else
                배경 = true;

            //해&달 변경
            if (해달rt.anchoredPosition.y < (해달rt.rect.height / 4))
                해달rt.anchoredPosition = Vector2.MoveTowards(해달rt.anchoredPosition, new Vector2(0, (해달rt.rect.height / 4)), 해달이속);
            else 해달 = true;

            if (해달 && 배경)
            {
                해달 = false;
                배경 = false;
                yield break;
            }
            else
                yield return new WaitForSecondsRealtime(0.01f);
        }
    }




    void 밥잠(int m)
    {
        bool check = _gameData.시간지남(m);
        if (check) 시간대확인();
    }

    public void 이벤트발생(int num)
    {
        클릭버튼 = num;

        //랜덤값 생성 - 추후 확률 조정
        랜덤값 = UnityEngine.Random.Range(0, 이벤트데이터[num].Count);
        EventData 발생이벤트 = 이벤트데이터[클릭버튼][랜덤값];

        //플레이어 스탯 조정
        영준이.이벤트발생(발생이벤트);

        //시간 조정
        bool check = _gameData.시간지남(발생이벤트.Get시간());
        if (check) 시간대확인();

        //스탯 저장
        스탯저장();

        //설명 창 띄우고 대사 등 애니메이션&화면 효과
    }


    public void 스탯초기화()
    {
        _gameData.InitData();
        string filePath = Application.persistentDataPath + GameDataFileName;
        System.IO.File.Delete(filePath);
    }

    public void 스탯불러오기()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;

        if (File.Exists(filePath))//파일이 있다면
        {
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
            _gameData.SetData();
        }
        else//파일이 없다면
        {
            _gameData = new GameData();
            _gameData.InitData();
            스탯저장();
        }
    }

    public void 스탯저장()
    {
        _gameData.SaveData();

        string ToJsonData = JsonUtility.ToJson(_gameData);
        string filePath = Application.persistentDataPath + GameDataFileName;

        File.WriteAllText(filePath, ToJsonData);
    }
}
