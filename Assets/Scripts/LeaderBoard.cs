using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LeaderBoard : MonoBehaviour
{
    public GameObject RowPrefab;

    public GameObject EasyScrollView;
    public GameObject ModerateScrollView;
    public GameObject HardScrollView;
    private Webservice webservice = new Webservice();

    private bool isActive = false;

    // Use this for initialization
    void Start()
    {
        webservice.OnError += OnErrorRecieveBoardData;
        webservice.OnRecieve += OnRecieveBoardData;
    }

    public void refreshData()
    {
        StartCoroutine(webservice.GetRequest("http://game.codetower.ir/api/getRecords"));
    }


    void OnRecieveBoardData(string message)
    {
        ResponseData responseData = ResponseData.CreateFromJSON("{\"records\":" + message + "}");
        foreach (Transform child in EasyScrollView.transform) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in ModerateScrollView.transform) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in HardScrollView.transform) {
            Destroy(child.gameObject);
        }
        foreach (Record record in responseData.records)
        {
            GameObject newRow =
                Instantiate(RowPrefab, EasyScrollView.transform.position, Quaternion.identity) as GameObject;
            switch (record.type)
            {
                case 1:
                    newRow.transform.parent = EasyScrollView.transform;
                    break;
                case 2:
                    newRow.transform.parent = ModerateScrollView.transform;
                    break;
                case 3:
                    newRow.transform.parent = HardScrollView.transform;
                    break;
            }

            RecordRowSetter RecordRowSetter = newRow.GetComponent<RecordRowSetter>();

            RecordRowSetter.SetRecord(record.name, record.record + "");
        }
    }

    void OnErrorRecieveBoardData(string message)
    {
    }
}