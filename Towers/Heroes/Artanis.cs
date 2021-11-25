namespace SC2Expansion.Towers{
    public static class Artanis{
        public static AssetBundle TowerAssets=AssetBundle.LoadFromMemory(Assets.Assets.artanis);
        public static void ArtanisSetup(){
            var Artanis=Game.instance.model.GetTowerFromId("Sauda");
            Artanis.name="Artanis";
            Artanis.display="ArtanisPrefab";
            Artanis.GetBehavior<DisplayModel>().display=Artanis.display;
            //Artanis.b
            var PsiBlades=Artanis.GetAttackModel();
        }
        [HarmonyPatch(typeof(Factory),"FindAndSetupPrototypeAsync")]
        public class FactoryFindAndSetupPrototypeAsync_Patch{
            [HarmonyPrefix]
            public static bool Prefix(Factory __instance,string objectId,Il2CppSystem.Action<UnityDisplayNode>onComplete){
                if(!DisplayDict.ContainsKey(objectId)&&objectId.Contains("Artanis")){
                    LoadModel(TowerAssets,objectId,__instance,onComplete);
                    return false;
                }
                if(DisplayDict.ContainsKey(objectId)){
                    onComplete.Invoke(DisplayDict[objectId]);
                    return false;
                }
                return true;
            }
        }
        [HarmonyPatch(typeof(ResourceLoader),"LoadSpriteFromSpriteReferenceAsync")]
        public class ResourceLoaderLoadSpriteFromSpriteReferenceAsync_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref uImage image){
                if(reference!=null&&reference.guidRef.StartsWith("Artanis")){
                    LoadImage(TowerAssets,reference.guidRef,image);
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),"SpawnDart")]
        public class WeaponSpawnDart_Patch{
            [HarmonyPostfix]
            public static void Postfix(ref Weapon __instance){
                MelonLogger.Msg(__instance.attack.tower.towerModel.name);
                if(__instance.attack.tower.towerModel.name.Contains("Artanis")){
                    __instance.attack.tower.Node.graphic.GetComponentInParent<Animator>().Play("ArtanisAttackClose");
                }
            }
        }
    }
}
