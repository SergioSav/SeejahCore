using Assets.Scripts.Core.Data.ScriptableObjects;
using Assets.Scripts.Core.Data.Services;
using Assets.Scripts.Core.Models;
using Assets.Scripts.Core.Utils;
using DG.Tweening;
using System.Linq;
using UnityEngine;
using VContainer;

public class ChipView : MonoBehaviour
{
    public static float PlacementPhaseScale = 0.4f;

    [SerializeField] private MeshRenderer chipMaterial;
    [SerializeField] private Material chipSelection;
    [SerializeField] private GameObject deadFX;
    [SerializeField] private GameObject chip;

    private Tweener _removeTween;
    private RandomProvider _random;
    private Color _colorTeam1;
    private Color _colorTeam2;
    private Material _defaultMaterial;

    public TeamType Team { get; private set; }

    [Inject]
    public void Construct(MatchModel matchModel, RandomProvider random, IConfigSupplier configSupplier, ConfigsStorage configsStorage)
    {
        _random = random;
        var colorId = configsStorage.CustomizationDataList
                    .FirstOrDefault(data => data.Id == matchModel.Options.ChipColorId)
                    .PrefabId;
        var colorConfig = configSupplier.GetConfig<ColorConfigScriptableObject>(colorId);
        _colorTeam1 = colorConfig.Team1Color;
        _colorTeam2 = colorConfig.Team2Color;
    }

    public void Setup(TeamType team)
    {
        Team = team;
        UpdateView();
        _defaultMaterial = chipMaterial.material;
    }

    public void SetSelected(bool isSelected)
    {
        chipSelection.color = chipMaterial.material.color;
        chipMaterial.material = isSelected ? chipSelection : _defaultMaterial;
    }

    private void UpdateView()
    {
        var color = Team == TeamType.FirstTeam ? _colorTeam1 : _colorTeam2;
        chipMaterial.material.color = color;
    }

    public void SetInfiniteRotate()
    {
        transform.parent.transform
            .DOLocalRotate(new Vector3(0, 360, 0), 2f, RotateMode.FastBeyond360)
            .SetLoops(-1)
            .SetEase(Ease.Linear)
            .SetLink(gameObject);
    }

    public void UpdatePos(Vector3 pos)
    {
        _random.GetRandom(-5, 5, out var randomAngle);
        var eulerAngles = chip.transform.localRotation.eulerAngles + Vector3.up * randomAngle;
        chip.transform.localRotation = Quaternion.Euler(eulerAngles);

        transform.DOMove(pos, 0.2f)
            .SetLink(gameObject);
    }

    public void PlaceOutBoard(Vector3 pos)
    {
        transform.localScale *= PlacementPhaseScale;
        UpdatePos(pos * PlacementPhaseScale);
    }

    public void PlaceOnBoard(Vector3 pos)
    {
        transform.localScale /= PlacementPhaseScale;

        UpdatePos(pos);
    }

    public void Attack(Vector3 pos)
    {
        transform
            .DOMove(transform.position - (transform.position - pos) * 0.3f, 0.2f)
            .SetLoops(2, LoopType.Yoyo)
            .SetLink(gameObject);
    }

    public void RemoveFromBoard()
    {
        var t = 0.5f;
        _removeTween = transform.DOMoveY(2, t - 0.1f);
        _removeTween.onComplete += () => {
            chip.SetActive(false);
            deadFX.SetActive(true);
        };
        Destroy(gameObject, t + 0.1f);
    }

    private void OnDestroy()
    {
        _removeTween.Kill();
        deadFX.SetActive(false);
    }
}
