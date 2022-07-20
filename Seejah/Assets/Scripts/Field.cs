using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Field : MonoBehaviour
{
    private const int CellSize = 1;
    private const int FieldSize = 5;
    
    [SerializeField] private Cell CellPrototype;
    [SerializeField] private Chip ChipPrototype;
    private Dictionary<(int,int), Cell> _cells;
    private Chip _selectedChip;

    private void Start()
    {
        _cells = new Dictionary<(int, int), Cell>();
        GenerateField();
    }

    private void GenerateField()
    {
        for (int i = 0; i < FieldSize; i++)
        {
            for (int j = 0; j < FieldSize; j++)
            {
                var pos = new Vector3(i * CellSize, 0, j * CellSize);
                var cell = Instantiate(CellPrototype, pos, Quaternion.identity);
                cell.Setup(i, j, pos);
                _cells[(i,j)] = cell;
            }
        }
    }

    private void Update()
    {
        Vector3 point = default;
        if (Input.GetMouseButtonUp(0))
        {
            point = GetFieldPoint(Input.mousePosition);
            if (_selectedChip)
                TryMoveSelectedChipTo(point);
            else
                TrySelectChip(point);
            Debug.Log(point);
        }

        if (Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButtonUp(0))
        {
            point = GetFieldPoint(Input.mousePosition);
            GenerateChip(point.x, point.z, TeamType.TeamBlue);
            Debug.Log(point);
        }
        
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButtonUp(0))
        {
            point = GetFieldPoint(Input.mousePosition);
            GenerateChip(point.x, point.z, TeamType.TeamRed);
            Debug.Log(point);
        }
    }

    private void TryMoveSelectedChipTo(Vector3 pos)
    {
        if (GetCellInPosition(pos.x, pos.z, out var cell))
        {
            if (cell.chip != null)
            {
                Debug.Log("Wrong move! Already has chip");
            }
            else if (Math.Abs(cell.row - _selectedChip.cell.row) > 1 || Math.Abs(cell.col - _selectedChip.cell.col) > 1)
            {
                Debug.LogWarning("Wrong move! Distance");
            }
            else
            {
                var selectedChipTeam = _selectedChip.Team;
                MakeMove(pos, cell);
                CheckNeighbours(cell, selectedChipTeam);
            }
        }
    }

    private void CheckNeighbours(Cell cell, TeamType movingTeam)
    {
        var result = new List<Chip>();
        var variants = new List<(int,int)>
        {
            (-1, -1),
            (-1, 0),
            (-1,1),
            (0,-1),
            (0,1),
            (1,-1),
            (1,0),
            (1,1)
        };
        foreach (var shifts in variants)
        {
            if (_cells.TryGetValue((cell.row + shifts.Item1, cell.col + shifts.Item2), out var neighbourCell))
            {
                if (neighbourCell.chip != null && neighbourCell.chip.Team != movingTeam)
                {
                    if (_cells.TryGetValue((cell.row + shifts.Item1 * 2, cell.col + shifts.Item2 * 2),
                        out var otherPlayerCell))
                    {
                        if (otherPlayerCell.chip != null && otherPlayerCell.chip.Team == movingTeam)
                        {
                            result.Add(neighbourCell.chip);
                        }
                    }
                }
            }
        }
        
        foreach (var chip in result)
        {
            Debug.LogWarning($"chip {chip.cell.row}-{chip.cell.col}");
            DeleteChip(chip);
        }
    }

    private void DeleteChip(Chip chip)
    {
        chip.cell.chip = null;
        Destroy(chip.gameObject);
    }

    private void MakeMove(Vector3 pos, Cell cell)
    {
        var prevCell = _selectedChip.cell;
        _selectedChip.cell = cell;
        cell.chip = _selectedChip;
        var normPos = GetNormalizedPosition(pos.x, pos.z);
        _selectedChip.UpdatePos(normPos);
        prevCell.chip = null;
        _selectedChip = null;
    }

    private void TrySelectChip(Vector3 pos)
    {
        if (GetCellInPosition(pos.x, pos.z, out var cell))
        {
            if (cell.chip != null) 
                _selectedChip = cell.chip;
        }
    }

    private Vector3 GetFieldPoint(Vector3 mouseInputPos)
    {
        var mousePos = mouseInputPos;
        mousePos.z = Camera.main.transform.position.y - transform.position.y;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        return mousePos - transform.position;
    }

    private void GenerateChip(float xPos, float zPos, TeamType team)
    {
        var chipPos = GetNormalizedPosition(xPos, zPos);
        Debug.Log(chipPos);
        var chip = Instantiate(ChipPrototype, chipPos, Quaternion.identity);
        chip.Setup(team);
        if (GetCellInPosition(xPos, zPos, out var cell))
        {
            cell.chip = chip;
            chip.cell = cell;
        }
    }

    private bool GetCellInPosition(float xPos, float zPos, out Cell cell)
    {
        cell = default;
        var pos = GetNormalizedPosition(xPos, zPos);
        foreach (var kvp in _cells)
        {
            if (kvp.Value.realPos == pos)
            {
                cell = kvp.Value;
                return true;
            }
        }
        return false;
    }

    private Vector3 GetNormalizedPosition(float xPos, float zPos)
    {
        var cellXPos = Mathf.Floor(Mathf.Abs(xPos / CellSize));
        var cellZPos = Mathf.Floor(Mathf.Abs(zPos / CellSize));
        return new Vector3(cellXPos, 0, cellZPos);
    }
}
