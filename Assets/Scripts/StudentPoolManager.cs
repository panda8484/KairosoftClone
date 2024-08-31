using System.Collections.Generic;
using UnityEngine;

public class StudentPoolManager : MonoBehaviour
{
    [SerializeField] private GameObject studentPrefab;
    private Queue<GameObject> studentPool = new Queue<GameObject>();
    private List<GameObject> activeStudents = new List<GameObject>(); // 활성화된 학생들을 추적하는 리스트
    private int initialPoolSize = 10;

    void Start()
    {
        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateStudent();
        }
    }

    GameObject CreateStudent()
    {
        GameObject student = Instantiate(studentPrefab);
        student.SetActive(false);
        studentPool.Enqueue(student);
        return student;
    }

    public GameObject GetStudent()
    {
        if (studentPool.Count == 0)
        {
            CreateStudent();
        }
        GameObject student = studentPool.Dequeue();
        student.SetActive(true);
        activeStudents.Add(student);
        return student;
    }

    public void ReturnStudent(GameObject student)
    {
        student.SetActive(false);
        activeStudents.Remove(student);
        studentPool.Enqueue(student);
    }

    public void IncreasePoolSize(int newSize)
    {
        int currentTotalSize = studentPool.Count + activeStudents.Count;
        while (currentTotalSize < newSize)
        {
            CreateStudent();
            currentTotalSize++;
        }
    }
}
