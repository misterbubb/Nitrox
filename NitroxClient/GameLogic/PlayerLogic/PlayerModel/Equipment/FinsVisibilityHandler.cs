using System.Collections.ObjectModel;
using NitroxClient.GameLogic.PlayerLogic.PlayerModel.Equipment.Abstract;
using UnityEngine;

namespace NitroxClient.GameLogic.PlayerLogic.PlayerModel.Equipment
{
    public class FinsVisibilityHandler : IEquipmentVisibilityHandler
    {
        private readonly GameObject fins;
        private readonly GameObject finsRoot;
        private readonly GameObject chargedFins;
        private readonly GameObject chargedFinsRoot;
        private readonly GameObject glideFins;
        private readonly GameObject glideFinsRoot;

        public FinsVisibilityHandler(GameObject playerModel)
        {
            Transform finsTransform = playerModel.transform.Find(PlayerEquipmentConstants.FINS_GAME_OBJECT_NAME);
            Transform finsRootTransform = playerModel.transform.Find(PlayerEquipmentConstants.FINS_ROOT_GAME_OBJECT_NAME);
            Transform chargedFinsTransform = playerModel.transform.Find(PlayerEquipmentConstants.CHARGED_FINS_GAME_OBJECT_NAME);
            Transform chargedFinsRootTransform = playerModel.transform.Find(PlayerEquipmentConstants.CHARGED_FINS_ROOT_GAME_OBJECT_NAME);
            Transform glideFinsTransform = playerModel.transform.Find(PlayerEquipmentConstants.GLIDE_FINS_GAME_OBJECT_NAME);
            Transform glideFinsRootTransform = playerModel.transform.Find(PlayerEquipmentConstants.GLIDE_FINS_ROOT_GAME_OBJECT_NAME);

            fins = finsTransform ? finsTransform.gameObject : null;
            finsRoot = finsRootTransform ? finsRootTransform.gameObject : null;
            chargedFins = chargedFinsTransform ? chargedFinsTransform.gameObject : null;
            chargedFinsRoot = chargedFinsRootTransform ? chargedFinsRootTransform.gameObject : null;
            glideFins = glideFinsTransform ? glideFinsTransform.gameObject : null;
            glideFinsRoot = glideFinsRootTransform ? glideFinsRootTransform.gameObject : null;

            if (fins == null || finsRoot == null || chargedFins == null || chargedFinsRoot == null || glideFins == null || glideFinsRoot == null)
            {
                Log.Error($"[FinsVisibilityHandler] Failed to find one or more GameObjects");
            }
        }

        public void UpdateEquipmentVisibility(ReadOnlyCollection<TechType> currentEquipment)
        {
            if (fins == null || finsRoot == null || chargedFins == null || chargedFinsRoot == null || glideFins == null || glideFinsRoot == null)
            {
                return;
            }

            bool basicFinsVisible = currentEquipment.Contains(TechType.Fins);
            bool chargedFinsVisible = currentEquipment.Contains(TechType.SwimChargeFins);
            bool glideFinsVisible = currentEquipment.Contains(TechType.UltraGlideFins);

            fins.SetActive(basicFinsVisible);
            finsRoot.SetActive(basicFinsVisible);
            chargedFins.SetActive(chargedFinsVisible);
            chargedFinsRoot.SetActive(chargedFinsVisible);
            glideFins.SetActive(glideFinsVisible);
            glideFinsRoot.SetActive(glideFinsVisible);
        }
    }
}
