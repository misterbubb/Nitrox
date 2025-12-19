using System.Collections.ObjectModel;
using NitroxClient.GameLogic.PlayerLogic.PlayerModel.Equipment.Abstract;
using UnityEngine;

namespace NitroxClient.GameLogic.PlayerLogic.PlayerModel.Equipment
{
    public class RadiationSuitVisibilityHandler : IEquipmentVisibilityHandler
    {
        private readonly GameObject head;
        private readonly GameObject helmet;
        private readonly GameObject gloves;
        private readonly GameObject suit;
        private readonly GameObject suitNeck;
        private readonly GameObject suitVest;
        private readonly GameObject tank;
        private readonly GameObject tankTubes;

        public RadiationSuitVisibilityHandler(GameObject playerModel)
        {
            Transform headTransform = playerModel.transform.Find(PlayerEquipmentConstants.RADIATION_HEAD_GAME_OBJECT_NAME);
            Transform helmetTransform = playerModel.transform.Find(PlayerEquipmentConstants.RADIATION_HELMET_GAME_OBJECT_NAME);
            Transform glovesTransform = playerModel.transform.Find(PlayerEquipmentConstants.RADIATION_GLOVES_GAME_OBJECT_NAME);
            Transform suitTransform = playerModel.transform.Find(PlayerEquipmentConstants.RADIATION_SUIT_GAME_OBJECT_NAME);
            Transform suitNeckTransform = playerModel.transform.Find(PlayerEquipmentConstants.RADIATION_SUIT_NECK_CLASP_GAME_OBJECT_NAME);
            Transform suitVestTransform = playerModel.transform.Find(PlayerEquipmentConstants.RADIATION_SUIT_VEST_GAME_OBJECT_NAME);
            Transform tankTransform = playerModel.transform.Find(PlayerEquipmentConstants.RADIATION_TANK_GAME_OBJECT_NAME);
            Transform tankTubesTransform = playerModel.transform.Find(PlayerEquipmentConstants.RADIATION_TANK_TUBES_GAME_OBJECT_NAME);

            head = headTransform ? headTransform.gameObject : null;
            helmet = helmetTransform ? helmetTransform.gameObject : null;
            gloves = glovesTransform ? glovesTransform.gameObject : null;
            suit = suitTransform ? suitTransform.gameObject : null;
            suitNeck = suitNeckTransform ? suitNeckTransform.gameObject : null;
            suitVest = suitVestTransform ? suitVestTransform.gameObject : null;
            tank = tankTransform ? tankTransform.gameObject : null;
            tankTubes = tankTubesTransform ? tankTubesTransform.gameObject : null;

            if (head == null || helmet == null || gloves == null || suit == null || suitNeck == null || suitVest == null || tank == null || tankTubes == null)
            {
                Log.Error($"[RadiationSuitVisibilityHandler] Failed to find one or more GameObjects");
            }
        }

        public void UpdateEquipmentVisibility(ReadOnlyCollection<TechType> currentEquipment)
        {
            if (head == null || helmet == null || gloves == null || suit == null || suitNeck == null || suitVest == null || tank == null || tankTubes == null)
            {
                return;
            }

            bool tankEquipped = currentEquipment.Contains(TechType.Tank) ||
                                currentEquipment.Contains(TechType.DoubleTank) ||
                                currentEquipment.Contains(TechType.HighCapacityTank) ||
                                currentEquipment.Contains(TechType.PlasteelTank);

            bool helmetVisible = currentEquipment.Contains(TechType.RadiationHelmet);
            bool glovesVisible = currentEquipment.Contains(TechType.RadiationGloves);
            bool bodyVisible = currentEquipment.Contains(TechType.RadiationSuit);
            bool vestVisible = bodyVisible || helmetVisible;
            bool tankVisible = tankEquipped && vestVisible;
            bool tubesVisible = tankVisible && helmetVisible;

            head.SetActive(helmetVisible);
            helmet.SetActive(helmetVisible);
            gloves.SetActive(glovesVisible);
            suit.SetActive(bodyVisible);
            suitNeck.SetActive(helmetVisible);
            suitVest.SetActive(vestVisible);
            tank.SetActive(tankVisible);
            tankTubes.SetActive(tubesVisible);
        }
    }
}
