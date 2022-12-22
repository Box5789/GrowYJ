using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    //���� ���� ��Ȳ ���� ����
    string GameDataFileName = "GameDataFile.json";


    //�̺�Ʈ
    [Header("- �̺�Ʈ")]
    public int Ŭ����ư;
    public int ������;
    GameObject[] �̺�Ʈ��ư�� = new GameObject[4];
    GameObject ��, ��;
    �÷��̾� ������;
    public List<Dictionary<int, EventData>> �̺�Ʈ������ = new List<Dictionary<int, EventData>>();

    enum ��ư { ����, ����, ���, �� }

    [Header("- �ð� ���")]
    [SerializeField] private int ��ð� = 60;
    [SerializeField] private int ��ð� = 480;

    [Header("- ������Ʈ")]
    [SerializeField] private GameObject �ð����̹���;
    [SerializeField] private GameObject ����̹���;
    [SerializeField] private float �ش��̼�;
    [SerializeField] private float ����̼�;
    Coroutine �ڷ�ƾ = null;
    Transform ���t;
    RectTransform �ش�rt;
    bool ��� = false, �ش� = false;


    //���� ����
    public enum ����enum { ����, ������, �Ƶ巹����, �α�, �ŷ�, �ָӴϻ��� }
    /*
    public struct ����
    {
        int �̸�, �̻�;
        string �ִϸ��̼�;

        public ����(int �̸�, int �̻�, string �ִϸ��̼�)
        {
            this.�̸� = �̸�;
            this.�̻� = �̻�;
            this.�ִϸ��̼� = �ִϸ��̼�;
        }

        public int Get�̸�() { return �̸�; }
        public int Get�̻�() { return �̻�; }
        public string Get�ִ�() { return �ִϸ��̼�; }
    }*/
    //public Dictionary<����enum, List<����>> ���ȿ��ص����� = new Dictionary<����enum, List<����>>();



    //������ Ŭ���� & �̱���
    [Serializable]
    public class GameData
    {
        [Header("- ��, ��")]
        public int ��ī��Ʈ;
        public int ��ī��Ʈ;

        public int ���͸�;
        public int ������;
        public int �Ƿε�;

        [Header("- ����(���� ������ �Ƶ巹���� �α� �ŷ� �ָӴϻ���)")]
        public int[] ���� = { 50, 50, 50, 50, 50, 50 };
        public string ���� = "�⺻";

        [Header("- �ð�")]
        public int ��;
        public int ��;
        public int ��;
        public TL �ð��� = TL.��;
        public enum TL { ��, �� };

        public void InitData()
        {
            ��ī��Ʈ = 0;
            ��ī��Ʈ = 0;

            ���͸� = 5;
            ������ = 10;
            �Ƿε� = 10;

            ���� = new int[6];
            for (int i = 0; i < ����.Length; i++)
                ����[i] = 50;

            �� = 6;
            �� = 0;
            �� = 0;

            SetData();
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

            ���ȼ�ġ�˻�();
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

            ���ȼ�ġ�˻�();
        }

        void ���ȼ�ġ�˻�()//������ ��ȭ ���� ������ �ӽ� : 20,80
        {
            Animator anim = GameObject.FindGameObjectWithTag("������").GetComponent<Animator>();

            List<List<int>> list = new List<List<int>>();
            for (int i = 0; i < ����.Length; i++)
                list.Add(new List<int> { i, ����[i] });
            var ������ = list.OrderBy(x => x[1]).ToArray();

            int row = ������[0][1], high = ������[5][1];

            if (row > 100 - high && high >= 80)//�ְ���
            {
                if (!����.Equals("�⺻"))
                    anim.SetBool(����, false);
                ���� = ((����enum)������[5][0]).ToString() + "_����";
                anim.SetBool(����, true);
            }
            else if (row < 100 - high && row < 20)//������
            {
                if (!����.Equals("�⺻"))
                    anim.SetBool(����, false);
                ���� = ((����enum)������[0][0]).ToString() + "_����";
                anim.SetBool(����, true);
            }
            else if (!����.Equals("�⺻"))
            {
                anim.SetBool(����, false);
                ���� = "�⺻";
            }
        }

        public bool �ð�����(int m)
        {
            �� += (�� + m) / 60;
            if (�� >= 24)
            {
                �� += (�� + (�� + m) / 60) / 24;
                �� %= 24;
            }
            �� = (�� + m) % 60;

            return true;
        }
        
    }
    [Header("- [ ���� ������ ] --------------------")]
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
        ������ = GameObject.FindGameObjectWithTag
            ("������").GetComponent<�÷��̾�>();


        ���t = ����̹���.GetComponent<Transform>();
        �ش�rt = �ð����̹���.GetComponent<RectTransform>();

        //��&�� ��ư
        �� = GameObject.FindGameObjectsWithTag("�̺�Ʈ��ư")[0];
        �� = GameObject.FindGameObjectsWithTag("�̺�Ʈ��ư")[1];
        ��.GetComponent<Button>().onClick.AddListener(delegate { ����(��ð�); });
        ��.GetComponent<Button>().onClick.AddListener(delegate { ����(��ð�); });

        //�̺�Ʈ ��Ʈ ������ �ε�
        for (int i = 0; i < System.Enum.GetValues(typeof(��ư)).Length; i++)
        {
            //Event DataSet�о����
            �̺�Ʈ������Read(((��ư)i).ToString());
            //�̺�Ʈ ��ư ã��
            �̺�Ʈ��ư��[i] = GameObject.FindGameObjectsWithTag("�̺�Ʈ��ư")[i + 2];
            //�̺�Ʈ ��ư�� Ŭ�� �Լ� �߰��ϱ�
            int x = i;
            �̺�Ʈ��ư��[i].GetComponent<Button>().onClick.AddListener(delegate {
                if (������.Get���͸�() > 0) //���͸� ������ �̺�Ʈ �߻� X
                    �̺�Ʈ�߻�(x);
            });
        }

        //���ȿ� ���� ������ �ε�
        //���ȿ���Read("���ȿ���(�׽�Ʈ)");

        //���� �ε�
        ���Ⱥҷ�����();

        //�ð��� Ȯ��
        �ð���Ȯ��();
    }



    private void �̺�Ʈ������Read(string path_name)
    {
        Dictionary<int, EventData> �ӽõ����� = new Dictionary<int, EventData>();

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
                int[] ���� = new int[6];

                int key = int.Parse(data_values[i++]);
                string �̸� = data_values[i++];
                string ���� = data_values[i++];
                string ��� = data_values[i++];

                int ������ = int.Parse(data_values[i++]);
                int �Ƿε� = int.Parse(data_values[i++]);

                for (int j = 0; j < ����.Length; j++)
                    ����[j] = int.Parse(data_values[i++]);

                int �ð� = int.Parse(data_values[i++]);
                �ӽõ�����.Add(key, new EventData(�̸�, ����, ���, ������, �Ƿε�, ����, �ð�));

                /* �̷��� �ص� �ǳ� >> ����� �� csv���� ��������� �׽�Ʈ
                    �ӽõ�����.Add(int.Parse(data_values[i++]), new EventData(data_values[i++], 
                data_values[i++], data_values[i++], int.Parse(data_values[i++]), 
                int.Parse(data_values[i++]), int.Parse(data_values[i++]), int.Parse(data_values[i++]), 
                int.Parse(data_values[i++]), int.Parse(data_values[i++]), int.Parse(data_values[i++]), 
                int.Parse(data_values[i++]), int.Parse(data_values[i++]), int.Parse(data_values[i++])));
                    */
            }
            else first = false;
        }

        �̺�Ʈ������.Add(�ӽõ�����);
    }

    /*
    private void ���ȿ���Read(string path_name)
    {
        List<����> �ӽõ����� = new List<����>();

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
                for (int i = 0; i < Enum.GetValues(typeof(����)).Length; i++)
                {
                    var data_values = data_String.Split('/');
                    while (data_values[0] == ((����)i).ToString())
                    {
                        ���� �ӽ�����1 = new ����(int.Parse(data_values[1]), int.Parse(data_values[2]), data_values[3]);
                        �ӽõ�����.Add(�ӽ�����1);
                        data_values = data_String.Split('/');
                    }
                    ���ȿ��ص�����.Add((����)i, �ӽõ�����);

                    �ӽõ�����.Clear();

                    ���� �ӽ�����2 = new ����(int.Parse(data_values[1]), int.Parse(data_values[2]), data_values[3]);
                    �ӽõ�����.Add(�ӽ�����2);
                }
                ���ȿ��ص�����.Add(Enum.GetValues(typeof(����)).Cast<����>().Last(), �ӽõ�����);

                end = true;
            }
            else first = false;
        }
    }
    */



    void �ð���Ȯ��()
    {
        int �ð��˻� = (_gameData.�� + 18) % 24;

        if (���t.position.y > 0 && �ð��˻� >= 12)
        {
            _gameData.�ð��� = GameData.TL.��;
            if (!�ش� && !���)
            {
                if (�ڷ�ƾ != null)
                    StopCoroutine(�ڷ�ƾ);
                �ڷ�ƾ = StartCoroutine(������());
            }
        }
        else if (���t.position.y < 0 && �ð��˻� < 12)
        {
            _gameData.�ð��� = GameData.TL.��;
            if (!�ش� && !���)
            {
                if (�ڷ�ƾ != null)
                    StopCoroutine(�ڷ�ƾ);
                StartCoroutine(������());
            }
        }
    }
    IEnumerator ������()
    {
        while (true)
        {
            //��� �̵�
            //�Ҽ� ��� ���� ������ int�� ��ȯ�ؼ� ���
            int �����ġ = (int)(���t.position.y * 10f);
            int ����ǥ = (int)(���t.localScale.y * 100f);

            if (�����ġ < ����ǥ)
                ���t.position = Vector2.MoveTowards(���t.position, new Vector2(0, (���t.localScale.y * 3)), ����̼�);
            else ��� = true;

            //��&�� ����
            if (�ش�rt.anchoredPosition.y > (�ش�rt.rect.height / -4))
                �ش�rt.anchoredPosition = Vector2.MoveTowards(�ش�rt.anchoredPosition, new Vector2(0, (�ش�rt.rect.height / -4)), �ش��̼�);
            else �ش� = true;

            if (�ش� && ���)
            {
                �ش� = false;
                ��� = false;
                yield break;
            }
            else
                yield return new WaitForSecondsRealtime(0.01f);
        }
    }
    IEnumerator ������()
    {
        while (true)
        {
            //��� �̵�
            //�Ҽ� ��� ���� ������ int�� ��ȯ�ؼ� ���
            int �����ġ = (int)(���t.position.y * 10f);
            int ����ǥ = (int)(���t.localScale.y * -100f);

            if (�����ġ > ����ǥ)
                ���t.position = Vector2.MoveTowards(���t.position, new Vector2(0, (���t.localScale.y * -3)), ����̼�);
            else
                ��� = true;

            //��&�� ����
            if (�ش�rt.anchoredPosition.y < (�ش�rt.rect.height / 4))
                �ش�rt.anchoredPosition = Vector2.MoveTowards(�ش�rt.anchoredPosition, new Vector2(0, (�ش�rt.rect.height / 4)), �ش��̼�);
            else �ش� = true;

            if (�ش� && ���)
            {
                �ش� = false;
                ��� = false;
                yield break;
            }
            else
                yield return new WaitForSecondsRealtime(0.01f);
        }
    }




    void ����(int m)
    {
        bool check = _gameData.�ð�����(m);
        if (check) �ð���Ȯ��();
    }

    public void �̺�Ʈ�߻�(int num)
    {
        Ŭ����ư = num;

        //������ ���� - ���� Ȯ�� ����
        ������ = UnityEngine.Random.Range(0, �̺�Ʈ������[num].Count);
        EventData �߻��̺�Ʈ = �̺�Ʈ������[Ŭ����ư][������];

        //�÷��̾� ���� ����
        ������.�̺�Ʈ�߻�(�߻��̺�Ʈ);

        //�ð� ����
        bool check = _gameData.�ð�����(�߻��̺�Ʈ.Get�ð�());
        if (check) �ð���Ȯ��();

        //���� ����
        ��������();

        //���� â ���� ��� �� �ִϸ��̼�&ȭ�� ȿ��
    }


    public void �����ʱ�ȭ()
    {
        _gameData.InitData();
        string filePath = Application.persistentDataPath + GameDataFileName;
        System.IO.File.Delete(filePath);
    }

    public void ���Ⱥҷ�����()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;

        if (File.Exists(filePath))//������ �ִٸ�
        {
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
            _gameData.SetData();
        }
        else//������ ���ٸ�
        {
            _gameData = new GameData();
            _gameData.InitData();
            ��������();
        }
    }

    public void ��������()
    {
        _gameData.SaveData();

        string ToJsonData = JsonUtility.ToJson(_gameData);
        string filePath = Application.persistentDataPath + GameDataFileName;

        File.WriteAllText(filePath, ToJsonData);
    }
}
