using BepInEx;
using GorillaLocomotion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ModLabsTemp
{
    // Token: 0x02000008 RID: 8
    [BepInPlugin("Creator.duv.ui4", "ui4", "1.7.0")]
    public class ModLabsTemp : BaseUnityPlugin
    {
        // Token: 0x0600001D RID: 29 RVA: 0x00002F58 File Offset: 0x00001158
        private void Update()
        {
            bool flag = ControllerInputPoller.instance == null;
            if (!flag)
            {
                bool leftX = SimpleInputs.LeftX;
                bool flag2 = leftX && !this.lastXButtonState;
                if (flag2)
                {
                    bool flag3 = this.uiPanel == null;
                    if (flag3)
                    {
                        this.SpawnUIPanel();
                    }
                    else
                    {
                        this.uiPanel.SetActive(true);
                    }
                    bool flag4 = this.fadeRoutine != null;
                    if (flag4)
                    {
                        base.StopCoroutine(this.fadeRoutine);
                    }
                    this.SetPanelAlpha(1f);
                    this.ShowPanel(true);
                }
                bool flag5 = !leftX && this.lastXButtonState;
                if (flag5)
                {
                    this.FadeOutPanel();
                }
                this.lastXButtonState = leftX;
                this.UpdateUIPanelPosition();
                this.UpdateGunRay();
                this.CheckButtonTouch();
                bool flag6 = this.selectedRig != null;
                if (flag6)
                {
                    this.playerFPSText.text = "<b>FPS:</b> " + GTUtility.GetFPS(this.selectedRig);
                    this.playerCosmeticsText.text = "<b>Cosmetics:</b> " + GTUtility.GetCosmetics(this.selectedRig);
                }
            }
        }

        // Token: 0x0600001E RID: 30 RVA: 0x0000307C File Offset: 0x0000127C
        private void ShowPanel(bool show)
        {
            bool flag = this.uiCanvasGroup == null;
            if (!flag)
            {
                this.uiCanvasGroup.interactable = show;
                this.uiCanvasGroup.blocksRaycasts = show;
            }
        }

        // Token: 0x0600001F RID: 31 RVA: 0x000030B8 File Offset: 0x000012B8
        private void SpawnUIPanel()
        {
            bool flag = this.uiPanel != null;
            if (flag)
            {
                this.ShowPanel(true);
            }
            else
            {
                this.currentPage = 1;
                this.uiPanel = new GameObject("ModLabsTemp");
                Canvas canvas = this.uiPanel.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.WorldSpace;
                canvas.worldCamera = Camera.main;
                this.uiCanvasGroup = this.uiPanel.AddComponent<CanvasGroup>();
                this.uiCanvasGroup.interactable = true;
                this.uiCanvasGroup.blocksRaycasts = true;
                CanvasScaler canvasScaler = this.uiPanel.AddComponent<CanvasScaler>();
                canvasScaler.dynamicPixelsPerUnit = 1000f;
                this.uiPanel.AddComponent<GraphicRaycaster>();
                RectTransform component = this.uiPanel.GetComponent<RectTransform>();
                component.sizeDelta = new Vector2(240f, 300f);
                this.uiPanel.transform.localScale = Vector3.one * 0.0015f;
                GameObject gameObject = new GameObject("Background");
                gameObject.transform.SetParent(this.uiPanel.transform, false);
                Image image = gameObject.AddComponent<Image>();
                image.color = new Color(this.panelColor.r, this.panelColor.g, this.panelColor.b, this.menuOpacity);
                image.sprite = this.CreateOptimizedRoundedSprite(512, 512, 40);
                image.type = Image.Type.Sliced;
                RectTransform component2 = gameObject.GetComponent<RectTransform>();
                component2.sizeDelta = component.sizeDelta;
                this.CreateInfoText(gameObject.transform, ref this.titleText, "ModLabsTemp", new Vector2(0f, 122f), 10f, FontStyles.Bold); // --Change "ModLabsTemp" to your checker name
                this.CreateInfoText(gameObject.transform, ref this.pageText, string.Format("Page {0}/{1}", this.currentPage, this.maxPage), new Vector2(0f, 100f), 7f, FontStyles.Bold);
                this.pageText.color = new Color(0.7f, 0.7f, 0.7f, 1f);
                this.CreateInfoText(gameObject.transform, ref this.playerNameText, "Name: Unknown", new Vector2(0f, 55f), 10f, FontStyles.Bold);
                this.CreateInfoText(gameObject.transform, ref this.playerPlatformText, "Platform: Unknown", new Vector2(0f, 28f), 8.5f, FontStyles.Bold);
                this.CreateInfoText(gameObject.transform, ref this.playerFPSText, "FPS: N/A", new Vector2(0f, 6f), 8.5f, FontStyles.Bold);
                this.CreateInfoText(gameObject.transform, ref this.playerCreationDateText, "Creation: Unknown", new Vector2(0f, -14f), 6f, FontStyles.Normal);
                this.CreateInfoText(gameObject.transform, ref this.playerModsText, "Mods/Cheats: None", new Vector2(0f, 10f), 9f, FontStyles.Bold);
                this.CreateInfoText(gameObject.transform, ref this.playerCosmeticsText, "cosmeticx mod: None", new Vector2(0f, -15f), 9f, FontStyles.Bold);
                this.gunRayToggle = this.CreateSettingToggle(gameObject.transform, "dont touch this", new Vector2(0f, 50f), new Action(this.ToggleGunRay));
                this.autoLockToggle = this.CreateSettingToggle(gameObject.transform, "dont touch", new Vector2(0f, 20f), new Action(this.ToggleAutoLock));
                this.opacityLowBtn = this.CreateSettingButton(gameObject.transform, "Low", new Vector2(-50f, -10f), delegate
                {
                    this.SetOpacity(0.7f);
                });
                this.opacityMedBtn = this.CreateSettingButton(gameObject.transform, "Med", new Vector2(0f, -10f), delegate
                {
                    this.SetOpacity(0.85f);
                });
                this.opacityHighBtn = this.CreateSettingButton(gameObject.transform, "High", new Vector2(50f, -10f), delegate
                {
                    this.SetOpacity(0.95f);
                });
                this.resetBtn = this.CreateSettingButton(gameObject.transform, "Reset", new Vector2(0f, -40f), new Action(this.ResetSettings));
                this.CreateInfoText(gameObject.transform, ref this.creditsText, "By TRTM & Meltixx", new Vector2(0f, -70f), 5.5f, FontStyles.Normal); // --Change "ModLabsTemp" to your checker name
                this.separatorLine = new GameObject("Separator");
                this.separatorLine.transform.SetParent(gameObject.transform, false);
                Image image2 = this.separatorLine.AddComponent<Image>();
                image2.color = this.separatorColor;
                RectTransform component3 = this.separatorLine.GetComponent<RectTransform>();
                component3.sizeDelta = new Vector2(240f, 1f);
                component3.anchoredPosition = new Vector2(0f, -90f);
                this.backButton = this.CreateButton(gameObject.transform, "Back", new Vector2(-64f, -118f), delegate
                {
                    this.ChangePage(-1);
                });
                this.homeButton = this.CreateButton(gameObject.transform, "Home", new Vector2(0f, -118f), delegate
                {
                    this.ChangePage(-this.currentPage + 1);
                });
                this.nextButton = this.CreateButton(gameObject.transform, "Next", new Vector2(64f, -118f), delegate
                {
                    this.ChangePage(1);
                });
                this.UpdateInfoVisibility();
                this.fadeRoutine = base.StartCoroutine(this.FadeAllGraphics(this.uiPanel, 0f, 1f, 0.12f, false));
            }
        }

        // Token: 0x06000020 RID: 32 RVA: 0x0000368C File Offset: 0x0000188C
        private void CreateInfoText(Transform parent, ref TextMeshProUGUI textRef, string content, Vector2 pos, float fontSize, FontStyles fontStyle)
        {
            GameObject gameObject = new GameObject("InfoText");
            gameObject.transform.SetParent(parent, false);
            textRef = gameObject.AddComponent<TextMeshProUGUI>();
            textRef.text = content;
            textRef.fontSize = fontSize;
            textRef.fontStyle = fontStyle;
            textRef.alignment = TextAlignmentOptions.Center;
            textRef.color = this.textLight;
            RectTransform component = gameObject.GetComponent<RectTransform>();
            component.sizeDelta = new Vector2(220f, 30f);
            component.anchoredPosition = pos;
        }

        // Token: 0x06000021 RID: 33 RVA: 0x0000371C File Offset: 0x0000191C
        private GameObject CreateSettingToggle(Transform parent, string label, Vector2 pos, Action onClick)
        {
            GameObject gameObject = new GameObject(label + "Toggle");
            gameObject.transform.SetParent(parent, false);
            Image image = gameObject.AddComponent<Image>();
            image.sprite = this.CreateOptimizedRoundedSprite(512, 512, 12);
            image.type = Image.Type.Sliced;
            image.color = this.elementDark;
            RectTransform component = gameObject.GetComponent<RectTransform>();
            component.sizeDelta = new Vector2(140f, 24f);
            component.anchoredPosition = pos;
            GameObject gameObject2 = new GameObject("Text");
            gameObject2.transform.SetParent(gameObject.transform, false);
            TextMeshProUGUI textMeshProUGUI = gameObject2.AddComponent<TextMeshProUGUI>();
            textMeshProUGUI.text = label;
            textMeshProUGUI.fontSize = 7f;
            textMeshProUGUI.alignment = TextAlignmentOptions.Center;
            textMeshProUGUI.color = this.textLight;
            RectTransform component2 = gameObject2.GetComponent<RectTransform>();
            component2.anchorMin = Vector2.zero;
            component2.anchorMax = Vector2.one;
            component2.offsetMin = Vector2.zero;
            component2.offsetMax = Vector2.zero;
            Button button = gameObject.AddComponent<Button>();
            button.onClick.AddListener(delegate ()
            {
                Action onClick2 = onClick;
                if (onClick2 != null)
                {
                    onClick2();
                }
            });
            return gameObject;
        }

        // Token: 0x06000022 RID: 34 RVA: 0x00003870 File Offset: 0x00001A70
        private GameObject CreateSettingButton(Transform parent, string label, Vector2 pos, Action onClick)
        {
            GameObject gameObject = new GameObject(label + "Btn");
            gameObject.transform.SetParent(parent, false);
            Image image = gameObject.AddComponent<Image>();
            image.sprite = this.CreateOptimizedRoundedSprite(512, 512, 10);
            image.type = Image.Type.Sliced;
            image.color = this.elementDark;
            RectTransform component = gameObject.GetComponent<RectTransform>();
            component.sizeDelta = new Vector2(42f, 20f);
            component.anchoredPosition = pos;
            GameObject gameObject2 = new GameObject("Text");
            gameObject2.transform.SetParent(gameObject.transform, false);
            TextMeshProUGUI textMeshProUGUI = gameObject2.AddComponent<TextMeshProUGUI>();
            textMeshProUGUI.text = label;
            textMeshProUGUI.fontSize = 6f;
            textMeshProUGUI.alignment = TextAlignmentOptions.Center;
            textMeshProUGUI.color = this.textLight;
            RectTransform component2 = gameObject2.GetComponent<RectTransform>();
            component2.anchorMin = Vector2.zero;
            component2.anchorMax = Vector2.one;
            component2.offsetMin = Vector2.zero;
            component2.offsetMax = Vector2.zero;
            Button button = gameObject.AddComponent<Button>();
            button.onClick.AddListener(delegate ()
            {
                Action onClick2 = onClick;
                if (onClick2 != null)
                {
                    onClick2();
                }
            });
            return gameObject;
        }

        // Token: 0x06000023 RID: 35 RVA: 0x000039C4 File Offset: 0x00001BC4
        private void ToggleGunRay()
        {
            this.gunRayEnabled = !this.gunRayEnabled;
            bool flag = this.gunRayToggle != null;
            if (flag)
            {
                this.gunRayToggle.GetComponent<Image>().color = (this.gunRayEnabled ? this.activeGreen : this.elementDark);
            }
            base.StartCoroutine(this.PlayRipple(this.gunRayToggle.GetComponent<Image>()));
        }

        // Token: 0x06000024 RID: 36 RVA: 0x00003A34 File Offset: 0x00001C34
        private void ToggleAutoLock()
        {
            this.autoLockEnabled = !this.autoLockEnabled;
            bool flag = this.autoLockToggle != null;
            if (flag)
            {
                this.autoLockToggle.GetComponent<Image>().color = (this.autoLockEnabled ? this.activeGreen : this.elementDark);
            }
            base.StartCoroutine(this.PlayRipple(this.autoLockToggle.GetComponent<Image>()));
        }

        // Token: 0x06000025 RID: 37 RVA: 0x00003AA4 File Offset: 0x00001CA4
        private void SetOpacity(float opacity)
        {
            this.menuOpacity = opacity;
            bool flag = this.uiPanel != null;
            if (flag)
            {
                Image componentInChildren = this.uiPanel.GetComponentInChildren<Image>();
                bool flag2 = componentInChildren != null;
                if (flag2)
                {
                    Color color = componentInChildren.color;
                    componentInChildren.color = new Color(color.r, color.g, color.b, opacity);
                }
            }
            this.opacityLowBtn.GetComponent<Image>().color = ((opacity == 0.7f) ? this.activeGreen : this.elementDark);
            this.opacityMedBtn.GetComponent<Image>().color = ((opacity == 0.85f) ? this.activeGreen : this.elementDark);
            this.opacityHighBtn.GetComponent<Image>().color = ((opacity == 0.95f) ? this.activeGreen : this.elementDark);
        }

        // Token: 0x06000026 RID: 38 RVA: 0x00003B80 File Offset: 0x00001D80
        private void ResetSettings()
        {
            this.gunRayEnabled = true;
            this.autoLockEnabled = false;
            this.SetOpacity(0.95f);
            bool flag = this.gunRayToggle != null;
            if (flag)
            {
                this.gunRayToggle.GetComponent<Image>().color = this.activeGreen;
            }
            bool flag2 = this.autoLockToggle != null;
            if (flag2)
            {
                this.autoLockToggle.GetComponent<Image>().color = this.elementDark;
            }
            base.StartCoroutine(this.PlayRipple(this.resetBtn.GetComponent<Image>()));
        }

        // Token: 0x06000027 RID: 39 RVA: 0x00003C10 File Offset: 0x00001E10
        private GameObject CreateButton(Transform parent, string label, Vector2 pos, Action onClick)
        {
            GameObject gameObject = new GameObject(label);
            gameObject.transform.SetParent(parent, false);
            Image image = gameObject.AddComponent<Image>();
            image.sprite = this.CreateOptimizedRoundedSprite(512, 512, 14);
            image.type = Image.Type.Sliced;
            image.color = this.elementDark;
            RectTransform component = gameObject.GetComponent<RectTransform>();
            component.sizeDelta = new Vector2(56f, 28f);
            component.anchoredPosition = pos;
            GameObject gameObject2 = new GameObject("Text");
            gameObject2.transform.SetParent(gameObject.transform, false);
            TextMeshProUGUI textMeshProUGUI = gameObject2.AddComponent<TextMeshProUGUI>();
            textMeshProUGUI.text = label;
            textMeshProUGUI.fontSize = 6f;
            textMeshProUGUI.alignment = TextAlignmentOptions.Center;
            textMeshProUGUI.color = this.textLight;
            RectTransform component2 = gameObject2.GetComponent<RectTransform>();
            component2.anchorMin = Vector2.zero;
            component2.anchorMax = Vector2.one;
            component2.offsetMin = Vector2.zero;
            component2.offsetMax = Vector2.zero;
            Button button = gameObject.AddComponent<Button>();
            button.onClick.AddListener(delegate ()
            {
                Action onClick2 = onClick;
                if (onClick2 != null)
                {
                    onClick2();
                }
            });
            return gameObject;
        }

        // Token: 0x06000028 RID: 40 RVA: 0x00003D5C File Offset: 0x00001F5C
        private void ChangePage(int dir)
        {
            this.currentPage = Mathf.Clamp(this.currentPage + dir, 1, this.maxPage);
            this.pageText.text = string.Format("Page {0}/{1}", this.currentPage, this.maxPage);
            this.UpdateInfoVisibility();
        }

        // Token: 0x06000029 RID: 41 RVA: 0x00003DB8 File Offset: 0x00001FB8
        private void UpdateInfoVisibility()
        {
            bool active = this.currentPage == 1;
            bool flag = this.playerNameText;
            if (flag)
            {
                this.playerNameText.gameObject.SetActive(active);
            }
            bool flag2 = this.playerPlatformText;
            if (flag2)
            {
                this.playerPlatformText.gameObject.SetActive(active);
            }
            bool flag3 = this.playerFPSText;
            if (flag3)
            {
                this.playerFPSText.gameObject.SetActive(active);
            }
            bool flag4 = this.playerCreationDateText;
            if (flag4)
            {
                this.playerCreationDateText.gameObject.SetActive(active);
            }
            bool active2 = this.currentPage == 2;
            bool flag5 = this.playerModsText;
            if (flag5)
            {
                this.playerModsText.gameObject.SetActive(active2);
            }
            bool flag6 = this.playerCosmeticsText;
            if (flag6)
            {
                this.playerCosmeticsText.gameObject.SetActive(active2);
            }
            bool active3 = this.currentPage == 3;
            bool flag7 = this.gunRayToggle;
            if (flag7)
            {
                this.gunRayToggle.SetActive(active3);
            }
            bool flag8 = this.autoLockToggle;
            if (flag8)
            {
                this.autoLockToggle.SetActive(active3);
            }
            bool flag9 = this.opacityLowBtn;
            if (flag9)
            {
                this.opacityLowBtn.SetActive(active3);
            }
            bool flag10 = this.opacityMedBtn;
            if (flag10)
            {
                this.opacityMedBtn.SetActive(active3);
            }
            bool flag11 = this.opacityHighBtn;
            if (flag11)
            {
                this.opacityHighBtn.SetActive(active3);
            }
            bool flag12 = this.resetBtn;
            if (flag12)
            {
                this.resetBtn.SetActive(active3);
            }
            bool flag13 = this.creditsText;
            if (flag13)
            {
                this.creditsText.gameObject.SetActive(active3);
            }
        }

        // Token: 0x0600002A RID: 42 RVA: 0x00003F8C File Offset: 0x0000218C
        private void UpdatePage()
        {
            if (this.selectedRig == null)
            {
                return;
            }

            TMP_Text nameText = this.playerNameText;
            NetPlayer owningNetPlayer = this.selectedRig.OwningNetPlayer;
            nameText.text = "<b>" + (((owningNetPlayer != null) ? owningNetPlayer.NickName : null) ?? "Unknown") + "</b>";

            this.playerFPSText.text = "<b>FPS:</b> " + GTUtility.GetFPS(this.selectedRig);

            string platform = GTUtility.GetPlatform(this.selectedRig);
            this.playerPlatformText.text = "<b>Platform:</b> " + platform;

            if (!string.IsNullOrEmpty(platform) && platform.ToLower().Contains("quest"))
            {
                this.playerPlatformText.color = Color.green;
            }
            else if (!string.IsNullOrEmpty(platform) && platform.ToLower().Contains("steam"))
            {
                this.playerPlatformText.color = Color.cyan;
            }
            else
            {
                this.playerPlatformText.color = this.textLight;
            }

            this.playerCreationDateText.text = "<b>Created:</b> " + GTUtility.GetAccountCreationDate(this.selectedRig);
            this.playerModsText.text = "<b>Mods/Cheats:</b> " + GTUtility.GetCheats(this.selectedRig);
            this.playerCosmeticsText.text = "<b>Cosmetics:</b> " + GTUtility.GetCosmetics(this.selectedRig);
        }

        // Token: 0x0600002B RID: 43 RVA: 0x000041E8 File Offset: 0x000023E8
        private void CheckButtonTouch()
        {
            bool flag = this.uiPanel == null || !this.canPressButtons || this.uiCanvasGroup == null || !this.uiCanvasGroup.interactable;
            if (!flag)
            {
                Transform rightControllerTransform = this.GetRightControllerTransform();
                bool flag2 = rightControllerTransform == null;
                if (!flag2)
                {
                    this.CheckTouch(rightControllerTransform, this.backButton, delegate
                    {
                        base.StartCoroutine(this.PlayRipple(this.backButton.GetComponent<Image>()));
                        this.ChangePage(-1);
                    });
                    this.CheckTouch(rightControllerTransform, this.homeButton, delegate
                    {
                        base.StartCoroutine(this.PlayRipple(this.homeButton.GetComponent<Image>()));
                        this.ChangePage(-this.currentPage + 1);
                    });
                    this.CheckTouch(rightControllerTransform, this.nextButton, delegate
                    {
                        base.StartCoroutine(this.PlayRipple(this.nextButton.GetComponent<Image>()));
                        this.ChangePage(1);
                    });
                    bool flag3 = this.currentPage == 3;
                    if (flag3)
                    {
                        this.CheckTouch(rightControllerTransform, this.gunRayToggle, delegate
                        {
                            this.ToggleGunRay();
                        });
                        this.CheckTouch(rightControllerTransform, this.autoLockToggle, delegate
                        {
                            this.ToggleAutoLock();
                        });
                        this.CheckTouch(rightControllerTransform, this.opacityLowBtn, delegate
                        {
                            base.StartCoroutine(this.PlayRipple(this.opacityLowBtn.GetComponent<Image>()));
                            this.SetOpacity(0.7f);
                        });
                        this.CheckTouch(rightControllerTransform, this.opacityMedBtn, delegate
                        {
                            base.StartCoroutine(this.PlayRipple(this.opacityMedBtn.GetComponent<Image>()));
                            this.SetOpacity(0.85f);
                        });
                        this.CheckTouch(rightControllerTransform, this.opacityHighBtn, delegate
                        {
                            base.StartCoroutine(this.PlayRipple(this.opacityHighBtn.GetComponent<Image>()));
                            this.SetOpacity(0.95f);
                        });
                        this.CheckTouch(rightControllerTransform, this.resetBtn, delegate
                        {
                            this.ResetSettings();
                        });
                    }
                }
            }
        }

        // Token: 0x0600002C RID: 44 RVA: 0x00004348 File Offset: 0x00002548
        private void CheckTouch(Transform hand, GameObject button, Action onTouch)
        {
            bool flag = button == null;
            if (!flag)
            {
                float num = Vector3.Distance(hand.position, button.transform.position);
                bool flag2 = num < 0.08f && this.canPressButtons;
                if (flag2)
                {
                    base.StartCoroutine(this.ButtonCooldown());
                    if (onTouch != null)
                    {
                        onTouch();
                    }
                }
            }
        }

        // Token: 0x0600002D RID: 45 RVA: 0x000021C4 File Offset: 0x000003C4
        private IEnumerator ButtonCooldown()
        {
            this.canPressButtons = false;
            yield return new WaitForSeconds(0.25f);
            this.canPressButtons = true;
            yield break;
        }

        // Token: 0x0600002E RID: 46 RVA: 0x000021D3 File Offset: 0x000003D3
        private IEnumerator PlayRipple(Image buttonImage)
        {
            bool flag = buttonImage == null;
            if (flag)
            {
                yield break;
            }
            GameObject ripple = new GameObject("Ripple");
            ripple.transform.SetParent(buttonImage.transform, false);
            Image rippleImg = ripple.AddComponent<Image>();
            rippleImg.color = new Color(1f, 1f, 1f, 0.12f);
            rippleImg.sprite = this.CreateOptimizedRoundedSprite(512, 512, 256);
            RectTransform rippleRect = ripple.GetComponent<RectTransform>();
            rippleRect.sizeDelta = Vector2.zero;
            float time = 0f;
            float duration = 0.38f;
            float maxSize = 96f;
            while (time < duration)
            {
                time += Time.deltaTime;
                float progress = time / duration;
                float size = Mathf.Lerp(0f, maxSize, progress);
                rippleRect.sizeDelta = new Vector2(size, size);
                rippleImg.color = new Color(1f, 1f, 1f, Mathf.Lerp(0.12f, 0f, progress));
                yield return null;
            }
            UnityEngine.Object.Destroy(ripple);
            yield break;
        }

        // Token: 0x0600002F RID: 47 RVA: 0x000043AC File Offset: 0x000025AC
        private void SetPanelAlpha(float alpha)
        {
            bool flag = this.uiPanel == null;
            if (!flag)
            {
                foreach (Graphic graphic in this.uiPanel.GetComponentsInChildren<Graphic>(true))
                {
                    Color color = graphic.color;
                    graphic.color = new Color(color.r, color.g, color.b, alpha);
                }
            }
        }

        // Token: 0x06000030 RID: 48 RVA: 0x00004418 File Offset: 0x00002618
        private void FadeOutPanel()
        {
            bool flag = this.uiPanel == null;
            if (!flag)
            {
                bool flag2 = this.fadeRoutine != null;
                if (flag2)
                {
                    base.StopCoroutine(this.fadeRoutine);
                }
                this.fadeRoutine = base.StartCoroutine(this.FadeOutAndDisable(this.uiPanel, 1f, 0f, 0.18f));
            }
        }

        // Token: 0x06000031 RID: 49 RVA: 0x000021E9 File Offset: 0x000003E9
        private IEnumerator FadeOutAndDisable(GameObject root, float from, float to, float duration)
        {
            List<Graphic> graphics = new List<Graphic>(root.GetComponentsInChildren<Graphic>(true));
            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(from, to, t / duration);
                foreach (Graphic g in graphics)
                {
                    bool flag = g == null;
                    if (!flag)
                    {
                        Color c = g.color;
                        g.color = new Color(c.r, c.g, c.b, alpha);
                        c = default(Color);

                    }
                }
                List<Graphic>.Enumerator enumerator = default(List<Graphic>.Enumerator);
                yield return null;
            }
            foreach (Graphic g2 in graphics)
            {
                bool flag2 = g2 == null;
                if (!flag2)
                {
                    Color c2 = g2.color;
                    g2.color = new Color(c2.r, c2.g, c2.b, to);
                    c2 = default(Color);

                }
            }
            List<Graphic>.Enumerator enumerator2 = default(List<Graphic>.Enumerator);
            this.ShowPanel(false);
            yield break;
        }

        // Token: 0x06000032 RID: 50 RVA: 0x00002215 File Offset: 0x00000415
        private IEnumerator FadeAllGraphics(GameObject root, float from, float to, float duration, bool destroyAfter = false)
        {
            List<Graphic> graphics = new List<Graphic>(root.GetComponentsInChildren<Graphic>(true));
            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float alpha = Mathf.Lerp(from, to, t / duration);
                foreach (Graphic g in graphics)
                {
                    bool flag = g == null;
                    if (!flag)
                    {
                        Color c = g.color;
                        g.color = new Color(c.r, c.g, c.b, alpha);
                        c = default(Color);

                    }
                }
                List<Graphic>.Enumerator enumerator = default(List<Graphic>.Enumerator);
                yield return null;
            }
            foreach (Graphic g2 in graphics)
            {
                bool flag2 = g2 == null;
                if (!flag2)
                {
                    Color c2 = g2.color;
                    g2.color = new Color(c2.r, c2.g, c2.b, to);
                    c2 = default(Color);

                }
            }
            List<Graphic>.Enumerator enumerator2 = default(List<Graphic>.Enumerator);
            if (destroyAfter)
            {
                root.SetActive(false);
            }
            yield break;
        }

        // Token: 0x06000033 RID: 51 RVA: 0x0000447C File Offset: 0x0000267C
        private Sprite CreateOptimizedRoundedSprite(int width, int height, int cornerRadius)
        {
            Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
            texture2D.filterMode = FilterMode.Bilinear;
            texture2D.wrapMode = TextureWrapMode.Clamp;
            Color white = Color.white;
            float num = 1.5f;
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    float a = 1f;
                    bool flag = j < cornerRadius;
                    bool flag2 = j >= width - cornerRadius;
                    bool flag3 = i < cornerRadius;
                    bool flag4 = i >= height - cornerRadius;
                    bool flag5 = (flag || flag2) && (flag3 || flag4);
                    if (flag5)
                    {
                        float num2 = (float)(flag ? cornerRadius : (width - cornerRadius));
                        float num3 = (float)(flag3 ? cornerRadius : (height - cornerRadius));
                        float num4 = (float)j - num2;
                        float num5 = (float)i - num3;
                        float num6 = Mathf.Sqrt(num4 * num4 + num5 * num5);
                        bool flag6 = num6 > (float)cornerRadius;
                        if (flag6)
                        {
                            a = Mathf.Clamp01(1f - (num6 - (float)cornerRadius) / num);
                        }
                    }
                    texture2D.SetPixel(j, i, new Color(1f, 1f, 1f, a));
                }
            }
            texture2D.Apply();
            return Sprite.Create(texture2D, new Rect(0f, 0f, (float)width, (float)height), new Vector2(0.5f, 0.5f), 100f, 1U, SpriteMeshType.FullRect, new Vector4((float)cornerRadius, (float)cornerRadius, (float)cornerRadius, (float)cornerRadius));
        }

        // Token: 0x06000034 RID: 52 RVA: 0x000045F0 File Offset: 0x000027F0
        private void UpdateGunRay()
        {
            Transform rightControllerTransform = this.GetRightControllerTransform();
            bool flag = rightControllerTransform == null;
            if (!flag)
            {
                bool flag2 = !SimpleInputs.RightGrab || !this.gunRayEnabled;
                if (flag2)
                {
                    bool flag3 = this.gunRay != null;
                    if (flag3)
                    {
                        UnityEngine.Object.Destroy(this.gunRay.gameObject);
                        this.gunRay = null;
                    }
                    bool flag4 = this.gunSphere != null;
                    if (flag4)
                    {
                        UnityEngine.Object.Destroy(this.gunSphere);
                        this.gunSphere = null;
                    }
                }
                else
                {
                    bool flag5 = this.gunRay == null;
                    if (flag5)
                    {
                        this.gunRay = new GameObject("GunRay").AddComponent<LineRenderer>();
                        this.gunRay.startWidth = 0.012f;
                        this.gunRay.endWidth = 0.012f;
                        this.gunRay.material = new Material(Shader.Find("Sprites/Default"));
                        this.gunRay.startColor = Color.white;
                        this.gunRay.endColor = Color.white;
                        this.gunRay.positionCount = 2;
                    }
                    bool flag6 = this.gunSphere == null;
                    if (flag6)
                    {
                        this.gunSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                        this.gunSphere.name = "GunEndSphere";
                        this.gunSphere.transform.localScale = Vector3.one * 0.15f;
                        Material material = new Material(Shader.Find("Unlit/Color"));
                        material.color = new Color(0.95f, 0.95f, 0.95f, 1f);
                        this.gunSphere.GetComponent<Renderer>().material = material;
                        UnityEngine.Object.Destroy(this.gunSphere.GetComponent<Collider>());
                    }
                    Vector3 position = rightControllerTransform.position;
                    Vector3 forward = rightControllerTransform.forward;
                    int layerMask = ~LayerMask.GetMask(new string[]
                    {
                        "IgnoreRaycast"
                    });
                    Vector3 b = position + forward * 100f;
                    bool flag7 = this.lockedTarget != null;
                    if (flag7)
                    {
                        b = this.lockedTarget.position + Vector3.up * 0.3f;
                    }
                    else
                    {
                        RaycastHit raycastHit;
                        bool flag8 = Physics.Raycast(position, forward, out raycastHit, 100f, layerMask);
                        if (flag8)
                        {
                            b = raycastHit.point;
                            VRRig componentInParent = raycastHit.collider.GetComponentInParent<VRRig>();
                            bool flag9 = componentInParent != null && (SimpleInputs.RightTrigger || this.autoLockEnabled);
                            if (flag9)
                            {
                                this.selectedRig = componentInParent;
                                this.lockedTarget = componentInParent.transform;
                                this.UpdatePage();
                            }
                            else
                            {
                                bool flag10 = componentInParent != null;
                                if (flag10)
                                {
                                    this.selectedRig = componentInParent;
                                }
                            }
                        }
                    }
                    bool flag11 = !SimpleInputs.RightTrigger && !this.autoLockEnabled;
                    if (flag11)
                    {
                        this.lockedTarget = null;
                    }
                    this.gunRay.SetPosition(0, position);
                    Vector3 a = (this.gunRay.positionCount > 1) ? this.gunRay.GetPosition(1) : (position + forward * 100f);
                    this.gunRay.SetPosition(1, Vector3.Lerp(a, b, Time.deltaTime * 15f));
                    this.gunSphere.transform.position = Vector3.Lerp(this.gunSphere.transform.position, b, Time.deltaTime * 15f);
                }
            }
        }

        // Token: 0x06000035 RID: 53 RVA: 0x00004978 File Offset: 0x00002B78
        private void UpdateUIPanelPosition()
        {
            bool flag = this.uiPanel == null;
            if (!flag)
            {
                Transform leftControllerTransform = this.GetLeftControllerTransform();
                bool flag2 = leftControllerTransform != null;
                if (flag2)
                {
                    this.uiPanel.transform.position = leftControllerTransform.position + leftControllerTransform.right * 0.09f - leftControllerTransform.up * 0.02f + leftControllerTransform.forward * 0.03f;
                    this.uiPanel.transform.rotation = leftControllerTransform.rotation * Quaternion.Euler(125f, 0f, 10f);
                }
            }
        }

        // Token: 0x06000036 RID: 54 RVA: 0x00004A38 File Offset: 0x00002C38
        private Transform GetRightControllerTransform()
        {
            bool flag = GTPlayer.Instance == null;
            Transform result;
            if (flag)
            {
                result = null;
            }
            else
            {
                Transform transform = GTPlayer.Instance.transform.Find("rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R");
                bool flag2 = transform != null;
                if (flag2)
                {
                    result = transform;
                }
                else
                {
                    foreach (Transform transform2 in GTPlayer.Instance.GetComponentsInChildren<Transform>())
                    {
                        string text = transform2.name.ToLower();
                        bool flag3 = text.Contains("right") && (text.Contains("hand") || text.Contains("controller"));
                        if (flag3)
                        {
                            return transform2;
                        }
                    }
                    result = null;
                }
            }
            return result;
        }

        // Token: 0x06000037 RID: 55 RVA: 0x00004AF8 File Offset: 0x00002CF8
        private Transform GetLeftControllerTransform()
        {
            bool flag = GTPlayer.Instance == null;
            Transform result;
            if (flag)
            {
                result = null;
            }
            else
            {
                Transform transform = GTPlayer.Instance.transform.Find("rig/body/shoulder.L/upper_arm.L/forearm.L/hand.L");
                bool flag2 = transform != null;
                if (flag2)
                {
                    result = transform;
                }
                else
                {
                    foreach (Transform transform2 in GTPlayer.Instance.GetComponentsInChildren<Transform>())
                    {
                        string text = transform2.name.ToLower();
                        bool flag3 = text.Contains("left") && (text.Contains("hand") || text.Contains("controller"));
                        if (flag3)
                        {
                            return transform2;
                        }
                    }
                    result = null;
                }
            }
            return result;
        }

        // Token: 0x04000009 RID: 9
        private bool lastXButtonState = false;

        // Token: 0x0400000A RID: 10
        private GameObject uiPanel;

        // Token: 0x0400000B RID: 11
        private Coroutine fadeRoutine;

        // Token: 0x0400000C RID: 12
        private bool canPressButtons = true;

        // Token: 0x0400000D RID: 13
        private const float TOUCH_DISTANCE = 0.08f;

        // Token: 0x0400000E RID: 14
        private TextMeshProUGUI titleText;

        // Token: 0x0400000F RID: 15
        private TextMeshProUGUI pageText;

        // Token: 0x04000010 RID: 16
        private TextMeshProUGUI playerNameText;

        // Token: 0x04000011 RID: 17
        private TextMeshProUGUI playerPlatformText;

        // Token: 0x04000012 RID: 18
        private TextMeshProUGUI playerFPSText;

        // Token: 0x04000013 RID: 19
        private TextMeshProUGUI playerCreationDateText;

        // Token: 0x04000014 RID: 20
        private TextMeshProUGUI playerModsText;

        // Token: 0x04000015 RID: 21
        private TextMeshProUGUI playerCosmeticsText;

        // Token: 0x04000016 RID: 22
        private GameObject gunRayToggle;

        // Token: 0x04000017 RID: 23
        private GameObject autoLockToggle;

        // Token: 0x04000018 RID: 24
        private GameObject opacityLowBtn;

        // Token: 0x04000019 RID: 25
        private GameObject opacityMedBtn;

        // Token: 0x0400001A RID: 26
        private GameObject opacityHighBtn;

        // Token: 0x0400001B RID: 27
        private GameObject resetBtn;

        // Token: 0x0400001C RID: 28
        private TextMeshProUGUI creditsText;

        // Token: 0x0400001D RID: 29
        private GameObject backButton;

        // Token: 0x0400001E RID: 30
        private GameObject homeButton;

        // Token: 0x0400001F RID: 31
        private GameObject nextButton;

        // Token: 0x04000020 RID: 32
        private GameObject separatorLine;

        // Token: 0x04000021 RID: 33
        private LineRenderer gunRay;

        // Token: 0x04000022 RID: 34
        private GameObject gunSphere;

        // Token: 0x04000023 RID: 35
        private VRRig selectedRig;

        // Token: 0x04000024 RID: 36
        private Transform lockedTarget;

        // Token: 0x04000025 RID: 37
        private bool gunRayEnabled = true;

        // Token: 0x04000026 RID: 38
        private bool autoLockEnabled = false;

        // Token: 0x04000027 RID: 39
        private float menuOpacity = 0.95f;

        // Token: 0x04000028 RID: 40
        private int currentPage = 1;

        // Token: 0x04000029 RID: 41
        private int maxPage = 3;

        // Token: 0x0400002A RID: 42
        private readonly Color panelColor = new Color(0.4f, 0.2f, 0.05f, 1f);

        // Token: 0x0400002B RID: 43
        private readonly Color elementDark = new Color(0.7f, 0.45f, 0.2f, 1f);

        // Token: 0x0400002C RID: 44
        private readonly Color textLight = new Color(0.86f, 0.86f, 0.86f, 1f);

        // Token: 0x0400002D RID: 45
        private readonly Color separatorColor = new Color(0.22f, 0.22f, 0.22f, 1f);

        // Token: 0x0400002E RID: 46
        private readonly Color activeGreen = new Color(0.2f, 0.8f, 0.3f, 1f);

        // Token: 0x0400002F RID: 47
        private CanvasGroup uiCanvasGroup;
    }
}
