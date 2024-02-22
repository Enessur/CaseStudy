// Toony Colors Pro+Mobile 2
// (c) 2014-2022 Jean Moreno

Shader "Toony Colors Pro 2/User/My TCP2 Shader"
{
	Properties
	{
		[TCP2HeaderHelp(Base)]
		_Color ("Color", Color) = (1,1,1,1)
		[TCP2ColorNoAlpha] _HColor ("Highlight Color", Color) = (0.75,0.75,0.75,1)
		[TCP2ColorNoAlpha] _SColor ("Shadow Color", Color) = (0.2,0.2,0.2,1)
		[HideInInspector] __BeginGroup_ShadowHSV ("Shadow HSV", Float) = 0
		_Shadow_HSV_H ("Hue", Range(-180,180)) = 0
		_Shadow_HSV_S ("Saturation", Range(-1,1)) = 0
		_Shadow_HSV_V ("Value", Range(-1,1)) = 0
		[HideInInspector] __EndGroup ("Shadow HSV", Float) = 0
		[TCP2Separator]

		[TCP2Header(Ramp Shading)]
		[TCP2Gradient] _Ramp ("Ramp Texture (RGB)", 2D) = "gray" {}
		[TCP2Separator]
		[TCP2HeaderHelp(Terrain)]
		[HideInInspector] TerrainMeta_maskMapTexture ("Mask Map", 2D) = "white" {}
		[HideInInspector] TerrainMeta_normalMapTexture ("Normal Map", 2D) = "bump" {}
		[HideInInspector] TerrainMeta_normalScale ("Normal Scale", Float) = 1
		[Toggle(_TERRAIN_INSTANCED_PERPIXEL_NORMAL)] _EnableInstancedPerPixelNormal("Enable Instanced per-pixel normal", Float) = 1.0
		[TCP2Separator]
		
		[TCP2HeaderHelp(Specular)]
		[Toggle(TCP2_SPECULAR)] _UseSpecular ("Enable Specular", Float) = 0
		[TCP2ColorNoAlpha] _SpecularColor ("Specular Color", Color) = (0.5,0.5,0.5,1)
		_SpecularShadowAttenuation ("Specular Shadow Attenuation", Float) = 0.25
		_SpecularRoughnessPBR ("Roughness", Range(0,1)) = 0.5
		[TCP2Separator]

		[TCP2HeaderHelp(Emission)]
		[TCP2ColorNoAlpha] [HDR] _Emission ("Emission Color", Color) = (0,0,0,1)
		[TCP2Separator]
		
		[TCP2HeaderHelp(Rim Lighting)]
		[TCP2ColorNoAlpha] _RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.5)
		_RimMinVert ("Rim Min", Range(0,2)) = 0.5
		_RimMaxVert ("Rim Max", Range(0,2)) = 1
		//Rim Direction
		_RimDirVert ("Rim Direction", Vector) = (0,0,1,1)
		[TCP2Separator]
		
		[TCP2HeaderHelp(Subsurface Scattering)]
		_SubsurfaceDistortion ("Distortion", Range(0,2)) = 0.2
		_SubsurfacePower ("Power", Range(0.1,16)) = 3
		_SubsurfaceScale ("Scale", Float) = 1
		[TCP2ColorNoAlpha] _SubsurfaceColor ("Color", Color) = (0.5,0.5,0.5,1)
		[TCP2Separator]
		
		[TCP2HeaderHelp(MatCap)]
		[NoScaleOffset] [NoScaleOffset] _MatCapTex ("MatCap (RGB)", 2D) = "gray" {}
		[TCP2ColorNoAlpha] _MatCapColor ("MatCap Color", Color) = (1,1,1,1)
		[TCP2Separator]
		[TCP2HeaderHelp(Ambient Lighting)]
		//AMBIENT CUBEMAP
		_AmbientCube ("Ambient Cubemap", Cube) = "_Skybox" {}
		[TCP2Separator]
		
		[TCP2HeaderHelp(Vertex Displacement)]
		_DisplacementTex ("Displacement Texture", 2D) = "black" {}
		 _DisplacementStrength ("Displacement Strength", Range(-1,1)) = 0.01
		[TCP2Separator]
		
		[TCP2HeaderHelp(Texture Blending)]
		[NoScaleOffset] _BlendingSource ("Blending Source", 2D) = "black" {}
		[TCP2Separator]
		[HideInInspector] __BeginGroup_ShadowHSV ("Shadow Line", Float) = 0
		_ShadowLineThreshold ("Threshold", Range(0,1)) = 0.5
		_ShadowLineSmoothing ("Smoothing", Range(0.001,0.1)) = 0.015
		_ShadowLineStrength ("Strength", Float) = 1
		_ShadowLineColor ("Color (RGB) Opacity (A)", Color) = (0,0,0,1)
		[HideInInspector] __EndGroup ("Shadow Line", Float) = 0
		
		_StylizedThreshold ("Stylized Threshold", 2D) = "gray" {}
		[TCP2Separator]
		
		[TCP2ColorNoAlpha] _DiffuseTint ("Diffuse Tint", Color) = (1,0.5,0,1)
		[TCP2Separator]
		
		[TCP2HeaderHelp(Outline)]
		_OutlineWidth ("Width", Range(0.1,4)) = 1
		_OutlineColorVertex ("Color", Color) = (0,0,0,1)
		[Space]
		_OutlineZSmooth ("Z Correction", Range(-3,3)) = 0
		
		[HideInInspector] [NoScaleOffset] _Normal0 ("Layer 0 Normal Map", 2D) = "bump" {}
		[HideInInspector] [NoScaleOffset] _Normal1 ("Layer 1 Normal Map", 2D) = "bump" {}
		[HideInInspector] [NoScaleOffset] _Normal2 ("Layer 2 Normal Map", 2D) = "bump" {}
		[HideInInspector] [NoScaleOffset] _Normal3 ("Layer 3 Normal Map", 2D) = "bump" {}
		[HideInInspector] _Splat0 ("Layer 0 Albedo", 2D) = "gray" {}
		[HideInInspector] _Splat1 ("Layer 1 Albedo", 2D) = "gray" {}
		[HideInInspector] _Splat2 ("Layer 2 Albedo", 2D) = "gray" {}
		[HideInInspector] _Splat3 ("Layer 3 Albedo", 2D) = "gray" {}

		// Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
			"Queue"="Geometry-100"
			"TerrainCompatible"="True"
		}

		CGINCLUDE

		#include "UnityCG.cginc"
		#include "UnityLightingCommon.cginc"	// needed for LightColor

		// Texture/Sampler abstraction
		#define TCP2_TEX2D_WITH_SAMPLER(tex)						UNITY_DECLARE_TEX2D(tex)
		#define TCP2_TEX2D_NO_SAMPLER(tex)							UNITY_DECLARE_TEX2D_NOSAMPLER(tex)
		#define TCP2_TEX2D_SAMPLE(tex, samplertex, coord)			UNITY_SAMPLE_TEX2D_SAMPLER(tex, samplertex, coord)
		#define TCP2_TEX2D_SAMPLE_LOD(tex, samplertex, coord, lod)	UNITY_SAMPLE_TEX2D_SAMPLER_LOD(tex, samplertex, coord, lod)

		// Terrain
		#define TERRAIN_INSTANCED_PERPIXEL_NORMAL

		//================================================================
		// Terrain Shader specific
		
		//----------------------------------------------------------------
		// Per-layer variables
		
		CBUFFER_START(_Terrain)
			float4 _Control_ST;
			float4 _Control_TexelSize;
			half _DiffuseHasAlpha0, _DiffuseHasAlpha1, _DiffuseHasAlpha2, _DiffuseHasAlpha3;
			half _LayerHasMask0, _LayerHasMask1, _LayerHasMask2, _LayerHasMask3;
			// half4 _Splat0_ST, _Splat1_ST, _Splat2_ST, _Splat3_ST;
			half _NormalScale0, _NormalScale1, _NormalScale2, _NormalScale3;
		
			#ifdef UNITY_INSTANCING_ENABLED
				float4 _TerrainHeightmapRecipSize;   // float4(1.0f/width, 1.0f/height, 1.0f/(width-1), 1.0f/(height-1))
				float4 _TerrainHeightmapScale;       // float4(hmScale.x, hmScale.y / (float)(kMaxHeight), hmScale.z, 0.0f)
			#endif
			#ifdef SCENESELECTIONPASS
				int _ObjectId;
				int _PassValue;
			#endif
		CBUFFER_END
		
		//----------------------------------------------------------------
		// Terrain textures
		
		TCP2_TEX2D_WITH_SAMPLER(_Control);
		
		#if defined(TERRAIN_BASE_PASS)
			TCP2_TEX2D_WITH_SAMPLER(_MainTex);
			TCP2_TEX2D_WITH_SAMPLER(_NormalMap);
		#endif
		
		//----------------------------------------------------------------
		// Terrain Instancing
		
		#if defined(UNITY_INSTANCING_ENABLED) && defined(_TERRAIN_INSTANCED_PERPIXEL_NORMAL)
			#define ENABLE_TERRAIN_PERPIXEL_NORMAL
		#endif
		
		#ifdef UNITY_INSTANCING_ENABLED
			TCP2_TEX2D_NO_SAMPLER(_TerrainHeightmapTexture);
			TCP2_TEX2D_WITH_SAMPLER(_TerrainNormalmapTexture);
		#endif
		
		UNITY_INSTANCING_BUFFER_START(Terrain)
			UNITY_DEFINE_INSTANCED_PROP(float4, _TerrainPatchInstanceData)  // float4(xBase, yBase, skipScale, ~)
		UNITY_INSTANCING_BUFFER_END(Terrain)
		
		void TerrainInstancing(inout float4 positionOS, inout float3 normal, inout float2 uv)
		{
		#ifdef UNITY_INSTANCING_ENABLED
			float2 patchVertex = positionOS.xy;
			float4 instanceData = UNITY_ACCESS_INSTANCED_PROP(Terrain, _TerrainPatchInstanceData);
		
			float2 sampleCoords = (patchVertex.xy + instanceData.xy) * instanceData.z; // (xy + float2(xBase,yBase)) * skipScale
			float height = UnpackHeightmap(_TerrainHeightmapTexture.Load(int3(sampleCoords, 0)));
		
			positionOS.xz = sampleCoords * _TerrainHeightmapScale.xz;
			positionOS.y = height * _TerrainHeightmapScale.y;
		
			#ifdef ENABLE_TERRAIN_PERPIXEL_NORMAL
				normal = float3(0, 1, 0);
			#else
				normal = _TerrainNormalmapTexture.Load(int3(sampleCoords, 0)).rgb * 2 - 1;
			#endif
			uv = sampleCoords * _TerrainHeightmapRecipSize.zw;
		#endif
		}
		
		void TerrainInstancing(inout float4 positionOS, inout float3 normal)
		{
			float2 uv = { 0, 0 };
			TerrainInstancing(positionOS, normal, uv);
		}
		
		//----------------------------------------------------------------
		// Terrain Holes
		
		#if defined(_ALPHATEST_ON)
			TCP2_TEX2D_WITH_SAMPLER(_TerrainHolesTexture);
		
			void ClipHoles(float2 uv)
			{
				float hole = TCP2_TEX2D_SAMPLE(_TerrainHolesTexture, _TerrainHolesTexture, uv).r;
				clip(hole == 0.0f ? -1 : 1);
			}
		#endif
		
		// Shader Properties
		TCP2_TEX2D_WITH_SAMPLER(_DisplacementTex);
		TCP2_TEX2D_WITH_SAMPLER(_Normal0);
		TCP2_TEX2D_NO_SAMPLER(_Normal1);
		TCP2_TEX2D_NO_SAMPLER(_Normal2);
		TCP2_TEX2D_NO_SAMPLER(_Normal3);
		TCP2_TEX2D_WITH_SAMPLER(_BlendingSource);
		TCP2_TEX2D_WITH_SAMPLER(_Splat0);
		TCP2_TEX2D_NO_SAMPLER(_Splat1);
		TCP2_TEX2D_NO_SAMPLER(_Splat2);
		TCP2_TEX2D_NO_SAMPLER(_Splat3);
		TCP2_TEX2D_WITH_SAMPLER(_MatCapTex);
		TCP2_TEX2D_WITH_SAMPLER(_StylizedThreshold);
		
		// Shader Properties
		float4 _DisplacementTex_ST;
		float _DisplacementStrength;
		float _OutlineZSmooth;
		float _OutlineWidth;
		fixed4 _OutlineColorVertex;
		float4 _RimDirVert;
		float _RimMinVert;
		float _RimMaxVert;
		float4 _Splat0_ST;
		float4 _Splat1_ST;
		float4 _Splat2_ST;
		float4 _Splat3_ST;
		fixed4 _Color;
		half4 _Emission;
		fixed4 _MatCapColor;
		float4 _StylizedThreshold_ST;
		float _Shadow_HSV_H;
		float _Shadow_HSV_S;
		float _Shadow_HSV_V;
		fixed4 _HColor;
		fixed4 _SColor;
		fixed4 _DiffuseTint;
		float _ShadowLineThreshold;
		float _ShadowLineStrength;
		float _ShadowLineSmoothing;
		fixed4 _ShadowLineColor;
		float _SubsurfaceDistortion;
		float _SubsurfacePower;
		float _SubsurfaceScale;
		fixed4 _SubsurfaceColor;
		float _SpecularRoughnessPBR;
		float _SpecularShadowAttenuation;
		fixed4 _SpecularColor;
		fixed4 _RimColor;

		sampler2D _Ramp;
		samplerCUBE _AmbientCube;

		//--------------------------------
		// HSV HELPERS
		// source: http://lolengine.net/blog/2013/07/27/rgb-to-hsv-in-glsl
		
		float3 rgb2hsv(float3 c)
		{
			float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
			float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
			float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));
		
			float d = q.x - min(q.w, q.y);
			float e = 1.0e-10;
			return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
		}
		
		float3 hsv2rgb(float3 c)
		{
			c.g = max(c.g, 0.0); //make sure that saturation value is positive
			float4 K = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
			float3 p = abs(frac(c.xxx + K.xyz) * 6.0 - K.www);
			return c.z * lerp(K.xxx, saturate(p - K.xxx), c.y);
		}
		
		float3 ApplyHSV_3(float3 color, float h, float s, float v)
		{
			float3 hsv = rgb2hsv(color.rgb);
			hsv += float3(h/360,s,v);
			return hsv2rgb(hsv);
		}
		float3 ApplyHSV_3(float color, float h, float s, float v) { return ApplyHSV_3(color.xxx, h, s ,v); }
		
		float4 ApplyHSV_4(float4 color, float h, float s, float v)
		{
			float3 hsv = rgb2hsv(color.rgb);
			hsv += float3(h/360,s,v);
			return float4(hsv2rgb(hsv), color.a);
		}
		float4 ApplyHSV_4(float color, float h, float s, float v) { return ApplyHSV_4(color.xxxx, h, s, v); }
		
		//Specular help functions (from UnityStandardBRDF.cginc)
		inline half3 SpecSafeNormalize(half3 inVec)
		{
			half dp3 = max(0.001f, dot(inVec, inVec));
			return inVec * rsqrt(dp3);
		}
		
		//GGX
		#define TCP2_PI			3.14159265359
		#define TCP2_INV_PI		0.31830988618f
		#if defined(SHADER_API_MOBILE)
			#define TCP2_EPSILON 1e-4f
		#else
			#define TCP2_EPSILON 1e-7f
		#endif
		inline half GGX(half NdotH, half roughness)
		{
			half a2 = roughness * roughness;
			half d = (NdotH * a2 - NdotH) * NdotH + 1.0f;
			return TCP2_INV_PI * a2 / (d * d + TCP2_EPSILON);
		}
		
		// Cubic pulse function
		// Adapted from: http://www.iquilezles.org/www/articles/functions/functions.htm (c) 2017 - Inigo Quilez - MIT License
		float linearPulse(float c, float w, float x)
		{
			x = abs(x - c);
			if (x > w)
			{
				return 0;
			}
			x /= w;
			return 1 - x;
		}
		
		ENDCG

		// Outline Include
		CGINCLUDE

		struct appdata_outline
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 texcoord0 : TEXCOORD0;
			#if TCP2_UV2_AS_NORMALS
			float4 texcoord1 : TEXCOORD1;
		#elif TCP2_UV3_AS_NORMALS
			float4 texcoord2 : TEXCOORD2;
		#elif TCP2_UV4_AS_NORMALS
			float4 texcoord3 : TEXCOORD3;
		#endif
		#if TCP2_COLORS_AS_NORMALS
			float4 vertexColor : COLOR;
		#endif
			float4 tangent : TANGENT;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f_outline
		{
			float4 vertex : SV_POSITION;
			float4 vcolor : TEXCOORD0;
			float4 pack1 : TEXCOORD1; /* pack1.xyz = worldNormal  pack1.w = ndl */
			UNITY_VERTEX_OUTPUT_STEREO
		};

		v2f_outline vertex_outline (appdata_outline v)
		{
			v2f_outline output;
			UNITY_INITIALIZE_OUTPUT(v2f_outline, output);
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

			TerrainInstancing(v.vertex, v.normal, v.texcoord0.xy);
				v.tangent.xyz = cross(v.normal, float3(0,0,1));
				v.tangent.w = -1;

			float3 worldNormalUv = mul(unity_ObjectToWorld, float4(v.normal, 1.0)).xyz;
			// Shader Properties Sampling
			float3 __vertexDisplacement = ( v.normal.xyz * TCP2_TEX2D_SAMPLE_LOD(_DisplacementTex, _DisplacementTex, v.texcoord0.xy * _DisplacementTex_ST.xy + _DisplacementTex_ST.zw, 0).rgb * _DisplacementStrength );
			float __outlineLightingWrapFactorVertex = ( 1.0 );
			float __outlineZsmooth = ( _OutlineZSmooth );
			float __outlineWidth = ( _OutlineWidth );
			float4 __outlineColorVertex = ( _OutlineColorVertex.rgba );

			v.vertex.xyz += __vertexDisplacement;
			output.pack1.xyz = worldNormalUv;
			float4 clipPos = output.vertex;

			// Screen Position
			float4 screenPos = ComputeScreenPos(clipPos);
			float3 objSpaceLight = normalize(mul(unity_WorldToObject, _WorldSpaceLightPos0).xyz);
			float3 normal = objSpaceLight.xyz;
			half lightWrap = __outlineLightingWrapFactorVertex;
			half ndl = max(0, (dot(v.normal.xyz, objSpaceLight.xyz) + lightWrap) / (1 + lightWrap));
			output.pack1.w = ndl;
		
			//Z correction in view space
			normal = mul(UNITY_MATRIX_V, float4(normal, 0)).xyz;
			normal.z += __outlineZsmooth;
			normal = mul(float4(normal, 0), UNITY_MATRIX_V).xyz;
		
			//Camera-independent outline size
			float dist = distance(_WorldSpaceCameraPos.xyz, mul(unity_ObjectToWorld, v.vertex).xyz);
			float size = dist;
		
		#if !defined(SHADOWCASTER_PASS)
			output.vertex = UnityObjectToClipPos(v.vertex.xyz + normal * __outlineWidth * size * 0.01);
		#else
			v.vertex = v.vertex + float4(normal,0) * __outlineWidth * size * 0.01;
		#endif
		
			output.vcolor.xyzw = __outlineColorVertex;

			return output;
		}

		float4 fragment_outline (v2f_outline input) : SV_Target
		{

			// Shader Properties Sampling
			float4 __outlineColor = ( float4(1,1,1,1) );

			half4 outlineColor = __outlineColor * input.vcolor.xyzw;
			outlineColor *= input.pack1.w;

			return outlineColor;
		}

		ENDCG
		// Outline Include End
		// Main Surface Shader

		CGPROGRAM

		#pragma surface surf ToonyColorsCustom vertex:vertex_surface exclude_path:deferred exclude_path:prepass keepalpha nolightmap nofog nolppv addshadow
		#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd
		#pragma target 3.0

		//================================================================
		// SHADER KEYWORDS

		#pragma shader_feature_local _TERRAIN_INSTANCED_PERPIXEL_NORMAL
		#pragma multi_compile_local_fragment __ _ALPHATEST_ON
		#pragma shader_feature_local_fragment TCP2_SPECULAR

		//================================================================
		// STRUCTS

		// Vertex input
		struct appdata_tcp2
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 texcoord0 : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
			half4 tangent : TANGENT;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct Input
		{
			half3 viewDir;
			half3 tangent;
			half3 worldNormal; INTERNAL_DATA
			half rim;
			half2 matcap;
			float2 texcoord0;
		};

		//================================================================

		// Custom SurfaceOutput
		struct SurfaceOutputCustom
		{
			half atten;
			half3 Albedo;
			half3 Normal;
			half3 worldNormal;
			half3 Emission;
			half Specular;
			half Gloss;
			half Alpha;
			float3 normalTS;

			Input input;

			half terrainWeight;

			// Shader Properties
			float __stylizedThreshold;
			float __stylizedThresholdScale;
			float __shadowHue;
			float __shadowSaturation;
			float __shadowValue;
			float3 __highlightColor;
			float3 __shadowColor;
			float3 __diffuseTint;
			float __shadowLineThreshold;
			float __shadowLineStrength;
			float __shadowLineSmoothing;
			float4 __shadowLineColor;
			float __ambientIntensity;
			float __subsurfaceDistortion;
			float __subsurfacePower;
			float __subsurfaceScale;
			float3 __subsurfaceColor;
			float __specularRoughnessPbr;
			float __specularShadowAttenuation;
			float3 __specularColor;
			float3 __rimColor;
			float __rimStrength;
		};

		//================================================================
		// VERTEX FUNCTION

		void vertex_surface(inout appdata_tcp2 v, out Input output)
		{
			UNITY_INITIALIZE_OUTPUT(Input, output);

			TerrainInstancing(v.vertex, v.normal, v.texcoord0.xy);
				v.tangent.xyz = cross(v.normal, float3(0,0,1));
				v.tangent.w = -1;

			// Texture Coordinates
			output.texcoord0 = v.texcoord0.xy;
			// Shader Properties Sampling
			float3 __vertexDisplacement = ( v.normal.xyz * TCP2_TEX2D_SAMPLE_LOD(_DisplacementTex, _DisplacementTex, output.texcoord0.xy * _DisplacementTex_ST.xy + _DisplacementTex_ST.zw, 0).rgb * _DisplacementStrength );
			float3 __rimDirVert = ( _RimDirVert.xyz );
			float __rimMinVert = ( _RimMinVert );
			float __rimMaxVert = ( _RimMaxVert );

			v.vertex.xyz += __vertexDisplacement;
			float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
			float4 clipPos = UnityObjectToClipPos(v.vertex);

			// Screen Position
			float4 screenPos = ComputeScreenPos(clipPos);
			half3 viewDir = normalize(UnityWorldSpaceViewDir(worldPos));

			output.tangent = mul(unity_ObjectToWorld, float4(v.tangent.xyz, 0)).xyz;
			half3 rViewDir = viewDir;
			half3 rimDir = __rimDirVert;
			rViewDir = normalize(UNITY_MATRIX_V[0].xyz * rimDir.x + UNITY_MATRIX_V[1].xyz * rimDir.y + UNITY_MATRIX_V[2].xyz * rimDir.z);
			half rim = 1.0f - saturate(dot(rViewDir, v.normal.xyz));
			rim = smoothstep(__rimMinVert, __rimMaxVert, rim);
			output.rim = rim;
			//MatCap
			float3 worldNorm = normalize(unity_WorldToObject[0].xyz * v.normal.x + unity_WorldToObject[1].xyz * v.normal.y + unity_WorldToObject[2].xyz * v.normal.z);
			worldNorm = mul((float3x3)UNITY_MATRIX_V, worldNorm);
			float3 perspectiveOffset = (screenPos.xyz / screenPos.w) - 0.5;
			worldNorm.xy -= (perspectiveOffset.xy * perspectiveOffset.z) * 0.5;
			output.matcap = worldNorm.xy * 0.5 + 0.5;

		}

		//================================================================
		// SURFACE FUNCTION

		void surf(Input input, inout SurfaceOutputCustom output)
		{

			input.worldNormal = WorldNormalVector(input, output.Normal);

			input.worldNormal = WorldNormalVector(input, output.Normal);
			// Shader Properties Sampling
			float4 __layer0NormalMap = ( TCP2_TEX2D_SAMPLE(_Normal0, _Normal0, input.texcoord0.xy * _Splat0_ST.xy + _Splat0_ST.zw).rgba );
			float4 __layer1NormalMap = ( TCP2_TEX2D_SAMPLE(_Normal1, _Normal0, input.texcoord0.xy * _Splat1_ST.xy + _Splat1_ST.zw).rgba );
			float4 __layer2NormalMap = ( TCP2_TEX2D_SAMPLE(_Normal2, _Normal0, input.texcoord0.xy * _Splat2_ST.xy + _Splat2_ST.zw).rgba );
			float4 __layer3NormalMap = ( TCP2_TEX2D_SAMPLE(_Normal3, _Normal0, input.texcoord0.xy * _Splat3_ST.xy + _Splat3_ST.zw).rgba );
			float4 __blendingSource = ( TCP2_TEX2D_SAMPLE(_BlendingSource, _BlendingSource, input.texcoord0.xy).rgba );
			float4 __layer0Albedo = ( TCP2_TEX2D_SAMPLE(_Splat0, _Splat0, input.texcoord0.xy * _Splat0_ST.xy + _Splat0_ST.zw).rgba );
			float4 __layer1Albedo = ( TCP2_TEX2D_SAMPLE(_Splat1, _Splat0, input.texcoord0.xy * _Splat1_ST.xy + _Splat1_ST.zw).rgba );
			float4 __layer2Albedo = ( TCP2_TEX2D_SAMPLE(_Splat2, _Splat0, input.texcoord0.xy * _Splat2_ST.xy + _Splat2_ST.zw).rgba );
			float4 __layer3Albedo = ( TCP2_TEX2D_SAMPLE(_Splat3, _Splat0, input.texcoord0.xy * _Splat3_ST.xy + _Splat3_ST.zw).rgba );
			float4 __mainColor = ( _Color.rgba );
			float3 __emission = ( _Emission.rgb );
			float3 __matcapColor = ( _MatCapColor.rgb );
			output.__stylizedThreshold = ( TCP2_TEX2D_SAMPLE(_StylizedThreshold, _StylizedThreshold, input.texcoord0.xy * _StylizedThreshold_ST.xy + _StylizedThreshold_ST.zw).a );
			output.__stylizedThresholdScale = ( 1.0 );
			output.__shadowHue = ( _Shadow_HSV_H );
			output.__shadowSaturation = ( _Shadow_HSV_S );
			output.__shadowValue = ( _Shadow_HSV_V );
			output.__highlightColor = ( _HColor.rgb );
			output.__shadowColor = ( _SColor.rgb );
			output.__diffuseTint = ( _DiffuseTint.rgb );
			output.__shadowLineThreshold = ( _ShadowLineThreshold );
			output.__shadowLineStrength = ( _ShadowLineStrength );
			output.__shadowLineSmoothing = ( _ShadowLineSmoothing );
			output.__shadowLineColor = ( _ShadowLineColor.rgba );
			output.__ambientIntensity = ( 1.0 );
			output.__subsurfaceDistortion = ( _SubsurfaceDistortion );
			output.__subsurfacePower = ( _SubsurfacePower );
			output.__subsurfaceScale = ( _SubsurfaceScale );
			output.__subsurfaceColor = ( _SubsurfaceColor.rgb );
			output.__specularRoughnessPbr = ( _SpecularRoughnessPBR );
			output.__specularShadowAttenuation = ( _SpecularShadowAttenuation );
			output.__specularColor = ( _SpecularColor.rgb );
			output.__rimColor = ( _RimColor.rgb );
			output.__rimStrength = ( 1.0 );

			output.input = input;

			// Terrain
			
			float2 terrainTexcoord0 = input.texcoord0.xy;
			
			#if defined(_ALPHATEST_ON)
				ClipHoles(terrainTexcoord0.xy);
			#endif
			
			#if defined(TERRAIN_BASE_PASS)
			
				half4 terrain_mixedDiffuse = TCP2_TEX2D_SAMPLE(_MainTex, _MainTex, terrainTexcoord0.xy).rgba;
				half3 normalTS = half3(0.0h, 0.0h, 1.0h);
			
			#else
			
				// Sample the splat control texture generated by the terrain
				// adjust splat UVs so the edges of the terrain tile lie on pixel centers
				float2 terrainSplatUV = (terrainTexcoord0.xy * (_Control_TexelSize.zw - 1.0f) + 0.5f) * _Control_TexelSize.xy;
				half4 terrain_splat_control_0 = TCP2_TEX2D_SAMPLE(_Control, _Control, terrainSplatUV);
			
				// Calculate weights and perform the texture blending
				half terrain_weight = dot(terrain_splat_control_0, half4(1,1,1,1));
			
				#if !defined(SHADER_API_MOBILE) && defined(TERRAIN_SPLAT_ADDPASS)
					clip(terrain_weight == 0.0f ? -1 : 1);
				#endif
			
				// Normalize weights before lighting and restore afterwards so that the overall lighting result can be correctly weighted
				terrain_splat_control_0 /= (terrain_weight + 1e-3f);
			
				// Sample terrain normal maps
				half4 normal0 = __layer0NormalMap;
				half4 normal1 = __layer1NormalMap;
				half4 normal2 = __layer2NormalMap;
				half4 normal3 = __layer3NormalMap;
				#define UnpackFunction UnpackNormalWithScale
				half3 normalTS = UnpackFunction(normal0, _NormalScale0) * terrain_splat_control_0.r;
				normalTS += UnpackFunction(normal1, _NormalScale1) * terrain_splat_control_0.g;
				normalTS += UnpackFunction(normal2, _NormalScale2) * terrain_splat_control_0.b;
				normalTS += UnpackFunction(normal3, _NormalScale3) * terrain_splat_control_0.a;
				normalTS.z += 1e-3f; // to avoid nan after normalizing
			
				output.Normal = normalTS;
			
			#endif // TERRAIN_BASE_PASS
			
			#if defined(INSTANCING_ON) && defined(SHADER_TARGET_SURFACE_ANALYSIS) && defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
				output.Normal = float3(0, 0, 1); // make sure that surface shader compiler realizes we write to normal, as UNITY_INSTANCING_ENABLED is not defined for SHADER_TARGET_SURFACE_ANALYSIS.
			#endif
				
			// Terrain normal, if using instancing and per-pixel normal map
			#if defined(UNITY_INSTANCING_ENABLED) && !defined(SHADER_API_D3D11_9X) && defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
				float2 terrainNormalCoords = (terrainTexcoord0.xy / _TerrainHeightmapRecipSize.zw + 0.5f) * _TerrainHeightmapRecipSize.xy;
				float3 geomNormal = normalize(TCP2_TEX2D_SAMPLE(_TerrainNormalmapTexture, _TerrainNormalmapTexture, terrainNormalCoords.xy).xyz * 2 - 1);
			
				float3 geomTangent = normalize(cross(geomNormal, float3(0, 0, 1)));
				float3 geomBitangent = normalize(cross(geomTangent, geomNormal));
				output.Normal = output.Normal.x * geomTangent
							  + output.Normal.y * geomBitangent
							  + output.Normal.z * geomNormal;
				output.Normal = output.Normal.xzy;
			#endif
			
			// Texture Blending: initialize
			fixed4 blendingSource = __blendingSource;

			half3 worldNormal = WorldNormalVector(input, output.Normal);
			output.worldNormal = worldNormal;

			output.Albedo = half3(1,1,1);
			output.Alpha = 1;

			#if !defined(TERRAIN_BASE_PASS)
				// Sample textures that will be blended based on the terrain splat map
				half4 splat0 = __layer0Albedo;
				half4 splat1 = __layer1Albedo;
				half4 splat2 = __layer2Albedo;
				half4 splat3 = __layer3Albedo;
			
				#define BLEND_TERRAIN_HALF4(outVariable, sourceVariable) \
					half4 outVariable = terrain_splat_control_0.r * sourceVariable##0; \
					outVariable += terrain_splat_control_0.g * sourceVariable##1; \
					outVariable += terrain_splat_control_0.b * sourceVariable##2; \
					outVariable += terrain_splat_control_0.a * sourceVariable##3;
				#define BLEND_TERRAIN_HALF(outVariable, sourceVariable) \
					half4 outVariable = dot(terrain_splat_control_0, half4(sourceVariable##0, sourceVariable##1, sourceVariable##2, sourceVariable##3));
			
				BLEND_TERRAIN_HALF4(terrain_mixedDiffuse, splat)
			
			#endif // !TERRAIN_BASE_PASS
			
			#if !defined(TERRAIN_BASE_PASS)
				output.terrainWeight = terrain_weight;
			#endif
			
			output.Albedo = terrain_mixedDiffuse.rgb;
			output.Alpha = terrain_mixedDiffuse.a;
			
			half4 albedoAlpha = half4(output.Albedo, output.Alpha);
			
			// Texture Blending: sample
			output.Albedo = albedoAlpha.rgb;
			
			output.Albedo *= __mainColor.rgb;
			output.Emission += __emission;
			
			//MatCap
			half2 capCoord = input.matcap;
			half3 matcap = ( TCP2_TEX2D_SAMPLE(_MatCapTex, _MatCapTex, capCoord).rgb ) * __matcapColor;
			output.Emission += matcap;

		}

		//================================================================
		// LIGHTING FUNCTION

		inline half4 LightingToonyColorsCustom(inout SurfaceOutputCustom surface, half3 viewDir, UnityGI gi)
		{

			half3 lightDir = gi.light.dir;
			#if defined(UNITY_PASS_FORWARDBASE)
				half3 lightColor = _LightColor0.rgb;
				half atten = surface.atten;
			#else
				// extract attenuation from point/spot lights
				half3 lightColor = _LightColor0.rgb;
				half atten = max(gi.light.color.r, max(gi.light.color.g, gi.light.color.b)) / max(_LightColor0.r, max(_LightColor0.g, _LightColor0.b));
			#endif

			half3 normal = normalize(surface.Normal);
			half ndl = dot(normal, lightDir);
			float stylizedThreshold = surface.__stylizedThreshold;
			stylizedThreshold -= 0.5;
			stylizedThreshold *= surface.__stylizedThresholdScale;
			ndl += stylizedThreshold;
			// Apply attenuation (shadowmaps & point/spot lights attenuation)
			ndl *= atten;
			half3 ramp;
			
			//Define ramp threshold and smoothstep depending on context
			#define		RAMP_TEXTURE	_Ramp
			half2 rampUv = ndl.xx * 0.5 + 0.5;
			ramp = tex2D(RAMP_TEXTURE, rampUv).rgb;

			//Shadow HSV
			float3 albedoShadowHSV = ApplyHSV_3(surface.Albedo, surface.__shadowHue, surface.__shadowSaturation, surface.__shadowValue);
			surface.Albedo = lerp(albedoShadowHSV, surface.Albedo, ramp);

			// Highlight/Shadow Colors
			#if !defined(UNITY_PASS_FORWARDBASE)
				ramp = lerp(half3(0,0,0), surface.__highlightColor, ramp);
			#else
				ramp = lerp(surface.__shadowColor, surface.__highlightColor, ramp);
			#endif

			// Diffuse Tint
			half3 diffuseTint = saturate(surface.__diffuseTint + ndl);
			ramp *= diffuseTint;
			
			//Shadow Line
			float ndlAtten = ndl * atten;
			float shadowLineThreshold = surface.__shadowLineThreshold;
			float shadowLineStrength = surface.__shadowLineStrength;
			float shadowLineFw = fwidth(ndlAtten);
			float shadowLineSmoothing = surface.__shadowLineSmoothing * shadowLineFw * 10;
			float shadowLine = min(linearPulse(ndlAtten, shadowLineSmoothing, shadowLineThreshold) * shadowLineStrength, 1.0);
			half4 shadowLineColor = surface.__shadowLineColor;
			ramp = lerp(ramp.rgb, shadowLineColor.rgb, shadowLine * shadowLineColor.a);

			// Output color
			half4 color;
			color.rgb = surface.Albedo * lightColor.rgb * ramp;
			color.a = surface.Alpha;

			// Apply indirect lighting (ambient)
			half occlusion = 1;
			#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
				half3 ambient = gi.indirect.diffuse;
				
				//Ambient Cubemap
				ambient.rgb += texCUBE(_AmbientCube, normal);
				ambient *= surface.Albedo * occlusion * surface.__ambientIntensity;

				color.rgb += ambient;
			#endif

				//Subsurface Scattering
			#if (POINT || SPOT)
				half3 ssLight = lightDir + normal * surface.__subsurfaceDistortion;
				half ssDot = pow(saturate(dot(viewDir, -ssLight)), surface.__subsurfacePower) * surface.__subsurfaceScale;
				half3 ssColor = (ssDot * surface.__subsurfaceColor);
			#if !defined(UNITY_PASS_FORWARDBASE)
				ssColor *= atten;
			#endif
				ssColor *= lightColor;
				color.rgb += surface.Albedo * ssColor;
			#endif

			#if defined(TCP2_SPECULAR)
			//Specular: GGX
			half3 halfDir = SpecSafeNormalize(lightDir + viewDir);
			half roughness = surface.__specularRoughnessPbr*surface.__specularRoughnessPbr;
			half nh = saturate(dot(normal, halfDir));
			half spec = GGX(nh, saturate(roughness));
			spec *= TCP2_PI * 0.05;
			#ifdef UNITY_COLORSPACE_GAMMA
				spec = max(0, sqrt(max(1e-4h, spec)));
				half surfaceReduction = 1.0 - 0.28 * roughness * surface.__specularRoughnessPbr;
			#else
				half surfaceReduction = 1.0 / (roughness*roughness + 1.0);
			#endif
			spec = max(0, spec * ndl);
			spec *= surfaceReduction;
			spec *= saturate(atten * ndl + surface.__specularShadowAttenuation);
			
			//Apply specular
			color.rgb += spec * lightColor.rgb * surface.__specularColor;
			#endif
			// Rim Lighting
			#if !defined(UNITY_PASS_FORWARDADD)
			half rim = surface.input.rim;
			rim = ( rim );
			half3 rimColor = surface.__rimColor;
			half rimStrength = surface.__rimStrength;
			color.rgb += rim * rimColor * rimStrength;
			#endif

			#if !defined(TERRAIN_BASE_PASS)
				color.rgb *= surface.terrainWeight;
			#endif

			return color;
		}

		void LightingToonyColorsCustom_GI(inout SurfaceOutputCustom surface, UnityGIInput data, inout UnityGI gi)
		{
			half3 normal = surface.Normal;

			// GI without reflection probes
			gi = UnityGlobalIllumination(data, 1.0, normal); // occlusion is applied in the lighting function, if necessary

			surface.atten = data.atten; // transfer attenuation to lighting function
			gi.light.color = _LightColor0.rgb; // remove attenuation

		}

		ENDCG

		// Outline
		Pass
		{
			Name "Outline"
			Tags
			{
				"LightMode"="ForwardBase"
			}
			Cull Front
			Offset 0,0

			CGPROGRAM
			#pragma vertex vertex_outline
			#pragma fragment fragment_outline
			#pragma target 3.0
			#pragma multi_compile _ TCP2_COLORS_AS_NORMALS TCP2_TANGENT_AS_NORMALS TCP2_UV1_AS_NORMALS TCP2_UV2_AS_NORMALS TCP2_UV3_AS_NORMALS TCP2_UV4_AS_NORMALS
			#pragma multi_compile _ TCP2_UV_NORMALS_FULL TCP2_UV_NORMALS_ZW
			#pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd
			ENDCG
		}
		//================================================================
		// SHADOW CASTER PASS

		// Shadow Caster (for shadows and depth texture)
		Pass
		{
			Name "ShadowCaster"
			Tags
			{
				"LightMode" = "ShadowCaster"
			}
			ZWrite On
			Blend Off

			CGPROGRAM

			#define SHADOWCASTER_PASS

			#pragma vertex vertex_shadowcaster
			#pragma fragment fragment_shadowcaster
			#pragma multi_compile_shadowcaster
			#pragma multi_compile_instancing
			#pragma instancing_options assumeuniformscaling nomatrices nolightprobe nolightmap forwardadd

			#pragma multi_compile TCP2_NONE TCP2_ZSMOOTH_ON
			#pragma multi_compile TCP2_NONE TCP2_OUTLINE_CONST_SIZE
			#pragma multi_compile _ TCP2_COLORS_AS_NORMALS TCP2_TANGENT_AS_NORMALS TCP2_UV1_AS_NORMALS TCP2_UV2_AS_NORMALS TCP2_UV3_AS_NORMALS TCP2_UV4_AS_NORMALS
			#pragma multi_compile _ TCP2_UV_NORMALS_FULL TCP2_UV_NORMALS_ZW

			// half _Cutoff;

			struct appdata_shadowcaster
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float4 texcoord0 : TEXCOORD0;
			#if TCP2_COLORS_AS_NORMALS
				float4 vertexColor : COLOR;
			#endif
			// TODO: need a way to know if texcoord1 is used in the Shader Properties
			#if TCP2_UV2_AS_NORMALS
				float2 uv2 : TEXCOORD1;
			#endif
				float4 tangent : TANGENT;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f_shadowcaster
			{
				V2F_SHADOW_CASTER_NOPOS
				float4 vcolor : TEXCOORD1;
				float4 pack1 : TEXCOORD2; /* pack1.xyz = worldNormal  pack1.w = ndl */
				UNITY_VERTEX_OUTPUT_STEREO
			};

			void vertex_shadowcaster (appdata_shadowcaster v, out v2f_shadowcaster output, out float4 opos : SV_POSITION)
			{
				UNITY_INITIALIZE_OUTPUT(v2f_shadowcaster, output);
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

				float3 worldNormalUv = mul(unity_ObjectToWorld, float4(v.normal, 1.0)).xyz;
				// Shader Properties Sampling
				float3 __vertexDisplacement = ( v.normal.xyz * TCP2_TEX2D_SAMPLE_LOD(_DisplacementTex, _DisplacementTex, v.texcoord0.xy * _DisplacementTex_ST.xy + _DisplacementTex_ST.zw, 0).rgb * _DisplacementStrength );
				float __outlineLightingWrapFactorVertex = ( 1.0 );
				float __outlineZsmooth = ( _OutlineZSmooth );
				float __outlineWidth = ( _OutlineWidth );
				float4 __outlineColorVertex = ( _OutlineColorVertex.rgba );

				v.vertex.xyz += __vertexDisplacement;
				output.pack1.xyz = worldNormalUv;
				float3 objSpaceLight = normalize(mul(unity_WorldToObject, _WorldSpaceLightPos0).xyz);
				float3 normal = objSpaceLight.xyz;
				half lightWrap = __outlineLightingWrapFactorVertex;
				half ndl = max(0, (dot(v.normal.xyz, objSpaceLight.xyz) + lightWrap) / (1 + lightWrap));
				output.pack1.w = ndl;
			
				//Z correction in view space
				normal = mul(UNITY_MATRIX_V, float4(normal, 0)).xyz;
				normal.z += __outlineZsmooth;
				normal = mul(float4(normal, 0), UNITY_MATRIX_V).xyz;
			
				//Camera-independent outline size
				float dist = distance(_WorldSpaceCameraPos.xyz, mul(unity_ObjectToWorld, v.vertex).xyz);
				float size = dist;
			
			#if !defined(SHADOWCASTER_PASS)
				output.vertex = UnityObjectToClipPos(v.vertex.xyz + normal * __outlineWidth * size * 0.01);
			#else
				v.vertex = v.vertex + float4(normal,0) * __outlineWidth * size * 0.01;
			#endif
			
				output.vcolor.xyzw = __outlineColorVertex;
				float4 clipPos = UnityObjectToClipPos(v.vertex);

				// Screen Position
				float4 screenPos = ComputeScreenPos(clipPos);

				TRANSFER_SHADOW_CASTER_NOPOS(output,opos)
			}

			half4 fragment_shadowcaster(v2f_shadowcaster input, UNITY_VPOS_TYPE vpos : VPOS) : SV_Target
			{

				SHADOW_CASTER_FRAGMENT(input)
			}

			ENDCG
		}
		UsePass "Hidden/Nature/Terrain/Utilities/PICKING"
		UsePass "Hidden/Nature/Terrain/Utilities/SELECTION"
	}

	Dependency "AddPassShader"    = "Hidden/Toony Colors Pro 2/User/My TCP2 Shader-AddPass"
	Dependency "BaseMapShader"    = "Hidden/Toony Colors Pro 2/User/My TCP2 Shader-BasePass"
	Dependency "BaseMapGenShader" = "Hidden/Toony Colors Pro 2/User/My TCP2 Shader-BaseGen"

	Fallback "Diffuse"
	CustomEditor "ToonyColorsPro.ShaderGenerator.MaterialInspector_SG2"
}

/* TCP_DATA u config(ver:"2.9.1";unity:"2021.3.14f1";tmplt:"SG2_Template_Default";features:list["UNITY_5_4","UNITY_5_5","UNITY_5_6","UNITY_2017_1","UNITY_2018_1","UNITY_2018_2","UNITY_2018_3","UNITY_2019_1","UNITY_2019_2","UNITY_2019_3","UNITY_2019_4","UNITY_2020_1","UNITY_2021_1","TERRAIN_SHADER","EMISSION","RIM","RIM_VERTEX","MATCAP_ADD","MATCAP","TEXTURE_RAMP","SHADOW_HSV","ATTEN_AT_NDL","SPEC_PBR_GGX","SPECULAR","SPECULAR_NO_ATTEN","SPECULAR_SHADER_FEATURE","RIM_DIR","SUBSURFACE_SCATTERING","MATCAP_PERSPECTIVE_CORRECTION","CUBE_AMBIENT","VERTEX_DISPLACEMENT","BUMP","WORLD_NORMAL_FROM_BUMP","TEXTURE_BLENDING","TEXBLEND_LINEAR","SHADOW_LINE","SHADOW_LINE_CRISP_AA","TEXTURED_THRESHOLD","DIFFUSE_TINT","OUTLINE","OUTLINE_CONSTANT_SIZE","OUTLINE_ZSMOOTH","OUTLINE_LIGHTING_VERT","OUTLINE_LIGHTING","OUTLINE_LIGHTING_WRAP","OUTLINE_FAKE_RIM_DIRLIGHT","OUTLINE_SHADOWCASTER"];flags:list[];flags_extra:dict[];keywords:dict[RENDER_TYPE="Opaque",RampTextureDrawer="[TCP2Gradient]",RampTextureLabel="Ramp Texture",SHADER_TARGET="3.0",BASEGEN_ALBEDO_DOWNSCALE="1",BASEGEN_MASKTEX_DOWNSCALE="1/2",BASEGEN_METALLIC_DOWNSCALE="1/4",BASEGEN_SPECULAR_DOWNSCALE="1/4",BASEGEN_DIFFUSEREMAPMIN_DOWNSCALE="1/4",BASEGEN_MASKMAPREMAPMIN_DOWNSCALE="1/4",RIM_LABEL="Rim Lighting"];shaderProperties:list[];customTextures:list[];codeInjection:codeInjection(injectedFiles:list[];mark:False);matLayers:list[]) */
/* TCP_HASH f23605b9003ba1ecbf4392ba19c7263e */
