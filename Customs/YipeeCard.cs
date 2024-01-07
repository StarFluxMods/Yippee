using System.Collections.Generic;
using KitchenData;
using KitchenLib.Customs;
using KitchenLib.Utils;

namespace Yipee.Customs
{
    public class YipeeCard : CustomUnlockCard
    {
        public override string UniqueNameID => "YipeeCard";
        public override List<UnlockEffect> Effects => new List<UnlockEffect>
        {
            new StatusEffect
            {
                Status = (RestaurantStatus)VariousUtils.GetID("YipeeCard")
            }
        };
        public override bool IsUnlockable => true;
        public override UnlockGroup UnlockGroup => UnlockGroup.Generic;
        public override CardType CardType => CardType.Default;

        public override List<(Locale, UnlockInfo)> InfoList => new List<(Locale, UnlockInfo)>
        {
            (Locale.English, LocalisationUtils.CreateUnlockInfo("Yippee!", "Customers are.. Yippee", ""))
        };
    }
}