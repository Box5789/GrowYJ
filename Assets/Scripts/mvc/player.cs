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
    //���ӵ����� ���� ���
    string fileName = "GameDataFile.json";


    public delegate void CallBack�Լ�(GameData ������);
    public static event CallBack�Լ� ���ÿϷ�;
    public static event CallBack�Լ� �̺�Ʈ�Ϸ�;
    public static event CallBack�Լ� ���ĿϷ�;
    public static event CallBack�Լ� ���¿Ϸ�;


    public enum ����num { ����, ������, �Ƶ巹����, �α�, �ŷ�, �ָӴϻ��� };
    public enum �ð��� { ��, �� };

    const int ��ð� = 90, ��ð� = 480, ����͸� = 1, ����͸� = 3; 

    public class GameData
    {
        public int[] ���� = new int[6];
        public int ��ī��Ʈ, ��ī��Ʈ, ���͸�, ������, �Ƿε�, ��, ��, ��;
        public string ��������, �������, �������;
        public string ���;
        public �ð��� ���ð���, ���ð���;
    }
    GameData ���ӵ����� = new GameData();


    public player()
    {
        controller.�̺�Ʈ += ��ġ����;
        controller.�̺�Ʈ += �̺�Ʈ������;
        
        controller.��Ա� += ��Ա�;
        controller.��Ա� += ���ĸ�����;
        
        controller.���ڱ� += ���ڱ�;
        controller.���ڱ� += ���ĸ�����;

        controller.���� += �����͸���;
        controller.���� += ���¸�����;

        Debug.Log("model Ŭ���� ���� : ");
    }
    void �׽�Ʈ�����ͼ�() { GameObject.Find("������").GetComponent<TestData>().DataSet(���ӵ�����); }

    public void �����ͼ���() 
    {
        string filePath = Application.persistentDataPath + fileName;

        //���ӵ����� �о����
        if (File.Exists(filePath))//������ �ִٸ� ���ӵ����� ���� : model
        {
            ���ӵ����� = JsonUtility.FromJson<player.GameData>(File.ReadAllText(filePath));
        }
        else//������ ���ٸ� ������ �ʱ�ȭ : model
        {
            �����͸���();
        }

        Debug.Log("������ �о����");
    }
    public void �����ͼ��ø�����() { �׽�Ʈ�����ͼ�(); ���ÿϷ�(���ӵ�����); }
    

    void ��ġ����(int event_num)
    {
        //�����߻�
        int num = UnityEngine.Random.Range(0, controller.�̺�Ʈ������[event_num].Count);

        //�̺�Ʈ������
        EventData ������ = controller.�̺�Ʈ������[event_num][num];

        //�ð�����
        �ð�����(������.Get�ð�());

        //��������
        ���ӵ�����.���͸�--;
        ���ӵ�����.������ += ������.Get������();
        ���ӵ�����.�Ƿε� += ������.Get�Ƿε�();
        ���ӵ�����.��� = ������.Get���();
        ���ӵ�����.������� = ������.Get�̸�();

        for (int j = 0; j < ���ӵ�����.����.Length; j++)
            ���ӵ�����.����[j] += ������.Get����()[j];

        List<List<int>> list = new List<List<int>>();
        for (int k = 0; k < ���ӵ�����.����.Length; k++)
            list.Add(new List<int> { k, ���ӵ�����.����[k] });
        var ������ = list.OrderBy(x => x[1]).ToArray();
        int row = ������[0][1], high = ������[5][1];

        ���ӵ�����.�������� = ���ӵ�����.�������;

        if (row > 100 - high && high >= 80)//�ְ���
            ���ӵ�����.������� = ((player.����num)������[5][0]).ToString() + "_����";
        else if (row < 100 - high && row < 20)//������
            ���ӵ�����.������� = ((player.����num)������[0][0]).ToString() + "_����";
        else if (!���ӵ�����.�������.Equals("�⺻"))
            ���ӵ�����.������� = "�⺻";

        //�˻�
        ����˻�();
        �����˻�();
    }
    void �ð�����(int m)
    {
        ���ӵ�����.�� += (���ӵ�����.�� + m) / 60;

        if (���ӵ�����.�� >= 24)
        {
            ���ӵ�����.�� += (���ӵ�����.�� + (���ӵ�����.�� + m) / 60) / 24;
            ���ӵ�����.�� %= 24;
        }

        ���ӵ�����.�� = (���ӵ�����.�� + m) % 60;

        ���ӵ�����.���ð��� = ���ӵ�����.���ð���;

        int �ð���˻� = (���ӵ�����.�� + 18) % 24;

        if (�ð���˻� >= 12)
            ���ӵ�����.���ð��� = �ð���.��;
        else if (�ð���˻� < 12)
            ���ӵ�����.���ð��� = �ð���.��;
    }
    void ����˻�()
    {
        GameManager.Instance.deathKey = null;
        for (int i = 0; i < ���ӵ�����.����.Length; i++)
            if (���ӵ�����.����[i] >= 100)
                GameManager.Instance.deathKey = ((����num)i).ToString() + "100";
            else if (���ӵ�����.����[i] <= 0)
                GameManager.Instance.deathKey = ((����num)i).ToString() + "0";
    }
    void �����˻�()
    {
        GameManager.Instance.endingKey = null;
        if (���ӵ�����.�� >= 28)
        {
            List<List<int>> list = new List<List<int>>();
            for (int k = 0; k < ���ӵ�����.����.Length; k++)
                list.Add(new List<int> { k, ���ӵ�����.����[k] });
            var ������ = list.OrderBy(x => x[1]).ToArray();
            int high1 = ������[4][0], high2 = ������[5][0];

            GameManager.Instance.endingKey = MadeKey(high1, high2);
        }
    }
    string MadeKey(int h1, int h2)
    {
        if(h1 > h2)
            return ((����num)h2).ToString() + "," + ((����num)h1).ToString();
        else
            return ((����num)h1).ToString() + "," + ((����num)h2).ToString();
    }
    void �̺�Ʈ������(int tmp) 
    {
        GameManager.Instance.�̺�Ʈ���(���ӵ�����.��, ���ӵ�����.���, ���ӵ�����.�������);

        �̺�Ʈ�Ϸ�(���ӵ�����);
    }


    void ��Ա�()
    {
        �ð�����(��ð�);

        ���ӵ�����.���͸� += ����͸�;
        if (���ӵ�����.���͸� > 5) ���ӵ�����.���͸� = 5;

        //�˻�
        ����˻�();
        �����˻�();
    }
    void ���ڱ�()
    {
        �ð�����(��ð�);

        ���ӵ�����.���͸� += ����͸�;
        if (���ӵ�����.���͸� > 5) ���ӵ�����.���͸� = 5;

        //�˻�
        ����˻�();
        �����˻�();
    }
    void ���ĸ�����() { �׽�Ʈ�����ͼ�(); ���ĿϷ�(���ӵ�����); }


    void �����͸���()
    {
        string filePath = Application.persistentDataPath + fileName;

        //���� ���� �����Ѵٸ� ���� ����
        if (File.Exists(filePath)) File.Delete(filePath);

        //������ �ʱ�ȭ
        ���ӵ����� = new GameData();

        ���ӵ�����.��ī��Ʈ = 0;
        ���ӵ�����.��ī��Ʈ = 0;

        ���ӵ�����.���͸� = 5;
        ���ӵ�����.������ = 10;
        ���ӵ�����.�Ƿε� = 10;

        ���ӵ�����.���� = new int[6];
        for (int i = 0; i < ���ӵ�����.����.Length; i++)
            ���ӵ�����.����[i] = 50;

        ���ӵ�����.�������� = "�⺻";
        ���ӵ�����.������� = "�⺻";

        ���ӵ�����.��� = "..�Ա���......\n�ȳ�....?";

        ���ӵ�����.�� = 6;
        ���ӵ�����.�� = 0;
        ���ӵ�����.�� = 1;

        ���ӵ�����.���ð��� = �ð���.��;
        ���ӵ�����.���ð��� = �ð���.��;
    }
    void ���¸�����() { ���¿Ϸ�(���ӵ�����); }

    
    public void �̺�Ʈ����()
    {
        controller.�̺�Ʈ -= ��ġ����;
        controller.�̺�Ʈ -= �̺�Ʈ������;

        controller.��Ա� -= ��Ա�;
        controller.��Ա� -= ���ĸ�����;

        controller.���ڱ� -= ���ڱ�;
        controller.���ڱ� -= ���ĸ�����;

        controller.���� -= �����͸���;
        controller.���� -= ���¸�����;
    }
}
