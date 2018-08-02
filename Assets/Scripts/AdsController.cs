using System;
using System.Collections;
using System.Collections.Generic;
using TapsellSDK;
using TapsellSimpleJSON;
using UnityEngine;

public class AdsController : MonoBehaviour
{
    public static bool videoAvailable = false;
    public static bool bannerAvailable = false;
    public static TapsellAd Videoad = null;
    public static TapsellAd Bannerad = null;
    public static bool bannerAllow = false;
    public static bool videoAllow = false;
    private string bannerZoneId = "5b2dcd935219ec0001883651";
    private string videoZoneId = "5b2e01fb3daef00001f0729c";
    private bool requestAdAgain = false;
    private string requestZone = "";

    public delegate void OnFinishVideo();
    public OnFinishVideo onFinishCallback;
    // Use this for initialization
    void Start()
    {
        Tapsell.initialize("cparbfdmekjrfrjeacobmgfqofknjhftsjmnlfhbhntnhehrmgblsboqekcpfptcdemkki");
        Tapsell.setPermissionHandlerConfig(Tapsell.PERMISSION_HANDLER_AUTO);
        Tapsell.setRewardListener (
            (TapsellAdFinishedResult result) => 
            {
                // onFinished, you may give rewards to user if result.completed and result.rewarded are both True
                Debug.Log ("onFinished, adId:"+result.adId+", zoneId:"+result.zoneId+", completed:"+result.completed+", rewarded:"+result.rewarded);

                // You can validate suggestion from you server by sending a request from your game server to tapsell, passing adId to validate it
                if(result.completed && result.rewarded)
                {
                    ValidateSuggestion(result.adId);
                }
            }
        );
        requestAd(bannerZoneId, false);
        requestAd(videoZoneId, false);
    }

    private void requestAd(string zone, bool cached)
    {
        Tapsell.requestAd(zone, cached,
            (TapsellAd result) =>
            {
                // onAdAvailable
                Debug.Log("Action: onAdAvailable");
                if (zone == videoZoneId)
                {
                    videoAvailable = true;
                    Videoad = result;
                }
                else
                {
                    bannerAvailable = true;
                    Bannerad = result;
                }
               
               
            },
            (string zoneId) =>
            {
                // onNoAdAvailable
                Debug.Log("No Ad Available");
            },
            (TapsellError error) =>
            {
                // onError
                Debug.Log(error.error);
            },
            (string zoneId) =>
            {
                // onNoNetwork
                Debug.Log("No Network: " + zoneId);
            },
            (TapsellAd result) =>
            {
                //onExpiring
                if (result.zoneId == videoZoneId)
                {
                    videoAvailable = false;
                    Videoad = null;
                }
                else
                {
                    bannerAvailable = false;
                    Bannerad = null;
                }
               
                requestAd(result.zoneId, false);
            }
        );
    }

    public void showBannerAd()
    {
        bannerAllow = true;
    }
    public void showVideoAd()
    {
        videoAllow = true;


    }
    private void Update()
    {
        if (videoAvailable && videoAllow)
        {
            videoAvailable = false;
            videoAllow = false;
            
            TapsellShowOptions options = new TapsellShowOptions();
            options.backDisabled = false;
            options.immersiveMode = false;
            options.rotationMode = TapsellShowOptions.ROTATION_LOCKED_LANDSCAPE;
            options.showDialog = true;
            Tapsell.showAd(Videoad, options);
            StartCoroutine(ReqAgain(videoZoneId));

        }
        else if (bannerAvailable && bannerAllow)
        {
            bannerAvailable = false;
            bannerAllow = false;
            TapsellShowOptions options = new TapsellShowOptions();
            options.backDisabled = false;
            options.immersiveMode = false;
            options.rotationMode = TapsellShowOptions.ROTATION_LOCKED_LANDSCAPE;
            options.showDialog = true;
            Tapsell.showAd(Bannerad, options);
            StartCoroutine(ReqAgain(bannerZoneId));

        }

       
    }

    private IEnumerator ReqAgain(String zoneId)
    {
        while (true)
        {
            yield return new WaitForSeconds(10);
            requestAd(zoneId, false);
            break;
        }
    }
    
    
    public void ValidateSuggestion(string suggestionId)
    {
        try
        {
            string ourPostData = "{\"suggestionId\":\"" + suggestionId +"\"}";
            System.Collections.Generic.Dictionary<string,string> headers = new System.Collections.Generic.Dictionary<string, string>();
            headers.Add("Content-Type", "application/json");

            byte[] pData = System.Text.Encoding.ASCII.GetBytes(ourPostData.ToCharArray());

            WWW api = new WWW("http://api.tapsell.ir/v2/suggestions/validate-suggestion", pData, headers);
            StartCoroutine(WaitForRequest(api));
        }
        catch (UnityException ex)
        { 
            Debug.Log(ex.Message); 
        }
        return;
    }

    IEnumerator WaitForRequest(WWW data)
    {
        Debug.Log("my start waiting...");
        yield return data; // Wait until the download is done
        if (data.error != null)
        {
            Debug.Log("my server error is " + data.error);
        }
        else
        {
            Debug.Log("my server result is "+data.text);

            JSONNode node = JSON.Parse (data.text);
            bool valid = node ["valid"].AsBool;
            if (valid) {
                onFinishCallback.Invoke();
             
            } 
           
        }
    }
}