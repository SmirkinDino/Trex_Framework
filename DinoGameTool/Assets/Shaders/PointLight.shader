

Shader "DRender/DiffuseAndSpecular"
{
    Properties {
		_Color ("Diffuse Material Color", Color) = (1,1,1,1) 
		_SpecColor ("Specular Material Color", Color) = (1,1,1,1) 
		_Shininess ("Shininess", Float) = 10
    }
	SubShader
	{
		Pass
		{
			Tags {"LightMode" = "ForwardBase"}

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			
			uniform float4 _Color;
			uniform float _Shininess;

			struct vert_input
			{
				float4 model_vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct frag_input
			{
				float4 pos : SV_POSITION;
				float4 world_vertex : TEXCOORD1;
				float3 world_normal : TEXCOORD0;
			};
			
			frag_input vert (vert_input v)
			{
				frag_input o;

				o.pos = UnityObjectToClipPos(v.model_vertex);
				o.world_vertex = mul(unity_ObjectToWorld, v.model_vertex);
				o.world_normal = v.normal;

				return o;
			}
			
			fixed4 frag (frag_input i) : Color
			{
				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;
 
				float3 normalDirection = normalize(mul(float4(i.world_normal, 0.0), modelMatrixInverse).xyz);
				float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
 
				float3 diffuseReflection = _LightColor0.rgb * _Color.rgb * max(0.0, dot(normalDirection, lightDirection));

				float3 viewDirection = normalize(_WorldSpaceCameraPos - i.world_vertex.xyz);

				float3 specularReflection;

				if(dot(normalDirection, lightDirection) < 0.0){
					specularReflection = float3(0.0, 0.0, 0.0);
				}
				else{
					specularReflection = _LightColor0.rgb * _SpecColor.rgb * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);
				}

				return float4(diffuseReflection + specularReflection + UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb, 1);
			}
			ENDCG
		}

		Pass
		{
			Tags {"LightMode" = "ForwardAdd"}

			Blend One One

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			
			uniform float4 _Color;
			uniform float _Shininess;

			struct vert_input
			{
				float4 model_vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct frag_input
			{
				float4 pos : SV_POSITION;
				float4 world_vertex : TEXCOORD1;
				float3 world_normal : TEXCOORD0;
			};
			
			frag_input vert (vert_input v)
			{
				frag_input o;

				o.pos = UnityObjectToClipPos(v.model_vertex);
				o.world_vertex = mul(unity_ObjectToWorld, v.model_vertex);
				o.world_normal = v.normal;

				return o;
			}
			
			fixed4 frag (frag_input i) : Color
			{
				float4x4 modelMatrix = unity_ObjectToWorld;
				float4x4 modelMatrixInverse = unity_WorldToObject;
 
				float3 normalDirection = normalize(mul(float4(i.world_normal, 0.0), modelMatrixInverse).xyz);
				float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
 
				float3 diffuseReflection = _LightColor0.rgb * _Color.rgb * max(0.0, dot(normalDirection, lightDirection));

				float3 viewDirection = normalize(_WorldSpaceCameraPos - i.world_vertex.xyz);

				float3 specularReflection;

				if(dot(normalDirection, lightDirection) < 0.0){
					specularReflection = float3(0.0, 0.0, 0.0);
				}
				else{
					specularReflection = _LightColor0.rgb * _SpecColor.rgb * pow(max(0.0, dot(reflect(-lightDirection, normalDirection), viewDirection)), _Shininess);
				}

				return float4(diffuseReflection + specularReflection + UNITY_LIGHTMODEL_AMBIENT.rgb * _Color.rgb, 1);
			}
			ENDCG
		}
	}
	Fallback "Specular"
}
