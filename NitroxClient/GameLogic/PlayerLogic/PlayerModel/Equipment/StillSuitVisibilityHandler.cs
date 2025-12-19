using System.Collections.ObjectModel;
using NitroxClient.GameLogic.PlayerLogic.PlayerModel.Equipment.Abstract;
using UnityEngine;

namespace NitroxClient.GameLogic.PlayerLogic.PlayerModel.Equipment
{
    public class StillSuitVisibilityHandler : IEquipmentVisibilityHandler
    {
        private readonly GameObject stillSuit;

        public StillSuitVisibilityHandler(GameObject playerModel)
        {
            Transform stillSuitTransform = playerModel.transform.Find(PlayerEquipmentConstants.STILL_SUIT_GAME_OBJECT_NAME);
            stillSuit = stillSuitTransform ? stillSuitTransform.gameObject : null;

            if (stillSuit == null)
            {
                Log.Error($"[StillSuitVisibilityHandler] Failed to find stillSuit GameObject");
            }
        }

        public void UpdateEquipmentVisibility(ReadOnlyCollection<TechType> currentEquipment)
        {
            if (stillSuit == null)
            {
                return;
            }

            bool bodyVisible = currentEquipment.Contains(TechType.WaterFiltrationSuit);

            stillSuit.SetActive(bodyVisible);
        }
    }
}
