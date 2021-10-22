using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.ModOptions;
using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading.Tasks;
using MelonLoader;
[assembly:MelonGame("Ninja Kiwi","BloonsTD6")]
[assembly:MelonInfo(typeof(SC2ExpansionModOptions.SC2ExpansionModOptions),"SC2ExpansionModOptions","1.0.0","Silentstorm#5336")]
namespace SC2ExpansionModOptions{
    public class SC2ExpansionModOptions:BloonsTD6Mod{
        public static readonly ModSettingBool ProtossEnabled=true;
        public static readonly ModSettingBool TerranEnabled=true;
        public static readonly ModSettingBool ZergEnabled=true;
        public static readonly ModSettingBool RemoveBaseTowers=false;
        private static readonly ModSettingBool HeroesEnabled=true;
        public override void OnApplicationStart(){
            Task.Run(()=>SettingsServerStart());
            MelonLogger.Msg("Server started");
        }
        public static void SettingsServerStart(){
            var server=new NamedPipeServerStream("SC2ExpansionModOptions");
            server.WaitForConnection();
            StreamWriter writer=new StreamWriter(server);
            StreamReader reader=new StreamReader(server);
            while(true){
                if(reader!=null){
                    object data="NULL";
                    switch(reader.ReadLine()){
                        case"ProtossEnabled":
                            data=ProtossEnabled.GetValue();
                            break;
                        case"TerranEnabled":
                            data=TerranEnabled.GetValue();
                            break;
                        case"ZergEnabled":
                            data=ZergEnabled.GetValue();
                            break;
                        case"RemoveBaseTowers":
                            data=RemoveBaseTowers.GetValue();
                            break;
                        case"HeroesEnabled":
                            data=HeroesEnabled.GetValue();
                            break;
                    }
                    writer.WriteLine(data);
                    writer.Flush();
                }
            }
        }
    }
}
