using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfessorInfo : MonoBehaviour
{
    // 교수 이름, 학과 등을 저장할 리스트
    List<string> professorNames = new List<string> { "문형근", "김지은", "박태성", "최보희", "김상만", "정의진", "마이클 잭슨", "나영석", "백정원", "정상철", "박찬욱", "빌 게이츠" };
    List<string> kind = new List<string> { "조교수", "부교수", "정교수" };
    List<string> departments = new List<string> { "문과대학", "이과대학", "상경대학", "공과대학", "의과대학" };

    // Professor 정보를 담을 구조체
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

    // 3명의 교수 정보를 저장할 변수
    Professor Professor1;
    Professor Professor2;
    Professor Professor3;

    // AddProfessorScreen 프리팹 참조
    public GameObject addProfessorScreenPrefab;

    void OnEnable()
    {
        // 3명의 교수 정보를 생성
        Professor1 = GenerateRandomProfessor();
        Professor2 = GenerateRandomProfessor();
        Professor3 = GenerateRandomProfessor();

        // UI에 교수 정보 업데이트
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

    // UpdateProfessorUI 메서드 내부에서 변수를 선언
    void UpdateProfessorUI(Professor professor, string professorListName)
    {
        GameObject professorList = GameObject.Find(professorListName);
        if (professorList != null)
        {
            Text professorNameText = professorList.transform.Find("ProfessorName").GetComponent<Text>();
            Text salaryText = professorList.transform.Find("HopeSalary").GetComponent<Text>();
            Text departmentText = professorList.transform.Find("Kind").GetComponent<Text>();

            professorNameText.text = "교수 이름: " + professor.name;
            salaryText.text = "교수 직급: " + professor.kind;
            departmentText.text = "학과: " + professor.department;

            // EmployButton을 찾아 클릭 이벤트 추가 및 비활성화
            Button employButton = professorList.transform.Find("EmployButton")?.GetComponent<Button>();
            if (employButton != null)
            {
                employButton.onClick.AddListener(() => OnEmployButtonClick(professorListName));
            }
        }
    }

    // OnEmployButtonClick 메서드 수정
    void OnEmployButtonClick(string professorListName)
    {
        // 교수 수 증가
        GameManager.Instance.IncreaseProfessorCount(1);

        // 교수의 직급에 따라 골드 차감
        float goldCost = 0;

        // 예시로 Professor1 사용 (필요에 따라 수정)
        if (Professor1.kind == "조교수")
        {
            goldCost = 500;
        }
        else if (Professor1.kind == "부교수")
        {
            goldCost = 500;
        }
        else if (Professor1.kind == "정교수")
        {
            goldCost = 1000;
        }

        // 골드 차감
        GameManager.Instance.UpdateGold(-goldCost);

        Debug.Log("교수 고용됨: " + Professor1.name + ", 골드 차감: " + goldCost);

        // EmployButton 비활성화
        GameObject professorList = GameObject.Find(professorListName);
        if (professorList != null)
        {
            Button employButton = professorList.transform.Find("EmployButton")?.GetComponent<Button>();
            if (employButton != null)
            {
                employButton.interactable = false;  // 버튼 비활성화
            }
        }
    }


}
