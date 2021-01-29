Shader "Unlit/Terrain"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
          _Color1 ("_Color1", COLOR) = (1,1,1,1)
          _Color2 ("_Color2", COLOR) =  (1,1,1,1)
            _Color3 ("_Color2", COLOR) =  (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100
        
        Pass
        {
           ColorMask 0
        }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float3 posWorld : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
             float4 _Color1;
              float4 _Color2;
              float4 _Color3;
              
               float pn(float3 p) {
                //noise function by CPU https://www.shadertoy.com/view/4sfGRH
                float3 i = floor(p); 
                float4 a = dot(i, float3(1., 57., 21.)) + float4(0., 57., 21., 78.);
                float3 f = cos((p-i)*3.141592653589793)*(-.5) + .5;  
                a = lerp(sin(cos(a)*a), sin(cos(1.+a)*(1.+a)), f.x);
                a.xy = lerp(a.xz, a.yw, f.y);   
                return lerp(a.x, a.y, f.z);
            }

            float getNoise(float2 pos, float pulse) {
                float3 q = float3(pos * 2., pos.x-pos.y );
                float b = (pulse * 1.6) + pn(q * 2.) + 2.8;
                b +=  .25 * pn(q * 4.);
                b +=  .25  * pn(q * 8.);
                b +=  .5  * pn(float3(pos, pos.x-pos.y ) * 12.23);
                b = pow(b,0.5);	
                return b;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.posWorld  = mul(unity_ObjectToWorld, v.vertex);
                o.normal = v.normal ;
             
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                 float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz -i.posWorld.xyz);
                 float aa = smoothstep(1,0,dot(i.normal,float3(0,1,0)));
                 float mix = smoothstep(0.0,1,dot(i.normal,viewDirection));
                 fixed4 col = tex2D(_MainTex, float2(0,aa));
                 col.rgb = lerp(_Color1,_Color2,mix);
                 col.rgb = lerp(col.rgb,_Color3,smoothstep(0.05,0.04,dot(i.normal,viewDirection)));
               // col.rgb = smoothstep(0.1,5,getNoise(i.uv+sin(_Time.x),1.0));
          
                // apply fog
            
                return col;
            }
            
           
            ENDCG
        }
    }
}
