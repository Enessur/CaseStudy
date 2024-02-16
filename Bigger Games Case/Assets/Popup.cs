using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Popup : MonoBehaviour
{
   [SerializeField] private GameObject content;
   [SerializeField] private float duration = 0.5f;
   [SerializeField] private Ease ease;

   public bool _isOpen;
   private Vector3 _startScale;

   private void OnEnable()
   {
      GridCanvasController.CloseMenuOnSelection += Close;
   }

   private void OnDisable()
   {
      GridCanvasController.CloseMenuOnSelection -= Close;
   }

   private void Awake()
   {
      _isOpen = true;
      Close();
   }
   
   protected virtual void Start()
   {
      Init();
   }
   
   protected virtual void Init()
   {
      _startScale = content.transform.lossyScale;
   }

   public void Open()
   {
      if (_isOpen)
      {
         return;
      }

      content.transform.DOScale(_startScale, duration).SetEase(ease);
      _isOpen = true;
   }
   public void Close()
   {
      if (!_isOpen)
      {
         return;
      }

      content.transform.DOScale(0, duration).SetEase(ease);
      _isOpen = false;
   }
}
