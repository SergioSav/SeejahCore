using DefaultNamespace;
using UnityEngine;

public class Chip : MonoBehaviour
{
    public Cell cell;
    public TeamType Team { get; private set; }

    [SerializeField] private MeshRenderer chipMaterial;

    public void Setup(TeamType team)
    {
        Team = team;
        UpdateView();
    }

    private void UpdateView()
    {
        chipMaterial.material.color = Team == TeamType.TeamRed ? Color.red : Color.blue;
    }

    public void UpdatePos(Vector3 pos)
    {
        transform.position = pos;
    }
}
