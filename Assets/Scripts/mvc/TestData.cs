using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestData : MonoBehaviour
{
    [Header("- ����")]
    public int ����;
    public int ������;
    public int �Ƶ巹����;
    public int �α�;
    public int �ŷ�;
    public int �ָӴϻ���;

    [Header("- ī��Ʈ")]
    public int ��ī��Ʈ;
    public int ��ī��Ʈ;

    [Header("�ΰ�����")]
    public int ���͸�;
    public int ������;
    public int �Ƿε�;

    [Header("�ð�")]
    public int ��;
    public int ��;
    public int ��;
    enum �ð� { ��, �� };
    [SerializeField]�ð� ���ð���, ���ð���;

    [Header("���� & ���")]
    public string ��������;
    public string �������;
    public string ���;
    
    public void DataSet(player.GameData ������)
    {
        ���� = ������.����[0];
        ������ = ������.����[1];
        �Ƶ巹���� = ������.����[2];
        �α� = ������.����[3];
        �ŷ� = ������.����[4];
        �ָӴϻ��� = ������.����[5];

        ��ī��Ʈ = ������.��ī��Ʈ;
        ��ī��Ʈ = ������.��ī��Ʈ;

        ���͸� = ������.���͸�;
        ������ = ������.������;
        �Ƿε� = ������.�Ƿε�;

        �� = ������.��;
        �� = ������.��;
        �� = ������.��;

        ���ð��� = (�ð�)������.���ð���;
        ���ð��� = (�ð�)������.���ð���;

        �������� = ������.��������;
        ������� = ������.�������;
        ��� = ������.���;
    }
}
