using System;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Combat.UI
{
    public class StatusEffectIconHelper : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TextMeshProUGUI turnCount;
        [SerializeField] private TextMeshProUGUI statusEffectTooltıp;

        private Image statusEffectImage;

        private void Start()
        {
            statusEffectImage = GetComponent<Image>();
        }

        public void Initialize(StatusInstance instance, StatusEffectIconDatabase.StatusEffectIconEntry entry)
        {
            if (statusEffectImage is null)
            {
                statusEffectImage  = GetComponent<Image>(); 
            }
            turnCount.text = instance.Duration.ToString();
            statusEffectTooltıp.text = entry.Description;
            statusEffectTooltıp.gameObject.SetActive(false);
            statusEffectImage.sprite = entry.Icon;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            statusEffectTooltıp.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            statusEffectTooltıp.gameObject.SetActive(false);
        }
    }
}
 