using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GachaScript : MonoBehaviour
{
    public Button spinButton;
    public TextMeshProUGUI spinText;
    public TextMeshProUGUI point;
    public GameObject popup;
    public TextMeshProUGUI popupText;
    public Button popupButton;
    public bool isRandom = false;
    public int prizeIndex;
    private int rotations;
    private float duration;
    private float currentTime;
    private float initialSpeed;
    private float totalRotation;
    private float totalRotated;
    private string[] prizes = new string[] { "5", "10", "20", "Quay lại", "50", "30", "100", "Nhân đôi", "200", "Chia đôi", "500" };
    private float segmentAngle;
    private int totalPoint = 0;

    //gán sự kiện onlick cho các button
    //tính góc quay cho điểm
    private void Start()
    {
        spinButton.onClick.AddListener(OnSpinButtonClick);
        popupButton.onClick.AddListener(OnPopupButtonClick);
        spinText.text = "0";
        segmentAngle = 360f / prizes.Length;
    }

    //bắt đầu quay
    private void OnSpinButtonClick()
    {
        if (isRandom)
        {
            prizeIndex = Random.Range(0, 10);
        }

        spinText.text = "Spinning...";

        //random thời gian 4 - 5s
        duration = Random.Range(4f, 5f);

        //random số lần quay tối thiểu 7 - 10
        rotations = Random.Range(7, 10);

        //random góc quay tới mục tiêu
        float targetRotation = Random.Range(prizeIndex * segmentAngle, (prizeIndex + 1) * segmentAngle);

        //tính tổng góc cần quay
        totalRotation = rotations * 360f + targetRotation; 

        //tốc độ ban đầu
        initialSpeed = totalRotation * 2 / duration; 

        // Số góc đã quay (0 nếu lần đầu tiên)
        totalRotated = transform.eulerAngles.z; 

        currentTime = 0f;
        
        spinButton.interactable = false;
    }

    //đóng popup
    private void OnPopupButtonClick()
    {
        popup.SetActive(false);
    }


    //thực hiện quay
    private void Update()
    {
        if (totalRotated < totalRotation)
        {
            float deltaTime = Time.deltaTime;

            // Tỷ lệ thời gian đã trôi qua
            float t = currentTime / duration; 

            // Tính tốc độ tại thời điểm currentTime
            float currentSpeed = initialSpeed * (1 - t); 

            //tính góc đã quay    
            float rotationAmount = currentSpeed * deltaTime;
            totalRotated += rotationAmount;

            //nếu trong frame cuối quay quá góc quy định, vd: frame cuối cần quay 5 để hoàn thành nhưng tính toán ra 6
            //thì chỉ quay đúng bằng số góc còn thiếu -> 5
            if (totalRotated > totalRotation)
            {
                rotationAmount -= totalRotated - totalRotation;
                totalRotated = totalRotation;
            }

            transform.Rotate(Vector3.forward, rotationAmount);
            currentTime += deltaTime;

            //cập nhật thông tin sau khi quay
            if (totalRotated == totalRotation)
            {
                spinButton.interactable = true;
                PointUpdate();
                spinText.text = prizes[prizeIndex];
                point.text = totalPoint.ToString() + "k";
                popupText.text = prizes[prizeIndex];
                popup.SetActive(true);
            }
        }
    }

    //update điểm khi quay xong
    private void PointUpdate()
    {
        string prize = prizes[prizeIndex];
        switch (prize)
        {
            case "Nhân đôi":
                totalPoint *= 2;
                break;
            case "Chia đôi":
                totalPoint /= 2;
                break;
            case "Quay lại":
                break;
            default:
                totalPoint += int.Parse(prize);
                break;
        }
    }
}