using System.Collections.ObjectModel;
using NitroxClient.GameLogic.PlayerLogic.PlayerModel.Equipment.Abstract;
using UnityEngine;

namespace NitroxClient.GameLogic.PlayerLogic.PlayerModel.Equipment
{
    public class ReinforcedSuitVisibilityHandler : IEquipmentVisibilityHandler
    {
        private readonly GameObject gloves;
        private readonly GameObject suit;

        public ReinforcedSuitVisibilityHandler(GameObject playerModel)
        {
            Transform glovesTransform = playerModel.transform.Find(PlayerEquipmentConstants.REINFORCED_GLOVES_GAME_OBJECT_NAME);
            Transform suitTransform = playerModel.transform.Find(PlayerEquipmentConstants.REINFORCED_SUIT_GAME_OBJECT_NAME);

            gloves = glovesTransform ? glovesTransform.gameObject : null;
            suit = suitTransform ? suitTransform.gameObject : null;

            if (gloves == null || suit == null)
            {
                Log.Error($"[ReinforcedSuitVisibilityHandler] Failed to find one or more GameObjects: gloves={gloves != null}, suit={suit != null}");
            }
        }

        public void UpdateEquipmentVisibility(ReadOnlyCollection<TechType> currentEquipment)
        {
            if (gloves == null || suit == null)
            {
                return;
            }

            bool glovesVisible = currentEquipment.Contains(TechType.ReinforcedGloves);
            bool bodyVisible = currentEquipment.Contains(TechType.ReinforcedDiveSuit);

            gloves.SetActive(glovesVisible);
            suit.SetActive(bodyVisible);
        }
    }
}
