using System;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class PlayerFPS : MonoBehaviour
{
	// Token: 0x17000001 RID: 1
	// (get) Token: 0x06000010 RID: 16 RVA: 0x000020F5 File Offset: 0x000002F5
	// (set) Token: 0x06000011 RID: 17 RVA: 0x000020FD File Offset: 0x000002FD
	public int CurrentFPS { get; private set; }

	// Token: 0x06000012 RID: 18 RVA: 0x00002106 File Offset: 0x00000306
	private void Update()
	{
		this.deltaTime += (Time.unscaledDeltaTime - this.deltaTime) * 0.1f;
		this.CurrentFPS = Mathf.RoundToInt(1f / this.deltaTime);
	}

	// Token: 0x04000008 RID: 8
	private float deltaTime = 0f;
}
