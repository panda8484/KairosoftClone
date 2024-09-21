using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToggleButtons : MonoBehaviour
{
    [System.Serializable]
    public class ScreenConfig
    {
        public string screenName;       // ��ũ�� �̸� (Button �̸��� ���� ����)
        public GameObject screenPrefab; // ������ Screen ������
        [HideInInspector] public GameObject currentScreen; // ���� ������ Screen ������Ʈ ����
    }

    public Canvas canvas;                // Canvas ����
    public ScreenConfig[] screens;       // �� ��ũ�� ������ �� ���� ���� �迭
    public Button[] buttons;             // ��ư �迭

    private void Start()
    {
        // �� ��ư�� Ŭ�� �̺�Ʈ ����
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // �̺�Ʈ�� Ŭ���� ���� ������ ���� ���� ���� ���
            buttons[i].onClick.AddListener(() => ToggleScreenObject(index));
        }
    }

    // �ش� index�� ��ư�� ������ �� ȣ��Ǵ� �޼���
    public void ToggleScreenObject(int index)
    {
        if (screens[index].currentScreen == null)
        {
            // Screen ������ ����
            screens[index].currentScreen = Instantiate(screens[index].screenPrefab, canvas.transform);

            // Canvas �߾ӿ� ��ġ ����
            RectTransform rectTransform = screens[index].currentScreen.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;

            // �ݱ� ��ư �̺�Ʈ ���
            Transform closingButtonTransform = screens[index].currentScreen.transform.Find("CloseButton");
            if (closingButtonTransform != null)
            {
                Button closingButton = closingButtonTransform.GetComponent<Button>();
                if (closingButton != null)
                {
                    closingButton.onClick.AddListener(() => CloseScreen(index));
                }
            }

            // �巡�� ��� �߰�
            AddDragFunctionality(screens[index].currentScreen);
        }
    }

    // ��ũ���� �ݴ� �޼���
    private void CloseScreen(int index)
    {
        if (screens[index].currentScreen != null)
        {
            Destroy(screens[index].currentScreen);
            screens[index].currentScreen = null;
        }
    }

    // �巡�� ��� �߰� �޼���
    private void AddDragFunctionality(GameObject screen)
    {
        // �θ� ������Ʈ���� �巡�� �̺�Ʈ�� ó���ϵ��� ����
        DragHandler dragHandler = screen.AddComponent<DragHandler>();

        // �巡�װ� ������ RectTransform �� Canvas�� ����
        RectTransform rectTransform = screen.GetComponent<RectTransform>();
        dragHandler.Initialize(rectTransform, canvas);
    }
}

// �巡�� �̺�Ʈ ó�� Ŭ����
public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalPosition;

    // �ʱ�ȭ �޼���
    public void Initialize(RectTransform rectTransform, Canvas canvas)
    {
        this.rectTransform = rectTransform;
        this.canvas = canvas;
    }

    // �巡�� ���� �� ȣ��
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
    }

    // �巡�� �� ȣ��
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = originalPosition + eventData.delta / canvas.scaleFactor;
    }
}
