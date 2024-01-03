using ComputerPlusPlus.Tools;
using CPPMaterials.Source.GorillaCosmetics;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace CPPMaterials.Source
{
    public class MaterialController : MonoBehaviourPunCallbacks
    {
        public static MaterialController LocalInstance;
        public VRRig rig;
        Material defaultMaterial;
        GorillaMaterial currentMaterial;
        public const string MaterialKey = "CPPMaterials::Material";

        void Awake()
        {
            rig = GetComponent<VRRig>();
            if (rig == GorillaTagger.Instance.offlineVRRig)
                LocalInstance = this;
            VRRigAddedPatch.OnRigAdded += (player, rig) =>
            {
                if(rig == this.rig)
                    OnPlayerPropertiesUpdate(player, player.CustomProperties);
            };
        }

        void Start()
        {
            defaultMaterial = rig.materialsToChangeTo[0];
        }

        public void ChangeMaterial(GorillaMaterial mat)
        {
            if (mat == null)
                mat = Plugin.Instance.materials[0];
            rig.materialsToChangeTo[0] = mat.GetMaterial();
            rig.ChangeMaterialLocal(rig.setMatIndex);
            currentMaterial = mat;
            if (this == LocalInstance)
            {
                Hashtable customProperties = new Hashtable();
                customProperties.Add(MaterialKey, mat.Descriptor.ID);
                PhotonNetwork.LocalPlayer.SetCustomProperties(customProperties);
                Logging.Debug("Set property: ", MaterialKey, mat.Descriptor.ID);
            }
            else
            {
                Logging.Debug($"    Changed {CurrentPlayer.NickName + CurrentPlayer.UserId[10]}'s material to: " + mat.Descriptor.Name);
            }
        }

        public void Reset()
        {
            ChangeMaterial(Plugin.Instance.materials[0]);
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            if (otherPlayer == CurrentPlayer && !otherPlayer.IsLocal)
                Reset();
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            if(this == LocalInstance)
                ChangeMaterial(currentMaterial);
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
            if (targetPlayer == CurrentPlayer && !targetPlayer.IsLocal)
            {
                if (changedProps.ContainsKey(MaterialKey)) // They just changed this property
                {
                    string matID = (string)changedProps[MaterialKey];
                    TryChangeMaterial(matID);
                }
                else if (targetPlayer.CustomProperties.ContainsKey(MaterialKey)) // They had this property but didn't change it
                {
                    string matID = (string)targetPlayer.CustomProperties[MaterialKey];
                    TryChangeMaterial(matID);
                }
                else // They don't have this property
                {
                    Reset();
                }
            }
        }

        void TryChangeMaterial(string matID)
        {
            GorillaMaterial mat = Plugin.Instance.GetMaterial(matID);
            if (mat != null)
                ChangeMaterial(mat);
            else
                Reset();
        }

        Player CurrentPlayer
        {
            get
            {
                var dict = GorillaParent.instance.vrrigDict;
                foreach (var item in dict)
                {
                    if (item.Value == rig)
                        return item.Key;
                }
                return null;
            }
        }

    }
}
