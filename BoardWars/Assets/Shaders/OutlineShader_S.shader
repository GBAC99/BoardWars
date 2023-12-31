Shader "Unlit/OutlineShader_S"
{
	Properties
	{
		_OutlineScale("OutlineScale", Float) = 1.1
		[HDR]_OutlineColor("OutlineColor", Color) = (1, 0, 0, 0)
		_Opacity("Opacity", Range(0, 1)) = 1
	}
		SubShader
	{
		Tags
	{
		"RenderPipeline" = "LightweightPipeline"
		"RenderType" = "Transparent"
		"UniversalMaterialType" = "Unlit"
		"Queue" = "Transparent+0"
	}
		Pass
	{
		Name "Pass"
		Tags
	{
		// LightMode: <None>
	}

	// Render State
		Cull Front
		Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
		ZTest LEqual
		ZWrite Off

		// Debug
		// <None>

		// --------------------------------------------------
		// Pass

		HLSLPROGRAM

		// Pragmas
#pragma target 4.5
#pragma exclude_renderers gles gles3 glcore
#pragma multi_compile_instancing
#pragma multi_compile_fog
#pragma multi_compile _ DOTS_INSTANCING_ON
#pragma vertex vert
#pragma fragment frag

		// DotsInstancingOptions: <None>
		// HybridV1InjectedBuiltinProperties: <None>

		// Keywords
#pragma multi_compile _ LIGHTMAP_ON
#pragma multi_compile _ DIRLIGHTMAP_COMBINED
#pragma shader_feature _ _SAMPLE_GI
		// GraphKeywords: <None>

		// Defines
#define _SURFACE_TYPE_TRANSPARENT 1
#define ATTRIBUTES_NEED_NORMAL
#define ATTRIBUTES_NEED_TANGENT
#define VARYINGS_NEED_CULLFACE
#define FEATURES_GRAPH_VERTEX
		/* WARNING: $splice Could not find named fragment 'PassInstancing' */
#define SHADERPASS SHADERPASS_UNLIT
		/* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

		// Includes
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

		// --------------------------------------------------
		// Structs and Packing

		struct Attributes
	{
		float3 positionOS : POSITION;
		float3 normalOS : NORMAL;
		float4 tangentOS : TANGENT;
#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : INSTANCEID_SEMANTIC;
#endif
	};
	struct Varyings
	{
		float4 positionCS : SV_POSITION;
#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : CUSTOM_INSTANCE_ID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
#endif
	};
	struct SurfaceDescriptionInputs
	{
		float FaceSign;
	};
	struct VertexDescriptionInputs
	{
		float3 ObjectSpaceNormal;
		float3 ObjectSpaceTangent;
		float3 ObjectSpacePosition;
	};
	struct PackedVaryings
	{
		float4 positionCS : SV_POSITION;
#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : CUSTOM_INSTANCE_ID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
#endif
	};

	PackedVaryings PackVaryings(Varyings input)
	{
		PackedVaryings output;
		output.positionCS = input.positionCS;
#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
#endif
		return output;
	}
	Varyings UnpackVaryings(PackedVaryings input)
	{
		Varyings output;
		output.positionCS = input.positionCS;
#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
#endif
		return output;
	}

	// --------------------------------------------------
	// Graph

	// Graph Properties
	CBUFFER_START(UnityPerMaterial)
		float _OutlineScale;
	float4 _OutlineColor;
	float _Opacity;
	CBUFFER_END

		// Object and Global properties

		// Graph Functions

		void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
	{
		Out = A * B;
	}

	void Unity_Branch_float(float Predicate, float True, float False, out float Out)
	{
		Out = Predicate ? True : False;
	}

	// Graph Vertex
	struct VertexDescription
	{
		float3 Position;
		float3 Normal;
		float3 Tangent;
	};

	VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
	{
		VertexDescription description = (VertexDescription)0;
		float _Property_4e77a4270a654dc1b68438fbdb35afd6_Out_0 = _OutlineScale;
		float3 _Multiply_4515fd1de82649029750c25bcea7cc00_Out_2;
		Unity_Multiply_float((_Property_4e77a4270a654dc1b68438fbdb35afd6_Out_0.xxx), IN.ObjectSpacePosition, _Multiply_4515fd1de82649029750c25bcea7cc00_Out_2);
		description.Position = _Multiply_4515fd1de82649029750c25bcea7cc00_Out_2;
		description.Normal = IN.ObjectSpaceNormal;
		description.Tangent = IN.ObjectSpaceTangent;
		return description;
	}

	// Graph Pixel
	struct SurfaceDescription
	{
		float3 BaseColor;
		float Alpha;
	};

	SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
	{
		SurfaceDescription surface = (SurfaceDescription)0;
		float4 _Property_9647e2796a1140c99a5ca7e26f838d82_Out_0 = IsGammaSpace() ? LinearToSRGB(_OutlineColor) : _OutlineColor;
		float _IsFrontFace_2f6ebd2e8cf04c6094812bf17eff70df_Out_0 = max(0, IN.FaceSign);
		float _Property_dfcbbf7308a64128bcd7e22912fd1159_Out_0 = _Opacity;
		float _Branch_98aebd08dfef4c2baee90ebe9ffaa24a_Out_3;
		Unity_Branch_float(_IsFrontFace_2f6ebd2e8cf04c6094812bf17eff70df_Out_0, _Property_dfcbbf7308a64128bcd7e22912fd1159_Out_0, 0, _Branch_98aebd08dfef4c2baee90ebe9ffaa24a_Out_3);
		surface.BaseColor = (_Property_9647e2796a1140c99a5ca7e26f838d82_Out_0.xyz);
		surface.Alpha = _Branch_98aebd08dfef4c2baee90ebe9ffaa24a_Out_3;
		return surface;
	}

	// --------------------------------------------------
	// Build Graph Inputs

	VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
	{
		VertexDescriptionInputs output;
		ZERO_INITIALIZE(VertexDescriptionInputs, output);

		output.ObjectSpaceNormal = input.normalOS;
		output.ObjectSpaceTangent = input.tangentOS.xyz;
		output.ObjectSpacePosition = input.positionOS;

		return output;
	}
	SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
	{
		SurfaceDescriptionInputs output;
		ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
		BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

			return output;
	}

	// --------------------------------------------------
	// Main

#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"

	ENDHLSL
	}
		Pass
	{
		Name "ShadowCaster"
		Tags
	{
		"LightMode" = "ShadowCaster"
	}

		// Render State
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
		ZTest LEqual
		ZWrite On
		ColorMask 0

		// Debug
		// <None>

		// --------------------------------------------------
		// Pass

		HLSLPROGRAM

		// Pragmas
#pragma target 4.5
#pragma exclude_renderers gles gles3 glcore
#pragma multi_compile_instancing
#pragma multi_compile _ DOTS_INSTANCING_ON
#pragma vertex vert
#pragma fragment frag

		// DotsInstancingOptions: <None>
		// HybridV1InjectedBuiltinProperties: <None>

		// Keywords
		// PassKeywords: <None>
		// GraphKeywords: <None>

		// Defines
#define _SURFACE_TYPE_TRANSPARENT 1
#define ATTRIBUTES_NEED_NORMAL
#define ATTRIBUTES_NEED_TANGENT
#define VARYINGS_NEED_CULLFACE
#define FEATURES_GRAPH_VERTEX
		/* WARNING: $splice Could not find named fragment 'PassInstancing' */
#define SHADERPASS SHADERPASS_SHADOWCASTER
		/* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

		// Includes
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

		// --------------------------------------------------
		// Structs and Packing

		struct Attributes
	{
		float3 positionOS : POSITION;
		float3 normalOS : NORMAL;
		float4 tangentOS : TANGENT;
#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : INSTANCEID_SEMANTIC;
#endif
	};
	struct Varyings
	{
		float4 positionCS : SV_POSITION;
#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : CUSTOM_INSTANCE_ID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
#endif
	};
	struct SurfaceDescriptionInputs
	{
		float FaceSign;
	};
	struct VertexDescriptionInputs
	{
		float3 ObjectSpaceNormal;
		float3 ObjectSpaceTangent;
		float3 ObjectSpacePosition;
	};
	struct PackedVaryings
	{
		float4 positionCS : SV_POSITION;
#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : CUSTOM_INSTANCE_ID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
#endif
	};

	PackedVaryings PackVaryings(Varyings input)
	{
		PackedVaryings output;
		output.positionCS = input.positionCS;
#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
#endif
		return output;
	}
	Varyings UnpackVaryings(PackedVaryings input)
	{
		Varyings output;
		output.positionCS = input.positionCS;
#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
#endif
		return output;
	}

	// --------------------------------------------------
	// Graph

	// Graph Properties
	CBUFFER_START(UnityPerMaterial)
		float _OutlineScale;
	float4 _OutlineColor;
	float _Opacity;
	CBUFFER_END

		// Object and Global properties

		// Graph Functions

		void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
	{
		Out = A * B;
	}

	void Unity_Branch_float(float Predicate, float True, float False, out float Out)
	{
		Out = Predicate ? True : False;
	}

	// Graph Vertex
	struct VertexDescription
	{
		float3 Position;
		float3 Normal;
		float3 Tangent;
	};

	VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
	{
		VertexDescription description = (VertexDescription)0;
		float _Property_4e77a4270a654dc1b68438fbdb35afd6_Out_0 = _OutlineScale;
		float3 _Multiply_4515fd1de82649029750c25bcea7cc00_Out_2;
		Unity_Multiply_float((_Property_4e77a4270a654dc1b68438fbdb35afd6_Out_0.xxx), IN.ObjectSpacePosition, _Multiply_4515fd1de82649029750c25bcea7cc00_Out_2);
		description.Position = _Multiply_4515fd1de82649029750c25bcea7cc00_Out_2;
		description.Normal = IN.ObjectSpaceNormal;
		description.Tangent = IN.ObjectSpaceTangent;
		return description;
	}

	// Graph Pixel
	struct SurfaceDescription
	{
		float Alpha;
	};

	SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
	{
		SurfaceDescription surface = (SurfaceDescription)0;
		float _IsFrontFace_2f6ebd2e8cf04c6094812bf17eff70df_Out_0 = max(0, IN.FaceSign);
		float _Property_dfcbbf7308a64128bcd7e22912fd1159_Out_0 = _Opacity;
		float _Branch_98aebd08dfef4c2baee90ebe9ffaa24a_Out_3;
		Unity_Branch_float(_IsFrontFace_2f6ebd2e8cf04c6094812bf17eff70df_Out_0, _Property_dfcbbf7308a64128bcd7e22912fd1159_Out_0, 0, _Branch_98aebd08dfef4c2baee90ebe9ffaa24a_Out_3);
		surface.Alpha = _Branch_98aebd08dfef4c2baee90ebe9ffaa24a_Out_3;
		return surface;
	}

	// --------------------------------------------------
	// Build Graph Inputs

	VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
	{
		VertexDescriptionInputs output;
		ZERO_INITIALIZE(VertexDescriptionInputs, output);

		output.ObjectSpaceNormal = input.normalOS;
		output.ObjectSpaceTangent = input.tangentOS.xyz;
		output.ObjectSpacePosition = input.positionOS;

		return output;
	}
	SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
	{
		SurfaceDescriptionInputs output;
		ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
		BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

			return output;
	}

	// --------------------------------------------------
	// Main

#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"

	ENDHLSL
	}
		Pass
	{
		Name "DepthOnly"
		Tags
	{
		"LightMode" = "DepthOnly"
	}

		// Render State
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
		ZTest LEqual
		ZWrite On
		ColorMask 0

		// Debug
		// <None>

		// --------------------------------------------------
		// Pass

		HLSLPROGRAM

		// Pragmas
#pragma target 4.5
#pragma exclude_renderers gles gles3 glcore
#pragma multi_compile_instancing
#pragma multi_compile _ DOTS_INSTANCING_ON
#pragma vertex vert
#pragma fragment frag

		// DotsInstancingOptions: <None>
		// HybridV1InjectedBuiltinProperties: <None>

		// Keywords
		// PassKeywords: <None>
		// GraphKeywords: <None>

		// Defines
#define _SURFACE_TYPE_TRANSPARENT 1
#define ATTRIBUTES_NEED_NORMAL
#define ATTRIBUTES_NEED_TANGENT
#define VARYINGS_NEED_CULLFACE
#define FEATURES_GRAPH_VERTEX
		/* WARNING: $splice Could not find named fragment 'PassInstancing' */
#define SHADERPASS SHADERPASS_DEPTHONLY
		/* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

		// Includes
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

		// --------------------------------------------------
		// Structs and Packing

		struct Attributes
	{
		float3 positionOS : POSITION;
		float3 normalOS : NORMAL;
		float4 tangentOS : TANGENT;
#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : INSTANCEID_SEMANTIC;
#endif
	};
	struct Varyings
	{
		float4 positionCS : SV_POSITION;
#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : CUSTOM_INSTANCE_ID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
#endif
	};
	struct SurfaceDescriptionInputs
	{
		float FaceSign;
	};
	struct VertexDescriptionInputs
	{
		float3 ObjectSpaceNormal;
		float3 ObjectSpaceTangent;
		float3 ObjectSpacePosition;
	};
	struct PackedVaryings
	{
		float4 positionCS : SV_POSITION;
#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : CUSTOM_INSTANCE_ID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
#endif
	};

	PackedVaryings PackVaryings(Varyings input)
	{
		PackedVaryings output;
		output.positionCS = input.positionCS;
#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
#endif
		return output;
	}
	Varyings UnpackVaryings(PackedVaryings input)
	{
		Varyings output;
		output.positionCS = input.positionCS;
#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
#endif
		return output;
	}

	// --------------------------------------------------
	// Graph

	// Graph Properties
	CBUFFER_START(UnityPerMaterial)
		float _OutlineScale;
	float4 _OutlineColor;
	float _Opacity;
	CBUFFER_END

		// Object and Global properties

		// Graph Functions

		void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
	{
		Out = A * B;
	}

	void Unity_Branch_float(float Predicate, float True, float False, out float Out)
	{
		Out = Predicate ? True : False;
	}

	// Graph Vertex
	struct VertexDescription
	{
		float3 Position;
		float3 Normal;
		float3 Tangent;
	};

	VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
	{
		VertexDescription description = (VertexDescription)0;
		float _Property_4e77a4270a654dc1b68438fbdb35afd6_Out_0 = _OutlineScale;
		float3 _Multiply_4515fd1de82649029750c25bcea7cc00_Out_2;
		Unity_Multiply_float((_Property_4e77a4270a654dc1b68438fbdb35afd6_Out_0.xxx), IN.ObjectSpacePosition, _Multiply_4515fd1de82649029750c25bcea7cc00_Out_2);
		description.Position = _Multiply_4515fd1de82649029750c25bcea7cc00_Out_2;
		description.Normal = IN.ObjectSpaceNormal;
		description.Tangent = IN.ObjectSpaceTangent;
		return description;
	}

	// Graph Pixel
	struct SurfaceDescription
	{
		float Alpha;
	};

	SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
	{
		SurfaceDescription surface = (SurfaceDescription)0;
		float _IsFrontFace_2f6ebd2e8cf04c6094812bf17eff70df_Out_0 = max(0, IN.FaceSign);
		float _Property_dfcbbf7308a64128bcd7e22912fd1159_Out_0 = _Opacity;
		float _Branch_98aebd08dfef4c2baee90ebe9ffaa24a_Out_3;
		Unity_Branch_float(_IsFrontFace_2f6ebd2e8cf04c6094812bf17eff70df_Out_0, _Property_dfcbbf7308a64128bcd7e22912fd1159_Out_0, 0, _Branch_98aebd08dfef4c2baee90ebe9ffaa24a_Out_3);
		surface.Alpha = _Branch_98aebd08dfef4c2baee90ebe9ffaa24a_Out_3;
		return surface;
	}

	// --------------------------------------------------
	// Build Graph Inputs

	VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
	{
		VertexDescriptionInputs output;
		ZERO_INITIALIZE(VertexDescriptionInputs, output);

		output.ObjectSpaceNormal = input.normalOS;
		output.ObjectSpaceTangent = input.tangentOS.xyz;
		output.ObjectSpacePosition = input.positionOS;

		return output;
	}
	SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
	{
		SurfaceDescriptionInputs output;
		ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
		BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

			return output;
	}

	// --------------------------------------------------
	// Main

#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

	ENDHLSL
	}
	}
		SubShader
	{
		Tags
	{
		"RenderPipeline" = "UniversalPipeline"
		"RenderType" = "Transparent"
		"UniversalMaterialType" = "Unlit"
		"Queue" = "Transparent"
	}
		Pass
	{
		Name "Pass"
		Tags
	{
		// LightMode: <None>
	}

	// Render State
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
		ZTest LEqual
		ZWrite Off

		// Debug
		// <None>

		// --------------------------------------------------
		// Pass

		HLSLPROGRAM

		// Pragmas
#pragma target 2.0
#pragma only_renderers gles gles3 glcore d3d11
#pragma multi_compile_instancing
#pragma multi_compile_fog
#pragma vertex vert
#pragma fragment frag

		// DotsInstancingOptions: <None>
		// HybridV1InjectedBuiltinProperties: <None>

		// Keywords
#pragma multi_compile _ LIGHTMAP_ON
#pragma multi_compile _ DIRLIGHTMAP_COMBINED
#pragma shader_feature _ _SAMPLE_GI
		// GraphKeywords: <None>

		// Defines
#define _SURFACE_TYPE_TRANSPARENT 1
#define ATTRIBUTES_NEED_NORMAL
#define ATTRIBUTES_NEED_TANGENT
#define VARYINGS_NEED_CULLFACE
#define FEATURES_GRAPH_VERTEX
		/* WARNING: $splice Could not find named fragment 'PassInstancing' */
#define SHADERPASS SHADERPASS_UNLIT
		/* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

		// Includes
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

		// --------------------------------------------------
		// Structs and Packing

		struct Attributes
	{
		float3 positionOS : POSITION;
		float3 normalOS : NORMAL;
		float4 tangentOS : TANGENT;
#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : INSTANCEID_SEMANTIC;
#endif
	};
	struct Varyings
	{
		float4 positionCS : SV_POSITION;
#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : CUSTOM_INSTANCE_ID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
#endif
	};
	struct SurfaceDescriptionInputs
	{
		float FaceSign;
	};
	struct VertexDescriptionInputs
	{
		float3 ObjectSpaceNormal;
		float3 ObjectSpaceTangent;
		float3 ObjectSpacePosition;
	};
	struct PackedVaryings
	{
		float4 positionCS : SV_POSITION;
#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : CUSTOM_INSTANCE_ID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
#endif
	};

	PackedVaryings PackVaryings(Varyings input)
	{
		PackedVaryings output;
		output.positionCS = input.positionCS;
#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
#endif
		return output;
	}
	Varyings UnpackVaryings(PackedVaryings input)
	{
		Varyings output;
		output.positionCS = input.positionCS;
#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
#endif
		return output;
	}

	// --------------------------------------------------
	// Graph

	// Graph Properties
	CBUFFER_START(UnityPerMaterial)
		float _OutlineScale;
	float4 _OutlineColor;
	float _Opacity;
	CBUFFER_END

		// Object and Global properties

		// Graph Functions

		void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
	{
		Out = A * B;
	}

	void Unity_Branch_float(float Predicate, float True, float False, out float Out)
	{
		Out = Predicate ? True : False;
	}

	// Graph Vertex
	struct VertexDescription
	{
		float3 Position;
		float3 Normal;
		float3 Tangent;
	};

	VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
	{
		VertexDescription description = (VertexDescription)0;
		float _Property_4e77a4270a654dc1b68438fbdb35afd6_Out_0 = _OutlineScale;
		float3 _Multiply_4515fd1de82649029750c25bcea7cc00_Out_2;
		Unity_Multiply_float((_Property_4e77a4270a654dc1b68438fbdb35afd6_Out_0.xxx), IN.ObjectSpacePosition, _Multiply_4515fd1de82649029750c25bcea7cc00_Out_2);
		description.Position = _Multiply_4515fd1de82649029750c25bcea7cc00_Out_2;
		description.Normal = IN.ObjectSpaceNormal;
		description.Tangent = IN.ObjectSpaceTangent;
		return description;
	}

	// Graph Pixel
	struct SurfaceDescription
	{
		float3 BaseColor;
		float Alpha;
	};

	SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
	{
		SurfaceDescription surface = (SurfaceDescription)0;
		float4 _Property_9647e2796a1140c99a5ca7e26f838d82_Out_0 = IsGammaSpace() ? LinearToSRGB(_OutlineColor) : _OutlineColor;
		float _IsFrontFace_2f6ebd2e8cf04c6094812bf17eff70df_Out_0 = max(0, IN.FaceSign);
		float _Property_dfcbbf7308a64128bcd7e22912fd1159_Out_0 = _Opacity;
		float _Branch_98aebd08dfef4c2baee90ebe9ffaa24a_Out_3;
		Unity_Branch_float(_IsFrontFace_2f6ebd2e8cf04c6094812bf17eff70df_Out_0, _Property_dfcbbf7308a64128bcd7e22912fd1159_Out_0, 0, _Branch_98aebd08dfef4c2baee90ebe9ffaa24a_Out_3);
		surface.BaseColor = (_Property_9647e2796a1140c99a5ca7e26f838d82_Out_0.xyz);
		surface.Alpha = _Branch_98aebd08dfef4c2baee90ebe9ffaa24a_Out_3;
		return surface;
	}

	// --------------------------------------------------
	// Build Graph Inputs

	VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
	{
		VertexDescriptionInputs output;
		ZERO_INITIALIZE(VertexDescriptionInputs, output);

		output.ObjectSpaceNormal = input.normalOS;
		output.ObjectSpaceTangent = input.tangentOS.xyz;
		output.ObjectSpacePosition = input.positionOS;

		return output;
	}
	SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
	{
		SurfaceDescriptionInputs output;
		ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
		BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

			return output;
	}

	// --------------------------------------------------
	// Main

#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"

	ENDHLSL
	}
		Pass
	{
		Name "ShadowCaster"
		Tags
	{
		"LightMode" = "ShadowCaster"
	}

		// Render State
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
		ZTest LEqual
		ZWrite On
		ColorMask 0

		// Debug
		// <None>

		// --------------------------------------------------
		// Pass

		HLSLPROGRAM

		// Pragmas
#pragma target 2.0
#pragma only_renderers gles gles3 glcore d3d11
#pragma multi_compile_instancing
#pragma vertex vert
#pragma fragment frag

		// DotsInstancingOptions: <None>
		// HybridV1InjectedBuiltinProperties: <None>

		// Keywords
		// PassKeywords: <None>
		// GraphKeywords: <None>

		// Defines
#define _SURFACE_TYPE_TRANSPARENT 1
#define ATTRIBUTES_NEED_NORMAL
#define ATTRIBUTES_NEED_TANGENT
#define VARYINGS_NEED_CULLFACE
#define FEATURES_GRAPH_VERTEX
		/* WARNING: $splice Could not find named fragment 'PassInstancing' */
#define SHADERPASS SHADERPASS_SHADOWCASTER
		/* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

		// Includes
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

		// --------------------------------------------------
		// Structs and Packing

		struct Attributes
	{
		float3 positionOS : POSITION;
		float3 normalOS : NORMAL;
		float4 tangentOS : TANGENT;
#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : INSTANCEID_SEMANTIC;
#endif
	};
	struct Varyings
	{
		float4 positionCS : SV_POSITION;
#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : CUSTOM_INSTANCE_ID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
#endif
	};
	struct SurfaceDescriptionInputs
	{
		float FaceSign;
	};
	struct VertexDescriptionInputs
	{
		float3 ObjectSpaceNormal;
		float3 ObjectSpaceTangent;
		float3 ObjectSpacePosition;
	};
	struct PackedVaryings
	{
		float4 positionCS : SV_POSITION;
#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : CUSTOM_INSTANCE_ID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
#endif
	};

	PackedVaryings PackVaryings(Varyings input)
	{
		PackedVaryings output;
		output.positionCS = input.positionCS;
#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
#endif
		return output;
	}
	Varyings UnpackVaryings(PackedVaryings input)
	{
		Varyings output;
		output.positionCS = input.positionCS;
#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
#endif
		return output;
	}

	// --------------------------------------------------
	// Graph

	// Graph Properties
	CBUFFER_START(UnityPerMaterial)
		float _OutlineScale;
	float4 _OutlineColor;
	float _Opacity;
	CBUFFER_END

		// Object and Global properties

		// Graph Functions

		void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
	{
		Out = A * B;
	}

	void Unity_Branch_float(float Predicate, float True, float False, out float Out)
	{
		Out = Predicate ? True : False;
	}

	// Graph Vertex
	struct VertexDescription
	{
		float3 Position;
		float3 Normal;
		float3 Tangent;
	};

	VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
	{
		VertexDescription description = (VertexDescription)0;
		float _Property_4e77a4270a654dc1b68438fbdb35afd6_Out_0 = _OutlineScale;
		float3 _Multiply_4515fd1de82649029750c25bcea7cc00_Out_2;
		Unity_Multiply_float((_Property_4e77a4270a654dc1b68438fbdb35afd6_Out_0.xxx), IN.ObjectSpacePosition, _Multiply_4515fd1de82649029750c25bcea7cc00_Out_2);
		description.Position = _Multiply_4515fd1de82649029750c25bcea7cc00_Out_2;
		description.Normal = IN.ObjectSpaceNormal;
		description.Tangent = IN.ObjectSpaceTangent;
		return description;
	}

	// Graph Pixel
	struct SurfaceDescription
	{
		float Alpha;
	};

	SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
	{
		SurfaceDescription surface = (SurfaceDescription)0;
		float _IsFrontFace_2f6ebd2e8cf04c6094812bf17eff70df_Out_0 = max(0, IN.FaceSign);
		float _Property_dfcbbf7308a64128bcd7e22912fd1159_Out_0 = _Opacity;
		float _Branch_98aebd08dfef4c2baee90ebe9ffaa24a_Out_3;
		Unity_Branch_float(_IsFrontFace_2f6ebd2e8cf04c6094812bf17eff70df_Out_0, _Property_dfcbbf7308a64128bcd7e22912fd1159_Out_0, 0, _Branch_98aebd08dfef4c2baee90ebe9ffaa24a_Out_3);
		surface.Alpha = _Branch_98aebd08dfef4c2baee90ebe9ffaa24a_Out_3;
		return surface;
	}

	// --------------------------------------------------
	// Build Graph Inputs

	VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
	{
		VertexDescriptionInputs output;
		ZERO_INITIALIZE(VertexDescriptionInputs, output);

		output.ObjectSpaceNormal = input.normalOS;
		output.ObjectSpaceTangent = input.tangentOS.xyz;
		output.ObjectSpacePosition = input.positionOS;

		return output;
	}
	SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
	{
		SurfaceDescriptionInputs output;
		ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
		BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

			return output;
	}

	// --------------------------------------------------
	// Main

#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"

	ENDHLSL
	}
		Pass
	{
		Name "DepthOnly"
		Tags
	{
		"LightMode" = "DepthOnly"
	}

		// Render State
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
		ZTest LEqual
		ZWrite On
		ColorMask 0

		// Debug
		// <None>

		// --------------------------------------------------
		// Pass

		HLSLPROGRAM

		// Pragmas
#pragma target 2.0
#pragma only_renderers gles gles3 glcore d3d11
#pragma multi_compile_instancing
#pragma vertex vert
#pragma fragment frag

		// DotsInstancingOptions: <None>
		// HybridV1InjectedBuiltinProperties: <None>

		// Keywords
		// PassKeywords: <None>
		// GraphKeywords: <None>

		// Defines
#define _SURFACE_TYPE_TRANSPARENT 1
#define ATTRIBUTES_NEED_NORMAL
#define ATTRIBUTES_NEED_TANGENT
#define VARYINGS_NEED_CULLFACE
#define FEATURES_GRAPH_VERTEX
		/* WARNING: $splice Could not find named fragment 'PassInstancing' */
#define SHADERPASS SHADERPASS_DEPTHONLY
		/* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

		// Includes
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

		// --------------------------------------------------
		// Structs and Packing

		struct Attributes
	{
		float3 positionOS : POSITION;
		float3 normalOS : NORMAL;
		float4 tangentOS : TANGENT;
#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : INSTANCEID_SEMANTIC;
#endif
	};
	struct Varyings
	{
		float4 positionCS : SV_POSITION;
#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : CUSTOM_INSTANCE_ID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
#endif
	};
	struct SurfaceDescriptionInputs
	{
		float FaceSign;
	};
	struct VertexDescriptionInputs
	{
		float3 ObjectSpaceNormal;
		float3 ObjectSpaceTangent;
		float3 ObjectSpacePosition;
	};
	struct PackedVaryings
	{
		float4 positionCS : SV_POSITION;
#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : CUSTOM_INSTANCE_ID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
#endif
	};

	PackedVaryings PackVaryings(Varyings input)
	{
		PackedVaryings output;
		output.positionCS = input.positionCS;
#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
#endif
		return output;
	}
	Varyings UnpackVaryings(PackedVaryings input)
	{
		Varyings output;
		output.positionCS = input.positionCS;
#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
#endif
		return output;
	}

	// --------------------------------------------------
	// Graph

	// Graph Properties
	CBUFFER_START(UnityPerMaterial)
		float _OutlineScale;
	float4 _OutlineColor;
	float _Opacity;
	CBUFFER_END

		// Object and Global properties

		// Graph Functions

		void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
	{
		Out = A * B;
	}

	void Unity_Branch_float(float Predicate, float True, float False, out float Out)
	{
		Out = Predicate ? True : False;
	}

	// Graph Vertex
	struct VertexDescription
	{
		float3 Position;
		float3 Normal;
		float3 Tangent;
	};

	VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
	{
		VertexDescription description = (VertexDescription)0;
		float _Property_4e77a4270a654dc1b68438fbdb35afd6_Out_0 = _OutlineScale;
		float3 _Multiply_4515fd1de82649029750c25bcea7cc00_Out_2;
		Unity_Multiply_float((_Property_4e77a4270a654dc1b68438fbdb35afd6_Out_0.xxx), IN.ObjectSpacePosition, _Multiply_4515fd1de82649029750c25bcea7cc00_Out_2);
		description.Position = _Multiply_4515fd1de82649029750c25bcea7cc00_Out_2;
		description.Normal = IN.ObjectSpaceNormal;
		description.Tangent = IN.ObjectSpaceTangent;
		return description;
	}

	// Graph Pixel
	struct SurfaceDescription
	{
		float Alpha;
	};

	SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
	{
		SurfaceDescription surface = (SurfaceDescription)0;
		float _IsFrontFace_2f6ebd2e8cf04c6094812bf17eff70df_Out_0 = max(0, IN.FaceSign);
		float _Property_dfcbbf7308a64128bcd7e22912fd1159_Out_0 = _Opacity;
		float _Branch_98aebd08dfef4c2baee90ebe9ffaa24a_Out_3;
		Unity_Branch_float(_IsFrontFace_2f6ebd2e8cf04c6094812bf17eff70df_Out_0, _Property_dfcbbf7308a64128bcd7e22912fd1159_Out_0, 0, _Branch_98aebd08dfef4c2baee90ebe9ffaa24a_Out_3);
		surface.Alpha = _Branch_98aebd08dfef4c2baee90ebe9ffaa24a_Out_3;
		return surface;
	}

	// --------------------------------------------------
	// Build Graph Inputs

	VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
	{
		VertexDescriptionInputs output;
		ZERO_INITIALIZE(VertexDescriptionInputs, output);

		output.ObjectSpaceNormal = input.normalOS;
		output.ObjectSpaceTangent = input.tangentOS.xyz;
		output.ObjectSpacePosition = input.positionOS;

		return output;
	}
	SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
	{
		SurfaceDescriptionInputs output;
		ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
		BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

			return output;
	}

	// --------------------------------------------------
	// Main

#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"

	ENDHLSL
	}
	}
		FallBack "Hidden/Shader Graph/FallbackError"
}