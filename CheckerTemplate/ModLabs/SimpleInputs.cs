using System;

// Token: 0x02000007 RID: 7
internal class SimpleInputs
{
	// Token: 0x17000002 RID: 2
	// (get) Token: 0x06000014 RID: 20 RVA: 0x00002154 File Offset: 0x00000354
	public static bool RightTrigger
	{
		get
		{
			return ControllerInputPoller.instance.rightControllerIndexFloat > 0.5f;
		}
	}

	// Token: 0x17000003 RID: 3
	// (get) Token: 0x06000015 RID: 21 RVA: 0x00002169 File Offset: 0x00000369
	public static bool RightGrab
	{
		get
		{
			return ControllerInputPoller.instance.rightGrab;
		}
	}

	// Token: 0x17000004 RID: 4
	// (get) Token: 0x06000016 RID: 22 RVA: 0x00002177 File Offset: 0x00000377
	public static bool RightA
	{
		get
		{
			return ControllerInputPoller.instance.rightControllerSecondaryButton;
		}
	}

	// Token: 0x17000005 RID: 5
	// (get) Token: 0x06000017 RID: 23 RVA: 0x00002177 File Offset: 0x00000377
	public static bool RightB
	{
		get
		{
			return ControllerInputPoller.instance.rightControllerSecondaryButton;
		}
	}

	// Token: 0x17000006 RID: 6
	// (get) Token: 0x06000018 RID: 24 RVA: 0x00002185 File Offset: 0x00000385
	public static bool LeftTrigger
	{
		get
		{
			return ControllerInputPoller.instance.leftControllerIndexFloat > 0.5f;
		}
	}

	// Token: 0x17000007 RID: 7
	// (get) Token: 0x06000019 RID: 25 RVA: 0x0000219A File Offset: 0x0000039A
	public static bool LeftGrab
	{
		get
		{
			return ControllerInputPoller.instance.leftGrab;
		}
	}

	// Token: 0x17000008 RID: 8
	// (get) Token: 0x0600001A RID: 26 RVA: 0x000021A8 File Offset: 0x000003A8
	public static bool LeftX
	{
		get
		{
			return ControllerInputPoller.instance.leftControllerPrimaryButton;
		}
	}

	// Token: 0x17000009 RID: 9
	// (get) Token: 0x0600001B RID: 27 RVA: 0x000021B6 File Offset: 0x000003B6
	public static bool LeftY
	{
		get
		{
			return ControllerInputPoller.instance.leftControllerSecondaryButton;
		}
	}
}
