using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public string GameDataFileName = "GameDataFile.json";

    private �ð� Ÿ��;

    [Serializable]
    public class GameData
    {
        public int ��ī��Ʈ;
        public int ��ī��Ʈ;

        public int ���͸�;
        public int ������;
        public int �Ƿε�;

        public int[] ����;

        public int ��;
        public int ��;
        public int ��;
        
        public void InitData()
        {
            ��ī��Ʈ = 0;
            ��ī��Ʈ = 0;

            ���͸� = 5;
            ������ = 10;
            �Ƿε� = 10;

            for (int i = 0; i < ����.Length; i++)
                ����[i] = 50;

            �� = 6;
            �� = 0;
            �� = 0;
        }

        public void SetData()
        {
            �÷��̾� ������ = GameObject.FindGameObjectWithTag("������").GetComponent<�÷��̾�>();

            ������.��ī��Ʈ = ��ī��Ʈ;
            ������.��ī��Ʈ = ��ī��Ʈ;

            ������.Set���͸�(���͸�);
            ������.Set������(������);
            ������.Set�Ƿε�(�Ƿε�);

            ������.Set����(����);
        }

        public void SaveData()
        {
            �÷��̾� ������ = GameObject.FindGameObjectWithTag("������").GetComponent<�÷��̾�>();

            ��ī��Ʈ = ������.��ī��Ʈ;
            ��ī��Ʈ = ������.��ī��Ʈ;

            ���͸� = ������.Get���͸�();
            ������ = ������.Get������();
            �Ƿε� = ������.Get�Ƿε�();

            ���� = ������.Get����();
        }

        public void SaveTime(int h, int m, int d)
        {
            �� = h;
            �� = m;
            �� = d;
        }
    }

    public GameData _gameData;
    public GameData gameData
    {
        get
        {
            if (_gameData == null)
            {
                ���Ⱥҷ�����();
                ��������();
            }
            return _gameData;
        }
    }

    private void Awake()
    {
        Ÿ�� = GetComponent<�ð�>();
        ���Ⱥҷ�����();
    }
    public void �����ʱ�ȭ()
    {
        _gameData.InitData();

        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + GameDataFileName;

        File.WriteAllText(filePath, ToJsonData);
    }

    public void ���Ⱥҷ�����()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;

        if (File.Exists(filePath))
        {
            Debug.Log("�ҷ����� ����!");
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
            _gameData.SetData();
            Ÿ��.SetTime(_gameData.��, _gameData.��, _gameData.��);
        }
        else
        {
            Debug.Log("���ο� ���� ����");
            _gameData = new GameData();
            _gameData.SaveData();
            _gameData.SaveTime(Ÿ��.Get��(), Ÿ��.Get��(), Ÿ��.Get��());
        }
    }

    public void ��������()
    {
        _gameData.SaveData();

        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + GameDataFileName;

        File.WriteAllText(filePath, ToJsonData);
    }
}
