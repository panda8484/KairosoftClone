using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static BuildingInfoManager;

public class BuildingInfoManager : MonoBehaviour
{
    // Building ����ü ����
    public struct Building
    {
        public string name;         // �ǹ� �̸�
        public int cost;            // �ʿ� ���
        public string size;         // �Ը� (����*����*����)
        public int capacity;        // �л� ����
        public float reputation;    // ��

        // ������
        public Building(string name, int cost, string size, int capacity, float reputation)
        {
            this.name = name;
            this.cost = cost;
            this.size = size;
            this.capacity = capacity;
            this.reputation = reputation;
        }
    }

    // �ǹ� ������ ������ ����Ʈ
    List<Building> buildingList = new List<Building>();

    // ScrollView�� Content ���� (�̹� �����ϴ� BuildingInfo���� ������)
    public RectTransform contentTransform;

    // ���� �Ŵ��� ����
    private GameManager gameManager;

    // BuildScene�� Ȱ��ȭ�� �� ȣ��
    void OnEnable()
    {
        // GameManager ������Ʈ ã�� �� gameManager �Ҵ�
        GameObject managerObject = GameObject.Find("GameManager");
        if (managerObject != null)
        {
            gameManager = managerObject.GetComponent<GameManager>();
            if (gameManager == null)
            {
                Debug.LogError("GameManager ��ũ��Ʈ�� ã�� �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogError("GameManager ������Ʈ�� ã�� �� �����ϴ�.");
        }

        // ���� �ǹ� ������ �߰�
        AddBuildingData();

        // �̹� �����ϴ� BuildingInfo�鿡 ���� ������Ʈ
        UpdateExistingBuildingInfoUI();
    }

    // ���� �ǹ� ������ �߰�
    void AddBuildingData()
    {
        buildingList.Add(new Building("���� ���ǽ�", 3000, "13*3*3", 20, 1));
        buildingList.Add(new Building("���� ���ǽ�", 5000, "3*6*3", 40, 2));
        buildingList.Add(new Building("�����", 15000, "6*6*6", 100, 5));
        buildingList.Add(new Building("�Ĵ�", 7000, "3*3*3", 20, 3));
        buildingList.Add(new Building("���", 100000, "10*10*1", 0, 10));
        buildingList.Add(new Building("����", 10, "1*1*1", 0, 0));
        buildingList.Add(new Building("����", 100, "1*1*2", 100, 0.01f));
        buildingList.Add(new Building("�⺻ Ÿ��", 0, "1*1*1", 0, 0));
    }

    // ���� BuildingInfo ������Ʈ�鿡 �ǹ� ���� �ݿ�
    void UpdateExistingBuildingInfoUI()
    {
        int childCount = contentTransform.childCount;

        for (int i = 0; i < Mathf.Min(buildingList.Count, childCount); i++)
        {
            Building building = buildingList[i];
            string buildingInfoName = "BuildingInfo" + (i + 1).ToString();

            try
            {
                Transform buildingInfoTransform = contentTransform.Find(buildingInfoName);

                Text buildingNameText = buildingInfoTransform.Find("BuildingName").GetComponent<Text>();
                Text goldText = buildingInfoTransform.Find("GoldText").GetComponent<Text>();
                Text sizeText = buildingInfoTransform.Find("SizeText").GetComponent<Text>();
                Text incomeText = buildingInfoTransform.Find("IncomeText").GetComponent<Text>();
                Text fameText = buildingInfoTransform.Find("FameText").GetComponent<Text>();

                buildingNameText.text = building.name;
                goldText.text = building.cost.ToString();
                sizeText.text = building.size;
                incomeText.text = building.capacity.ToString();
                fameText.text = building.reputation.ToString();

                Button installButton = buildingInfoTransform.Find("InstallButton").GetComponent<Button>();
                if (installButton != null)
                {
                    installButton.onClick.AddListener(() => InstallBuilding(building, buildingInfoName));
                }
            }
            catch (Exception e)
            {
                Debug.LogError("BuildingInfo ������Ʈ �� ���� �߻�: " + e.Message);
            }
        }
    }

    void InstallBuilding(Building building, string buildingInfoName)
    {
        GameObject buildingInfoObject = GameObject.Find(buildingInfoName);
        if (buildingInfoObject == null)
        {
            Debug.LogError("BuildingInfo ������Ʈ�� ã�� �� �����ϴ�: " + buildingInfoName);
            return;
        }

        Text buildingNameText = buildingInfoObject.transform.Find("BuildingName")?.GetComponent<Text>();
        if (buildingNameText == null)
        {
            Debug.LogError("'BuildingName' �ڽ� ��ü�� ã�� �� �����ϴ�: " + buildingInfoName);
            return;
        }

        if (building.name == buildingNameText.text)
        {
            if (gameManager != null)
            {
                int cost = building.cost;
                gameManager.UpdateGold(-cost);
                Debug.Log(building.name + " ��ġ �Ϸ�! ���: -" + cost + " ��� ������.");
            }
            else
            {
                Debug.LogError("GameManager�� �Ҵ���� �ʾҽ��ϴ�.");
            }

            Button installButton = buildingInfoObject.transform.Find("InstallButton")?.GetComponent<Button>();
        }
        else
        {
            Debug.LogError("�ǹ� �̸��� ��ġ���� �ʽ��ϴ�: " + building.name + " != " + buildingNameText.text);
        }
    }
}
