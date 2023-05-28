using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Framework;
using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace Assets.Scripts.Core.SceneInstallers
{
    public class CustomizationModel : DisposableContainer
    {
        private readonly List<CustomizationData> _customizationDataList;
        private ReactiveProperty<int> _selectedChipId;
        private ReactiveProperty<int> _selectedChipColorId;
        private ReactiveProperty<int> _selectedBoardId;
        private ReactiveProperty<int> _selectedFloorId;
        private ReactiveProperty<CustomizationState> _currentState;

        public IReadOnlyReactiveProperty<int> SelectedChipId => _selectedChipId;
        public IReadOnlyReactiveProperty<int> SelectedChipColorId => _selectedChipColorId;
        public IReadOnlyReactiveProperty<int> SelectedBoardId => _selectedBoardId;
        public IReadOnlyReactiveProperty<int> SelectedFloorId => _selectedFloorId;
        public IReadOnlyReactiveProperty<CustomizationState> CurrentState => _currentState;

        public List<CustomizationData> FullDataList => _customizationDataList;
        public List<CustomizationData> ChipDataList => _customizationDataList.Where(d => d.Type == CustomizationType.Chip).ToList();
        public List<CustomizationData> ChipColorDataList => _customizationDataList.Where(d => d.Type == CustomizationType.ChipColor).ToList();
        public List<CustomizationData> BoardDataList => _customizationDataList.Where(d => d.Type == CustomizationType.Board).ToList();
        public List<CustomizationData> FloorDataList => _customizationDataList.Where(d => d.Type == CustomizationType.Floor).ToList();

        public CustomizationModel(List<CustomizationData> customizationDataList)
        {
            _customizationDataList = customizationDataList;
            _currentState = AddForDispose(new ReactiveProperty<CustomizationState>());
            _selectedChipId = AddForDispose(new ReactiveProperty<int>());
            _selectedChipColorId = AddForDispose(new ReactiveProperty<int>());
            _selectedBoardId = AddForDispose(new ReactiveProperty<int>());
            _selectedFloorId = AddForDispose(new ReactiveProperty<int>());
        }

        public void SelectChip(int id)
        {
            _selectedChipId.Value = id;
        }

        public void SelectColorChip(int id)
        {
            _selectedChipColorId.Value = id;
        }

        public void SelectBoard(int id)
        {
            _selectedBoardId.Value = id;
        }

        public void SelectFloor(int id)
        {
            _selectedFloorId.Value = id;
        }

        public void StartChipCustomization()
        {
            _currentState.Value = CustomizationState.ChipCustomization;
        }

        public void StartChipColorCustomization()
        {
            _currentState.Value = CustomizationState.ChipColorCustomization;
        }

        public void StartBoardCustomization()
        {
            _currentState.Value = CustomizationState.BoardCustomization;
        }

        public void StartFloorCustomization()
        {
            _currentState.Value = CustomizationState.FloorCustomization;
        }

        public void EndConcreteCustomization()
        {
            _currentState.Value = CustomizationState.CustomizationTypeSelect;
        }
    }
}