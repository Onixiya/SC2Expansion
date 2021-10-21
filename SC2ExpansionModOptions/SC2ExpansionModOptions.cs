using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.ModOptions;
using System;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Threading.Tasks;
using MelonLoader;
namespace SC2ExpansionModOptions{
    public class SC2ExpansionModOptions:BloonsTD6Mod{
        public static readonly ModSettingBool ProtossEnabled=true;
        public static readonly ModSettingBool TerranEnabled=true;
        public static readonly ModSettingBool ZergEnabled=true;
        public static readonly ModSettingBool RemoveBaseTowers=false;
        private static readonly ModSettingBool HeroesEnabled=true;
        public static void StartServer(){
            var server=new NamedPipeServerStream("SC2ExpansionModOptions");
            server.WaitForConnection();
            StreamWriter writer=new StreamWriter(server);
            StreamReader reader=new StreamReader(server);
            while(true){
                if(reader!=null){
                    writer.WriteLine(reader);
                    writer.Flush();
                }
            }
        }
    }
}
