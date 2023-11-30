using MobileInvitation.Config;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MobileInvitation.Payment
{
    public interface ITossPaymentService
    {
        /// <summary>
        /// 결제 승인 호출
        /// </summary>
        Task<TossPayment> ConfirmAsync(TossPostPaymentConfirm requst, string IdempotencyKey = null);
		Task<TossPayment> CancelAsync(string paymentKey, TossPostPaymentCancel postData, string IdempotencyKey = null);
		string ClientKey { get; }
    }
    public class TossPaymentService: ITossPaymentService
    {
        private readonly PgMertInfo _pgMertInfo;
        private readonly Uri _url;
        private readonly IHttpClientFactory _httpClientFactory;

        public TossPaymentService(List<PgMertInfo> pgMertInfos, IHttpClientFactory httpClientFactory) 
        {
            //API URL, 고정으로 Config에 설정하지 않음.
            _url = new Uri("https://api.tosspayments.com");
            _httpClientFactory = httpClientFactory;
            _pgMertInfo = pgMertInfos.FirstOrDefault(m => m.Id == "barunsonmcard");
        }

        /// <summary>
        /// API 호출 시 인증 헤더 값
        /// </summary>
        private string Base64EncodedAuthenticationString
        {
            get
            {
                if (_pgMertInfo == null)
                    return string.Empty;
                else
                {
                    //시크릿 키 뒤에 :을 추가하고 base64로 인코딩
                    var encData_byte = Encoding.ASCII.GetBytes(_pgMertInfo.SecretKey + ":");
                    return Convert.ToBase64String(encData_byte);
                }
            }
        }

        /// <summary>
        /// 클라이언트키
        /// </summary>
        public string ClientKey => _pgMertInfo?.ClientKey;
        /// <summary>
        /// 결제 승인 호출
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        public async Task<TossPayment> ConfirmAsync(TossPostPaymentConfirm postData, string IdempotencyKey = null)
        {
            var apiUri = new Uri(_url, "/v1/payments/confirm");
            var httpClient = _httpClientFactory.CreateClient();
            TossPayment tossPayment = null;

            var bodystr = JsonSerializer.Serialize(postData);
            using (var request = new HttpRequestMessage())
            {
                request.Method = HttpMethod.Post;
                request.RequestUri = apiUri;
                request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Base64EncodedAuthenticationString);
                if (!string.IsNullOrEmpty(IdempotencyKey))
                    request.Headers.Add("Idempotency-Key", IdempotencyKey);

                request.Content = new StringContent(bodystr, Encoding.UTF8, "application/json");

                var response = await httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                var restr = await response.Content.ReadAsStringAsync();
                tossPayment = JsonSerializer.Deserialize<TossPayment>(restr);
            }
            
            return tossPayment;
        }
		/// <summary>
		/// 결제 취소
		/// 결제 취소에 성공했다면 Payment 객체의 cancels 필드에 취소 객체가 배열로 돌아옵니다.
		/// </summary>
		/// <param name="paymentKey"></param>
		/// <returns></returns>
		public async Task<TossPayment> CancelAsync(string paymentKey, TossPostPaymentCancel postData, string IdempotencyKey = null)
        {
			var apiUri = new Uri(_url, $"/v1/payments/{paymentKey}/cancel");
			var httpClient = _httpClientFactory.CreateClient();
			TossPayment tossPayment = null;

			var bodystr = JsonSerializer.Serialize(postData);
			using (var request = new HttpRequestMessage())
			{
				request.Method = HttpMethod.Post;
				request.RequestUri = apiUri;
				request.Headers.Authorization = new AuthenticationHeaderValue("Basic", Base64EncodedAuthenticationString);
				if (!string.IsNullOrEmpty(IdempotencyKey))
					request.Headers.Add("Idempotency-Key", IdempotencyKey);

				request.Content = new StringContent(bodystr, Encoding.UTF8, "application/json");

				var response = await httpClient.SendAsync(request);
				response.EnsureSuccessStatusCode();

				var restr = await response.Content.ReadAsStringAsync();
				tossPayment = JsonSerializer.Deserialize<TossPayment>(restr);
			}

			return tossPayment;
		}
	}
}
