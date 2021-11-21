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
                    MelonLogger.Msg(objectId);
                    var udn=uObject.Instantiate(TowerAssets.LoadAsset(objectId).Cast<GameObject>(),__instance.PrototypeRoot).AddComponent<UnityDisplayNode>();
                    udn.transform.position=new(-3000,0);
                    udn.name="SC2Expansion-Artanis";
                    udn.isSprite=false;
                    onComplete.Invoke(udn);
                    DisplayDict.Add(objectId,udn);
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
        public record ResourceLoaderLoadSpriteFromSpriteReferenceAsync_Patch{
            [HarmonyPostfix]
            public static void Postfix(SpriteReference reference,ref uImage image){
                if(reference!=null&&reference.guidRef.Contains("Artanis")){
                    var text=TowerAssets.LoadAsset(reference.guidRef).Cast<Texture2D>();
                    image.canvasRenderer.SetTexture(text);
                    image.sprite=Sprite.Create(text,new(0,0,text.width,text.height),new());
                }
            }
        }
        [HarmonyPatch(typeof(Weapon),"SpawnDart")]
        public static class WeaponSpawnDart_Patch{
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
