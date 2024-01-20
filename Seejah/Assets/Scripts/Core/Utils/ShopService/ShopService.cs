using Assets.Scripts.Core.Data;
using Assets.Scripts.Core.Models;

namespace Assets.Scripts.Core.Utils
{
    public class ShopService
    {
        private readonly UserModel _userModel;

        public ShopService(UserModel userModel)
        {
            _userModel = userModel;
        }

        public bool CanBuyFor(CurrencyType currencyType, int value)
        {
            if (currencyType == CurrencyType.Gold)
                return _userModel.CurrentGold.Value >= value;
            return false;
        }

        public bool CanBuyFor(PriceData price)
        {
            return CanBuyFor(price.Currency, price.Value);
        }

        public void ProcessBuyFor(CurrencyType currencyType, int value)
        {
            if (currencyType == CurrencyType.Gold)
                _userModel.ReduceGold(value);
        }

        public void ProcessBuyFor(PriceData price)
        {
            ProcessBuyFor(price.Currency, price.Value);
        }
    }
}
