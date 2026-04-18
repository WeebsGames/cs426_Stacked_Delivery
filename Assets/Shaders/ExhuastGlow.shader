// Very good reference of shader basics
// Link: https://youtu.be/kfM-yu0iQBk?si=HzDNSNUV2FBmecv4
Shader "ExhaustGlow"
{
    // Set of input data
    // Colors for the exhaust, and other stuff to tweak like
    // how intense the glow is
    Properties
    {
        _ColdColor ("Cold Color", Color) = (0.1, 0.1, 0.1, 1)
        _HotColor ("Hot Glow Color", Color) = (1, 0.3, 0.05, 1)
        _HeatIntensity ("Heat Intensity", Range(0, 1)) = 0
        _EmissionStrength ("Emission Strength", Float) = 4
    }

    // The exhaust shader, only one subshader for now
    SubShader
    {
        // How should this shader render? Specify under tags
        Tags
        {
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry"
        }
        
        // Render pass, just one
        Pass
        {
            // HLSLPROGRAM block is where we write the actual shader 
            // code in HLSL.
            HLSLPROGRAM

            // Tell Unity which functions are the vertex and fragment shaders.
            // vertex shader - Take all the vertices of the mesh, 
            //  to like move grass from side to side (for each vertex)
            //
            // fragment shader - (for each on every "pixel", but 
            // fragments) and set the color of each fragment
            #pragma vertex vert
            #pragma fragment frag

            // Include URP's core shader library so we have access 
            // to URP-specific helpers.
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Material properties
            float4 _ColdColor;
            float4 _HotColor;
            float _HeatIntensity;
            float _EmissionStrength;

            // MeshData: data coming from the mesh 
            // (input to vertex shader).
            struct MeshData
            {
                float4 positionOS : POSITION; // OS = object space
            };

            // Interpolators: data passed from vertex to fragment 
            // shader. Any data passed from the vertex shader to
            // the fragment shader will be interpolated data
            struct Interpolators
            {
                float4 positionCS : SV_POSITION; // CS = clip space 
            };
            
            // The function for the vertex shader
            // Vertex shader: runs once per vertex.
            // Transforms the vertex position from object
            // space to clip space.
            Interpolators vert(MeshData IN)
            {
                Interpolators OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }
            
            // The function for the fragment shader
            // Fragment shader: runs once per pixel being drawn.
            // This is where the "hot or cold" color logic happens.
            // SV_Target: This fragment should output to the frame 
            // buffer
            float4 frag(Interpolators IN) : SV_Target
            {
                float3 baseColor = _ColdColor.rgb;

                // Glow color scaled by heat intensity and emission strength.
                // At heat 0 = black (no glow), at heat 1 = full hot color brightness.
                float3 emission = _HotColor.rgb * _HeatIntensity * _EmissionStrength;

                // Cold base plus hot emission on top.
                float3 finalColor = baseColor + emission;

                return float4(finalColor, 1);
            }

            ENDHLSL
        }
    }
}

