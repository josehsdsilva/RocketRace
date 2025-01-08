using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NotificationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text notificationText;
    [SerializeField] private Button closeButton;

    private Action onClose;

    private void Start()
    {
        closeButton.onClick.AddListener(HideNotification);
        notificationPanel.SetActive(false);
    }

    internal void ShowNotification(string title, string notification, Action onClose = null)
    {
        this.onClose = onClose;
        titleText.text = title.ToLower();
        notificationText.text = notification.ToLower();
        notificationPanel.SetActive(true);
    }

    internal void HideNotification()
    {
        notificationPanel.SetActive(false);
        onClose?.Invoke();
    }
}
