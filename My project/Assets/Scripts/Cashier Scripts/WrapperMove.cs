using UnityEngine;

public class WrapperMove : MonoBehaviour
{
    private Vector3 closedPosition;
    private Vector3 openPosition;
    public AudioClip cashRegClip;
    public AudioClip cashRegClosingClip;

    private void Start()
    {
        closedPosition = transform.position; // Kapal� konum
        openPosition = new Vector3(closedPosition.x, closedPosition.y, closedPosition.z + 0.295f); // A��k konum, Y ekseninde de�i�iklik yap�ld�

        CashierManager.Instance.OnTransactionCompleted += HandleTransactionCompleted;
        CashierManager.Instance.OnReceivedMoneyUpdated += HandleReceivedMoneyUpdated;

        // Ba�lang��ta kapal� konumda
        transform.position = closedPosition;
    }

    private void OnDestroy()
    {
        if (CashierManager.Instance != null)
        {
            CashierManager.Instance.OnTransactionCompleted -= HandleTransactionCompleted;
            CashierManager.Instance.OnReceivedMoneyUpdated -= HandleReceivedMoneyUpdated;
        }
    }

    private void HandleTransactionCompleted()
    {
        transform.position = closedPosition; // Kapal� konuma geri d�n
        AudioSource.PlayClipAtPoint(cashRegClosingClip, transform.position);
    }

    private void HandleReceivedMoneyUpdated(decimal receivedMoney)
    {
        transform.position = openPosition; // A��k konuma ge�
        AudioSource.PlayClipAtPoint(cashRegClip, transform.position);
    }
}
