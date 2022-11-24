using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Framework;
using System.Collections.Generic;
using UniRx;

namespace Assets.Scripts.Core.SceneInstallers
{
    public class CustomizationModel : DisposableContainer
    {
        private readonly List<CustomizationData> _customizationDataList;
        private ReactiveProperty<int> _selectedItem;

        public IReadOnlyReactiveProperty<int> SelectedItem => _selectedItem;

        public List<CustomizationData> DataList => _customizationDataList;

        public CustomizationModel(List<CustomizationData> customizationDataList)
        {
            _customizationDataList = customizationDataList;
            _selectedItem = AddForDispose(new ReactiveProperty<int>());
        }

        public void SelectItem(int id)
        {
            _selectedItem.Value = id;
        }
    }
}