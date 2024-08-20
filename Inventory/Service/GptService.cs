using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

public class GptService
{
    private readonly HttpClient _httpClient;

    public GptService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetGptResponseAsync(string searchTerm, List<string> mergedData, string customTask = null)
    {
        string languagePrompt;

        if (!string.IsNullOrEmpty(customTask))
        {
            languagePrompt = $@"
            {customTask}
            The Given data is: {string.Join(", ", mergedData)}
            Note: Don't give question number after analysis; if required, please give details for questions.
        ";
        }
        else
        {
            languagePrompt = $@"
            You are an expert in data analysis and can answer detailed questions based on the provided dataset.
            The given data is as follows:
            {string.Join(", ", mergedData)}
            
            Based on this data, provide comprehensive and detailed answers to any questions that might be asked.
            For instance:
            - Identify trends or patterns in the data.
            - Describe notable insights about the data.
            - Analyze user behavior or product popularity.
            - Comment on order statuses and delivery details.
        

            Provide your answers with clear explanations and include relevant statistics or observations. Use emojis to highlight key findings if applicable.withi in 150 words. Start your response with 'Detailed Analysis:'.
        ";
        }

        var requestBody = new
        {
            model = "gpt-3.5-turbo",
            messages = new[]
            {
                new { role = "system", content = "You are an expert in data analysis and can provide detailed insights based on the provided dataset." },
                new { role = "user", content = languagePrompt }
            },
            temperature = 0.7
        };

        // Log the request body
        Console.WriteLine("Request Body: " + JsonSerializer.Serialize(requestBody));

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
        {
            Content = JsonContent.Create(requestBody)
        };

        request.Headers.Add("Authorization", $"Bearer"); // Replace with your actual API key

        try
        {
            var response = await _httpClient.SendAsync(request);

            var jsonResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Raw API Response: " + jsonResponse);

            if (response.IsSuccessStatusCode)
            {
                var jsonDocument = JsonDocument.Parse(jsonResponse);
                var chatGptResponse = jsonDocument.RootElement.GetProperty("choices")[0]
                    .GetProperty("message").GetProperty("content").GetString();

                return chatGptResponse;
            }
            else
            {
                Console.WriteLine($"API request failed with status code {response.StatusCode}: {response.ReasonPhrase}");
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error processing API request: " + ex.Message);
            return null;
        }
    }
}
