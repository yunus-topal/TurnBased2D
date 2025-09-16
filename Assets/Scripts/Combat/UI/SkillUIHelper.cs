using Models;
using Models.Scriptables;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Combat.UI {
    public class SkillUIHelper : MonoBehaviour
    {
        private Character _character;
        private TurnManager _turnManager;

        [Header("UI")] [SerializeField] private Image portrait;
        [SerializeField] private SkillButton[] skillButtons;

        [Header("Details Panel")] [SerializeField]
        private TextMeshProUGUI skillNameText;

        [SerializeField] private TextMeshProUGUI skillDescText;
        [SerializeField] private GameObject detailsPanel; // toggle on hover

        private SkillButton _selectedButton; // currently selected (if any)

        #region Setup

        private bool _isSetup = false;

        public void InitializeSkillsUI(Character character, TurnManager turnManager)
        {
            _character = character;
            _turnManager = turnManager;
            if (portrait) portrait.sprite = character.Sprite;
            _selectedButton = null;
            SetupSkills();
            RegisterEvents();
            ClearDetailsPanel();
            DeselectSkillButton();
        }

        private void RegisterEvents()
        {
            if (_isSetup) return;
            foreach (var btn in skillButtons)
            {
                btn.HoverEnter += OnSkillHoverEnter;
                btn.HoverExit += OnSkillHoverExit;
                btn.Clicked += OnSkillClicked;
                btn.Clicked += _turnManager.SetSelectedSkill;
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
                    btn.Bind(_character.Skills[i], _character.Team == Team.Player, _character.skillsUpgraded[i]);
                }
                else
                {
                    skillButtons[i].Unbind();
                }
            }
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
                btn.Clicked -= _turnManager.SetSelectedSkill;
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
            string prefix = _character.IsSkillUpgraded(skill) ? "+" : "";  
            if (skillNameText) skillNameText.text = $"{skill.skillName}{prefix}";
            if (skillDescText)
            {
                // You can format extended info here (AP cost, range, cooldown, etc.)
                skillDescText.text = skill.description;
            }
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
