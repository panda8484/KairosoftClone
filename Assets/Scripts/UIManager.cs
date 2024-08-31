using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI  studentCountText;
    [SerializeField] private TextMeshProUGUI  goldText;
    [SerializeField] private TextMeshProUGUI  professorCountText;
    [SerializeField] private TextMeshProUGUI  fameCountText;
    [SerializeField] private TextMeshProUGUI  monthText;
    [SerializeField] private Button hireProfessorButton;

    void Start()
    {
        GameManager.Instance.OnStudentCountChanged += UpdateStudentCount;
        GameManager.Instance.OnGoldChanged += UpdateGold;
        GameManager.Instance.OnProfessorCountChanged += UpdateProfessorCount;
        GameManager.Instance.OnFameChanged += UpdateFameCount;
        GameManager.Instance.OnSeasonChanged += UpdateMonth;

        hireProfessorButton.onClick.AddListener(OnHireProfessorButtonClicked);

        // 초기 UI 업데이트
        UpdateStudentCount(GameManager.Instance.studentNum);
        UpdateGold(GameManager.Instance.gold);
        UpdateProfessorCount(GameManager.Instance.professorNum);
        UpdateFameCount(GameManager.Instance.fame);
    }

    private void OnHireProfessorButtonClicked()
    {
        // 교수 수를 1명 증가시킴
        GameManager.Instance.IncreaseProfessorCount(1);
    }

    private void UpdateStudentCount(int count)
    {
        studentCountText.text = $"Students: {count}";
    }

    private void UpdateGold(float amount)
    {
        goldText.text = $"Gold: {amount}";
    }

    private void UpdateProfessorCount(int count)
    {
        professorCountText.text = $"Professors: {count}";
    }

    private void UpdateFameCount(float amount)
    {
        fameCountText.text = $"Fame: {amount}";
    }
    private void UpdateMonth(int season)
    {
        monthText.text = $"Month: {season}";
    }

    void OnDestroy()
    {
        // 이벤트 구독 해제
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStudentCountChanged -= UpdateStudentCount;
            GameManager.Instance.OnGoldChanged -= UpdateGold;
            GameManager.Instance.OnProfessorCountChanged -= UpdateProfessorCount;
            GameManager.Instance.OnFameChanged -= UpdateFameCount;
        }
    }
}
