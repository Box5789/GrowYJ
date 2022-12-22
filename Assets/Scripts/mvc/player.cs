using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player
{
    //게임데이터 저장 경로
    string fileName = "GameDataFile.json";


    public delegate void CallBack함수(GameData 데이터);
    public static event CallBack함수 셋팅완료;
    public static event CallBack함수 이벤트완료;
    public static event CallBack함수 숙식완료;
    public static event CallBack함수 리셋완료;


    public enum 스탯num { 지식, 지지도, 아드레날린, 인기, 매력, 주머니사정 };
    public enum 시간대 { 낮, 밤 };

    const int 밥시간 = 90, 잠시간 = 480, 밥배터리 = 1, 잠배터리 = 3; 

    public class GameData
    {
        public int[] 스탯 = new int[6];
        public int 밥카운트, 잠카운트, 배터리, 포만감, 피로도, 시, 분, 일;
        public string 이전상태, 현재상태, 결과상태;
        public string 대사;
        public 시간대 전시간대, 현시간대;
    }
    GameData 게임데이터 = new GameData();


    public player()
    {
        controller.이벤트 += 수치조정;
        controller.이벤트 += 이벤트마무리;
        
        controller.밥먹기 += 밥먹기;
        controller.밥먹기 += 숙식마무리;
        
        controller.잠자기 += 잠자기;
        controller.잠자기 += 숙식마무리;

        controller.리셋 += 데이터리셋;
        controller.리셋 += 리셋마무리;

        Debug.Log("model 클래스 생성 : ");
    }
    void 테스트데이터셋() { GameObject.Find("데이터").GetComponent<TestData>().DataSet(게임데이터); }

    public void 데이터셋팅() 
    {
        string filePath = Application.persistentDataPath + fileName;

        //게임데이터 읽어오기
        if (File.Exists(filePath))//파일이 있다면 게임데이터 셋팅 : model
        {
            게임데이터 = JsonUtility.FromJson<player.GameData>(File.ReadAllText(filePath));
        }
        else//파일이 없다면 데이터 초기화 : model
        {
            데이터리셋();
        }

        Debug.Log("데이터 읽어오기");
    }
    public void 데이터셋팅마무리() { 테스트데이터셋(); 셋팅완료(게임데이터); }
    

    void 수치조정(int event_num)
    {
        //난수발생
        int num = UnityEngine.Random.Range(0, controller.이벤트데이터[event_num].Count);

        //이벤트데이터
        EventData 데이터 = controller.이벤트데이터[event_num][num];

        //시간조정
        시간지남(데이터.Get시간());

        //스탯조정
        게임데이터.배터리--;
        게임데이터.포만감 += 데이터.Get포만감();
        게임데이터.피로도 += 데이터.Get피로도();
        게임데이터.대사 = 데이터.Get대사();
        게임데이터.결과상태 = 데이터.Get이름();

        for (int j = 0; j < 게임데이터.스탯.Length; j++)
            게임데이터.스탯[j] += 데이터.Get스탯()[j];

        List<List<int>> list = new List<List<int>>();
        for (int k = 0; k < 게임데이터.스탯.Length; k++)
            list.Add(new List<int> { k, 게임데이터.스탯[k] });
        var 정렬후 = list.OrderBy(x => x[1]).ToArray();
        int row = 정렬후[0][1], high = 정렬후[5][1];

        게임데이터.이전상태 = 게임데이터.현재상태;

        if (row > 100 - high && high >= 80)//최고점
            게임데이터.현재상태 = ((player.스탯num)정렬후[5][0]).ToString() + "_높음";
        else if (row < 100 - high && row < 20)//최저점
            게임데이터.현재상태 = ((player.스탯num)정렬후[0][0]).ToString() + "_낮음";
        else if (!게임데이터.현재상태.Equals("기본"))
            게임데이터.현재상태 = "기본";

        //검사
        사망검사();
        엔딩검사();
    }
    void 시간지남(int m)
    {
        게임데이터.시 += (게임데이터.분 + m) / 60;

        if (게임데이터.시 >= 24)
        {
            게임데이터.일 += (게임데이터.시 + (게임데이터.분 + m) / 60) / 24;
            게임데이터.시 %= 24;
        }

        게임데이터.분 = (게임데이터.분 + m) % 60;

        게임데이터.전시간대 = 게임데이터.현시간대;

        int 시간대검사 = (게임데이터.시 + 18) % 24;

        if (시간대검사 >= 12)
            게임데이터.현시간대 = 시간대.밤;
        else if (시간대검사 < 12)
            게임데이터.현시간대 = 시간대.낮;
    }
    void 사망검사()
    {
        GameManager.Instance.deathKey = null;
        for (int i = 0; i < 게임데이터.스탯.Length; i++)
            if (게임데이터.스탯[i] >= 100)
                GameManager.Instance.deathKey = ((스탯num)i).ToString() + "100";
            else if (게임데이터.스탯[i] <= 0)
                GameManager.Instance.deathKey = ((스탯num)i).ToString() + "0";
    }
    void 엔딩검사()
    {
        GameManager.Instance.endingKey = null;
        if (게임데이터.일 >= 28)
        {
            List<List<int>> list = new List<List<int>>();
            for (int k = 0; k < 게임데이터.스탯.Length; k++)
                list.Add(new List<int> { k, 게임데이터.스탯[k] });
            var 정렬후 = list.OrderBy(x => x[1]).ToArray();
            int high1 = 정렬후[4][0], high2 = 정렬후[5][0];

            GameManager.Instance.endingKey = MadeKey(high1, high2);
        }
    }
    string MadeKey(int h1, int h2)
    {
        if(h1 > h2)
            return ((스탯num)h2).ToString() + "," + ((스탯num)h1).ToString();
        else
            return ((스탯num)h1).ToString() + "," + ((스탯num)h2).ToString();
    }
    void 이벤트마무리(int tmp) 
    {
        GameManager.Instance.이벤트결과(게임데이터.일, 게임데이터.대사, 게임데이터.결과상태);

        이벤트완료(게임데이터);
    }


    void 밥먹기()
    {
        시간지남(밥시간);

        게임데이터.배터리 += 밥배터리;
        if (게임데이터.배터리 > 5) 게임데이터.배터리 = 5;

        //검사
        사망검사();
        엔딩검사();
    }
    void 잠자기()
    {
        시간지남(잠시간);

        게임데이터.배터리 += 잠배터리;
        if (게임데이터.배터리 > 5) 게임데이터.배터리 = 5;

        //검사
        사망검사();
        엔딩검사();
    }
    void 숙식마무리() { 테스트데이터셋(); 숙식완료(게임데이터); }


    void 데이터리셋()
    {
        string filePath = Application.persistentDataPath + fileName;

        //기존 파일 존재한다면 파일 삭제
        if (File.Exists(filePath)) File.Delete(filePath);

        //데이터 초기화
        게임데이터 = new GameData();

        게임데이터.밥카운트 = 0;
        게임데이터.잠카운트 = 0;

        게임데이터.배터리 = 5;
        게임데이터.포만감 = 10;
        게임데이터.피로도 = 10;

        게임데이터.스탯 = new int[6];
        for (int i = 0; i < 게임데이터.스탯.Length; i++)
            게임데이터.스탯[i] = 50;

        게임데이터.이전상태 = "기본";
        게임데이터.현재상태 = "기본";

        게임데이터.대사 = "..왔구나......\n안녕....?";

        게임데이터.시 = 6;
        게임데이터.분 = 0;
        게임데이터.일 = 1;

        게임데이터.전시간대 = 시간대.낮;
        게임데이터.현시간대 = 시간대.낮;
    }
    void 리셋마무리() { 리셋완료(게임데이터); }

    
    public void 이벤트해제()
    {
        controller.이벤트 -= 수치조정;
        controller.이벤트 -= 이벤트마무리;

        controller.밥먹기 -= 밥먹기;
        controller.밥먹기 -= 숙식마무리;

        controller.잠자기 -= 잠자기;
        controller.잠자기 -= 숙식마무리;

        controller.리셋 -= 데이터리셋;
        controller.리셋 -= 리셋마무리;
    }
}
