using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfessorInfo : MonoBehaviour
{
    // ���� �̸�, �а� ���� ������ ����Ʈ
    List<string> professorNames = new List<string> { "������", "������", "���¼�", "�ֺ���", "���", "������", "����Ŭ �轼", "������", "������", "����ö", "������", "�� ������" };
    List<string> kind = new List<string> { "������", "�α���", "������" };
    List<string> departments = new List<string> { "��������", "�̰�����", "������", "��������", "�ǰ�����" };

    // Professor ������ ���� ����ü
    public struct Professor
    {
        public string name;
        public string kind;
        public string department;

        public Professor(string name, string kind, string department)
        {
            this.name = name;
            this.kind = kind;
            this.department = department;
        }
    }

    // 3���� ���� ������ ������ ����
    Professor Professor1;
    Professor Professor2;
    Professor Professor3;

    // AddProfessorScreen ������ ����
    public GameObject addProfessorScreenPrefab;

    void OnEnable()
    {
        // 3���� ���� ������ ����
        Professor1 = GenerateRandomProfessor();
        Professor2 = GenerateRandomProfessor();
        Professor3 = GenerateRandomProfessor();

        // UI�� ���� ���� ������Ʈ
        UpdateProfessorUI(Professor1, "ProfessorList");
        UpdateProfessorUI(Professor2, "ProfessorList2");
        UpdateProfessorUI(Professor3, "ProfessorList3");
    }

    Professor GenerateRandomProfessor()
    {
        string randomName = professorNames[Random.Range(0, professorNames.Count)];
        string randomKind = kind[Random.Range(0, kind.Count)];
        string randomDepartment = departments[Random.Range(0, departments.Count)];

        return new Professor(randomName, randomKind, randomDepartment);
    }

    // UpdateProfessorUI �޼��� ���ο��� ������ ����
    void UpdateProfessorUI(Professor professor, string professorListName)
    {
        GameObject professorList = GameObject.Find(professorListName);
        if (professorList != null)
        {
            Text professorNameText = professorList.transform.Find("ProfessorName").GetComponent<Text>();
            Text salaryText = professorList.transform.Find("HopeSalary").GetComponent<Text>();
            Text departmentText = professorList.transform.Find("Kind").GetComponent<Text>();

            professorNameText.text = "���� �̸�: " + professor.name;
            salaryText.text = "���� ����: " + professor.kind;
            departmentText.text = "�а�: " + professor.department;

            // EmployButton�� ã�� Ŭ�� �̺�Ʈ �߰� �� ��Ȱ��ȭ
            Button employButton = professorList.transform.Find("EmployButton")?.GetComponent<Button>();
            if (employButton != null)
            {
                employButton.onClick.AddListener(() => OnEmployButtonClick(professorListName));
            }
        }
    }

    // OnEmployButtonClick �޼��� ����
    void OnEmployButtonClick(string professorListName)
    {
        // ���� �� ����
        GameManager.Instance.IncreaseProfessorCount(1);

        // ������ ���޿� ���� ��� ����
        float goldCost = 0;

        // ���÷� Professor1 ��� (�ʿ信 ���� ����)
        if (Professor1.kind == "������")
        {
            goldCost = 500;
        }
        else if (Professor1.kind == "�α���")
        {
            goldCost = 500;
        }
        else if (Professor1.kind == "������")
        {
            goldCost = 1000;
        }

        // ��� ����
        GameManager.Instance.UpdateGold(-goldCost);

        Debug.Log("���� ����: " + Professor1.name + ", ��� ����: " + goldCost);

        // EmployButton ��Ȱ��ȭ
        GameObject professorList = GameObject.Find(professorListName);
        if (professorList != null)
        {
            Button employButton = professorList.transform.Find("EmployButton")?.GetComponent<Button>();
            if (employButton != null)
            {
                employButton.interactable = false;  // ��ư ��Ȱ��ȭ
            }
        }
    }


}
