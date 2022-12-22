using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static GameManager;

public class GameManager : MonoBehaviour
{
    [Header("- 이벤트 결과")]
    public int day;
    public string result_text;
    public string result_state;

    [Header("- 엔딩 데이터")]
    public string endingKey = null;
    public Dictionary<string, 모음집> 엔딩모음집;
    public Dictionary<string, 데이터> 엔딩데이터;

    [Header("- 사망 정보")]
    public string deathKey = null;
    public Dictionary<string, 모음집> 사망모음집;
    public Dictionary<string, 데이터> 사망데이터;


    public enum 스탯num { 지식, 지지도, 아드레날린, 인기, 매력, 주머니사정 };
    public class 모음집
    {
        public bool 달성여부;
        string 이름;
        Sprite 사진;
        public string 날짜;
        public 모음집(bool c, string n, Sprite p, string d) { 달성여부 = c; 이름 = n; 사진 = p; 날짜 = d; }
    }
    public class 데이터
    {
        string 이름;
        public string 대사;
        public 데이터(string n, string t) { 이름 = n; 대사 = t; }
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
        엔딩데이터 = new Dictionary<string, 데이터>();
        사망데이터 = new Dictionary<string, 데이터>();
        엔딩모음집 = new Dictionary<string, 모음집>();
        사망모음집 = new Dictionary<string, 모음집>();
        데이터읽어오기(엔딩데이터, 엔딩모음집, "ending.csv");
        데이터읽어오기(사망데이터, 사망모음집, "death.csv");
        기록읽어오기(엔딩모음집, "EndingFile.json");
        기록읽어오기(사망모음집, "DeathFile.json");//추후 피로도 포만감 추가
    }

    void 데이터읽어오기(Dictionary<string, 데이터> dic, Dictionary<string, 모음집> 모음, string fileName)//추후 csv파일이랑 경로 수정
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
                Sprite pic = Resources.Load<Sprite>("모음집/" + key);//추후 경로에 사진 추가(사진 이름 key로 할건지 name으로 할건지 확인
                Debug.Log(name + text);
                dic.Add(key, new 데이터(name, text));
                모음.Add(key, new 모음집(false, name, pic, "?"));
            }
            else first = false;
        }
    }
    void 기록읽어오기(Dictionary<string, 모음집> dic, string fileName)
    {
        string filePath = Application.persistentDataPath + fileName;

        if(File.Exists(filePath))
            dic = JsonUtility.FromJson<Dictionary<string, 모음집>>(File.ReadAllText(filePath));
        else
        {
            string ToJsonData = JsonUtility.ToJson(dic);
            File.WriteAllText(filePath, ToJsonData);
        }
    }
    public void 사망모음집추가()
    {
        사망모음집[deathKey].달성여부 = true;
        사망모음집[deathKey].날짜 = DateTime.Now.ToString("yyyy.MM.dd");

        string filePath = Application.persistentDataPath + "DeathFile.json";
        string ToJsonData = JsonUtility.ToJson(사망모음집);
        File.WriteAllText(filePath, ToJsonData);

        string GameData = Application.persistentDataPath + "GameDataFile.json";
        File.Delete(GameData);
    }
    public void 엔딩모음집추가()
    {
        엔딩모음집[endingKey].달성여부 = true;
        엔딩모음집[endingKey].날짜 = DateTime.Now.ToString("yyyy.MM.dd");

        string filePath = Application.persistentDataPath + "EndingFile.json";
        string ToJsonData = JsonUtility.ToJson(사망모음집);
        File.WriteAllText(filePath, ToJsonData);

        string GameData = Application.persistentDataPath + "GameDataFile.json";
        File.Delete(GameData);
    }

    public void 이벤트결과(int d, string t, string s)
    {
        day = d;
        result_text = t;
        result_state = s;
    }
}
