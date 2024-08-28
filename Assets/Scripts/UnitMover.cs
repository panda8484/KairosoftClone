using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

// buildmanager와 옵저버패턴을 적용해서 맵의 상태가 바꿨는지 확인 가능
// 맵 상태가 바꿨으면 새 경로 탐색하는 코드 추가하기

// this class controls unit movement on tile
public class UnitMover : MonoBehaviour
{   

    [SerializeField] GameObject unit;
    [SerializeField] float yAxis;
    // Start is called before the first frame update
    void Start()
    {
        TestInstanciate(unit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void TestInstanciate(GameObject unit)
    {   
        Vector3 position = new Vector3(0,yAxis,0);
        Instantiate(unit, position, Quaternion.identity);
    }

    void MoveUnit(GameObject unit)
    {
        int unitXposition = (int) Math.Round(unit.transform.position.x);
        int unitZposition = (int) Math.Round(unit.transform.position.z);

    }
}
