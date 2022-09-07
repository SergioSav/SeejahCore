using Assets.Scripts.Core.Models;
using UnityEngine;

public class ChipView : MonoBehaviour
{
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
