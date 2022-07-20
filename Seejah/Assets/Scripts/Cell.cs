using TMPro;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Vector3 realPos;
    public int row;
    public int col;
    public Chip chip;
    public TextMeshPro text;

    public void Setup(int i, int j, Vector3 pos)
    {
        row = i;
        col = j;
        realPos = pos;
        text.text = $"({i},{j})";
    }
}
