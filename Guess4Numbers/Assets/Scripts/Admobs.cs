using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.UI;

public class Admobs : MonoBehaviour
{
	private BannerView adBanner;
	private InterstitialAd adInterstitial;
	private RewardBasedVideoAd adReward;
	private string idApp, idBanner, idInterstitial, idReward;
	public GameObject gameOverPopup;
	public GameObject gameScene;
	public Button BtnInterstitial;
	public Button BtnReward;

	public Text popupText;

	void Start()
	{
		BtnInterstitial.interactable = true;

		idApp = "ca-app-pub-2169946569801510~2527153230";
		idBanner = "ca-app-pub-3940256099942544/6300978111";
		idInterstitial = "ca-app-pub-2169946569801510/4518891753";
		idReward = "ca-app-pub-2169946569801510/5217707827";

		adReward = RewardBasedVideoAd.Instance;

		MobileAds.Initialize(idApp);

		//RequestBannerAd();
		RequestInterstitialAd();
	}

	public void OnGetContinueClicked()
	{
		BtnReward.interactable = false;
		BtnInterstitial.interactable = false;
		popupText.text = "Loading Ad...";
		RequestRewardAd();
	}

	#region Reward video methods ------------------------------------
	public void RequestRewardAd()
	{
		AdRequest request =  AdRequestBuild();
		adReward.LoadAd(request, idReward);

		adReward.OnAdLoaded += this.HandleOnRewardedAdLoaded;
		adReward.OnAdRewarded += this.HandleOnAdRewarded;
		adReward.OnAdClosed += this.HandleOnRewardedAdClosed;
	}

	public void ShowRewardAd()
	{
		if (adReward.IsLoaded())
			adReward.Show();
	}
	private void HandleOnRewardedAdLoaded(object sender, EventArgs e)
	{
		// Ad closed  Even if not finish watching
		ShowRewardAd();
	}

	private void HandleOnAdRewarded(object sender, Reward e)
	{
		//Ad opened
	}

	private void HandleOnRewardedAdClosed(object sender, EventArgs e)
	{
		//Ad loaded
		BtnReward.interactable = true;
		BtnInterstitial.interactable = true;
		gameOverPopup.SetActive(false);
		gameScene.SetActive(true);
		popupText.text = "Watch Ad To Continue";
		gameObject.GetComponent<GameplayScript>().timeEnd = false;
		gameObject.GetComponent<GameplayScript>().countTime = gameObject.GetComponent<GameplayScript>().gameTime;

		adReward.OnAdLoaded -= this.HandleOnRewardedAdLoaded;
		adReward.OnAdRewarded -= this.HandleOnAdRewarded;
		adReward.OnAdClosed -= this.HandleOnRewardedAdClosed;
	}



	#endregion

	#region Banner Methods --------------------------------------------------

	public void RequestBannerAd()
	{
		adBanner = new BannerView(idBanner, AdSize.Banner, AdPosition.Bottom);
		AdRequest request = AdRequestBuild();
		adBanner.LoadAd(request);
	}

	public void DestroyBannerAd()
	{
		if (adBanner != null)
			adBanner.Destroy();
	}

	#endregion
	
	#region Interstitial methods ---------------------------------------------

	public void RequestInterstitialAd()
	{
		adInterstitial = new InterstitialAd(idInterstitial);
		AdRequest request =  AdRequestBuild();
		adInterstitial.LoadAd(request);

		adInterstitial.OnAdLoaded += this.HandleOnAdLoaded;
		adInterstitial.OnAdOpening += this.HandleOnAdOpening;
		adInterstitial.OnAdClosed += this.HandleOnAdClosed;
	}
	public void ShowInterstitialAd()
	{
		if (adInterstitial.IsLoaded())
			adInterstitial.Show();
	}
	public void DestroyInterstitialAd()
	{
		adInterstitial.Destroy();
	}
	private void HandleOnAdLoaded(object sender, EventArgs e)
	{
		BtnInterstitial.interactable = true;
	}
	private void HandleOnAdOpening(object sender, EventArgs e)
	{
		BtnInterstitial.interactable = false;
	}
	private void HandleOnAdClosed(object sender, EventArgs e)
	{
		adInterstitial.OnAdLoaded -= this.HandleOnAdLoaded;
		adInterstitial.OnAdOpening -= this.HandleOnAdOpening;
		adInterstitial.OnAdClosed -= this.HandleOnAdClosed;

		RequestInterstitialAd();
	}

	#endregion
	//------------------------------------------------------------------------
	AdRequest AdRequestBuild()
	{
		return new AdRequest.Builder().Build();
	}

	void OnDestroy()
	{
		DestroyBannerAd();
		DestroyInterstitialAd();

		adInterstitial.OnAdLoaded -= this.HandleOnAdLoaded;
		adInterstitial.OnAdOpening -= this.HandleOnAdOpening;
		adInterstitial.OnAdClosed -= this.HandleOnAdClosed;

		adReward.OnAdLoaded -= this.HandleOnRewardedAdLoaded;
		adReward.OnAdRewarded -= this.HandleOnAdRewarded;
		adReward.OnAdClosed -= this.HandleOnRewardedAdClosed;
	}

}