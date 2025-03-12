using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Reflection;

namespace GnoMoGnome
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class GnoMoGnome : BaseUnityPlugin
    {
        private const string pluginGuid = "oofie.repo.gnomognome";
        private const string pluginName = "GnoMoGnome";
        private const string pluginVersion = "1.0.0";

        public void Awake()
        {
            Logger.LogInfo(pluginName + " version " + pluginVersion + " loaded :3");
            Logger.LogInfo("Hi Spiffers");

            Harmony harmony = new Harmony(pluginGuid);

            MethodInfo original = AccessTools.Method(typeof(EnemyGnomeDirector), "StateAttackSet");

            MethodInfo patch = AccessTools.Method(typeof(MyPatches), "StateAttackSet_Patch");

            harmony.Patch(original, new HarmonyMethod(patch));
        }
    }

    [HarmonyPatch(typeof(EnemyGnomeDirector), "StateAttackSet")]
    public class MyPatches
    {
        public static bool StateAttackSet_Patch(EnemyGnomeDirector __instance, ref bool ___stateImpulse)
        {
            if (___stateImpulse)
            {
                ___stateImpulse = false;

                MethodInfo updateStateMethod = AccessTools.Method(typeof(EnemyGnomeDirector), "UpdateState");

                if (updateStateMethod != null)
                {
                    updateStateMethod.Invoke(__instance, new object[] { EnemyGnomeDirector.State.AttackPlayer });
                }
                else
                {
                    UnityEngine.Debug.LogError("Failed to find UpdateState method.");
                }
            }

            return false;
        }
    }
}
