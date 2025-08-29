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
        private bool _inTargetSelection;

        #region Setup

        public void InitializeSkillsUI(Character character, TurnManager turnManager)
        {
            _character = character;
            _turnManager = turnManager;
            if (portrait) portrait.sprite = character.Sprite;
            SetupSkills();
            ClearDetailsPanel();
            ExitTargetSelection(silent: true);
        }

        private void SetupSkills()
        {
            // Bind up to array length (e.g., 4)
            for (int i = 0; i < skillButtons.Length; i++)
            {
                if (_character.Skills != null && i < _character.Skills.Count)
                {
                    var btn = skillButtons[i];
                    btn.Bind(_character.Skills[i]);

                    // Subscribe once per setup
                    btn.HoverEnter += OnSkillHoverEnter;
                    btn.HoverExit += OnSkillHoverExit;
                    btn.Clicked += OnSkillClicked;
                    btn.Clicked += _turnManager.SetSelectedSkill;
                }
                else
                {
                    // Ensure unbound/hidden and no leaks
                    if (skillButtons[i].isActiveAndEnabled)
                    {
                        skillButtons[i].HoverEnter -= OnSkillHoverEnter;
                        skillButtons[i].HoverExit -= OnSkillHoverExit;
                        skillButtons[i].Clicked -= OnSkillClicked;
                        skillButtons[i].Clicked -= _turnManager.SetSelectedSkill;
                    }

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
            }
        }

        private void Update()
        {
            // Cancel targeting via Esc
            if (_inTargetSelection && Input.GetKeyDown(KeyCode.Escape))
            {
                ExitTargetSelection();
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
            if (!_inTargetSelection)
                ClearDetailsPanel();
        }

        private void OnSkillClicked(SkillButton btn)
        {
            if (btn == null || btn.BoundSkill == null) return;

            // Re-click same button while targeting -> cancel
            if (_inTargetSelection && _selectedButton == btn)
            {
                ExitTargetSelection();
                return;
            }

            // Select new button (or select first time)
            SelectButton(btn);
            EnterTargetSelection();
        }

        #endregion

        #region Skill Selection
        private void SelectButton(SkillButton btn)
        {
            // Deselect previous
            if (_selectedButton != null && _selectedButton != btn)
            {
                _selectedButton.SetSelected(false);
                _selectedButton.SetTargeting(false);
            }

            _selectedButton = btn;
            _selectedButton.SetSelected(true);
            // optional: also show its details while selected
            ShowDetails(btn.BoundSkill);
        }

        private void EnterTargetSelection()
        {
            _inTargetSelection = true;
            if (_selectedButton) _selectedButton.SetTargeting(true);

            // TODO: hand off to your game flow: enable target reticle, raycasts, etc.
            // Example:
            // TargetingSystem.BeginSelectTarget(_selectedButton.BoundSkill, onTargetChosen: HandleTargetChosen, onCanceled: ExitTargetSelection);
        }

        private void ExitTargetSelection(bool silent = false)
        {
            _inTargetSelection = false;

            if (_selectedButton)
            {
                _selectedButton.SetTargeting(false);
                _selectedButton.SetSelected(false);
                _selectedButton = null;
            }

            // TODO: notify your game flow
            // TargetingSystem.Cancel();

            if (!silent)
                ClearDetailsPanel();
        }
        

        #endregion

        #region Details Panel

        private void ShowDetails(Skill skill)
        {
            if (detailsPanel) detailsPanel.SetActive(true);
            if (skillNameText) skillNameText.text = skill.skillName;
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
