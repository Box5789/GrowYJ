using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public string GameDataFileName = "GameDataFile.json";

    private 시간 타임;

    [Serializable]
    public class GameData
    {
        public int 밥카운트;
        public int 잠카운트;

        public int 배터리;
        public int 포만감;
        public int 피로도;

        public int[] 스탯;

        public int 시;
        public int 분;
        public int 일;
        
        public void InitData()
        {
            밥카운트 = 0;
            잠카운트 = 0;

            배터리 = 5;
            포만감 = 10;
            피로도 = 10;

            for (int i = 0; i < 스탯.Length; i++)
                스탯[i] = 50;

            시 = 6;
            분 = 0;
            일 = 0;
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
        }

        public void SaveTime(int h, int m, int d)
        {
            시 = h;
            분 = m;
            일 = d;
        }
    }

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
        타임 = GetComponent<시간>();
        스탯불러오기();
    }
    public void 스탯초기화()
    {
        _gameData.InitData();

        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + GameDataFileName;

        File.WriteAllText(filePath, ToJsonData);
    }

    public void 스탯불러오기()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;

        if (File.Exists(filePath))
        {
            Debug.Log("불러오기 성공!");
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
            _gameData.SetData();
            타임.SetTime(_gameData.시, _gameData.분, _gameData.일);
        }
        else
        {
            Debug.Log("새로운 파일 생성");
            _gameData = new GameData();
            _gameData.SaveData();
            _gameData.SaveTime(타임.Get시(), 타임.Get분(), 타임.Get일());
        }
    }

    public void 스탯저장()
    {
        _gameData.SaveData();

        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + GameDataFileName;

        File.WriteAllText(filePath, ToJsonData);
    }
}
