using UnityEngine;

public class CustomerNPC : MonoBehaviour, INPCOrderable
{
    public bool HasOrder { get; private set; }

    [SerializeField]
    private OrderData currentOrder;

    private void Update()
    {
        TestSeat();
    }

    // 주문 받기 기능 테스트를 위한 코드
    public void TestSeat()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            CreateOrder();
            Debug.Log($"주문 생성: {currentOrder.customerName}");
        }
    }

    public void SitOnSeat(Seat seat)
    {
        // 자리 착석 기능 구현 후 주석 제거 예정
        // seat.AssignNPC(this);
        CreateOrder();
    }

    private void CreateOrder()
    {
        HasOrder = true;
        // 주문 데이터 중 랜덤으로 선택해 currentOrder에 설정
    }

    public OrderData GetOrder()
    {
        return currentOrder;
    }

    public void MarkOrderReceived()
    {
        HasOrder = false;
    }
}
