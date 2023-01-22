using UnityEngine;
using System.Collections;

public class EdgeDetectNormalsAndDepth : PostEffectsBase {
  public Shader edgeDetectShader;
  private Material edgeDetectMaterial = null;
  public Material material {
    get {
      edgeDetectMaterial = CheckShaderAndCreateMaterial(edgeDetectShader, edgeDetectMaterial);
      return edgeDetectMaterial;
    }
  }

  [Range(0.0f, 1.0f)]
  public float edgesOnly = 0.0f;

  public Color edgeColor = Color.black;

  public Color backgroundColor = Color.white;

  // 对深度纹理和法线纹理使用的采样距离
  public float sampleDistance = 1.0f;

  public float sensitivityDepth = 1.0f;

  public float sensitivityNormals = 1.0f;

  void OnEnable() {
    // 获取摄像机的深度纹理和法线纹理
    GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
  }

  [ImageEffectOpaque]
  void OnRenderImage(RenderTexture src, RenderTexture dest) {
    if (material != null) {
      material.SetFloat("_EdgeOnly", edgesOnly);
      material.SetColor("_EdgeColor", edgeColor);
      material.SetColor("_BackgroundColor", backgroundColor);
      material.SetFloat("_SampleDistance", sampleDistance);
      material.SetVector("_Sensitivity", new Vector4(sensitivityNormals, sensitivityDepth, 0.0f, 0.0f));

      Graphics.Blit(src, dest, material);
    } else {
      Graphics.Blit(src, dest);
    }
  }
}
