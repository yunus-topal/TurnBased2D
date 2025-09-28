using System;
using Combat;
using Combat.UI;
using Helpers;
using Models;
using Models.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rest
{
    public class UpgradeSkillUIHelper : MonoBehaviour
    {
        private Character _character;

        [Header("UI")] 
        [SerializeField] private Image portrait;

        [SerializeField] private Button backButton;
        [SerializeField] private Button upgradeSelectButton;
        [SerializeField] private TextMeshProUGUI warningText;
        [SerializeField] private SkillButton[] skillButtons;

        [Header("Details Panel")] [SerializeField]
        private TextMeshProUGUI skillNameText;

        [SerializeField] private TextMeshProUGUI skillDescText;
        [SerializeField] private GameObject detailsPanel; // toggle on hover

        private SkillButton _selectedButton; // currently selected (if any)
        private RestManager _restManager;
        private RestPanelHelper _restPanelHelper;

        #region Setup

        private bool _isSetup = false;

        private void Awake()
        {
            _restPanelHelper = FindAnyObjectByType<RestPanelHelper>();
            _restManager = FindAnyObjectByType<RestManager>();
        }

        public void InitializeSkillsUI(Character character)
        {
            _character = character;
            if (portrait) portrait.sprite = character.Sprite;
            _selectedButton = null;
            SetupSkills();
            RegisterEvents();
            ClearDetailsPanel();
            DeselectSkillButton();
            
            upgradeSelectButton.onClick.RemoveAllListeners();
            upgradeSelectButton.onClick.AddListener(OnUpgradeSelected);
            
            backButton.onClick.RemoveAllListeners();
            backButton.onClick.AddListener(OnUpgradeCancelled);
        }

        private void RegisterEvents()
        {
            if (_isSetup) return;
            foreach (var btn in skillButtons)
            {
                btn.HoverEnter += OnSkillHoverEnter;
                btn.HoverExit += OnSkillHoverExit;
                btn.Clicked += OnSkillClicked;
            }
            _isSetup = true;
        }

        private void SetupSkills()
        {
            // Bind up to array length (e.g., 4)
            for (int i = 0; i < skillButtons.Length; i++)
            {
                if (_character.Skills != null && i < _character.Skills.Count)
                {
                    var btn = skillButtons[i];
                    var upgraded = _character.skillsUpgraded[i];
                    btn.Bind(_character.Skills[i], !upgraded, upgraded);
                }
                else
                {
                    skillButtons[i].Unbind();
                }
            }
        }

        private void OnUpgradeCancelled()
        {
            // Hide submenu overlay
            // toggle rest on.
            _selectedButton = null;
            warningText.gameObject.SetActive(false);
            _restManager.UpdateChoiceAsRest(_character, RestAction.Rest);
            _restPanelHelper.CancelUpgrade(_character);
            _restPanelHelper.SetSubMenuOverlayActive(_character, false);
        }
        private void OnUpgradeSelected()
        {
            if (_selectedButton is null)
            {
                // activate warning text
                warningText.gameObject.SetActive(true);
                StartCoroutine(SystemHelper.InvokeLambda(() => warningText.gameObject.SetActive(false), 3f));
                return;
            }
            // send selected skill to rest manager.
            _restManager.UpdateChoiceAsUpgrade(_character, _selectedButton.BoundSkill);
            _restPanelHelper.SetSubMenuOverlayActive(_character, false);
        }

        #endregion

        #region Unity Functions

        private void OnDestroy()
        {
            // Clean up subs if object is destroyed
            foreach (var btn in skillButtons)
            {
                if (!btn) continue;
                btn.HoverEnter -= OnSkillHoverEnter;
                btn.HoverExit -= OnSkillHoverExit;
                btn.Clicked -= OnSkillClicked;
            }
        }

        private void Update()
        {
            // Cancel targeting via Esc
            if (_selectedButton && Input.GetKeyDown(KeyCode.Escape))
            {
                DeselectSkillButton();
            }
        }


        #endregion

        #region Button Events

        private void OnSkillHoverEnter(SkillButton btn)
        {
            if (!btn || btn.BoundSkill == null) return;
            ShowDetails(btn.BoundSkill);
        }

        private void OnSkillHoverExit(SkillButton btn)
        {
            // Hide details only if not targeting; or keep if you prefer always show last hovered
            if (!_selectedButton)
                ClearDetailsPanel();
            else
                ShowDetails(_selectedButton.BoundSkill);
        }

        private void OnSkillClicked(SkillButton btn)
        {
            if (!btn || !btn.BoundSkill) return;

            // Re-click same button while targeting -> cancel
            if (_selectedButton && _selectedButton == btn)
            {
                DeselectSkillButton();
                return;
            }
            // Select new button (or select first time)
            SelectSkillButton(btn);
        }

        #endregion

        #region Skill Selection
        private void SelectSkillButton(SkillButton btn)
        {
            // Deselect previous
            if (_selectedButton && _selectedButton != btn)
            {
                _selectedButton.SetSelected(false);
            }

            _selectedButton = btn;
            _selectedButton.SetSelected(true);
            ShowDetails(btn.BoundSkill);
        }
        
        private void DeselectSkillButton(bool silent = false)
        {
            if (_selectedButton)
            {
                _selectedButton.SetSelected(false);
                _selectedButton = null;
            }
            
            if (!silent)
                ClearDetailsPanel();
        }
        

        #endregion

        #region Details Panel

        private void ShowDetails(Skill skill)
        {
            if (detailsPanel) detailsPanel.SetActive(true);
            // if skill upgraded, show skill name as skill+

            var skillDetails = skill.GetSkillDetails(_character.IsSkillUpgraded(skill));
            if (skillNameText) skillNameText.text = skillDetails.Item1;
            if (skillDescText) skillDescText.text = skillDetails.Item2;
        }

        private void ClearDetailsPanel()
        {
            if (skillNameText) skillNameText.text = string.Empty;
            if (skillDescText) skillDescText.text = string.Empty;
            if (detailsPanel) detailsPanel.SetActive(false);
        }

        #endregion

    }
}