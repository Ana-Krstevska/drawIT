﻿using drawIT.API.Services.Interfaces;
using drawIT.Models;
using drawIT.Services.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace drawIT.Services
{
    public class LlamaService : ILlamaService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IDrawingRequestService _drawingRequestService;

        public LlamaService(HttpClient httpClient, IConfiguration configuration,
                            IDrawingRequestService drawingRequestService)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _drawingRequestService = drawingRequestService;
        }

        public async Task<DrawingRequest> SendPromptToLlamaApiAsync(string userDescription)
        {
            var requestUrl = _configuration.GetValue<string>("LLM-URL");
            string currentDirectory = Directory.GetCurrentDirectory();
            var templatePath = Path.Combine(currentDirectory + _configuration.GetValue<string>("PromptTemplatePath"));
            var templateText = await File.ReadAllTextAsync(templatePath);
            var prompt = templateText + userDescription;

            var requestBody = new
            {
                model = "llama3",
                messages = new[]
                {
                new { role = "user", content = prompt }
            },
                stream = false
            };

            var jsonRequest = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(requestUrl, content);

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var request = await _drawingRequestService.WriteDrawingRequestToDatabaseAsync(jsonResponse);
                return request;
            }
            else
            {
                throw new HttpRequestException($"Error: {response.StatusCode}, {response.ReasonPhrase}");
            }
        }
    }
}
