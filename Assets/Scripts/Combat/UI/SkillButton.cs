using System;
using Helpers;
using Models.Scriptables;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Combat.UI
{
    [RequireComponent(typeof(Button))]
    public class SkillButton :  MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {    
        [Header("Wiring")]
        [SerializeField] private Button button;          // required
        [SerializeField] private Image icon;             // required
        [SerializeField] private Color selectedColor;  // optional highlight
        [SerializeField] private GameObject targetingFx; // optional “aiming” highlight
        
        private Color _defaultColor;
        
        public event Action<SkillButton> HoverEnter;
        public event Action<SkillButton> HoverExit;
        public event Action<SkillButton> Clicked;

        public Skill BoundSkill { get; private set; }

        void Awake()
        {
            if (!button) button = GetComponent<Button>();
            if (!icon)   icon   = GetComponent<Image>();
            
            _defaultColor = icon.color;
        }

        void OnEnable()
        {
            if (button != null) button.onClick.AddListener(HandleClick);
        }

        void OnDisable()
        {
            if (button != null) button.onClick.RemoveListener(HandleClick);
        }

        public void Bind(Skill skill, bool interactable = true, bool upgraded = false)
        {
            BoundSkill = skill;
            if (icon) icon.sprite = skill.skillIcon;
            if (button) button.interactable = skill.skillType == SkillType.Active; // your rule here
            SetSelected(false);
            gameObject.SetActive(true);
            button.interactable = interactable && skill.skillType == SkillType.Active;
        }

        public void Unbind()
        {
            BoundSkill = null;
            gameObject.SetActive(false);
        }

        public void SetSelected(bool on)
        {
            icon.color = on ? selectedColor : _defaultColor;
        }

        private void HandleClick()
        {
            Clicked?.Invoke(this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            HoverEnter?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            HoverExit?.Invoke(this);
        }
    }
}