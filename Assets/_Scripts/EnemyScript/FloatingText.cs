using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    [SerializeField] private float destroyTime = 2f;
    public TMP_Text tmp;
    private void Start()
    {
        Destroy(gameObject, destroyTime);

        transform.localPosition += new Vector3(Random.Range(-1f, 1f), Random.Range(-1.5f, 1.5f), 0);
    }

    public void SetText(string text)
    {
        if (tmp != null)
        {
            tmp.text = text;
        }
        else
        {
            tmp = GetComponent<TMP_Text>();
            if (tmp != null) tmp.text = text;
        }
    }
}