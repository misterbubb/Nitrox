using System.Collections.ObjectModel;
using NitroxClient.GameLogic.PlayerLogic.PlayerModel.Equipment.Abstract;
using UnityEngine;

namespace NitroxClient.GameLogic.PlayerLogic.PlayerModel.Equipment
{
    public class DiveSuitVisibilityHandler : IEquipmentVisibilityHandler
    {
        private readonly GameObject head;
        private readonly GameObject body;
        private readonly GameObject hands;

        public DiveSuitVisibilityHandler(GameObject playerModel)
        {
            Transform headTransform = playerModel.transform.Find(PlayerEquipmentConstants.NORMAL_HEAD_GAME_OBJECT_NAME);
            Transform bodyTransform = playerModel.transform.Find(PlayerEquipmentConstants.DIVE_SUIT_GAME_OBJECT_NAME);
            Transform handsTransform = playerModel.transform.Find(PlayerEquipmentConstants.NORMAL_HANDS_GAME_OBJECT_NAME);

            head = headTransform ? headTransform.gameObject : null;
            body = bodyTransform ? bodyTransform.gameObject : null;
            hands = handsTransform ? handsTransform.gameObject : null;

            if (head == null || body == null || hands == null)
            {
                Log.Error($"[DiveSuitVisibilityHandler] Failed to find one or more GameObjects: head={head != null}, body={body != null}, hands={hands != null}");
            }
        }

        public void UpdateEquipmentVisibility(ReadOnlyCollection<TechType> currentEquipment)
        {
            if (head == null || body == null || hands == null)
            {
                return;
            }

            bool headVisible = !currentEquipment.Contains(TechType.RadiationHelmet) && !currentEquipment.Contains(TechType.Rebreather);
            bool bodyVisible = !currentEquipment.Contains(TechType.RadiationSuit) &&
                               !currentEquipment.Contains(TechType.WaterFiltrationSuit) &&
                               !currentEquipment.Contains(TechType.ReinforcedDiveSuit);
            bool handsVisible = !currentEquipment.Contains(TechType.RadiationGloves) && !currentEquipment.Contains(TechType.ReinforcedGloves);

            head.SetActive(headVisible);
            body.SetActive(bodyVisible);
            hands.SetActive(handsVisible);
        }
    }
}
