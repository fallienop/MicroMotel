﻿using MicroMotel.Shared.DTOs;
using MicroMotel.Web.Models.FakePayment;
using MicroMotel.Web.Services.Interface;

namespace MicroMotel.Web.Services.Abstract
{
    public class PaymentService : IPaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Card> GetCard(string cardnumber)
        {
            var response = await _httpClient.GetFromJsonAsync<Response<Card>>($"fakepayment/{cardnumber}");
            var card =response.Data;
            return card;
        }

        public async Task<bool> ReceivePayment(PaymentInput paymentInput)
        {
            var response = await _httpClient.PostAsJsonAsync("fakepayment",paymentInput);
            var resp = await response.Content.ReadFromJsonAsync<Response<bool>>();
            return resp.Data;
        }
        public async Task<bool> TestCard(PaymentInput paymentInput)
        {
            var response = await _httpClient.PostAsJsonAsync("fakepayment/getcard", paymentInput);
            var resp = await response.Content.ReadFromJsonAsync<Response<bool>>();
            return resp.Data;
        }
    }
}
