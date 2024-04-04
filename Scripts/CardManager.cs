using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
public class CardManager : MonoBehaviour, IDragAndDropEvent, IPointerUpHandler, IPointerDownHandler
{
    private Card _cardSO;

    public Card CardSO
    {
        get => _cardSO;
        set { _cardSO = value; } 
    }
    private GameObject _draggingBuilding;
    private Building _building;

    private Vector2Int _gridSize = new Vector2Int(10, 15);

    private bool _isAvailableToBuild;

    private Building[,] _grid;

    private void Awake()
    {
        _grid = new Building[_gridSize.x, _gridSize.y];
    }


    public void OnDrag(PointerEventData eventData)
    {
        if (_draggingBuilding != null)
        {
            var groundLane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (groundLane.Raycast(ray, out float pos))
            {
                Vector3 worldPostion = ray.GetPoint(pos);
                int x = Mathf.RoundToInt(worldPostion.x);
                int y = Mathf.RoundToInt(worldPostion.y);

                if (x < 0 || x > _gridSize.x - _building.BuildingSize.x)
                {
                    _isAvailableToBuild = false;
                }
                else if (y < 0 || y > _gridSize.y - _building.BuildingSize.y)
                {
                    _isAvailableToBuild = false;
                }
                else
                {
                    _isAvailableToBuild = true;
                }

                if(_isAvailableToBuild && isPlaceTaken(x,y))
                {
                    _isAvailableToBuild = false;
                }

                if((x % 2 == 1) || (y % 2 == 1))
                {
                    _isAvailableToBuild = false;
                }

                _draggingBuilding.transform.position = new Vector3(x, y, 0);

                _building.SetColor(_isAvailableToBuild);
            }
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _draggingBuilding = Instantiate(_cardSO.prefab, Vector3.zero, Quaternion.identity);
        _building = _draggingBuilding.GetComponent<Building>();
        var groundLane = new Plane(Vector3.up, Vector3.zero);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(groundLane.Raycast(ray, out float pos))
        {
            Vector3 worldPostion = ray.GetPoint(pos);
            int x = Mathf.RoundToInt(worldPostion.x);
            int y = Mathf.RoundToInt(worldPostion.y);

            _draggingBuilding.transform.position = new Vector3(x, y, 0);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!_isAvailableToBuild)
            Destroy(_draggingBuilding);
        else
        {
            _grid[(int)_draggingBuilding.transform.position.x, (int)_draggingBuilding.transform.position.y] = _building;
        }
    }

    private bool isPlaceTaken(int x , int y)
    {
        if (_grid[x,y] != null)
        {
            return true;
        }
        return false;
    }

}
