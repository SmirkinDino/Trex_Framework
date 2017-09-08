// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/PointLight2"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_SourcePos("SourcePos", Vector) = (0, 0, 0, 0)
		_LightIntensity("Intensitiy", Float) = 0
		_LightRangeT("Range", Float) = 0
		_SpotTowards("Towards", Vector) = (0, 0, 1, 0)
		_Breath("Breath", Float) = 0
	}

		SubShader{

		Tags{ "Queue" = "Transparent" }

		ZWrite Off

		GrabPass{}

		Pass{

		CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#include "UnityCG.cginc"

		sampler2D _GrabTexture;
	float4 _GrabTexture_ST;

	sampler2D _MainTex;
	float4 _MainTex_ST;
	float4 _SourcePos;
	float _LightIntensity;
	float _LightRangeT;
	float4 _SpotTowards;
	float _Breath;

	struct v2f {
		float4 pos : POSITION;
		float3 worldPos : TEXCOORD1;
		float4 uv : TEXCOORD0;
	};

	v2f vert(appdata_base v)
	{
		v2f o;

		o.pos = UnityObjectToClipPos(v.vertex);

#if UNITY_UV_STARTS_AT_TOP
		float scale = -1.0;
#else
		float scale = 1.0;
#endif

		o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
		o.uv.xy = (float2(o.pos.x, o.pos.y*scale) + o.pos.w) * 0.5;
		o.uv.zw = o.pos.zw;
		return o;
	}

	float4 frag(v2f i) : COLOR
	{
		// 计算平行正面光照衰减
		float dis = min(distance(_SourcePos.xyz , i.worldPos),_LightIntensity);

	float intensitiy = 1.0 - dis / _LightIntensity;

	// 计算侧面光照衰减
	float3 dir = normalize(i.worldPos - _SourcePos.xyz);

	float cosT = dot(_SpotTowards.xyz, dir) / (length(_SpotTowards.xyz) * length(dir));

	float sinT = sqrt(1 - cosT * cosT);

	float intensitiyCross = 1 - (sinT / _LightRangeT);

	float outColor = min(intensitiyCross * intensitiy * intensitiy, 1);// * _Breath;

	float last_x = i.uv.x / i.uv.w;
	float last_y = i.uv.y / i.uv.w;
	half4 texCol = tex2D(_GrabTexture, float2(last_x, last_y));
	// 颜色反相，便于观察效果
	return texCol  * 10;
	}
		ENDCG
	}
	}
}
