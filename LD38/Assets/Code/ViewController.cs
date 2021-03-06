﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ViewController : MonoBehaviour
{
  public Shader standard;
  public Shader diffuse;

  GameObject CameraObj;

  [SerializeField]
  LayerMask layersToHide;
  [Range(0, 1)]
  float transparecyRange = 0.3f;

  [SerializeField]
  float areaEffectedRadius = 1f;
  [SerializeField]
  public float checkForTargetsCooldown = 0.5f;

  RaycastHit[] hits;

  void Start()
  {
    CameraObj = Camera.main.gameObject;
    StartCoroutine(CheckInTheWay());
  }

  IEnumerator CheckInTheWay()
  {
    while(true)
    {
      if(TurnController.HasPlayer)
      {
        if(hits != null)
        {
          for(int i = 0; i < hits.Length; i++)
          {
            RaycastHit hit = hits[i];
            if(hit.transform == null)
            {
              continue;
            }

            Renderer rend = hit.transform.GetComponentInChildren<Renderer>();

            if(rend)
            {
              // Change the material of all hit colliders
              // to use a transparent shader.
              rend.material.shader = standard;//Shader.Find("Standard");
              Color tempColor = rend.material.color;
              tempColor.a = 1F;
              rend.material.color = tempColor;
            }
          }
        }

        hits = Physics.SphereCastAll(CameraObj.transform.position, areaEffectedRadius, CameraObj.transform.forward,
          Vector3.Distance(TurnController.CurrentPlayer.transform.position, CameraObj.transform.position), layersToHide);

        for(int i = 0; i < hits.Length; i++)
        {
          RaycastHit hit = hits[i];
          Renderer rend = hit.transform.GetComponentInChildren<Renderer>();

          if(rend)
          {
            // Change the material of all hit colliders
            // to use a transparent shader.
            rend.material.shader = diffuse;//Shader.Find("Transparent/Diffuse");
            Color tempColor = rend.material.color;
            tempColor.a = transparecyRange;
            rend.material.color = tempColor;
          }
        }
      }

      yield return new WaitForSeconds(checkForTargetsCooldown);
    }
  }
}