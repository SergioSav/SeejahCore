using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Models.MatchModels;
using DG.Tweening;
using UnityEngine;
using VContainer;

public class ChipView : MonoBehaviour
{
    private const float OutBoardScaleMultiplier = 0.4f;

    [SerializeField] private MeshRenderer chipMaterial;
    private Tweener _removeTween;
    private MatchOptions _matchOptions;
    public TeamType Team { get; private set; }

    [Inject]
    public void Construct(MatchModel matchModel)
    {
        _matchOptions = matchModel.Options;
    }

    public void Setup(TeamType team)
    {
        Team = team;
        UpdateView();
    }

    private void UpdateView()
    {
        chipMaterial.material.color = Team == TeamType.FirstTeam ? _matchOptions.FirstTeamColor : _matchOptions.SecondTeamColor;
    }

    public void UpdatePos(Vector3 pos)
    {
        transform.DOMove(pos, 0.2f);
    }

    public void PlaceOutBoard(Vector3 pos)
    {
        transform.localScale *= OutBoardScaleMultiplier;
        UpdatePos(pos * OutBoardScaleMultiplier);
    }

    public void PlaceOnBoard(Vector3 pos)
    {
        transform.localScale /= OutBoardScaleMultiplier;
        UpdatePos(pos);
    }

    public void Attack(Vector3 pos)
    {
        transform
            .DOMove(transform.position - (transform.position - pos) * 0.3f, 0.2f)
            .SetLoops(2, LoopType.Yoyo);
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
