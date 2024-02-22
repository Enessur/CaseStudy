// Toony Colors Pro+Mobile 2
// (c) 2014-2022 Jean Moreno

Shader "Toony Colors Pro 2/User/Stone"
{
	Properties
	{
	[TCP2HeaderHelp(BASE, Base Properties)]
		//TOONY COLORS
		_Color ("Color", Color) = (1,1,1,1)
		_HColor ("Highlight Color", Color) = (0.785,0.785,0.785,1.0)
		_SColor ("Shadow Color", Color) = (0.195,0.195,0.195,1.0)

		//DIFFUSE
		_MainTex ("Main Texture", 2D) = "white" {}
	[TCP2Separator]

		//TOONY COLORS RAMP
		[TCP2Header(RAMP SETTINGS)]

		[TCP2Gradient] _Ramp			("Toon Ramp (RGB)", 2D) = "gray" {}
		_ThresholdTex ("Threshold Texture (Alpha)", 2D) = "gray" {}
	[TCP2Separator]

	[TCP2HeaderHelp(NORMAL MAPPING, Normal Bump Map)]
		//BUMP
		_BumpMap ("Normal map (RGB)", 2D) = "bump" {}
		_BumpScale ("Scale", Float) = 1.0
		[NoScaleOffset] _ParallaxMap ("Heightmap (Alpha)", 2D) = "black" {}
		_Parallax ("Height", Range (0.005, 0.08)) = 0.02
	[TCP2Separator]

	[TCP2HeaderHelp(SPECULAR, Specular)]
		//SPECULAR
		_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
		_Smoothness ("Size", Float) = 0.2
		_SpecSmooth ("Smoothness", Range(0,1)) = 0.05
	[TCP2Separator]

	[TCP2HeaderHelp(RIM, Rim)]
		//RIM LIGHT
		_RimColor ("Rim Color", Color) = (0.8,0.8,0.8,0.6)
		_RimMin ("Rim Min", Range(0,2)) = 0.5
		_RimMax ("Rim Max", Range(0,2)) = 1.0
	[TCP2Separator]

	[TCP2HeaderHelp(SKETCH, Sketch)]
		//SKETCH
		_SketchTex ("Sketch (Alpha)", 2D) = "white" {}
		_SketchSpeed ("Sketch Anim Speed", Range(1.1, 10)) = 6
		_SketchColor ("Sketch Color (RGB)", Color) = (0,0,0,1)
		_SketchHalftoneMin ("Sketch Halftone Min", Range(0,1)) = 0.2
		_SketchHalftoneMax ("Sketch Halftone Max", Range(0,1)) = 1.0
	[TCP2Separator]

	[TCP2HeaderHelp(OUTLINE, Outline)]
		//OUTLINE
		_OutlineColor ("Outline Color", Color) = (0.2, 0.2, 0.2, 1.0)
		_Outline ("Outline Width", Float) = 1

		//Outline Textured
		[Toggle(TCP2_OUTLINE_TEXTURED)] _EnableTexturedOutline ("Color from Texture", Float) = 0
		[TCP2KeywordFilter(TCP2_OUTLINE_TEXTURED)] _TexLod ("Texture LOD", Range(0,10)) = 5

		//Constant-size outline
		[Toggle(TCP2_OUTLINE_CONST_SIZE)] _EnableConstSizeOutline ("Constant Size Outline", Float) = 0

		//ZSmooth
		[Toggle(TCP2_ZSMOOTH_ON)] _EnableZSmooth ("Correct Z Artefacts", Float) = 0
		//Z Correction & Offset
		[TCP2KeywordFilter(TCP2_ZSMOOTH_ON)] _ZSmooth ("Z Correction", Range(-3.0,3.0)) = -0.5
		[TCP2KeywordFilter(TCP2_ZSMOOTH_ON)] _Offset1 ("Z Offset 1", Float) = 0
		[TCP2KeywordFilter(TCP2_ZSMOOTH_ON)] _Offset2 ("Z Offset 2", Float) = 0

		//This property will be ignored and will draw the custom normals GUI instead
		[TCP2OutlineNormalsGUI] __outline_gui_dummy__ ("_unused_", Float) = 0
		//Blending
		[TCP2Header(OUTLINE BLENDING)]
		[Enum(UnityEngine.Rendering.BlendMode)] _SrcBlendOutline ("Blending Source", Float) = 5
		[Enum(UnityEngine.Rendering.BlendMode)] _DstBlendOutline ("Blending Dest", Float) = 10
			_StencilRef ("Stencil Outline Group", Range(0,255)) = 1
	[TCP2Separator]


		//Avoid compile error if the properties are ending with a drawer
		[HideInInspector] __dummy__ ("unused", Float) = 0
	}

	SubShader
	{

		Stencil
		{
			Ref [_StencilRef]
			Comp Always
			Pass Replace
		}

		Tags { "RenderType"="Opaque" }

		CGPROGRAM

		#pragma surface surf ToonyColorsCustom vertex:vert exclude_path:deferred exclude_path:prepass
		#pragma target 3.0

		//================================================================
		// VARIABLES

		fixed4 _Color;
		sampler2D _MainTex;
		sampler2D _ThresholdTex;
		fixed _SketchSpeed;
		sampler2D _BumpMap;
		half _BumpScale;
		sampler2D _ParallaxMap;
		float _Parallax;
		fixed _Smoothness;
		fixed4 _RimColor;
		fixed _RimMin;
		fixed _RimMax;
		float4 _RimDir;
		fixed4 _Random;

		#define UV_MAINTEX uv_MainTex

		struct Input
		{
			half2 uv_MainTex;
			half2 uv_BumpMap;
			float3 viewDir;
			fixed rim;
			half2 sketchUv;
			half2 uv_ThresholdTex;
		};

		//================================================================
		// CUSTOM LIGHTING

		//Lighting-related variables
		fixed4 _HColor;
		fixed4 _SColor;
		sampler2D _Ramp;
		fixed _SpecSmooth;
		sampler2D _SketchTex;
		float4 _SketchTex_ST;
		fixed4 _SketchColor;
		fixed _SketchHalftoneMin;
		fixed _SketchHalftoneMax;

		// Instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		//Custom SurfaceOutput
		struct SurfaceOutputCustom
		{
			half atten;
			fixed3 Albedo;
			fixed3 Normal;
			fixed3 Emission;
			half Specular;
			fixed Gloss;
			fixed Alpha;
			half2 ScreenUVs;
			fixed TexThreshold;
		};

		inline half4 LightingToonyColorsCustom (inout SurfaceOutputCustom s, half3 viewDir, UnityGI gi)
		{
		#define IN_NORMAL s.Normal
	
			half3 lightDir = gi.light.dir;
		#if defined(UNITY_PASS_FORWARDBASE)
			half3 lightColor = _LightColor0.rgb;
			half atten = s.atten;
		#else
			half3 lightColor = gi.light.color.rgb;
			half atten = 1;
		#endif

			IN_NORMAL = normalize(IN_NORMAL);
			fixed ndl = max(0, dot(IN_NORMAL, lightDir));
			#define NDL ndl
			NDL += s.TexThreshold;


			#define		RAMP_TEXTURE	_Ramp

			fixed3 ramp = tex2D(RAMP_TEXTURE, fixed2(NDL,NDL)).rgb;
		#if !(POINT) && !(SPOT)
			ramp *= atten;
		#endif
			//Sketch
			#define SKETCH_RGB	sketch
			fixed sketch = tex2D(_SketchTex, s.ScreenUVs).a;
			sketch = smoothstep(sketch - 0.2, sketch, clamp(ramp, _SketchHalftoneMin, _SketchHalftoneMax));	//Gradient halftone
		// Note: we consider that a directional light with a cookie is supposed to be the main one (even though Unity renders it as an additional light).
		// Thus when using a main directional light AND another directional light with a cookie, then the shadow color might be applied twice.
		// You can remove the DIRECTIONAL_COOKIE check below the prevent that.
		#if !defined(UNITY_PASS_FORWARDBASE) && !defined(DIRECTIONAL_COOKIE)
			_SColor = fixed4(0,0,0,1);
		#endif
			_SColor = lerp(_HColor, _SColor, _SColor.a);	//Shadows intensity through alpha
			ramp = lerp(_SColor.rgb, _HColor.rgb, ramp);
			//Blinn-Phong Specular (legacy)
			half3 h = normalize(lightDir + viewDir);
			float ndh = max(0, dot (IN_NORMAL, h));
			float spec = pow(ndh, s.Specular*128.0) * s.Gloss * 2.0;
			spec = smoothstep(0.5-_SpecSmooth*0.5, 0.5+_SpecSmooth*0.5, spec);
			spec *= atten;
			fixed4 c;
			c.rgb = s.Albedo * lightColor.rgb * ramp;
		#if (POINT || SPOT)
			c.rgb *= atten;
		#endif

			#define SPEC_COLOR	_SpecColor.rgb
			c.rgb += lightColor.rgb * SPEC_COLOR * spec;
			c.a = s.Alpha;
			c.rgb *= lerp(_SketchColor.rgb, fixed3(1,1,1), sketch);

		#ifdef UNITY_LIGHT_FUNCTION_APPLY_INDIRECT
			c.rgb += s.Albedo * gi.indirect.diffuse;
		#endif

			return c;
		}

		void LightingToonyColorsCustom_GI(inout SurfaceOutputCustom s, UnityGIInput data, inout UnityGI gi)
		{
			half colorNoAtten = max(gi.light.color.r, max(gi.light.color.g, gi.light.color.b));
			gi = UnityGlobalIllumination(data, 1.0, IN_NORMAL);

			s.atten = max(gi.light.color.r, max(gi.light.color.g, gi.light.color.b)) / colorNoAtten;	//try to extract attenuation (shadowmap + shadowmask) for lighting function
			gi.light.color = _LightColor0.rgb;	//remove attenuation
		}

		//Vertex input
		struct appdata_tcp2
		{
			float4 vertex : POSITION;
			float3 normal : NORMAL;
			float4 texcoord : TEXCOORD0;
			float4 texcoord1 : TEXCOORD1;
			float4 texcoord2 : TEXCOORD2;
			float4 tangent : TANGENT;
	#if UNITY_VERSION >= 550
			UNITY_VERTEX_INPUT_INSTANCE_ID
	#endif
		};

		//================================================================
		// VERTEX FUNCTION

		void vert(inout appdata_tcp2 v, out Input o)
		{
			UNITY_INITIALIZE_OUTPUT(Input, o);
			float3 viewDir = normalize(ObjSpaceViewDir(v.vertex));
			half rim = 1.0f - saturate( dot(viewDir, v.normal) );
			o.rim = smoothstep(_RimMin, _RimMax, rim);

			//Sketch
			float4 pos = UnityObjectToClipPos(v.vertex);
			float4 screenPos = ComputeScreenPos(pos);
			float2 screenUV = screenPos.xy / screenPos.w;
			float screenRatio = _ScreenParams.y / _ScreenParams.x;
			screenUV.y *= screenRatio;
			o.sketchUv = screenUV;
			o.sketchUv.xy = TRANSFORM_TEX(o.sketchUv, _SketchTex);
			_Random.x = round(_Time.z * _SketchSpeed) / _SketchSpeed;
			_Random.y = -round(_Time.z * _SketchSpeed) / _SketchSpeed;
			o.sketchUv.xy += frac(_Random.xy);
		}

		//================================================================
		// SURFACE FUNCTION

		void surf(Input IN, inout SurfaceOutputCustom o)
		{
			//Parallax Offset
			fixed height = tex2D(_ParallaxMap, IN.uv_BumpMap).a;
			float2 offset = ParallaxOffset(height, _Parallax, IN.viewDir);
			IN.UV_MAINTEX += offset;
			IN.uv_BumpMap += offset;
			fixed4 mainTex = tex2D(_MainTex, IN.UV_MAINTEX);
			o.Albedo = mainTex.rgb * _Color.rgb;
			o.Alpha = mainTex.a * _Color.a;

			//Sketch
			o.ScreenUVs = IN.sketchUv;

			//Specular
			o.Gloss = 1;
			o.Specular = _Smoothness;

			//Normal map
			half4 normalMap = tex2D(_BumpMap, IN.uv_BumpMap.xy);
			o.Normal = UnpackScaleNormal(normalMap, _BumpScale);

			//Rim
			o.Albedo = lerp(o.Albedo.rgb, _RimColor.rgb, IN.rim);

			//Textured Threshold
			o.TexThreshold = tex2D(_ThresholdTex, IN.uv_ThresholdTex).a - 0.5;
		}

		ENDCG
		//Outline behind stencil
		UsePass "Hidden/Toony Colors Pro 2/Outline Stencil/OUTLINE"
	}

	Fallback "Diffuse"
	CustomEditor "TCP2_MaterialInspector_SG"
}
