using HarmonyLib;
using BepInEx;
using UnityEngine;
using BepInEx.Unity.IL2CPP;
using System.Net.NetworkInformation;
using Il2CppInterop.Runtime;
using System.Reflection;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem.Net.NetworkInformation;
using System.Collections;
using System.Diagnostics;
using HarmonyLib.Tools;
using System.Reflection.Metadata.Ecma335;
using Il2CppSystem.Globalization;
//using Il2CppSystem.Diagnostics;
namespace NeverWorldENMod
{


    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class Plugin : BasePlugin
    {


        public const string pluginGuid = "Cadenza.NWNod.0.5";
        public const string pluginName = "NWNod";
        public const string pluginVersion = "0.5";
        public static BepInEx.Logging.ManualLogSource log;
        public static bool shouldpreg = false;
        static public int i;


        public override void Load()
        {
            log = Log;
            // Plugin startup logic
            AddComponent<mbmb>();
            Log.LogInfo($"Plugin {pluginName} is loaded!");
            var harmony = new Harmony("Cadenza.Game.EN.MOD");
            harmony.PatchAll();
            HarmonyFileLog.Enabled = true;
        }

    }

    class mbmb : MonoBehaviour
    {
        // Token: 0x06000018 RID: 24 RVA: 0x000032D4 File Offset: 0x000014D4
        public mbmb(IntPtr handle)
            : base(handle)
        {
        }
        private void Update()
        { 
            bool keyDown = Input.GetKeyDown(KeyCode.F1);
            if (keyDown)
            {
                foreach (var human in WorldData.GetHumans())
                {
                    if (human.IsPlayer)
                    {
                        if (human.Sex == ProfileData.SexCode.Male)
                        {
                            human.profData.ChangeSex(ProfileData.SexCode.Female);
                            Plugin.log.LogInfo($"Changed {human.Name}'s sex from Male to Female.");
                        }
                        else if (human.Sex == ProfileData.SexCode.Female)
                        {
                            human.profData.ChangeSex(ProfileData.SexCode.Male);
                            Plugin.log.LogInfo($"Changed {human.Name}'s sex from Female to Male");
                        }
                        else
                        {
                            Plugin.log.LogWarning($"Unhandled SexCode for {human.Name}: {human.Sex}");
                        }
                    }
                }

            }
        

        }

    }


        //Useful I think
        [HarmonyPatch(typeof(HumanData), "TestLoveNextStep")]
    static class Patch1_Lover
    {
        static void Postfix(HumanData __instance, ref bool __result)
        {
            if(__instance.IsPlayer)
            { 
            Plugin.log.LogInfo("TestLoveNextStep.HumanData_Name : " + __instance.Name);
            Plugin.log.LogInfo("TestLoveNextStep.HumanData___result : " + __result);
            Plugin.log.LogInfo("TestLoveNextStep.HumanData_TestAfterDate : " + __instance.TestAfterDate);
            Plugin.log.LogInfo("TestLoveNextStep.HumanData_Isei : " + __instance.Isei);
            }

            if (__result == false)
            {
                __result = true;
            }

        }
    }


    //CRITICAL
    [HarmonyPatch(typeof(HumanData), "get_Isei")]
    class Patch_IseiGetter
    {
        static bool Prefix(HumanData __instance, ref ProfileData.SexCode __result)
        {
            if (__instance.IsPlayer && TalkCondition.chkOkMakeChild(__instance, __instance.GetHusband) == true)
            {
                Plugin.log.LogInfo("Prefix.Patch_IseiGetter.newPregCheck.Old : " + __result);
                __result = ProfileData.SexCode.Male;
                Plugin.log.LogInfo("Prefix.Patch_IseiGetter.newPregCheck.New : " + __result);
                return false;
            }
            else
            {
                return true;
            }
        }
        static void Postfix(HumanData __instance, ref ProfileData.SexCode __result)
        {
            if(__instance.IsPlayer && TalkCondition.chkOkMakeChild(__instance, __instance.GetHusband) == true)
            {
                Plugin.log.LogInfo("Patch_IseiGetter.newPregCheck.Old : " + __result);
                __result = ProfileData.SexCode.Male;
                Plugin.log.LogInfo("Patch_IseiGetter.newPregCheck.New : " + __result);
            }
            else
            { 
            if (__instance.IsPlayer)
            {
                Plugin.log.LogInfo("Patch_IseiGetter.__instance.name : " + __instance.Name);
                Plugin.log.LogInfo("Patch_IseiGetter.__result!!!.Old : " + __result);
                __result = ProfileData.SexCode.Male;
                Plugin.log.LogInfo("Patch_IseiGetter.__instance.IsPlayer.__result!!!.New : " + __result);
                

            }
            if(__instance.GetLover == WorldData.PlayerData)
            {
                Plugin.log.LogInfo("Testing WorldData.PlayerData.Name : " + WorldData.PlayerData.Name);
                Plugin.log.LogInfo("Patch_IseiGetter.Lover.Testing WorldData.PlayerData.GetLover.Name : " + WorldData.PlayerData.GetLover.Name);
                Plugin.log.LogInfo("1Lover.Result.OLD : " + __result);

                __result = ProfileData.SexCode.Male;
                Plugin.log.LogInfo("1Lover.Result.NEW : " + __result);

            }
            try
            { 
            if (WorldData.PlayerData.TodayDate.Data != null)
            { 
                if(__instance == WorldData.PlayerData.TodayDate.Data)
                {
                    Plugin.log.LogInfo("Patch_IseiGetter.__instance.name : " + __instance.Name);
                    Plugin.log.LogInfo("2Lover.Result.OLD : " + __result);
                    __result = ProfileData.SexCode.Male;
                    Plugin.log.LogInfo("2Lover.Result.NEW : " + __result);

                    Plugin.log.LogInfo("Patch_IseiGetter.__result!!!.New : " + __result);
                }
            }
            if(WorldData.PlayerData.GetHusband == __instance)
                {
                    __result = ProfileData.SexCode.Male;
                }
            }
            catch
            {

            }
            }
        }
    }




    
    //CRITICAL
    [HarmonyPatch(typeof(BloodData), "IsCanLover")]
    static class Patch2_Lover
    {
        static void Postfix(HumanData me, HumanData you, bool __result)
        {

            if (me.IsPlayer == true || you.IsPlayer == true)
            {
                {

                    if (me.Sex == you.Sex)
                    {
                        /*Plugin.log.LogInfo("BloodData_MeName : " + me.Name);
                        Plugin.log.LogInfo("BloodData_MeSex : " + me.Sex);
                        Plugin.log.LogInfo("BloodData_Meme.IsPlayer : " + me.IsPlayer);
                        Plugin.log.LogInfo("BloodData_YouName : " + you.Name);
                        Plugin.log.LogInfo("BloodData_YouSex : " + you.Sex);
                        Plugin.log.LogInfo("BloodData_Youme.IsPlayer : " + you.IsPlayer);
                        Plugin.log.LogInfo("BloodData___resultOld: " + __result);*/

                        __result = true;
                        /*Plugin.log.LogInfo("BloodData___resultNew: " + __result);
                        Plugin.log.LogInfo("Adult Me ? : " + BloodData.testAdult(me));
                        Plugin.log.LogInfo("Adult You ? : " + BloodData.testAdult(you));
                        Plugin.log.LogInfo("Lover ? : " + BloodData.testLoverRelation(me, you))*/;



                    }

                }

            }
        }
    }
    
    //I think this one is key and allows for the first step : Dating.
    [HarmonyPatch(typeof(BloodData), "testLoverRelation")]
    static class Patch3_Lover
    {

        static void Postfix(ref HumanData me, ref HumanData you, ref bool __result)
        {

            if (me.IsPlayer == true || you.IsPlayer == true)
            {
                {

                    if (me.Sex == you.Sex)
                    {

                        __result = true;

                    }

                }

            }
        }
    }
    //Uselss ? Not sure.

   [HarmonyPatch(typeof(BloodData), "IsCanMarriage")]
    static class Patch4_Lover
    {
        static void Prefix(bool testJob, HumanData me, HumanData you, bool __result)
        {

            if (me.IsPlayer == true || you.IsPlayer == true)
            {
                {

                    if (me.Sex == you.Sex)
                    {
                        testJob = true;
                        /*Plugin.log.LogInfo("BloodData_MeName : " + me.Name);
                        Plugin.log.LogInfo("BloodData_MeSex : " + me.Sex);
                        Plugin.log.LogInfo("BloodData_Meme.IsPlayer : " + me.IsPlayer);
                        Plugin.log.LogInfo("BloodData_YouName : " + you.Name);
                        Plugin.log.LogInfo("BloodData_YouSex : " + you.Sex);
                        Plugin.log.LogInfo("BloodData_Youme.IsPlayer : " + you.IsPlayer);
                        Plugin.log.LogInfo("BloodData___resultOld: " + __result);*/

                        __result = true;
                        /*Plugin.log.LogInfo("BloodData___resultNew: " + __result);
                        Plugin.log.LogInfo("Adult Me ? : " + BloodData.testAdult(me));
                        Plugin.log.LogInfo("Adult You ? : " + BloodData.testAdult(you));
                        Plugin.log.LogInfo("Lover ? : " + BloodData.testLoverRelation(me, you));*/



                    }

                }

            }
        }
        static void Postfix(bool testJob, HumanData me, HumanData you, bool __result)
        {

            if (me.IsPlayer == true || you.IsPlayer == true)
            {
                {

                    if (me.Sex == you.Sex)
                    {
                        testJob = true;
                        /*Plugin.log.LogInfo("BloodData_MeName : " + me.Name);
                        Plugin.log.LogInfo("BloodData_MeSex : " + me.Sex);
                        Plugin.log.LogInfo("BloodData_Meme.IsPlayer : " + me.IsPlayer);
                        Plugin.log.LogInfo("BloodData_YouName : " + you.Name);
                        Plugin.log.LogInfo("BloodData_YouSex : " + you.Sex);
                        Plugin.log.LogInfo("BloodData_Youme.IsPlayer : " + you.IsPlayer);
                        Plugin.log.LogInfo("BloodData___resultOld: " + __result);*/

                        __result = true;
                        /*Plugin.log.LogInfo("BloodData___resultNew: " + __result);
                        Plugin.log.LogInfo("Adult Me ? : " + BloodData.testAdult(me));
                        Plugin.log.LogInfo("Adult You ? : " + BloodData.testAdult(you));
                        Plugin.log.LogInfo("Lover ? : " + BloodData.testLoverRelation(me, you));*/



                    }

                }

            }
        }
    }

    [HarmonyPatch(typeof(TalkCondition), "chkOkMakeChild")]
    static class TalkCondition_chkOkMakeChild
    {
        static void Postfix(TalkCondition __instance, ref AnimalData spk, ref AnimalData lsn, ref bool __result)
        {
            
            Plugin.log.LogInfo("TalkCondition_chkOkMakeChild.old : " + __result);
            __result = true;
            Plugin.log.LogInfo("TalkCondition_chkOkMakeChild.new : " + __result);
        }
    }

    //Critical for pregnancy
    [HarmonyPatch(typeof(TalkEffect), "exeMakeChild")]
    class TalkCondition_exeMakeChild
    {
       
        public static bool changed = false;
        static ProfileData.SexCode opposite = new ProfileData.SexCode();
        static ProfileData.SexCode orig = new ProfileData.SexCode();
        static void Prefix(AnimalData speaker, AnimalData listener)
        {
            if (speaker.GetHuman.Sex == listener.GetHuman.Sex)
            {
                Plugin.shouldpreg = true;
                Plugin.log.LogInfo("Speaker orig sex in Prefix : " + speaker.Sex);
                orig = speaker.Sex;
                if (speaker.Sex == ProfileData.SexCode.Female)
                {
                    opposite = ProfileData.SexCode.Male;
                }
                if (speaker.Sex == ProfileData.SexCode.Male)
                {
                    opposite = ProfileData.SexCode.Female;
                }
                speaker.GetHuman.profData.ChangeSex(opposite);
                Plugin.log.LogInfo("Speaker new sex in Prefix : " + speaker.Sex);
                changed = true;
                //Plugin.log.LogInfo("TalkEffect_exeMakeChild.eventLabel : " + TalkEffect.eventLabel.ToString());
            }
        }

        static void Postfix(AnimalData speaker, AnimalData listener, ref int option)
        {
            if (speaker.GetHuman.Sex != listener.GetHuman.Sex && changed == true)
            { 

                Plugin.log.LogInfo("Speaker orig sex in Postfix : " + speaker.Sex);

            Plugin.log.LogInfo("TalkEffect_exeMakeChild.speaker : " + speaker.Name);
            Plugin.log.LogInfo("TalkEffect_exeMakeChild.listener : " + listener.Name);
            Plugin.log.LogInfo("TalkEffect_exeMakeChild.optionOld : " + option);


            Plugin.log.LogInfo("TalkEffect_exeMakeChild.optionNew : " + option);

            Plugin.log.LogWarning("Pregnant ? speaker : " + speaker.GetHuman.IsPregnancy);
            Plugin.log.LogWarning("Pregnant ? listener : " + listener.GetHuman.IsPregnancy);

            if (speaker.Sex == ProfileData.SexCode.Female)
            {
                opposite = ProfileData.SexCode.Male;
            }
            if (speaker.Sex == ProfileData.SexCode.Male)
            {
                opposite = ProfileData.SexCode.Female;
            }
            speaker.GetHuman.profData.ChangeSex(opposite);
                if (speaker.GetHuman.IsPregnancy && speaker.IsMale)
                {
                    speaker.GetHuman.setHumanFlags(HumanData.humanFlags.Pregnant);
                    foreach(var b in speaker.GetHuman.GetBabys(false))
                    {
                        Plugin.log.LogError("bbbbbbbbbbb : " + b.IsActive);
                    }
                      
                }
            Plugin.log.LogInfo("Speaker new sex in Postfix : " + speaker.Sex);
             changed = false;
            }
        }

        
    }
    [HarmonyPatch(typeof(WorldData), "TestMakeBabyLife")]
    static class zero
    {
        static void Postfix(ref bool __result)
        {
            Plugin.log.LogError("Hide ? " + __result);

        }
    }
    [HarmonyPatch(typeof(HumanData), "get_IsMale")]
    static class one
    {
        static bool Prefix(HumanData __instance, ref bool __result)
        {
            if (__instance == WorldData.PlayerData)
            {
                Plugin.log.LogError("IsMalePrefix");
                Plugin.log.LogError(__result);


                __result = true;
                return false;
            }
            return true;
        }
        static void Postfix(HumanData __instance, ref bool __result)
        {
            if(__instance == WorldData.PlayerData)
            {
                Plugin.log.LogError("IsMalePostfix");
                Plugin.log.LogError(__result);

                __result = true;
            }
        }
    }
    [HarmonyPatch(typeof(HumanData), "get_IsFemale")]
    static class two
    {
        static bool Prefix(HumanData __instance, ref bool __result)
        {
            if (__instance == WorldData.PlayerData)
            {
                Plugin.log.LogError("IsFemalePrefix");
                Plugin.log.LogError(__result);


                __result = true;
                return false;
            }
            return true;
        }
        static void Postfix(HumanData __instance, ref bool __result)
        {
            if (__instance == WorldData.PlayerData)
            {
                Plugin.log.LogError("IsFemalePostfix : ");
                Plugin.log.LogError(__result);
                __result = true;
            }
        }
    }

    //Also tested for Pregnancy : EventCheck.Find // RankManager.EventLifeLeader // MatchData.UpdateMatch // EventCheck.AddResult // ScheduleSystem.EventFromTalk.IsMatch // ScheduleSystem.TestReserve // CalendarListCelle.EventObject // CalendarData___ctor_b__29_0
    //TalkEffet.IsMakeEvent // get_Sex etc etc.



    /*[HarmonyPatch(typeof(FamilyData), "AddChild")]
    static class Family
    {
        static void Postfix(FamilyData __instance, ref bool __result)
        {
            Plugin.log.LogInfo(__instance.name);
            Plugin.log.LogInfo("???? : " + __result);
        }
    }*/
    /*[HarmonyPatch(typeof(HumanData), "get_IsPregnancy")]
    static class HumanData_IsPregnancy
    {
        static void Postfix(HumanData __instance, ref bool __result)
        {
            var player = WorldData.PlayerData;
            var partner = WorldData.PlayerData.GetHusband;

            if (__instance.Name == WorldData.PlayerData.Name || __instance.Name == WorldData.PlayerData.GetHusband.Name)
            {

                Plugin.log.LogInfo("Checking for pregnancy for ..." + __instance.Name + " : " + __result);
                if (TalkCondition.chkOkMakeChild(player, partner) == true || TalkCondition.chkOkMakeChild(partner, player))
                {
                    Plugin.log.LogInfo("Now getting serious");
                    Plugin.log.LogInfo("shouldpreg status : " + Plugin.shouldpreg);
                    if (Plugin.shouldpreg == true)
                    {
                        Plugin.log.LogInfo("Shouldpreg is true");
                        __result = true;
                        Plugin.log.LogInfo("Result set to true, someone should be pregnant");
                        Plugin.log.LogInfo("Result for IsPregnancy : " + __instance.Name + " is " + __result);
                    }

                }
            }
        }
    }*/


    [HarmonyPatch(typeof(WorldData), "TestMakeBabyLife")]
    static class TWorldData_TestMakeBabyLife
    {
        static bool Prefix(HumanData mama, ref bool __result)
        {
            Plugin.log.LogInfo("TWorldData_TestMakeBabyLife.mama : " + mama.Name);
            Plugin.log.LogInfo("TWorldData_TestMakeBabyLife.__result : " + __result);
            __result = true;
            return false;
        }
        static void Postfix(HumanData mama, ref bool __result)
        {
            Plugin.log.LogInfo("TWorldData_TestMakeBabyLife.mama : " + mama.Name);
            Plugin.log.LogInfo("TWorldData_TestMakeBabyLife.__result : " + __result);
            __result = true;
        }
    }
    [HarmonyPatch(typeof(TalkEffect), "pregnantCheckerRoyal")]
    static class TalkEffect_pregnantCheckerRoyal
    {
        static void Postfix(HumanData papa, HumanData mama, ref bool __result)
        {
            if(papa.IsPlayer || mama.IsPlayer ||papa == WorldData.PlayerData.GetHusband  || mama == WorldData.PlayerData.GetHusband)
            { 
            Plugin.log.LogInfo("TalkEffect_pregnantCheckerRoyal.mama : " + papa.Name);
            Plugin.log.LogInfo("TalkEffect_pregnantCheckerRoyal.mama : " + mama.Name);
            Plugin.log.LogInfo("TalkEffect_pregnantCheckerRoyal.__result : " + __result);
                papa = WorldData.PlayerData.GetHusband;
                mama = WorldData.PlayerData;
            __result = true;
            }
        }
    }
    [HarmonyPatch(typeof(TalkEffect), "pregnantCheckerBaby")]
    static class TalkEffect_pregnantCheckerBaby
    {
        static void Postfix(HumanData chkMan, ref bool __result)
        {
            Plugin.log.LogInfo("TalkEffect_pregnantCheckerRoyal.mama : " + chkMan.Name);
            Plugin.log.LogInfo("TalkEffect_pregnantCheckerRoyal.__result : " + __result);
            __result = true;
        }
    }

    /*[HarmonyPatch(typeof(TalkData), "ExecuteEffect")]
    static class TalkData_ExecuteEffect
    {
        static void Postfix(int talkId, AnimalData speaker, AnimalData listener, ref bool __result)
        {
            if (speaker == WorldData.PlayerData || listener == WorldData.PlayerData)
            {
            Plugin.log.LogInfo("TalkCata_ExecuteEffect.talkID: " + talkId);
            Plugin.log.LogInfo("TalkCata_ExecuteEffect.speaker : " + speaker.Name);
            Plugin.log.LogInfo("TalkCata_ExecuteEffect.listener : " + listener.Name);
            Plugin.log.LogInfo("TalkCata_ExecuteEffect.__result : " + __result);
            

            }
        }

    }*/


    /*[HarmonyPatch(typeof(EventDelegate), "get_IsValid")]
    static class fdffdf
    {
        static void Postfix(EventDelegate __instance, bool __result)
        {
            Plugin.log.LogWarning("!!!!!!!!!!!!!__result : " + __result);
        }
    }




        static void Postfix(System.Reflection.MethodBase __originalMethod)
        {


                Plugin.log.LogInfo("OriginalMethod called: " + __originalMethod.Name);

        */


    /*[HarmonyPatch(typeof(WorldData), "CreateBaby")]
    static class WorldData2
    {
        static void Postfix(HumanData mama, string pname, ProfileData.SexCode sex, bool yesterday)
        {
            Plugin.log.LogWarning("yep");
        }
    }
    [HarmonyPatch(typeof(FamilyData), "AddChild")]
    static class WorldData3
    {
        static void Postfix(FamilyData __instance)
        {
            Plugin.log.LogWarning("yop");
            Plugin.log.LogWarning(__instance.Husband);
        }
    }


    [HarmonyPatch(typeof(FamilyData), "RecoverBaby4150")]
    static class WorldData5
    {
        static void Postfix(FamilyData __instance, bool __result)
        {
            Plugin.log.LogWarning("yap");
        }
    }

    [HarmonyPatch(typeof(BloodData), "switchSexIndex")]
    static class WorldData6
    {
        static void Postfix(BloodData.Relation __result)
        {
            Plugin.log.LogWarning("ywp");
        }
    }*/
    //IsMother => _CleanupFamily_b__0 => FamilyData
    [HarmonyPatch(typeof(HumanData), "testSafePregnancyLife")]
    static class HumanData_testSafePregnancyLife
    {
        static void Postfix(HumanData __instance, ref bool __result)
        {
            Plugin.log.LogInfo("HumanData_testSafePregnancyLife.Name : " + __instance.Name);
            Plugin.log.LogInfo("HumanData_testSafePregnancyLife : " + __result);
            __result = true;

        }
    }
        [HarmonyPatch(typeof(TalkCondition), "chkYouPregnancyAfter")]
    static class HumanData_chkSpeakPregnancyAfter
    {
        static void Postfix(TalkCondition __instance, ref bool __result, AnimalData spk, AnimalData lsn)
        {
            if (spk.IsPlayer || lsn.IsPlayer)
            {
                Plugin.log.LogInfo("HumanData_chkSpeakPregnancyAfter.__result : " + __result);
                __result = true;
            }
        }
    }
    [HarmonyPatch(typeof(HumanData), "get_IsEnablePregnancy")]
    static class HumanData_get_isEnablePregnancy
    {
        static void Postfix(HumanData __instance, ref bool __result)
        {
            Plugin.log.LogInfo("HumanData_get_isEnablePregnancy.__instance.Name : " + __instance.Name);
            Plugin.log.LogInfo("HumanData_get_isEnablePregnancy.__result : " + __result);
            __result = true;
        }
    }
    /*[HarmonyPatch(typeof(TalkEffect), "Execute")]
    static class TalkCondition_Execute
    {
        static void Postfix(int code, AnimalData speaker, AnimalData listener, int option, ref bool __result)
        {
            Plugin.log.LogInfo("TalkCondition_Execute.code : " + code);
            Plugin.log.LogInfo("TalkCondition_Execute.speaker.Name : " + speaker.Name);
            Plugin.log.LogInfo("TalkCondition_Execute.listener.Name : " + listener.Name);
            Plugin.log.LogInfo("TalkCondition_Execute.option : " + option);
            Plugin.log.LogInfo("TalkCondition_Execute.__result : " + __result);



        }
    }*/


    /*[HarmonyPatch(typeof(TalkCondition), "chkInfertility")]
    static class TalkCondition_chkInfertility
    {
        static void Postfix(TalkCondition __instance, AnimalData spk, AnimalData lsn, ref bool __result)
        {
            Plugin.log.LogInfo("TalkCondition_chkInfertility : " + __result);
        }
    }
    [HarmonyPatch(typeof(TalkCondition), "chkNotInfertility")]
    static class TalkCondition_chkNotInfertility
    {
        static void Postfix(TalkCondition __instance, AnimalData spk, AnimalData lsn, ref bool __result)
        {
            Plugin.log.LogInfo("TalkCondition_chkNotInfertility : " + __result);
        }
    }
    [HarmonyPatch(typeof(TalkEffect), "TestInfertility")]
    static class TalkEffectn_TestInfertility
    {
        static void Postfix(HumanData mama, bool disableLifeCheck, ref bool __result)
        {
            Plugin.log.LogInfo("TalkEffectn_TestInfertility.mama.name : " + mama.Name);
            Plugin.log.LogInfo("TalkEffectn_TestInfertility.disableLifeCheck : " + disableLifeCheck);
            Plugin.log.LogInfo("TalkEffectn_TestInfertility.__result : " + __result);
        }
    }*/
    /*//Useless ?

    [HarmonyPatch(typeof(HumanData), "SetLover")]
    static class HumanDataPatch
    {
        static void Postfix(HumanData __instance, HumanData man, bool __result)
        {
            Plugin.log.LogInfo("HumanDataPatch_man.Name : " + man.Name);
            Plugin.log.LogInfo("HumanDataPatch___result : " + __result);
            if (__result == false)
            {
                __result = true;
                Plugin.log.LogInfo("HumanDataPatch___resultNew : " + __result);

            }

        }
    }*/

    [HarmonyPatch(typeof(HumanData), "testPartner")]
    static class HumanData_testPartner
    {
        static void Postfix(HumanData __instance, HumanData you, ref HumanData.DateType __result)
        {
            Plugin.log.LogInfo("HumanData_testPartner.__instance.Name : " + __instance.Name);
            Plugin.log.LogInfo("HumanData_testPartner.you : " + you.Name);
            Plugin.log.LogInfo("HumanData_testPartner.__result : " + __result);
            Plugin.log.LogInfo("HumanData_testPartner.TestAfterDate : " + __instance.TestAfterDate);




        }
    }


    [HarmonyPatch(typeof(HumanData), "TestLoveNextStep")]
    static class HumanData_TestLoveNextStep
    {
        static void Postfix(HumanData __instance, HumanData lover, bool up, bool __result)
        {
            if(__instance.IsPlayer || lover.IsPlayer)
            { 
            Plugin.log.LogInfo("HumanData_TestLoveNextStep lover.name: " + lover.Name);
            Plugin.log.LogInfo("HumanData_TestLoveNextStep up : " + up);
            Plugin.log.LogInfo("HumanData_TestLoveNextStep __resultOld : " + __result);
            if (__result == false)
            {
                __result = true;
                Plugin.log.LogInfo("HumanDataPatch___resultNew : " + __result);
                __instance.SetLover(lover);

            }
            }
        }
    }
    [HarmonyPatch(typeof(TalkCondition), "chkOtherSex")]
    static class TalkCondition_chkOtherSex
    {
        static void Postfix(TalkCondition __instance, AnimalData spk, AnimalData lsn, ref bool __result)
        {
            if (spk.IsPlayer || lsn.IsPlayer)
            {
                Plugin.log.LogInfo("TalkCondition_chkOtherSex.__result : " + __result);
                Plugin.log.LogInfo("TalkCondition_chkOtherSex.spk : " + spk.Name);
                Plugin.log.LogInfo("TalkCondition_chkOtherSex.lsn : " + lsn.Name);
            }
        }
    }
    [HarmonyPatch(typeof(TalkCondition), "chkEqualSex")]
    static class TalkCondition_chkEqualSex
    {
        static void Postfix(TalkCondition __instance, AnimalData spk, AnimalData lsn, ref bool __result)
        {
            Plugin.log.LogInfo("TalkCondition_chkEqualSex.__result : " + __result);
            Plugin.log.LogInfo("TalkCondition_chkEqualSex.spk : " + spk.Name);
            Plugin.log.LogInfo("TalkCondition_chkEqualSex.lsn : " + lsn.Name);
        }
    }
    /*
    //Uselss ?

    [HarmonyPatch(typeof(FriendInfo), "ChangeFriendRank")]
    static class FriendInfo_ChangeFriendRank
    {

        static void Postfix(ref FriendRank now, ref FriendRank next, ref bool __result)
        {
            Plugin.log.LogInfo("FriendInfo_ChangeFriendRank now : " + now.GetLabel());
            Plugin.log.LogInfo("FriendInfo_ChangeFriendRank next : " + next.GetLabel());
            Plugin.log.LogInfo("FriendInfo_ChangeFriendRank __result : " + __result);

        }
    }*/



    //Useless I think ?
    [HarmonyPatch(typeof(FriendInfo), "get_testOtherGroup")]
    static class FriendInfo_get_testOtherGroup
    {

        static void Postfix(ref bool __result)
        {
            Plugin.log.LogInfo("FriendInfo_get_testOtherGroup __result : " + __result);

        }
    }

    //Uselss ?
    /*[HarmonyPatch(typeof(TalkEffect), "changeFriendRank")]
    static class TalkEffect_changeFriendRank
    {

        static bool Prefix(HumanData spk, HumanData lsn, FriendRank now, FriendRank next, bool __result)
        {
            Plugin.log.LogInfo("TalkEffect_changeFriendRank now : " + now.GetLabel());
            Plugin.log.LogInfo("TalkEffect_changeFriendRank next : " + next.GetLabel());
            Plugin.log.LogInfo("TalkEffect_changeFriendRank spk.Name : " + spk.Name);
            Plugin.log.LogInfo("TalkEffect_changeFriendRank lsn.Name : " + lsn.Name);
            Plugin.log.LogInfo("TalkEffect_changeFriendRank __result : " + __result);
            if (__result == false)
            {
                __result = true;
                return false;
            }
            return true;
        }
    }*/
    //TestCodeSexToOriginalMethod
    /*[HarmonyPatch(typeof(EventPanel), "SetMessage")]
    static class Patch_DisplayMessage
    {
        static void Prefix(EventPanel __instance, string title, string message, EventActor actor)
        {
            Harmony.DEBUG = true;
            Plugin.log.LogInfo("Patch_DisplayMessage.Hello");
            Plugin.log.LogInfo("Patch_DisplayMessage.title : " + title);
            Plugin.log.LogInfo("Patch_DisplayMessage message : " + message);
            Plugin.log.LogInfo("Patch_DisplayMessage actor : " + actor.name + " // actor.Sex" + actor.animalData.Sex);


            var stackTrace = new System.Diagnostics.StackTrace();
            Plugin.log.LogInfo("Full Call Stack:");
            foreach (var frame in stackTrace.GetFrames())
            {
                var method = frame.GetMethod();
                Plugin.log.LogInfo($" - {method.DeclaringType?.Name}.{method.Name}");
            }
        }
    }*/






    //Test - But I think it's useless if I manage to fix IsEI to also effect the partner.
    /*[HarmonyPatch(typeof(HumanData), "IsLover")]
    static class HumanData_IsLover
    {
        static bool Prefix(HumanData __instance, HumanData you, bool __result)
        {

            if( __instance.Name == "Marcel" && you.Name == "Yvon" || __instance.Name == "Yvon" && you.Name == "Marcel")
            { 
            Plugin.log.LogInfo("HumanData_IsLover.__instance.Name : " + __instance.Name);
            Plugin.log.LogInfo("HumanData_IsLover.you.Name : " + you.Name);
            Plugin.log.LogInfo("HumanData_IsLover.__resultOld : " + __result);
            __result = true;
                Plugin.log.LogInfo("HumanData_IsLover.__resultNew : " + __result);
                return false;
            }
            return true;
        }
    }

    //Breaks everything

    /*[HarmonyPatch(typeof(FriendInfo), "get_IsLover1_3")]
    static class FriendInfo_get_IsLover1_3
    {
        static bool Prefix(FriendInfo __instance, bool __result)
        {
            Plugin.log.LogInfo("FriendInfo_get_IsLover1_3___instance.animalData.Name : " + __instance.animalData.Name);
            Plugin.log.LogInfo("FriendInfo_get_IsLover1_3__result : " + __result);
            return true;
        }
    }*/


    //Uselss ?
    [HarmonyPatch(typeof(HumanData), "MakeSilentCouple")]
class Patch_MakeSilentCouple
{
    static bool Prefix(HumanData __instance, HumanData love, ref bool __result)
    {
        if (__instance.IsPlayer)
        {
            Plugin.log.LogInfo("Patch_MakeSilentCouple.__instance.Name : " + __instance.Name);
            Plugin.log.LogInfo("Patch_MakeSilentCouple.__love.Name : " +love.Name);
            Plugin.log.LogInfo("??? : " + love.NewLoverInFriends.Name);
            __result = true; // Override the getter result
            return false; // Skip original getter
        }
        else
        {
            return true;
        }
    }
}
}
