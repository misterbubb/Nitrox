using System.Collections.ObjectModel;
using NitroxClient.GameLogic.PlayerLogic.PlayerModel.Equipment.Abstract;
using UnityEngine;

namespace NitroxClient.GameLogic.PlayerLogic.PlayerModel.Equipment
{
    public class ScubaSuitVisibilityHandler : IEquipmentVisibilityHandler
    {
        private readonly GameObject rebreather;
        private readonly GameObject scuba;
        private readonly GameObject scubaTank;
        private readonly GameObject scubaTankTubes;

        public ScubaSuitVisibilityHandler(GameObject playerModel)
        {
            Transform rebreatherTransform = playerModel.transform.Find(PlayerEquipmentConstants.REBREATHER_GAME_OBJECT_NAME);
            Transform scubaTransform = playerModel.transform.Find(PlayerEquipmentConstants.SCUBA_ROOT_GAME_OBJECT_NAME);
            Transform scubaTankTransform = playerModel.transform.Find(PlayerEquipmentConstants.SCUBA_TANK_GAME_OBJECT_NAME);
            Transform scubaTankTubesTransform = playerModel.transform.Find(PlayerEquipmentConstants.SCUBA_TANK_TUBES_GAME_OBJECT_NAME);

            rebreather = rebreatherTransform ? rebreatherTransform.gameObject : null;
            scuba = scubaTransform ? scubaTransform.gameObject : null;
            scubaTank = scubaTankTransform ? scubaTankTransform.gameObject : null;
            scubaTankTubes = scubaTankTubesTransform ? scubaTankTubesTransform.gameObject : null;

            if (rebreather == null || scuba == null || scubaTank == null || scubaTankTubes == null)
            {
                Log.Error($"[ScubaSuitVisibilityHandler] Failed to find one or more GameObjects: rebreather={rebreather != null}, scuba={scuba != null}, scubaTank={scubaTank != null}, scubaTankTubes={scubaTankTubes != null}");
            }
        }

        public void UpdateEquipmentVisibility(ReadOnlyCollection<TechType> currentEquipment)
        {
            if (rebreather == null || scuba == null || scubaTank == null || scubaTankTubes == null)
            {
                return;
            }

            bool tankEquipped = currentEquipment.Contains(TechType.Tank) ||
                                currentEquipment.Contains(TechType.DoubleTank) ||
                                currentEquipment.Contains(TechType.HighCapacityTank) ||
                                currentEquipment.Contains(TechType.PlasteelTank);

            bool rebreatherVisible = currentEquipment.Contains(TechType.Rebreather);
            bool radiationHelmetVisible = currentEquipment.Contains(TechType.RadiationHelmet);
            bool tankVisible = tankEquipped && !currentEquipment.Contains(TechType.RadiationSuit);
            bool tubesVisible = (rebreatherVisible || radiationHelmetVisible) && tankVisible;
            bool rootVisible = rebreatherVisible || tankVisible;

            rebreather.SetActive(rebreatherVisible);
            scuba.SetActive(rootVisible);
            scubaTank.SetActive(tankVisible);
            scubaTankTubes.SetActive(tubesVisible);
        }
    }
}
