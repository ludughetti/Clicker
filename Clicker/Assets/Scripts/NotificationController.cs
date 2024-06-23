using System.Collections;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationController : MonoBehaviour
{
#if UNITY_ANDROID
    private static string _prefKey_notifInitialized = "HighScore";
    private static string _notif_groupId = "Main";
    private static string _notif_channelId = "notis01";

    void Start()
    {
        if (!PlayerPrefs.HasKey(_prefKey_notifInitialized))
        {
            var group = new AndroidNotificationChannelGroup()
            {
                Id = _notif_groupId,
                Name = "Main notifications"
            };
            AndroidNotificationCenter.RegisterNotificationChannelGroup(group);

            var channel = new AndroidNotificationChannel()
            {
                Id = _notif_channelId,
                Name = "All notifications",
                Importance = Importance.Default,
                Description = "Canal primario de notificaciones",
                Group = _notif_groupId
            };
            AndroidNotificationCenter.RegisterNotificationChannel(channel);

            StartCoroutine(RequestPermission());
            PlayerPrefs.SetString(_prefKey_notifInitialized, true.ToString());
            PlayerPrefs.Save();
        } else
        {
            ScheduleNotifications();
        }
    }

    private IEnumerator RequestPermission()
    {
        var request = new PermissionRequest();
        while (PermissionStatus.RequestPending.Equals(request.Status))
            yield return new WaitForEndOfFrame();

        ScheduleNotifications();
    }

    private void ScheduleNotifications()
    {
        Debug.Log("Cleaning up scheduled notifications");
        AndroidNotificationCenter.CancelAllScheduledNotifications();

        Debug.Log("Scheduling notifications");
        var tenMinInactiveNotif = new AndroidNotification
        {
            Title = "Han pasado 10 minutos desde que jugaste",
            Text = "¿Podrás obtener un nuevo puntaje?",
            FireTime = System.DateTime.Now.AddMinutes(10)
        };
        AndroidNotificationCenter.SendNotification(tenMinInactiveNotif, _notif_channelId);
    }
#else
    private void Awake()
    {
        gameObject.SetActive(false);
    }
#endif
}
