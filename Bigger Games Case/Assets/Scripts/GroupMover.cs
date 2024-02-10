using Lean.Common;
using UnityEngine;
using Lean.Touch;

public class GroupMover : MonoBehaviour
{
    private LeanSelectable selectedGroup;
    private Vector2 lastFingerPosition;

    private void OnEnable()
    {
        // LeanTouch olaylarını dinle
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUpdate += OnFingerUpdate;
    }

    private void OnDisable()
    {
        // LeanTouch olaylarını durdur
        LeanTouch.OnFingerDown -= OnFingerDown;
        LeanTouch.OnFingerUpdate -= OnFingerUpdate;
    }

    private void OnFingerDown(LeanFinger finger)
    {
        // Raycast ile dokunulan nesneyi kontrol et
        Ray ray = Camera.main.ScreenPointToRay(finger.ScreenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            // LeanSelectable bileşeni varsa seç
            LeanSelectable leanSelectable = hit.transform.GetComponent<LeanSelectable>();

            if (leanSelectable != null)
            {
                selectedGroup = leanSelectable;
                lastFingerPosition = finger.ScreenPosition;
            }
        }
    }

    private void OnFingerUpdate(LeanFinger finger)
    {
        // Seçili grup varsa, hareket ettir
        if (selectedGroup != null)
        {
            Vector2 delta = finger.ScreenPosition - lastFingerPosition;
            MoveGroup(selectedGroup, delta);
            lastFingerPosition = finger.ScreenPosition;
        }
    }

    private void MoveGroup(LeanSelectable group, Vector2 delta)
    {
        // Grubu belirtilen delta ile hareket ettir
        group.transform.Translate(delta.x * Time.deltaTime, delta.y * Time.deltaTime, 0);
    }
}