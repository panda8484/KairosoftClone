using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovementManager : Singleton<UnitMovementManager>
{
    [SerializeField]
    private List<UnitMover> unitMovers; // 모든 UnitMover 인스턴스를 추적하는 리스트

    void Start()
    {
        // 초기화할 수 있는 로직이 필요한 경우 여기에 작성
    }

    void Update()
    {
        // UpdateAllUnits();
    }

    // 모든 유닛의 위치를 업데이트하는 메소드
    private void UpdateAllUnits()
    {
        foreach (UnitMover mover in unitMovers)
        {
            // 여기에 각 유닛을 업데이트하는 로직을 구현
            // 예: 타겟 위치를 재설정하거나, 경로를 재계산하는 로직 등
            // mover.MoveUnit(mover.unitToMove, mover.GetCurrentTargetPosition());
            // mover.MoveUnit((10,10));
            mover.StartMoveUnit((10,10));
            
        }
    }


    // 유닛을 추가하는 메소드
    public void AddUnitMover(UnitMover mover)
    {
        if (!unitMovers.Contains(mover))
        {
            unitMovers.Add(mover);
        }
    }

    // 유닛을 제거하는 메소드
    public void RemoveUnitMover(UnitMover mover)
    {
        if (unitMovers.Contains(mover))
        {
            unitMovers.Remove(mover);
        }
    }
}
