using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
// when user is in build mode, other ui should not be shown
// so when user is in build mode, we can consider stopping time in game like 
// void Update() -> other gameManager update function
// {
//     if (buildMode) return; -> put this code on top of update function
// }
// ?? Or is there any other way ??
// -> we can add UIManager class and control UI overlap
// this class get info of current ui mode such as (build mode, 교수채용모드, 등) from other manager class
public class BuildManager : Singleton<BuildManager>
{
    // Start is called before the first frame update
    public GameObject basicTile;
    public Building basicBuilding;
    bool isConstructionMode;

    [SerializeField] int xAxisNum = 100; // basic map x axis tile num
    [SerializeField] int zAxisNum = 100; // basic map z axis tile num
    [SerializeField] int length = 1; // tile prefab length of side
    [SerializeField] private Subject subjectToObserve;

    public int minXAxisPosition => - xAxisNum / 2 * length;
    public int maxXAxisPosition => - xAxisNum / 2 * length + (xAxisNum - 1) * length;
    public int minZAxisPosition => - zAxisNum / 2 * length;
    public int maxZAxisPosition => - zAxisNum / 2 * length + (zAxisNum - 1) * length;

    
    public (int,int) entrancePosition => ((minXAxisPosition + maxXAxisPosition) / 2, minZAxisPosition); //학생들이 들어오는 정문
    // private Dictionary<Building, (int centerX, int centerY)> buildingCenters = new Dictionary<Building, (int centerX, int centerY)>();
    // 빌딩의 ID를 키로 사용하여 중심 좌표를 저장하는 해시맵
    private Dictionary<int, (int centerX, int centerY)> buildingCenters = new Dictionary<int, (int centerX, int centerY)>();


    public Graph graph; // save tile position as graph
    public int totalBuildingsConstructedNum = 0;


    public override void Awake()
    {
        base.Awake(); // singleton class awake method executed
        if (subjectToObserve != null)
        {
            subjectToObserve.ThingHappened += OnThingHappened;
        }

        CreateTileGrid(xAxisNum, zAxisNum, length);

        (int, int) startNodeKey = (0,0); 
        (int, int) endNodeKey = (10,10);
        List<Vector3> shortestPath = graph.GetShortestPath(startNodeKey, endNodeKey);
    }
    void Start()
    {
        

        // foreach (Vector3 p in shortestPath)
        // {
        //     Debug.Log($"path {p.x}, {p.z}");
        // }
    }

    // Update is called once per frame
    void Update()
    {
        UserBuild();
    }


    private void OnThingHappened()
    {       
        isConstructionMode = true; // 건설 모드 활성화
        // Debug.Log("Observer responds");
    }

    private void UserBuild()
    {
        if (isConstructionMode && Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼을 클릭했는지 확인
        {   
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // hit.point는 클릭된 지점의 정확한 월드 좌표입니다.
                Vector3 buildPosition = hit.point;
                buildPosition.y = 0; // 지면 높이로 설정(높이 조정이 필요하면 변경)

                // 타일의 정확한 위치를 계산하기 위해 빌딩 크기의 반을 고려하여 정렬합니다.
                int xTilePosition = Mathf.FloorToInt(buildPosition.x / length) * length;
                int zTilePosition = Mathf.FloorToInt(buildPosition.z / length) * length;

                BuildObjectOnTile(basicBuilding, xTilePosition, zTilePosition);
                basicBuilding.setId(totalBuildingsConstructedNum);
                
            }
            totalBuildingsConstructedNum += 1;
            isConstructionMode = false;
        }
    }


    


    private void CreateTileGrid(int xAxisNum,  int zAxisNum, int length)  
    {   
        // float offset = 50.0f;
        Vector3 startPosition = new Vector3(minXAxisPosition, 0, minZAxisPosition);
        for (int i = 0; i < xAxisNum; i++)
        {
            for (int j = 0; j < zAxisNum; j++)
            {
                Vector3 position = new Vector3(startPosition.x + length * i, startPosition.y, startPosition.z + length * j);
                Instantiate(basicTile, position, Quaternion.identity);
            }
        }
        graph = new Graph(xAxisNum, zAxisNum, length);
    }


    private void BuildObjectOnTile(Building building, int xPosition, int zPosition)
    {
        
        int xLength = building.XLength;
        int yLength = building.YLength;
        int zLength = building.ZLength;
        List<(int,int)> occupiedTiles = new List<(int,int)> ();

        int startX = xPosition - (xLength - 1) / 2;
        int startZ = zPosition - (zLength - 1) / 2;


        for (int i = 0; i < xLength; i++)
        {
            for (int j = 0; j < zLength; j++)
            {   
                if (j == building.centerZ && i <= building.centerX) continue; // 빌딩 중심까지 빈칸으로 남겨둠
                occupiedTiles.Add((startX + i * length, startZ + j * length));
            }
        }




        if (isConstructable(occupiedTiles))
        {   
            int globalCoorBuilingCenterX = startX + building.centerX;
            int globalCoorBuilingCenterZ = startZ + building.centerZ;

            Debug.Log($"constructable!!!!!, building center is {globalCoorBuilingCenterX}, {globalCoorBuilingCenterZ}");
            
            Vector3 position = new Vector3(xPosition, (float) (yLength / 2.0), zPosition);
            Building newBuilding = Instantiate(building, position, Quaternion.identity);
            newBuilding.setId(totalBuildingsConstructedNum);
            Debug.Log($"building.Id: {newBuilding.Id}");

            GameManager.Instance.UpdateGold(-newBuilding.BaseCost);
            graph.ChangeNodeOccupiedState(occupiedTiles, true);
            buildingCenters[newBuilding.Id] = (globalCoorBuilingCenterX, globalCoorBuilingCenterZ);
        }
        else
        {
            Debug.Log("not constructable");
        }
    }

    // bool isConstructable(List<(int,int)> occupiedTiles)
    private bool isConstructable(List<(int x, int z)> TilePositionList)
    {
        foreach (var tilePositionPair in TilePositionList)
        {
            int xPosition = tilePositionPair.x;
            int zPosition = tilePositionPair.z;

            if (xPosition < minXAxisPosition) return false;
            if (xPosition > maxXAxisPosition) return false;
            if (zPosition < minZAxisPosition) return false;
            if (zPosition > maxZAxisPosition) return false;
            if (graph.isOccupied(tilePositionPair)) return false;
        }
        return true;
    }


    public (int centerX, int centerY) GetRandomBuildingCenter()
    {
        if (buildingCenters.Count > 0)
        {
            // 빌딩들이 존재할 경우, 랜덤한 빌딩을 선택하여 그 중심 좌표를 반환
            int randomIndex = UnityEngine.Random.Range(0, buildingCenters.Count);
            int randomBuildingId = 0;

            // Dictionary의 key 값 중 랜덤하게 하나 선택
            int currentIndex = 0;
            foreach (var building in buildingCenters.Keys)
            {
                if (currentIndex == randomIndex)
                {
                    randomBuildingId = building;
                    break;
                }
                currentIndex++;
            }

            return buildingCenters[randomBuildingId];
        }
        else
        {
            // 빌딩이 없을 경우, 랜덤한 좌표를 반환
            int randomX = UnityEngine.Random.Range(minXAxisPosition, maxXAxisPosition);
            int randomY = UnityEngine.Random.Range(minZAxisPosition, maxZAxisPosition);
            Debug.LogWarning("No buildings available. Returning random coordinates.");
            return (randomX, randomY);
        }
    }
}
