using Kitchen;
using KitchenData;
using KitchenLib.Preferences;
using KitchenLib.Utils;
using KitchenMods;
using Unity.Collections;
using Unity.Entities;

namespace Yipee.Systems
{
    public class ReplaceCustomers : GameSystemBase, IModSystem
    {
        private EntityQuery query;
        protected override void Initialise()
        {
            base.Initialise();
            query = GetEntityQuery(typeof(CRequiresView));
        }

        protected override void OnUpdate()
        {
            if (!HasStatus((RestaurantStatus)VariousUtils.GetID("YipeeCard")) && Mod.manager.GetPreference<PreferenceBool>("shouldOnlyUseCard").Value) return;
            
            NativeArray<Entity> entities = query.ToEntityArray(Allocator.TempJob);
            foreach (Entity entity in entities)
            {
                if (!Require(entity, out CRequiresView cRequiresView) || cRequiresView.Type != ViewType.Customer) continue;
                cRequiresView.Type = (ViewType)VariousUtils.GetID("hoardingBugCustomer");
                EntityManager.SetComponentData(entity, cRequiresView);
            }
            entities.Dispose();
        }
    }
}