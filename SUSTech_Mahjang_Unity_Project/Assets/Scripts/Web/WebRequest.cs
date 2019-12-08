using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;


using Assets.Scripts.GameMain;

namespace Assets.Scripts.Web
{
	class WebRequest
	{
		string serverUri;
		PlayDesk clientPlayDesk;
		int timeout;

		public WebRequest(string serverUri, PlayDesk clientPlayDesk, int timeout=1)
		{
			this.serverUri = serverUri;
			this.clientPlayDesk = clientPlayDesk;
			this.timeout = timeout;
		}

		IEnumerator Get(string method)
		{
			string uri = serverUri + method;
			using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
			{
				webRequest.timeout = timeout;
				// Request and wait for the desired page.
				yield return webRequest.SendWebRequest();

				string[] pages = uri.Split('/');
				int page = pages.Length - 1;

				if (webRequest.isNetworkError || webRequest.isHttpError)
				{
					Debug.Log(pages[page] + ": Error: " + webRequest.error);
				}
				else
				{
					Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
				}
			}
		}

		IEnumerator Post(string method, WWWForm form)
		{
			string uri = serverUri + method;
			using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
			{
				www.timeout = timeout;
				yield return www.SendWebRequest();

				if (www.isNetworkError || www.isHttpError)
				{
					Debug.Log(www.error);
				}
				else
				{
					Debug.Log("Form upload complete!\n" + form);
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public int Init()
		{
			// TODO
			return 0;
		}

		public IEnumerator Play(GameMain.Tile tile)
		{
			WWWForm form = new WWWForm();
			form.AddField("hand_tile", tile.id);
			yield return Post("play", form);
		}

	}

}
