using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class controller : MonoBehaviour
{
    //게임데이터 저장 경로
    string fileName = "GameDataFile.json";

    //model
    public delegate void void함수();
    public delegate void int함수(int i);
    public static event void함수 초기셋팅;
    public static event int함수 이벤트;
    public static event void함수 밥먹기;
    public static event void함수 잠자기;
    public static event void함수 리셋;

    //view
    public delegate void 뷰(player.GameData 결과);
    public static event 뷰 뷰조정;

    //설정창 버튼
    public Button 인생리셋버튼;
    public Button 메인으로;

    //데이터
    enum 버튼 { 공부, 여가, 사랑, 돈 }
    public static List<Dictionary<int, EventData>> 이벤트데이터 = new List<Dictionary<int, EventData>>();
    player 영준이Model;
    player_ui 영준이View;

    private void Awake()
    {
        //이벤트함수 붙이기
        영준이Model = new player();
        영준이View = GetComponent<player_ui>();

        player.이벤트완료 += 게임데이터저장;
        player.이벤트완료 += 이벤트씬이동;
        player.숙식완료 += 게임데이터저장;
        player.숙식완료 += 뷰실행;
        player.리셋완료 += 게임데이터저장;
        player.리셋완료 += 메인씬이동;
        player.셋팅완료 += 뷰실행;

        초기셋팅 += 오브젝트셋팅;
        초기셋팅 += 영준이View.뷰셋팅;
        초기셋팅 += 영준이Model.데이터셋팅;
        초기셋팅 += 영준이Model.데이터셋팅마무리;
        뷰조정 += 영준이View.뷰조정;

        Debug.Log("controller 클래스 생성 : ");

        초기셋팅();
    }
    void 오브젝트셋팅()
    {
        GameObject[] 이벤트버튼 = GameObject.FindGameObjectsWithTag("이벤트버튼");
        for (int i = 0; i < System.Enum.GetValues(typeof(버튼)).Length; i++)
        {
            이벤트_데이터_읽어오기(((버튼)i).ToString());
            int x = i;
            이벤트버튼[i].GetComponent<Button>().onClick.AddListener(delegate { 이벤트(x); });
        }
        GameObject.Find("밥").gameObject.GetComponent<Button>().onClick.AddListener(delegate { 밥먹기(); });
        GameObject.Find("잠").gameObject.GetComponent<Button>().onClick.AddListener(delegate { 잠자기(); });
        인생리셋버튼.onClick.AddListener(delegate { 리셋(); });
        메인으로.onClick.AddListener(delegate { SceneManager.LoadScene("Main Scene"); });

        Debug.Log("controller : 오브젝트 셋팅");
    }


    void 게임데이터저장(player.GameData 게임데이터)
    {
        string filePath = Application.persistentDataPath + fileName;
        string ToJsonData = JsonUtility.ToJson(게임데이터);
        File.WriteAllText(filePath, ToJsonData);

        Debug.Log("데이터 저장");
    }
    void 이벤트씬이동(player.GameData tmp) { SceneManager.LoadScene("Event Scene"); }
    void 메인씬이동(player.GameData tmp) { SceneManager.LoadScene("Main Scene"); }
    void 뷰실행(player.GameData 결과)
    {/*
        if (GameManager.Instance.endingKey == null && GameManager.Instance.deathKey == null)
            SceneManager.LoadScene("Event Scene");
        else
            */

        뷰조정(결과);
    }


    void 이벤트_데이터_읽어오기(string path)//이벤트 시트 완성되면 구조 수정
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

    private void OnDestroy()
    {
        player.이벤트완료 -= 게임데이터저장;
        player.이벤트완료 -= 이벤트씬이동;
        player.숙식완료 -= 게임데이터저장;
        player.숙식완료 -= 뷰실행;
        player.리셋완료 -= 게임데이터저장;
        player.리셋완료 -= 메인씬이동;
        player.셋팅완료 -= 뷰실행;

        초기셋팅 -= 오브젝트셋팅;
        초기셋팅 -= 영준이View.뷰셋팅;
        초기셋팅 -= 영준이Model.데이터셋팅;
        초기셋팅 -= 영준이Model.데이터셋팅마무리;
        뷰조정 -= 영준이View.뷰조정;

        영준이Model.이벤트해제();
    }
}

