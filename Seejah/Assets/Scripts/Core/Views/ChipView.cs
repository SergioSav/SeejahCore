using Assets.Scripts.Core.Models;
using DG.Tweening;
using UnityEngine;

public class ChipView : MonoBehaviour
{
    public TeamType Team { get; private set; }

    [SerializeField] private MeshRenderer chipMaterial;
    private Tweener _removeTween;

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
        transform.DOMove(pos, 0.2f);
    }

    public void RemoveFromBoard()
    {
        var t = 0.5f;
        _removeTween = transform.DOMoveY(2, t-0.1f);
        Destroy(gameObject, t);
    }

    private void OnDestroy()
    {
        _removeTween.Kill();
    }
}
