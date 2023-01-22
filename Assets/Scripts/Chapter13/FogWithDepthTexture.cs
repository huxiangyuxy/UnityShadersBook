﻿using UnityEngine;
using System.Collections;

public class FogWithDepthTexture : PostEffectsBase {
  public Shader fogShader;
  private Material fogMaterial = null;

  public Material material {
    get {
      fogMaterial = CheckShaderAndCreateMaterial(fogShader, fogMaterial);
      return fogMaterial;
    }
  }

  private Camera myCamera;
  public Camera camera {
    get {
      if (myCamera == null) {
        myCamera = GetComponent<Camera>();
      }
      return myCamera;
    }
  }

  private Transform myCameraTransform;
  public Transform cameraTransform {
    get {
      if (myCameraTransform == null) {
        myCameraTransform = camera.transform;
      }

      return myCameraTransform;
    }
  }

  // 雾的浓度
  [Range(0.0f, 3.0f)]
  public float fogDensity = 1.0f;

  // 雾的颜色
  public Color fogColor = Color.white;

  // 雾的起始高度和终止高度
  public float fogStart = 0.0f;
  public float fogEnd = 2.0f;

  void OnEnable() {
    // 获取摄像机的深度纹理
    camera.depthTextureMode |= DepthTextureMode.Depth;
  }

  void OnRenderImage(RenderTexture src, RenderTexture dest) {
    if (material != null) {
      Matrix4x4 frustumCorners = Matrix4x4.identity;

      float fov = camera.fieldOfView;
      float near = camera.nearClipPlane;
      float aspect = camera.aspect;

      // 计算 toRight toTop
      float halfHeight = near * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
      Vector3 toRight = cameraTransform.right * halfHeight * aspect;
      Vector3 toTop = cameraTransform.up * halfHeight;

      // 计算 topLeft topRight bottomLeft bottomRight
      Vector3 topLeft = cameraTransform.forward * near + toTop - toRight;
      // 计算 scale
      float scale = topLeft.magnitude / near;

      topLeft.Normalize();
      topLeft *= scale;

      Vector3 topRight = cameraTransform.forward * near + toRight + toTop;
      topRight.Normalize();
      topRight *= scale;

      Vector3 bottomLeft = cameraTransform.forward * near - toTop - toRight;
      bottomLeft.Normalize();
      bottomLeft *= scale;

      Vector3 bottomRight = cameraTransform.forward * near + toRight - toTop;
      bottomRight.Normalize();
      bottomRight *= scale;

      frustumCorners.SetRow(0, bottomLeft);
      frustumCorners.SetRow(1, bottomRight);
      frustumCorners.SetRow(2, topRight);
      frustumCorners.SetRow(3, topLeft);

      material.SetMatrix("_FrustumCornersRay", frustumCorners);

      material.SetFloat("_FogDensity", fogDensity);
      material.SetColor("_FogColor", fogColor);
      material.SetFloat("_FogStart", fogStart);
      material.SetFloat("_FogEnd", fogEnd);

      Graphics.Blit(src, dest, material);
    } else {
      Graphics.Blit(src, dest);
    }
  }
}
