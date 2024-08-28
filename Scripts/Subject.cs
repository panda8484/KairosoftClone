using UnityEngine;
using UnityEngine.UI; // UI 컴포넌트 사용을 위해 추가
using System;

public class Subject : MonoBehaviour
{
    public Button myButton; // Unity Editor에서 할당할 버튼
    public event Action ThingHappened;

    void Start()
    {
        // 버튼의 onClick 이벤트에 메소드 연결
        if (myButton != null)
        {
            myButton.onClick.AddListener(DoThing);
        }
    }

    public void DoThing()
    {
        ThingHappened?.Invoke();
    }

    void OnDestroy()
    {
        // 리스너 제거
        if (myButton != null)
        {
            myButton.onClick.RemoveListener(DoThing);
        }
    }
}
