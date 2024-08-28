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

    int minXAxisPosition => - xAxisNum / 2 * length;
    int maxXAxisPosition => - xAxisNum / 2 * length + (xAxisNum - 1) * length;
    int minZAxisPosition => - zAxisNum / 2 * length;
    int maxZAxisPosition => - zAxisNum / 2 * length + (zAxisNum - 1) * length;
    // private void OnThingHappened()
    // {
    //     // this function is executed when subjectToObserve changes 
    //     Debug.Log("Observer responds");
    // }
    private void OnThingHappened()
    {       
        isConstructionMode = true; // 건설 모드 활성화
        Debug.Log("Observer responds");
    }

    private void UserBuild()
    {
        if (isConstructionMode && Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼을 클릭했는지 확인
        {   
            Debug.Log("mouse click!!");
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
                
            }
            isConstructionMode = false;
        }
    }


    private Graph graph; // save tile position as graph


    public override void Awake()
    {
        base.Awake(); // singleton class awake method executed
        if (subjectToObserve != null)
        {
            subjectToObserve.ThingHappened += OnThingHappened;
        }
    }
    void Start()
    {
        CreateTileGrid(xAxisNum, zAxisNum, length);

        (int, int) startNodeKey = (0,0); 
        (int, int) endNodeKey = (10,10);
        List<Vector3> shortestPath = graph.GetShortestPath(startNodeKey, endNodeKey);

        foreach (Vector3 p in shortestPath)
        {
            Debug.Log($"path {p.x}, {p.z}");
        }

        BuildObjectOnTile(basicBuilding, 4, 4);
        BuildObjectOnTile(basicBuilding, 7, 7);
    }

    // Update is called once per frame
    void Update()
    {
        UserBuild();
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
                occupiedTiles.Add((startX + i * length, startZ + j * length));
            }
        }

        if (isConstructable(occupiedTiles))
        {   
            Debug.Log("constructable!!!!!");
            Vector3 position = new Vector3(xPosition, (float) (yLength / 2.0), zPosition);
            Instantiate(building, position, Quaternion.identity);
            graph.ChangeNodeOccupiedState(occupiedTiles, true);
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
}
