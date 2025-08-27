namespace SC2Expansion{
    [RegisterTypeInIl2Cpp]
    public class SC2BeamCom:MonoBehaviour{
        public SC2BeamCom(IntPtr ptr):base(ptr){}
        public virtual string Name=>"";
        public virtual Vector4 LightColour=>new(1,1,1,1);
        public virtual Vector4 DarkColour=>new(0.5f,0.5f,0.5f,1);
        public virtual Vector4 Colour=>new(1,1,1,0);
        public virtual int Pulse=>14;
        public virtual float Width=>0.16f;
        public virtual float Clip=>0.03f;
        void Start(){
            Material material=GetComponentInChildren<MeshRenderer>().materials[0];
            material.SetColor("_Glow_Light",LightColour);
            material.SetColor("_Glow_Dark",DarkColour);
            material.SetColor("_Beam_Colour",Colour);
            material.SetFloat("_Pulse_Speed",Pulse);
            material.SetFloat("_Beam_Width",Width);
            material.SetFloat("_EdgeClip",Clip);
        }
    }
}