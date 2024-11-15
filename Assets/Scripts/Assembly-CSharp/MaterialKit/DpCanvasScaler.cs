﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MaterialKit
{
	 
	[RequireComponent(typeof(Canvas))]
	[ExecuteInEditMode]
	[AddComponentMenu("Layout/DP Canvas Scaler")]
	public class DpCanvasScaler : UIBehaviour
	{
		 
		protected DpCanvasScaler()
		{
		}

		 
		// (get) Token: 0x0600094C RID: 2380 RVA: 0x0002176D File Offset: 0x0001FB6D
		// (set) Token: 0x0600094D RID: 2381 RVA: 0x00021775 File Offset: 0x0001FB75
		public float referencePixelsPerUnit
		{
			get
			{
				return this.m_ReferencePixelsPerUnit;
			}
			set
			{
				this.m_ReferencePixelsPerUnit = value;
			}
		}

		 
		// (get) Token: 0x0600094E RID: 2382 RVA: 0x0002177E File Offset: 0x0001FB7E
		// (set) Token: 0x0600094F RID: 2383 RVA: 0x00021786 File Offset: 0x0001FB86
		public float fallbackScreenDPI
		{
			get
			{
				return this.m_FallbackScreenDPI;
			}
			set
			{
				this.m_FallbackScreenDPI = value;
			}
		}

		 
		// (get) Token: 0x06000950 RID: 2384 RVA: 0x0002178F File Offset: 0x0001FB8F
		// (set) Token: 0x06000951 RID: 2385 RVA: 0x00021797 File Offset: 0x0001FB97
		public float defaultSpriteDPI
		{
			get
			{
				return this.m_DefaultSpriteDPI;
			}
			set
			{
				this.m_DefaultSpriteDPI = value;
			}
		}

		 
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x000217A0 File Offset: 0x0001FBA0
		// (set) Token: 0x06000953 RID: 2387 RVA: 0x000217A8 File Offset: 0x0001FBA8
		public float dynamicPixelsPerUnit
		{
			get
			{
				return this.m_DynamicPixelsPerUnit;
			}
			set
			{
				this.m_DynamicPixelsPerUnit = value;
			}
		}

		 
		protected override void OnEnable()
		{
			base.OnEnable();
			this.m_Canvas = base.GetComponent<Canvas>();
			this.Handle();
		}

		 
		protected override void OnDisable()
		{
			this.SetScaleFactor(1f);
			this.SetReferencePixelsPerUnit(100f);
			base.OnDisable();
		}

		 
		protected virtual void Update()
		{
			this.Handle();
		}

		 
		protected virtual void Handle()
		{
			if (this.m_Canvas == null || !this.m_Canvas.isRootCanvas)
			{
				return;
			}
			if (this.m_Canvas.renderMode == RenderMode.WorldSpace)
			{
				this.HandleWorldCanvas();
				return;
			}
			this.HandleConstantPhysicalSize();
		}

		 
		protected virtual void HandleWorldCanvas()
		{
			this.SetScaleFactor(this.m_DynamicPixelsPerUnit);
			this.SetReferencePixelsPerUnit(this.m_ReferencePixelsPerUnit);
		}

		 
		protected virtual void HandleConstantPhysicalSize()
		{
			float dpi = Screen.dpi;
			float num = (dpi != 0f) ? dpi : this.m_FallbackScreenDPI;
			float num2 = 160f;
			this.SetScaleFactor(num / num2);
			this.SetReferencePixelsPerUnit(this.m_ReferencePixelsPerUnit * num2 / this.m_DefaultSpriteDPI);
		}

		 
		protected void SetScaleFactor(float scaleFactor)
		{
			if (scaleFactor == this.m_PrevScaleFactor)
			{
				return;
			}
			this.m_Canvas.scaleFactor = scaleFactor;
			this.m_PrevScaleFactor = scaleFactor;
		}

		 
		protected void SetReferencePixelsPerUnit(float referencePixelsPerUnit)
		{
			if (referencePixelsPerUnit == this.m_PrevReferencePixelsPerUnit)
			{
				return;
			}
			this.m_Canvas.referencePixelsPerUnit = referencePixelsPerUnit;
			this.m_PrevReferencePixelsPerUnit = referencePixelsPerUnit;
		}

		 
		[Tooltip("If a sprite has this 'Pixels Per Unit' setting, then one pixel in the sprite will cover one unit in the UI.")]
		[SerializeField]
		protected float m_ReferencePixelsPerUnit = 100f;

		 
		private const float kLogBase = 2f;

		 
		[Tooltip("The DPI to assume if the screen DPI is not known.")]
		[SerializeField]
		protected float m_FallbackScreenDPI = 96f;

		 
		[Tooltip("The pixels per inch to use for sprites that have a 'Pixels Per Unit' setting that matches the 'Reference Pixels Per Unit' setting.")]
		[SerializeField]
		protected float m_DefaultSpriteDPI = 96f;

		 
		[Tooltip("The amount of pixels per unit to use for dynamically created bitmaps in the UI, such as Text.")]
		[SerializeField]
		protected float m_DynamicPixelsPerUnit = 1f;

		 
		private Canvas m_Canvas;

		 
		[NonSerialized]
		private float m_PrevScaleFactor = 1f;

		 
		[NonSerialized]
		private float m_PrevReferencePixelsPerUnit = 100f;
	}
}
